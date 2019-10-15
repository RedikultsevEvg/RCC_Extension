using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry.Mathematic;

namespace RDBLL.Entity.Common.NDM
{
    public class StiffnessCoefficient
    {
        public double[,] Coefficients { get; set; } 
        public StiffnessCoefficient()
        {
            Coefficients = new double[3, 3];
        }
        public StiffnessCoefficient(List<NdmArea> ndmAreas, Curvature curvature = null)
        {
            Coefficients = new double[3, 3];
            double D11 = 0, D12 = 0, D13 = 0, D22 = 0, D23 = 0, D33 = 0;
            double secantModulus;
            foreach (NdmArea ndmArea in ndmAreas)
            {
                if (curvature == null) { secantModulus = ndmArea.ElasticModulus; }
                else
                    {
                        double epsilon = curvature.CurvatureX * ndmArea.CenterX + curvature.CurvatureY * ndmArea.CenterY + curvature.Epsilon;
                        secantModulus = ndmArea.ElasticModulus;
                    }
                #region
                D11 += ndmArea.Area * ndmArea.CenterX * ndmArea.CenterX * secantModulus;
                D12 += ndmArea.Area * ndmArea.CenterX * ndmArea.CenterY * secantModulus;
                D13 += ndmArea.Area * ndmArea.CenterX * secantModulus;
                D22 += ndmArea.Area * ndmArea.CenterY * ndmArea.CenterY * secantModulus;
                D23 += ndmArea.Area * ndmArea.CenterY * secantModulus;
                D33 += ndmArea.Area * secantModulus;
                #endregion
            }
            #region
            this.Coefficients[0, 0] = D11;
            this.Coefficients[0, 1] = D12;
            this.Coefficients[0, 2] = D13;
            this.Coefficients[1, 0] = D12;
            this.Coefficients[1, 1] = D22;
            this.Coefficients[1, 2] = D23;
            this.Coefficients[2, 0] = D13;
            this.Coefficients[2, 1] = D23;
            this.Coefficients[2, 2] = D33;
            #endregion
        }
        /// <summary>
        /// Обращение к массиву коэффициентов по индексу
        /// </summary>
        /// <param name="x">Порядковый номер члена массива по X</param>
        /// <param name="y">Порядковый номер члена массива по Y</param>
        /// <returns></returns>
        public double this[int x, int y]
            {
                get
                {
                    return this.Coefficients[x, y];
                }
                set
                {
                    this.Coefficients[x, y] = value;
                }
            }
        /// <summary>
        /// Получение матрицы из класса коэффициентов
        /// </summary>
        /// <returns></returns>
        public Matrix ToMatrix()
        {
            Matrix matrix = new Matrix(3, 3);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[i, j] = this[i, j];
                }
            }
            return matrix;
        }
    }
}
