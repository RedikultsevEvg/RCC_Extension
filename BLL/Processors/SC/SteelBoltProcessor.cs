using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.Common.NDM.Processors;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM.Interfaces;
using RDBLL.Entity.Common.NDM.MaterialModels;

namespace RDBLL.Processors.SC
{
    public static class SteelBoltProcessor
    {
        public static List<SteelBolt> GetSteelBoltsFromBolt (SteelBolt steelBolt)
        {
            List<SteelBolt> steelBolts = new List<SteelBolt>();
            steelBolts.Add(steelBolt);
            if (steelBolt.AddSymmetricX)
            {
                SteelBolt newSteelBolt = (SteelBolt)steelBolt.Clone();
                newSteelBolt.Id = ProgrammSettings.CurrentTmpId;
                newSteelBolt.CenterY = (-1.0) * steelBolt.CenterY;
                steelBolts.Add(newSteelBolt);
            }
            if (steelBolt.AddSymmetricY)
            {
                SteelBolt newSteelBolt = (SteelBolt)steelBolt.Clone();
                newSteelBolt.Id = ProgrammSettings.CurrentTmpId;
                newSteelBolt.CenterX = (-1.0) * steelBolt.CenterX;
                steelBolts.Add(newSteelBolt);
            }
            if (steelBolt.AddSymmetricX & steelBolt.AddSymmetricY)
            {
                SteelBolt newSteelBolt = (SteelBolt)steelBolt.Clone();
                newSteelBolt.Id = ProgrammSettings.CurrentTmpId;
                newSteelBolt.CenterX = (-1.0) * steelBolt.CenterX;
                newSteelBolt.CenterY = (-1.0) * steelBolt.CenterY;
                steelBolts.Add(newSteelBolt);
            }
            return steelBolts;
        }

        public static void GetSubParts(SteelBolt steelBolt)
        {
            IMaterialModel materialModel = new LinearIsotropic(2e+11, 0.000001, 1);
            steelBolt.SubPart = new NdmCircleArea(materialModel);
            steelBolt.SubPart.Diametr = steelBolt.Diameter;
            steelBolt.SubPart.CenterX = steelBolt.CenterX;
            steelBolt.SubPart.CenterY = steelBolt.CenterY;
        }

        public static double GetStressNonLinear(SteelBolt steelBolt, Curvature curvature)
        {
            double stress;
            NdmArea ndmArea = steelBolt.SubPart;
            stress = NdmAreaProcessor.GetStrainFromCuvature(ndmArea, curvature)[1];
            return stress;
        }

        public static double GetMaxStressNonLinear(SteelBolt steelBolt, List<Curvature> curvatures)
        {
            List<double> stresses = new List<double>();
            foreach (Curvature curvature in curvatures)
            {
                stresses.Add(GetStressNonLinear(steelBolt, curvature));
            }
            return stresses.Max();
        }

        public static double GetMaxStressNonLinear(SteelBolt steelBolt)
        {
            List<double> stresses = new List<double>();
            foreach (ForceDoubleCurvature forceCurvature in steelBolt.SteelBase.ForceCurvatures)
            {
                //При расчете по упрощенном методу кривизна бетона и болтов не совпадает
                if (steelBolt.SteelBase.UseSimpleMethod)
                {
                    double stress = GetStressNonLinear(steelBolt, forceCurvature.DesignCurvature);
                    //Проверяем, находится ли болт в сжатой зоне бетона
                    if (stress <= 0) { stresses.Add(0); } //Если находится, то напряжения принимаем равными нулю
                    else { stresses.Add(GetStressNonLinear(steelBolt, forceCurvature.SecondDesignCurvature)); }
                }
                else
                {
                    stresses.Add(GetStressNonLinear(steelBolt, forceCurvature.SecondDesignCurvature));
                }

            }
            return stresses.Max();
        }
    }
}
