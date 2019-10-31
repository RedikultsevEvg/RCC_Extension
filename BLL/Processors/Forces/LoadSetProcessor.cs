using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using System.Collections.ObjectModel;
using RDBLL.Common.Geometry;
using System.Windows.Forms;

namespace RDBLL.Processors.Forces
{
    public class LoadSetProcessor
    {
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
                    if (oldForceParameter.Kind_id == secondForceParameter.Kind_id) //Если вид нагрузки совпадает
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
                    forceParameter.Kind_id = secondForceParameter.Kind_id;
                    oldLoadSet.ForceParameters.Add(forceParameter);
                }
            }
            oldLoadSet.Name += secondLoadSet.Name;
            //oldLoadSet.LoadSet.PartialSafetyFactor = tmpPartialFactor;
            if (show_koeff) { oldLoadSet.Name += "*(" + Convert.ToString(koeff) + ")"; }
            return;
        }

        public static LoadSet SumForcesInNew(LoadSet oldLoadSet, LoadSet secondLoadSet, double koeff = 1.0)
        {
            LoadSet newLoadSet = new LoadSet();
            SumForces(newLoadSet, oldLoadSet, 1, false);
            SumForces(newLoadSet, secondLoadSet, koeff, true);
            return newLoadSet;
        }

        public static LoadSet DeduplicateLoadSet(LoadSet loadSet)
        {
            LoadSet deduplicatedSet = new LoadSet();
            deduplicatedSet.IsLiveLoad = loadSet.IsLiveLoad;
            deduplicatedSet.BothSign = loadSet.BothSign;
            deduplicatedSet.PartialSafetyFactor = loadSet.PartialSafetyFactor;
            SumForces(deduplicatedSet, loadSet, 1, false);
            return deduplicatedSet;
        }

        public static List<LoadSet> GetLoadCases(ObservableCollection<ForcesGroup> forcesGroups)
        {
            List<LoadSet> LoadCases = new List<LoadSet>(); //Комбинация нагрузок, которую будем в итоге получать
            LoadCases.Add(new LoadSet());
            foreach (ForcesGroup forcesGroup in forcesGroups)
            {
                foreach (LoadSet LoadSet in forcesGroup.LoadSets)
                {
                    LoadSet tmpLoadSet = DeduplicateLoadSet(LoadSet);
                    int count = LoadCases.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (! tmpLoadSet.IsLiveLoad) //Если нагрузка является постоянной, то просто добавляем данную нагрузку
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
                switch (forceParameter.Kind_id)
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
    }
}
