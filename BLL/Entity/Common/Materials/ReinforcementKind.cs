using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Reinforcement class
    /// </summary>
    public class ReinforcementKind
    {
        /// <summary>
        /// Код класс
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование класса
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Нормативное сопротивление сжатию
        /// </summary>
        public double CrcCompStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление сжатию для 1-й группы предельных состояний
        /// </summary>
        public double FstCompStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление сжатию для 2-й группы предельных состояний
        /// </summary>
        public double SndCompStrength { get; set; }
        /// <summary>
        /// Нормативное сопротивление растяжению
        /// </summary>
        public double CrcTensStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление растяжению для 1-й группы предельных состояний
        /// </summary>
        public double FstTensStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление растяжению для 2-й группы предельных состояний
        /// </summary>
        public double SndTensStrength { get; set; }
        /// <summary>
        /// Относительные деформации при разрушении при сжатии
        /// </summary>
        public double Eps2Comp { get; set; }
        /// <summary>
        /// Относительные деформации при разрушении при сжатии
        /// </summary>
        public double Eps2Tens { get; set; }
        /// <summary>
        /// модуль упругости
        /// </summary>
        public double ElasticModulus { get; set; }
    }
}
