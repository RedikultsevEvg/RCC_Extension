using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry.Mathematic;

namespace RDBLL.Entity.Common.NDM
{
    public class Curvature
    {
        public Matrix CurvMatrix { get; set; }
        #region Constructors
        public Curvature()
        {
            Matrix CurvMatrix = new Matrix(3, 1);
        }
        public Curvature(SumForces sumForces, StiffnessCoefficient stifMatrix)
        {
            CurvMatrix = stifMatrix.StifMatrix.CreateInvertibleMatrix() * sumForces.ForceMatrix;
        }
        #endregion
    }
}
