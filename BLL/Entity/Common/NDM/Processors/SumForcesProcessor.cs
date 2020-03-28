using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM;

namespace RDBLL.Entity.Common.NDM.Processors
{
    /// <summary>
    /// Процессор суммарных усилий нелинейной деформационной модели
    /// </summary>
    public static class SumForcesProcessor
    {
        /// <summary>
        /// Возвращает среднеквадратичное отклонение суммарного усилия от заданного суммарного усилия
        /// </summary>
        /// <param name="fstSumForces"></param>
        /// <param name="sndSumForces"></param>
        /// <returns></returns>
        public static double GetSigmaAccuracy(SumForces fstSumForces, SumForces sndSumForces)
        {
            double dMX = fstSumForces.ForceMatrix[0, 0] - sndSumForces.ForceMatrix[0, 0];
            double dMy = fstSumForces.ForceMatrix[1, 0] - sndSumForces.ForceMatrix[1, 0];
            double dN = fstSumForces.ForceMatrix[2, 0] - sndSumForces.ForceMatrix[2, 0];
            double accuracy = Math.Sqrt((dMX * dMX + dMy * dMy + dN * dN) / 3);
            return accuracy;
        }
    }
}
