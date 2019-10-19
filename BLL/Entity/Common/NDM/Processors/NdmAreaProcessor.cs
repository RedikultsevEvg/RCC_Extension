using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM;

namespace RDBLL.Entity.Common.NDM.Processors
{
    public static class NdmAreaProcessor
    {
        /// <summary>
        /// Возвращает массив пары деформации-напряжения по элементарному участку и кривизне
        /// </summary>
        /// <param name="ndmArea"></param>
        /// <param name="curvature"></param>
        /// <returns></returns>
        public static double[] GetStrainFromCuvature(NdmArea ndmArea, Curvature curvature)
        {
            double strain = ndmArea.CenterX * curvature.CurvMatrix[0, 0];
            strain += ndmArea.CenterY * curvature.CurvMatrix[1, 0];
            strain += curvature.CurvMatrix[2, 0];
            double stress = ndmArea.GetSecantModulus(strain);
            return new double[2] { strain, stress }; 
        }
    }
}
