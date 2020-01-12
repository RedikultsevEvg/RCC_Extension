using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soil
{
    /// <summary>
    /// Класс скального грунта
    /// </summary>
    public class RockSoil :BearingSoil
    {
        /// <summary>
        /// Нормативное сопротивление
        /// </summary>
        public double CrcStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление по 1-й группе ПС
        /// </summary>
        public double FstDesignStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление по 2-й группе ПС
        /// </summary>
        public double SndDesignStrength { get; set; }
    }
}
