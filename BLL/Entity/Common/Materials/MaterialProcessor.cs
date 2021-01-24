using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Materials.Interfaces;
using RDBLL.Common.Service;

namespace RDBLL.Entity.Common.Materials
{
    public static class MaterialProcessor
    {
        public static IMaterialKind GetMaterialKindById (string materialKindName, int Id)
        {
            switch (materialKindName)
            {
                case "Concrete":
                    foreach (ConcreteKind concreteKind in ProgrammSettings.ConcreteKinds)
                    {
                        if (concreteKind.Id == Id)
                        {
                            return concreteKind;
                        }
                    }
                    break;
                case "Reinforcement":
                    foreach (ReinforcementKind reinforcementKind in ProgrammSettings.ReinforcementKinds)
                    {
                        if (reinforcementKind.Id == Id)
                        {
                            return reinforcementKind;
                        }
                    }
                    break;
                default: throw new NotImplementedException();
            }
            return null;
        }

        /// <summary>
        /// Возвращает интегральный коэффициент надежности.
        /// Если дополнительных коэффициентов нет, то возвращаются единицы
        /// дла 1ПСи 2ПС
        /// </summary>
        /// <returns></returns>
        public static double[] GetTotalSafetyFactor(MaterialUsing materialUsing)
        {
            double[] initialSafetyFactor = new double[2] { 1, 1 };
            foreach (SafetyFactor safetyFactor in materialUsing.SafetyFactors)
            {
                initialSafetyFactor[0] *= safetyFactor.PsfFst;
                initialSafetyFactor[1] *= safetyFactor.PsfSnd;
            }
            return initialSafetyFactor;
        }
    }
}
