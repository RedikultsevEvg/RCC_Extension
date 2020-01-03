using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM
{
    /// <summary>
    /// Класс матрицы жесткостных коэффициентов
    /// </summary>
    public class StiffnessCoefficient
    { 
        /// <summary>
        /// Матрица жесткостных коэффициентов
        /// </summary>
        public Matrix StifMatrix { get; set; }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public StiffnessCoefficient()
        {
            StifMatrix = new Matrix(3,3);
        }
        /// <summary>
        /// Конструктор матрицы жесткостных коэффициентов по коллекции элементарных участков и заданной кривизне
        /// Если кривизна не задана, то жесткостные коэффициенты определяются в соответствии с начальным модулем упругости
        /// это нужно на первом этапе расчета, когда кривизна еще не известна даже в первом приближении
        /// </summary>
        /// <param name="ndmAreas"></param>
        /// <param name="curvature"></param>
        public StiffnessCoefficient(List<NdmArea> ndmAreas, Curvature curvature = null)
        {
            StifMatrix = new Matrix(3, 3);
            List<double> secantMods = new List<double>();
            double D11 = 0, D12 = 0, D13 = 0, D22 = 0, D23 = 0, D33 = 0;
            double secantModulus;
            foreach (NdmArea ndmArea in ndmAreas)
            {
                //Если кривизна не указана, то принимаем жесткостные характеристики в соответствии с начальным модулем упругости
                if (curvature == null) { secantModulus = ndmArea.MaterialModel.ElasticModulus; }
                else //Иначе если кривизна указана
                    {
                    //Определяем деформации по указанной кривизне
                    double strain = curvature.CurvMatrix[0,0] * ndmArea.CenterY + curvature.CurvMatrix[1, 0] * ndmArea.CenterX + curvature.CurvMatrix[2, 0];
                    //По полученной деформации определяем секущий модуль упругости
                    secantModulus = ndmArea.GetSecantModulus(strain);
                    //Секущий модуль упругости на каждом подшаге (для отладки)
                    secantMods.Add(secantModulus);
                    }
                #region
                D11 += ndmArea.Area * ndmArea.CenterY * ndmArea.CenterY * secantModulus;
                D12 += ndmArea.Area * ndmArea.CenterX * ndmArea.CenterY * secantModulus;
                D13 += ndmArea.Area * ndmArea.CenterY * secantModulus;
                D22 += ndmArea.Area * ndmArea.CenterX * ndmArea.CenterX * secantModulus;
                D23 += ndmArea.Area * ndmArea.CenterX * secantModulus;
                D33 += ndmArea.Area * secantModulus;
                #endregion                
                #region по СП
                //Вариант, который должен быть по СП
                //D11 += ndmArea.Area * ndmArea.CenterX * ndmArea.CenterX * secantModulus;
                //D12 += ndmArea.Area * ndmArea.CenterX * ndmArea.CenterY * secantModulus;
                //D13 += ndmArea.Area * ndmArea.CenterX * secantModulus;
                //D22 += ndmArea.Area * ndmArea.CenterY * ndmArea.CenterY * secantModulus;
                //D23 += ndmArea.Area * ndmArea.CenterY * secantModulus;
                //D33 += ndmArea.Area * secantModulus;
                #endregion
            }
            #region
            StifMatrix[0, 0] = D11;
            //В СП даны коэффициенты для момента в плоскоскости X
            //Для учета моменто относительно оси, меняем коэффициенты местами
            //StifMatrix[0, 0] = D22;
            StifMatrix[0, 1] = D12;
            StifMatrix[0, 2] = D13;
            StifMatrix[1, 0] = D12;
            StifMatrix[1, 1] = D22;
            //StifMatrix[1, 1] = D11;
            StifMatrix[1, 2] = D23;
            StifMatrix[2, 0] = D13;
            StifMatrix[2, 1] = D23;
            StifMatrix[2, 2] = D33;
            #endregion
        }
    }
}
