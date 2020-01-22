using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс вида (класса) бетон
    /// </summary>
    public class ConcreteKind
    {
        /// <summary>
        /// Код класса бетона
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
        /// Относительные деформации условных пластических деформаций при сжатии
        /// </summary>
        public double Epsb1Comp { get; set; }
        /// <summary>
        /// Относительные деформации при временном сопротивлении сжатию
        /// </summary>
        public double EpsbMaxComp { get; set; }
        /// <summary>
        /// Относительные деформации при разрушении при сжатии
        /// </summary>
        public double Epsb2Comp { get; set; }
        /// <summary>
        /// Относительные деформации условных пластических деформаций при растяжении
        /// </summary>
        public double Epsb1Tens { get; set; }
        /// <summary>
        /// Относительные деформации при временном сопротивлении растяжению
        /// </summary>
        public double EpsbMaxTens { get; set; }
        /// <summary>
        /// Относительные деформации при разрушении при сжатии
        /// </summary>
        public double Epsb2Tens { get; set; }
        /// <summary>
        /// модуль упругости
        /// </summary>
        public double ElasticModulus { get; set; }
        /// <summary>
        /// Коэффициент Пуассона
        /// </summary>
        public double PoissonRatio { get; set; }
        /// <summary>
        /// Коэффициент ползучести
        /// </summary>
        public double FiBCr { get; set; }
    }
}
