using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Forces;

namespace RDBLL.Entity.Common.NDM
{
    public class SumForces
    {
        public double ForceMx { get; set; }
        public double ForceMy { get; set; }
        public double ForceN { get; set; }

        #region Constructors
        public SumForces()
        {
        }
        public SumForces(StiffnessCoefficient stifCoef, Curvature curvature)
        {
            Matrix StifMatrix = stifCoef.ToMatrix();
            Matrix CurvMatrix = curvature.ToMatrix();
            Matrix ForceMatrix = StifMatrix * CurvMatrix;
            ForceMx = ForceMatrix[0, 0];
            ForceMy = ForceMatrix[1, 0];
            ForceN = ForceMatrix[2, 0];
        }
        public SumForces(LoadSet loadCase, bool designLoad = true)
        {
            foreach (ForceParameter forceParameter in loadCase.ForceParameters)
            {
                switch (forceParameter.Kind_id)
                {
                    case 1:
                        if (designLoad) { ForceN = forceParameter.DesignValue; } else { ForceN = forceParameter.CrcValue; }
                        break;
                    case 2:
                        if (designLoad) { ForceMx = forceParameter.DesignValue; } else { ForceMx = forceParameter.CrcValue; }
                        break;
                    case 3:
                        if (designLoad) { ForceMy = forceParameter.DesignValue; } else { ForceMy = forceParameter.CrcValue; }
                        break;
                }
            }
        }
        #endregion

        public Matrix ToMatrix()
        {
            Matrix matrix = new Matrix(3, 1);
            matrix[0, 0] = ForceMx;
            matrix[1, 0] = ForceMy;
            matrix[2, 0] = ForceN;
            return matrix;
        }
    }
}
