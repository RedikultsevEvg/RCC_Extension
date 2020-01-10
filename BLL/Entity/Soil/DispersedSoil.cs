using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soil
{
    /// <summary>
    /// Класс дисперсного грунта
    /// </summary>
    public class DispersedSoil :Soil
    {
        public override double[] GetSoilResistance()
        {
            double[] resistance = new double[2];
            return resistance;
        }
    }
}
