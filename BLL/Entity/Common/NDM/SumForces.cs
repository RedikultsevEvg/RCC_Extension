using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Forces;

namespace RDBLL.Entity.Common.NDM
{
    /// <summary>
    /// Матрица усилий
    /// [0,0] Mx
    /// [1,0] My
    /// [2,0] N
    /// </summary>
    public class SumForces
    {
        public Matrix ForceMatrix { get; set; }
        #region Constructors
        public SumForces()
        {
            ForceMatrix = new Matrix(3, 1);
        }
        public SumForces(StiffnessCoefficient stifCoef, Curvature curvature)
        {
            ForceMatrix = stifCoef.StifMatrix * curvature.CurvMatrix;
        }
        public SumForces(LoadSet loadCase, bool designLoad = true)
        {
            ForceMatrix = new Matrix(3, 1);
            foreach (ForceParameter forceParameter in loadCase.ForceParameters)
            {
                switch (forceParameter.KindId)
                {
                    case 1: //Продольная сила
                        if (designLoad) { ForceMatrix[2, 0] = forceParameter.DesignValue; } else { ForceMatrix[2, 0] = forceParameter.CrcValue; }
                        break;
                    case 2: //Момент относительно оси X
                        if (designLoad) { ForceMatrix[0, 0] = forceParameter.DesignValue; } else { ForceMatrix[0, 0] = forceParameter.CrcValue; }
                        break;
                    case 3: //Момент относительно оси Y
                        if (designLoad) { ForceMatrix[1, 0] = forceParameter.DesignValue; } else { ForceMatrix[1, 0] = forceParameter.CrcValue; }
                        break;
                }
            }
        }
        public SumForces(SumForces initForces, SumForces secForces)
        {
            ForceMatrix = initForces.ForceMatrix - secForces.ForceMatrix;
        }
        #endregion
    }
}
