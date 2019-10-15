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
        public double CurvatureX { get; set; }
        public double CurvatureY { get; set; }
        public double Epsilon { get; set; }
        #region Constructors
        public Curvature()
        {
        }
        public Curvature(SumForces sumForces, StiffnessCoefficient stifCoef)
        {
            Matrix StifMatrix = stifCoef.ToMatrix();
            Matrix InvertStifMatrix = StifMatrix.CreateInvertibleMatrix();
            Matrix ForceMatrix = sumForces.ToMatrix();
            Matrix CurvMatrix = InvertStifMatrix * ForceMatrix;
            CurvatureX = CurvMatrix[0, 0];
            CurvatureY = CurvMatrix[1, 0];
            Epsilon = CurvMatrix[2, 0];
        }
        #endregion

        /// <summary>
        /// Получение матрицы кривизны
        /// </summary>
        /// <returns></returns>
        public Matrix ToMatrix()
        {
            Matrix matrix = new Matrix(3, 1);
            matrix[0, 0] = CurvatureX;
            matrix[1, 0] = CurvatureY;
            matrix[2, 0] = Epsilon;
            return matrix;
        }
    }
}
