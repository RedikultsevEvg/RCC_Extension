using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс элементарного слоя грунта
    /// </summary>
    public class SoilElementaryLayer :SoilLayer
    {
        /// <summary>
        /// Отметка низа слоя
        /// </summary>
        public double BottomLevel { get; set; }
        /// <summary>
        /// Коэффициент уменьшения напряжений по глубине
        /// </summary>
        public double AlphaCoeff { get; set; }
        /// <summary>
        /// Флаг обводненности грунта
        /// </summary>
        public bool HasGroundWater { get; set; }
    }
}
