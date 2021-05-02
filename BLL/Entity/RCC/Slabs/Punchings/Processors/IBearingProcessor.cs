using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors
{
    public interface IBearingProcessor
    {
        /// <summary>
        /// Возвращает коэффициент несущей способности по заданным усилиям для заданного контура
        /// </summary>
        /// <param name="contour"></param>
        /// <param name="force"></param>
        /// <param name="moment"></param>
        /// <returns></returns>
        double GetBearindCapacityCoefficient(PunchingContour contour, double force, double Mx, double My, bool fullLoad = true);
    }
}
