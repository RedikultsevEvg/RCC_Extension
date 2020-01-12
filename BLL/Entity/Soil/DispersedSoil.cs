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
    public class DispersedSoil :BearingSoil
    {
        /// <summary>
        /// Нормативное значение угла внутреннего трения
        /// </summary>
        public double CrcFi { get; set; }
        /// <summary>
        /// Расчетное значение угла внутреннего трения для 1-й группы ПС
        /// </summary>
        public double FstDesignFi { get; set; }
        /// <summary>
        /// Расчетное значение угла внутреннего трения для 2-й группы ПС
        /// </summary>
        public double SndDesignFi { get; set; }
        /// <summary>
        /// Нормативное значение сцепления, Па
        /// </summary>
        public double CrcCohesion { get; set; }
        /// <summary>
        /// Расчетное значение сцепления для 1-й группы ПС
        /// </summary>
        public double FstDesigncCohesion { get; set; }
        /// <summary>
        /// Расчетное значение сцепления для 2-й группы ПС
        /// </summary>
        public double SndDesigncCohesion { get; set; }
    }
}
