using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using RDBLL.Entity.Results.Forces;
using System.Windows.Forms;
using RDBLL.Common.Geometry;
using System.Collections.ObjectModel;

namespace RDBLL.Processors.Forces
{
    /// <summary>
    /// Процессор для обработки нагрузки на стержень
    /// </summary>
    public class BarLoadSetProcessor
    {
        /// <summary>
        /// Метод суммирования двух комбинаций нагрузки (к старой нагрузке добавляется новая с нужным коэффициентом)
        /// </summary>
        /// <param name="oldLoadSet">Существующая комбинация нагрузок</param>
        /// <param name="secondLoadSet">Добавляемая комбинация нагрузок</param>
        /// <param name="koeff">Коэффициент надежности по нагрузке</param>
        public static void SumForces(BarLoadSet oldLoadSet, BarLoadSet secondLoadSet, double koeff = 1.0)
        {
            if (oldLoadSet == null) oldLoadSet = new BarLoadSet();
            bool coindence; //Флаг совпадения вида нагрузки
            if (! String.IsNullOrEmpty(oldLoadSet.LoadSet.Name)) { oldLoadSet.LoadSet.Name += " + "; }
            foreach (ForceParameter secondForceParameter in secondLoadSet.LoadSet.ForceParameters) 
            {
                coindence = false; //Обнуляем флаг
                foreach (ForceParameter oldForceParameter in oldLoadSet.LoadSet.ForceParameters)
                {
                    if (oldForceParameter.Kind_id == secondForceParameter.Kind_id) //Если вид нагрузки совпадает
                    {
                        oldForceParameter.Value += secondForceParameter.Value; //Складываем значения параметра нагрузки
                        coindence = true;
                        break; //Дальше проходить по циклу смысла нет, так как каждый вид нагрузки встречается только один раз
                    }
                }

                if (!coindence) //Если вид нагрузки так и не совпал
                {
                    //Добавляем в набор новый вид нагрузки нужного типа
                    ForceParameter forceParameter = new ForceParameter();
                    forceParameter.Value = secondForceParameter.Value;
                    forceParameter.Kind_id = secondForceParameter.Kind_id;
                    oldLoadSet.LoadSet.ForceParameters.Add(forceParameter);
                }
            }
            oldLoadSet.LoadSet.Name += secondLoadSet.LoadSet.Name + "*(" + Convert.ToString(secondLoadSet.LoadSet.PartialSafetyFactor * koeff) + ")";
            return;
        }

        public static BarLoadSet SumForcesInNew(BarLoadSet oldLoadSet, BarLoadSet secondLoadSet, double koeff = 1.0)
        {
            BarLoadSet newBarLoadSet = new BarLoadSet();
            SumForces(newBarLoadSet, oldLoadSet);
            SumForces(newBarLoadSet, secondLoadSet);
            return newBarLoadSet;
        }

        /// <summary>
        /// Суммирование дубликатов нагрузки если они встречаются в наборе 
        /// </summary>
        /// <param name="barLoadSet"></param>
        /// <returns></returns>
        public static BarLoadSet DeduplicateLoadSet (BarLoadSet barLoadSet)
        {
            BarLoadSet deduplicatedSet = new BarLoadSet();
            deduplicatedSet.LoadSet.IsDeadLoad = barLoadSet.LoadSet.IsDeadLoad;
            SumForces(deduplicatedSet, barLoadSet);
            return deduplicatedSet;
        }

        /// <summary>
        /// Метод преобразует набор групп усилий в набор комбинаций нагрузок
        /// В результате преобразования множество групп усилий перебором комбинируются в никальные комбинации
        /// </summary>
        /// <param name="forcesGroups"></param>
        /// <returns></returns>
        public static List<BarLoadSet> LoadCases(ObservableCollection<ForcesGroup> forcesGroups)
        {
            List<BarLoadSet> LoadCases = new List<BarLoadSet>();
            LoadCases.Add(new BarLoadSet());
            foreach (ForcesGroup forcesGroup in forcesGroups)
            {
                foreach (BarLoadSet barLoadSet in forcesGroup.Loads)
                {
                    BarLoadSet tmpBarLoadSet = DeduplicateLoadSet(barLoadSet);
                    foreach (BarLoadSet _barLoadSet in LoadCases)
                    {
                        if (tmpBarLoadSet.LoadSet.IsDeadLoad) //Если нагрузка является постоянной, то просто добавляем данную нагрузку
                        {
                            SumForces(_barLoadSet, tmpBarLoadSet);
                        }
                        else //Если нагрузка не постоянная, то исходное сочетание не трогаем и добавляем новые с положительным и отрицательным значением
                        {
                            BarLoadSet newBarLoadSet = SumForcesInNew(_barLoadSet, tmpBarLoadSet);
                            LoadCases.Add(newBarLoadSet);
                            if (tmpBarLoadSet.LoadSet.BothSign) //Если нагрузка может быть знакопеременной, то добавляем сочетание с обратным знаком
                            {
                                BarLoadSet newNegBarLoadSet = SumForcesInNew(_barLoadSet, tmpBarLoadSet, -1.0);
                                LoadCases.Add(newNegBarLoadSet);
                            }
                        }
                    }

                }
            }

            return LoadCases;
        }
        /// <summary>
        /// Определяет минимальные и максимальные напряжения для прямоугольного сечения
        /// </summary>
        /// <param name="loadCase"></param>
        /// <param name="massProperty"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static StressInRect MInMaxStressInBarSection(BarLoadSet loadCase, MassProperty massProperty, double dx, double dy)
        {
            StressInRect stress = new StressInRect();
            double Nz = 0; // loadCase.Force.Force_Nz;
            double Mx = 0; //loadCase.Force.Force_Mx;
            double My = 0; //loadCase.Force.Force_My;
            double A = massProperty.A;
            double Ix = massProperty.Ix;
            double Iy = massProperty.Iy;
            stress.MinStress = Nz / A - Math.Abs(Mx / (Ix / dy)) - Math.Abs(My / (Iy / dx));
            stress.MaxStress = Nz / A + Math.Abs(Mx / (Ix / dy)) + Math.Abs(My / (Iy / dx));
            return stress;
        }
        public static StressInRect MinMaxStressInBarSection(BarLoadSet loadCase, MassProperty massProperty)
        {
            StressInRect stress = new StressInRect();
            double Nz = 0; //loadCase.Force.Force_Nz;
            double Mx = 0; //loadCase.Force.Force_Mx;
            double My = 0; //loadCase.Force.Force_My;
            double A = massProperty.A;
            double Wx = massProperty.Wx;
            double Wy = massProperty.Wy;
            stress.MinStress = Nz / A - Math.Abs(Mx / Wx) - Math.Abs(My / Wy);
            stress.MaxStress = Nz / A + Math.Abs(Mx / Wx) + Math.Abs(My / Wy);
            return stress;
        }

        /// <summary>
        /// Определяет максимальное и минимальное напряжения для сечения в произвольной точке
        /// в соответствии с гипотезой плоских сечений
        /// </summary>
        /// <param name="loadCase"></param>
        /// <param name="massProperty"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static double StressInBarSection(BarLoadSet loadCase, MassProperty massProperty, double dx, double dy)
        {
            double stress;
            double Nz = 0; //loadCase.Force.Force_Nz;
            double Mx = 0; //loadCase.Force.Force_Mx;
            double My = 0; //loadCase.Force.Force_My;
            double A = massProperty.A;
            double Ix = massProperty.Ix;
            double Iy = massProperty.Iy;
            stress = Nz / A + Mx / (Ix / dy) - My / (Iy / dx);
            return stress;
        }
    }
}
