using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soil
{
    /// <summary>
    /// Абстрактный класс грунта
    /// </summary>
    public abstract class Soil
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ElasticModulus { get; set; }
        public double PoissonRatio { get; set; }

        /// <summary>
        /// Получение расчетного сопротивления грунта
        /// для 1-й и 2-й группы предельных состояний
        /// </summary>
        /// <returns></returns>
        public abstract double[] GetSoilResistance();
    }
}
