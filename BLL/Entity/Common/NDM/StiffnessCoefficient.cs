using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM
{
    public class StiffnessCoefficient
    { 
        public Matrix StifMatrix { get; set; }


        public StiffnessCoefficient()
        {
            StifMatrix = new Matrix(3,3);
        }
        public StiffnessCoefficient(List<NdmArea> ndmAreas, Curvature curvature = null)
        {
            StifMatrix = new Matrix(3, 3);
            List<double> secantMods = new List<double>();
            double D11 = 0, D12 = 0, D13 = 0, D22 = 0, D23 = 0, D33 = 0;
            double secantModulus;
            foreach (NdmArea ndmArea in ndmAreas)
            {
                if (curvature == null) { secantModulus = ndmArea.ElasticModulus; }
                else
                    {
                        double epsilon = curvature.CurvMatrix[0,0] * ndmArea.CenterX + curvature.CurvMatrix[1, 0] * ndmArea.CenterY + curvature.CurvMatrix[2, 0];
                    
                        secantModulus = ndmArea.GetSecantModulus(epsilon);
                        secantMods.Add(secantModulus);
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
            //StifMatrix[0, 0] = D11;  В СП даны коэффициенты для момента в плоскоскости X
            //Для учета моменто относительно оси, меняем коэффициенты местами
            StifMatrix[0, 0] = D22;
            StifMatrix[0, 1] = D12;
            StifMatrix[0, 2] = D13;
            StifMatrix[1, 0] = D12;
            //StifMatrix[1, 1] = D22;
            StifMatrix[1, 1] = D11;
            StifMatrix[1, 2] = D23;
            StifMatrix[2, 0] = D13;
            StifMatrix[2, 1] = D23;
            StifMatrix[2, 2] = D33;
            #endregion
        }
    }
}
