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
using RDBLL.Common.Geometry;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;

namespace RDBLL.Processors.SC
{
    public static class SteelBoltProcessor
    {
        public static double GetStressNonLinear(SteelBolt steelBolt, Curvature curvature)
        {
            double stress = 0;
            //NdmArea ndmArea = steelBolt.SubPart;
            //stress = NdmAreaProcessor.GetStrainFromCuvature(ndmArea, curvature)[1];
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
            SteelBase steelBase = steelBolt.ParentMember as SteelBase;
            foreach (ForceDoubleCurvature forceCurvature in steelBase.ForceCurvatures)
            {
                //При расчете по упрощенном методу кривизна бетона и болтов не совпадает
                if (steelBase.UseSimpleMethod)
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
