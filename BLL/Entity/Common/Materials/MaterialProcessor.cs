using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Materials.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;

namespace RDBLL.Entity.Common.Materials
{
    public enum MaterialKindTypes
    {
        Concrete,
        Reinforcement,
        Steel,
    }
    public static class MaterialProcessor
    {
        public static IMaterialKind GetMaterialKindById (MaterialKindTypes type, int Id)
        {
            switch (type)
            {
                case MaterialKindTypes.Concrete:
                    foreach (ConcreteKind concreteKind in ProgrammSettings.ConcreteKinds)
                    {
                        if (concreteKind.Id == Id)
                        {
                            return concreteKind;
                        }
                    }
                    break;
                case MaterialKindTypes.Reinforcement:
                    foreach (ReinforcementKind reinforcementKind in ProgrammSettings.ReinforcementKinds)
                    {
                        if (reinforcementKind.Id == Id)
                        {
                            return reinforcementKind;
                        }
                    }
                    break;
                case MaterialKindTypes.Steel:
                    foreach (SteelKind steelKind in ProgrammSettings.SteelKinds)
                    {
                        if (steelKind.Id == Id)
                        {
                            return steelKind;
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
