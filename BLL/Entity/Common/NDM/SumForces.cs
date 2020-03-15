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
        /// <summary>
        /// Конструктор по трем усилиям
        /// </summary>
        /// <param name="Mx">Mx</param>
        /// <param name="My">My</param>
        /// <param name="N">N</param>
        public SumForces(double Mx, double My, double N)
        {
            ForceMatrix = new Matrix(3, 1);
            ForceMatrix[0, 0] = Mx;
            ForceMatrix[1, 0] = My;
            ForceMatrix[2, 0] = N;
        }
        /// <summary>
        /// Конструктор по исходной матрице усилий и координатом точки, к которой приводится новая матрица
        /// </summary>
        /// <param name="initSumForces">Исходная матрица</param>
        /// <param name="dX">координата X новой точки</param>
        /// <param name="dY">координата Y новой точки</param>
        public SumForces(SumForces initSumForces, double dX, double dY)
        {
            ForceMatrix = new Matrix(3, 1);
            ForceMatrix[0, 0] = initSumForces.ForceMatrix[0, 0] - initSumForces.ForceMatrix[2, 0] * dY;
            ForceMatrix[1, 0] = initSumForces.ForceMatrix[1, 0] + initSumForces.ForceMatrix[2, 0] * dX; ;
            ForceMatrix[2, 0] = initSumForces.ForceMatrix[2, 0];
        }
        #endregion
    }
}
