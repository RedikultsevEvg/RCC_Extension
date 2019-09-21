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
        public static void SumForces(BarLoadSet oldLoadSet, BarLoadSet secondLoadSet, double koeff = 1.0, bool show_koeff = true)
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
                        oldForceParameter.CrcValue += secondForceParameter.CrcValue * koeff; //Складываем значения параметра нагрузки
                        oldForceParameter.DesignValue += secondForceParameter.CrcValue * secondLoadSet.LoadSet.PartialSafetyFactor * koeff; //Складываем значения параметра нагрузки
                        coindence = true;
                        break; //Дальше проходить по циклу смысла нет, так как каждый вид нагрузки встречается только один раз
                    }
                }

                if (!coindence) //Если вид нагрузки так и не совпал
                {
                    //Добавляем в набор новый вид нагрузки нужного типа
                    ForceParameter forceParameter = new ForceParameter();
                    forceParameter.CrcValue = secondForceParameter.CrcValue * koeff;
                    forceParameter.DesignValue = secondForceParameter.CrcValue * secondLoadSet.LoadSet.PartialSafetyFactor * koeff;
                    oldLoadSet.LoadSet.PartialSafetyFactor = secondLoadSet.LoadSet.PartialSafetyFactor;
                    forceParameter.Kind_id = secondForceParameter.Kind_id;
                    oldLoadSet.LoadSet.ForceParameters.Add(forceParameter);
                }
            }
            oldLoadSet.LoadSet.Name += secondLoadSet.LoadSet.Name;
            //oldLoadSet.LoadSet.PartialSafetyFactor = tmpPartialFactor;
            if (show_koeff) { oldLoadSet.LoadSet.Name += "*(" + Convert.ToString(koeff) + ")"; }
            return;
        }
        public static BarLoadSet SumForcesInNew(BarLoadSet oldLoadSet, BarLoadSet secondLoadSet, double koeff = 1.0)
        {
            BarLoadSet newBarLoadSet = new BarLoadSet();
            SumForces(newBarLoadSet, oldLoadSet,1,false);
            SumForces(newBarLoadSet, secondLoadSet, koeff, true);
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
            deduplicatedSet.LoadSet.BothSign = barLoadSet.LoadSet.BothSign;
            deduplicatedSet.LoadSet.PartialSafetyFactor = barLoadSet.LoadSet.PartialSafetyFactor;
            SumForces(deduplicatedSet, barLoadSet, 1, false);
            return deduplicatedSet;
        }
        /// <summary>
        /// Метод преобразует набор групп усилий в набор комбинаций нагрузок
        /// В результате преобразования множество групп усилий перебором комбинируются в уникальные комбинации
        /// </summary>
        /// <param name="forcesGroups"></param>
        /// <returns></returns>
        public static List<BarLoadSet> GetLoadCases(ObservableCollection<ForcesGroup> forcesGroups)
        {
            List<BarLoadSet> LoadCases = new List<BarLoadSet>(); //Комбинация нагрузок, которую будем в итоге получать
            LoadCases.Add(new BarLoadSet());
            foreach (ForcesGroup forcesGroup in forcesGroups)
            {
                foreach (BarLoadSet barLoadSet in forcesGroup.Loads)
                {
                    BarLoadSet tmpBarLoadSet = DeduplicateLoadSet(barLoadSet);
                    int count = LoadCases.Count;
                    for (int i=0; i< count; i++)
                    {
                        if (tmpBarLoadSet.LoadSet.IsDeadLoad) //Если нагрузка является постоянной, то просто добавляем данную нагрузку
                        {
                            SumForces(LoadCases[i], tmpBarLoadSet);
                        }
                        else //Если нагрузка не постоянная, то исходное сочетание не трогаем и добавляем новые с положительным и отрицательным значением
                        {
                            LoadCases.Add(SumForcesInNew(LoadCases[i], tmpBarLoadSet));
                            if (tmpBarLoadSet.LoadSet.BothSign) //Если нагрузка может быть знакопеременной, то добавляем сочетание с обратным знаком
                            {
                                LoadCases.Add(SumForcesInNew(LoadCases[i], tmpBarLoadSet, -1.0));
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
        public static MinMaxStressInRect MinMaxStressInBarSection(BarLoadSet loadCase, MassProperty massProperty, double dx, double dy)
        {
            MinMaxStressInRect stress = new MinMaxStressInRect();
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
        /// <summary>
        /// Определяет напряжение для сечения в произвольной точке
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
            double Nz = 0;
            double Mx = 0;
            double My = 0;
            double A = massProperty.A;
            double Ix = massProperty.Ix;
            double Iy = massProperty.Iy;

            foreach (ForceParameter forceParameter in loadCase.LoadSet.ForceParameters)
            {
                switch (forceParameter.Kind_id)
                {
                    case 1:
                        Nz = forceParameter.CrcValue;
                        break;
                    case 2:
                        Mx = forceParameter.CrcValue;
                        break;
                    case 3:
                        My = forceParameter.CrcValue;
                        break;
                }                   
            }

            stress = Nz / A + Mx / (Ix / dy) - My / (Iy / dx);

            if ((!(Mx == 0)) & (!(My == 0)) & (stress>0))
            {
                MessageBox.Show("Программа не предназначена для учета моментов в двух плоскостях при наличии растяжения", "Ошибка");
            }

            return stress;
        }
        /// <summary>
        /// !!!!Класс не доработан! Вычисляет напряжения в указанной точке при наличии растяжения
        /// </summary>
        /// <param name="loadCase"></param>
        /// <param name="massProperty"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static double StressInBarSectionTensionBolts(BarLoadSet loadCase, MassProperty massProperty, double WidthBoltDist, double LengthBoltDist, double dx, double dy)
        {
            double stress;
            double Nz = 0;
            double Mx = 0;
            double My = 0;
            double A = massProperty.A;
            double Ix = massProperty.Ix;
            double Iy = massProperty.Iy;

            foreach (ForceParameter forceParameter in loadCase.LoadSet.ForceParameters)
            {
                switch (forceParameter.Kind_id)
                {
                    case 1:
                        Nz = forceParameter.CrcValue;
                        break;
                    case 2:
                        Mx = forceParameter.CrcValue;
                        break;
                    case 3:
                        My = forceParameter.CrcValue;
                        break;
                }
            }

            stress = Nz / A + Mx / (Ix / dy) - My / (Iy / dx);

            if ((!(Mx == 0)) & (!(My == 0)) & (stress > 0))
            {
                MessageBox.Show("Программа не предназначена для учета моментов в двух плоскостях при наличии растяжения", "Ошибка");
                MessageBox.Show("Расчет будет выполнен по наибольшему напряжению", "Ошибка");
            }

            return stress;
        }
    }
}
