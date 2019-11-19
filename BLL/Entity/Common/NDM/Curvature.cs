using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry.Mathematic;

namespace RDBLL.Entity.Common.NDM
{
    /// <summary>
    /// Класс набора кривизны
    /// </summary>
    public class Curvature
    {
        /// <summary>
        /// Матрица хранения кривизн
        /// Матрица-cтолбец 3x1
        /// </summary>
        public Matrix CurvMatrix { get; set; }
        #region Constructors
        /// <summary>
        /// Конструкто по умолчанию, создает пустую матрицу
        /// </summary>
        public Curvature()
        {
            Matrix CurvMatrix = new Matrix(3, 1);
        }
        /// <summary>
        /// Возвращает кривизну по известным суммарным усилиям и матрице жесткостей
        /// </summary>
        /// <param name="sumForces"></param>
        /// <param name="stifMatrix"></param>
        public Curvature(SumForces sumForces, StiffnessCoefficient stifMatrix)
        {
            CurvMatrix = stifMatrix.StifMatrix.CreateInvertibleMatrix() * sumForces.ForceMatrix;
        }
        #endregion
    }
}
