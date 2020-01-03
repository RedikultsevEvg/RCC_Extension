using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using System.Collections.ObjectModel;
using RDBLL.Common.Geometry;
using System.Windows.Forms;
using RDBLL.Entity.Results.NDM;
using RDBLL.Common.Service;

namespace RDBLL.Processors.Forces
{
    /// <summary>
    /// Процессор наборов нагрузок
    /// </summary>
    public class LoadSetProcessor
    {
        /// <summary>
        /// Возвращает сумму двух комбинаций
        /// </summary>
        /// <param name="oldLoadSet">Первая комбинация</param>
        /// <param name="secondLoadSet">Вторая комбинация</param>
        /// <param name="koeff">Коэффициент, с которым прибавляется вторая комбинация</param>
        /// <param name="show_koeff">Флаг отображения принятого коэффициента в названии комбинации</param>
        public static void SumForces(LoadSet oldLoadSet, LoadSet secondLoadSet, double koeff = 1.0, bool show_koeff = true)
        {
            if (oldLoadSet == null) oldLoadSet = new LoadSet();
            bool coindence; //Флаг совпадения вида нагрузки
            if (!String.IsNullOrEmpty(oldLoadSet.Name)) { oldLoadSet.Name += " + "; }
            foreach (ForceParameter secondForceParameter in secondLoadSet.ForceParameters)
            {
                coindence = false; //Обнуляем флаг
                foreach (ForceParameter oldForceParameter in oldLoadSet.ForceParameters)
                {
                    if (oldForceParameter.KindId == secondForceParameter.KindId) //Если вид нагрузки совпадает
                    {
                        oldForceParameter.CrcValue += secondForceParameter.CrcValue * koeff; //Складываем значения параметра нагрузки
                        oldForceParameter.DesignValue += secondForceParameter.CrcValue * secondLoadSet.PartialSafetyFactor * koeff; //Складываем значения параметра нагрузки
                        coindence = true;
                        break; //Дальше проходить по циклу смысла нет, так как каждый вид нагрузки встречается только один раз
                    }
                }

                if (!coindence) //Если вид нагрузки так и не совпал
                {
                    //Добавляем в набор новый вид нагрузки нужного типа
                    ForceParameter forceParameter = new ForceParameter();
                    forceParameter.CrcValue = secondForceParameter.CrcValue * koeff;
                    forceParameter.DesignValue = secondForceParameter.CrcValue * secondLoadSet.PartialSafetyFactor * koeff;
                    oldLoadSet.PartialSafetyFactor = secondLoadSet.PartialSafetyFactor;
                    forceParameter.KindId = secondForceParameter.KindId;
                    oldLoadSet.ForceParameters.Add(forceParameter);
                }
            }
            oldLoadSet.Name += secondLoadSet.Name;
            //oldLoadSet.LoadSet.PartialSafetyFactor = tmpPartialFactor;
            if (show_koeff) { oldLoadSet.Name += "*(" + Convert.ToString(koeff) + ")"; }
            return;
        }
        /// <summary>
        /// Возвращает сумму двух комбинаций в виде новой комбинации
        /// </summary>
        /// <param name="oldLoadSet">Первая комбинация</param>
        /// <param name="secondLoadSet">Вторая комбинация</param>
        /// <param name="koeff">Коэффициент, с которым прибавляется вторая комбинация</param>
        /// <returns></returns>
        public static LoadSet SumForcesInNew(LoadSet oldLoadSet, LoadSet secondLoadSet, double koeff = 1.0)
        {
            LoadSet newLoadSet = new LoadSet() { Id = ProgrammSettings.CurrentTmpId };
            SumForces(newLoadSet, oldLoadSet, 1, false);
            SumForces(newLoadSet, secondLoadSet, koeff, true);
            return newLoadSet;
        }
        /// <summary>
        /// Складывает дубликаты нагрузок в комбинации
        /// </summary>
        /// <param name="loadSet">Комбинация нагрузок</param>
        /// <returns></returns>
        public static LoadSet DeduplicateLoadSet(LoadSet loadSet)
        {
            LoadSet deduplicatedSet = new LoadSet();
            deduplicatedSet.IsLiveLoad = loadSet.IsLiveLoad;
            deduplicatedSet.BothSign = loadSet.BothSign;
            deduplicatedSet.PartialSafetyFactor = loadSet.PartialSafetyFactor;
            SumForces(deduplicatedSet, loadSet, 1, false);
            return deduplicatedSet;
        }
        /// <summary>
        /// Получает коллекцию комбинаций нагрузок по коллекции нагрузок
        /// </summary>
        /// <param name="forcesGroups">Коллекция нагрузок</param>
        /// <returns></returns>
        public static ObservableCollection<LoadSet> GetLoadCases(ObservableCollection<ForcesGroup> forcesGroups)
        {
            ObservableCollection<LoadSet> LoadCases = new ObservableCollection<LoadSet>(); //Комбинация нагрузок, которую будем в итоге получать
            LoadCases.Add(new LoadSet());
            foreach (ForcesGroup forcesGroup in forcesGroups)
            {
                foreach (LoadSet LoadSet in forcesGroup.LoadSets)
                {
                    LoadSet tmpLoadSet = DeduplicateLoadSet(LoadSet);
                    int count = LoadCases.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (!tmpLoadSet.IsLiveLoad) //Если нагрузка является постоянной, то просто добавляем данную нагрузку
                        {
                            SumForces(LoadCases[i], tmpLoadSet);
                        }
                        else //Если нагрузка не постоянная, то исходное сочетание не трогаем и добавляем новые с положительным и отрицательным значением
                        {
                            LoadCases.Add(SumForcesInNew(LoadCases[i], tmpLoadSet));
                            if (tmpLoadSet.BothSign) //Если нагрузка может быть знакопеременной, то добавляем сочетание с обратным знаком
                            {
                                LoadCases.Add(SumForcesInNew(LoadCases[i], tmpLoadSet, -1.0));
                            }
                        }
                    }
                }
            }

            return LoadCases;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadCase"></param>
        /// <param name="massProperty"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static double StressInBarSection(LoadSet loadCase, MassProperty massProperty, double dx, double dy)

        {
            double stress;
            double Nz = 0;
            double Mx = 0;
            double My = 0;
            double A = massProperty.A;
            double Ix = massProperty.Ix;
            double Iy = massProperty.Iy;

            foreach (ForceParameter forceParameter in loadCase.ForceParameters)
            {
                switch (forceParameter.KindId)
                {
                    case 1:
                        Nz = forceParameter.DesignValue;
                        break;
                    case 2:
                        Mx = forceParameter.DesignValue;
                        break;
                    case 3:
                        My = forceParameter.DesignValue;
                        break;
                }
            }

            stress = Nz / A + Mx / (Ix / dy) - My / (Iy / dx);
            return stress;
        }
        /// <summary>
        /// Возвращает комбинацию нагрузок приведенную к другой точке
        /// </summary>
        /// <param name="loadCase">Исходная комбинация нагрузок</param>
        /// <param name="delta">Массив разницы координат dX, dY, dZ</param>
        /// <returns></returns>
        public static LoadSet GetLoadSetTransform(LoadSet loadCase, double[] delta)
        {
            double dx = delta[0];
            double dy = delta[1];
            double dz = delta[2];
            double[] Nz = new double[2] { 0, 0};
            double[] Mx = new double[2] { 0, 0 };
            double[] My = new double[2] { 0, 0 };
            double[] Qx = new double[2] { 0, 0 };
            double[] Qy = new double[2] { 0, 0 };
            double[] Mz = new double[2] { 0, 0 };

            LoadSet deduplicatedSet = DeduplicateLoadSet(loadCase);
            foreach (ForceParameter forceParameter in deduplicatedSet.ForceParameters)
            {
                switch (forceParameter.KindId)
                {
                    case 1:
                        Nz[0] = forceParameter.CrcValue;
                        Nz[1] = forceParameter.DesignValue;
                        break;
                    case 2:
                        Mx[0] = forceParameter.CrcValue;
                        Mx[1] = forceParameter.DesignValue;
                        break;
                    case 3:
                        My[0] = forceParameter.CrcValue;
                        My[1] = forceParameter.DesignValue;
                        break;
                    case 4:
                        Qx[0] = forceParameter.CrcValue;
                        Qx[1] = forceParameter.DesignValue;
                        break;
                    case 5:
                        Qy[0] = forceParameter.CrcValue;
                        Qy[1] = forceParameter.DesignValue;
                        break;
                    case 6:
                        Mz[0] = forceParameter.CrcValue;
                        Mz[1] = forceParameter.DesignValue;
                        break;
                }
            }
            Mx[0] += Qy[0] * dz - Nz[0] * dy;
            Mx[1] += Qy[1] * dz - Nz[1] * dy;
            //У моментов по оси Y другие знаки, так как тройка осей координат правая
            My[0] += - Qx[0] * dz + Nz[0] * dx;
            My[1] += - Qx[1] * dz + Nz[1] * dx;

            Mz[0] += Qx[0] * dy + Qy[0] * dx;
            Mz[1] += Qx[1] * dy + Qy[1] * dx;

            LoadSet newLoadSet = new LoadSet();
            newLoadSet.Name = deduplicatedSet.Name;
            ForceParameter newForceParameter;
            if (Nz[0] !=0 || Nz[1] != 0)
            {
                newForceParameter = new ForceParameter(newLoadSet);
                newForceParameter.KindId = 1;
                newForceParameter.CrcValue = Nz[0];
                newForceParameter.DesignValue = Nz[1];
            }
            if (Mx[0] != 0 || Mx[1] != 0)
            {
                newForceParameter = new ForceParameter(newLoadSet);
                newForceParameter.KindId = 2;
                newForceParameter.CrcValue = Mx[0];
                newForceParameter.DesignValue = Mx[1];
            }
            if (My[0] != 0 || My[1] != 0)
            {
                newForceParameter = new ForceParameter(newLoadSet);
                newForceParameter.KindId = 3;
                newForceParameter.CrcValue = My[0];
                newForceParameter.DesignValue = My[1];
            }
            if (Qx[0] != 0 || Qx[1] != 0)
            {
                newForceParameter = new ForceParameter(newLoadSet);
                newForceParameter.KindId = 4;
                newForceParameter.CrcValue = Qx[0];
                newForceParameter.DesignValue = Qx[1];
            }
            if (Qy[0] != 0 || Qy[1] != 0)
            {
                newForceParameter = new ForceParameter(newLoadSet);
                newForceParameter.KindId = 5;
                newForceParameter.CrcValue = Qy[0];
                newForceParameter.DesignValue = Qy[1];
            }
            if (Mz[0] != 0 || Mz[1] != 0)
            {
                newForceParameter = new ForceParameter(newLoadSet);
                newForceParameter.KindId = 6;
                newForceParameter.CrcValue = Mz[0];
                newForceParameter.DesignValue = Mz[1];
            }
            return newLoadSet;
        }
        /// <summary>
        /// Возвращает коллекцию комбинаций нагрузок приведенную к другой точке
        /// </summary>
        /// <param name="loadCases">Исходная коллекция комбинаций нагрузок</param>
        /// <param name="delta">Массив разницы координат dX, dY, dZ</param>
        /// <returns></returns>
        public static ObservableCollection<LoadSet> GetLoadSetsTransform(ObservableCollection<LoadSet> loadCases, double[] delta)
        {
            ObservableCollection<LoadSet> newLoadSets = new ObservableCollection<LoadSet>();
            foreach (LoadSet loadSet in loadCases)
            {
                newLoadSets.Add(GetLoadSetTransform(loadSet, delta));
            }
            return newLoadSets;
        }
    }
}
