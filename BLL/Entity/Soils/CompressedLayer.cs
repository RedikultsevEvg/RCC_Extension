using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс сжатого слоя
    /// </summary>
    public class CompressedLayer
    {
        /// <summary>
        /// Ссылка на элементарный слой
        /// </summary>
        public SoilElementaryLayer SoilElementaryLayer { get; set; }
        public double Zlevel { get; set; }
        public double Alpha { get; set; }
        public double SigmZg { get; set; }
        public double SigmZgamma { get; set; }
        public double SigmZp { get; set; }
        /// <summary>
        /// Осадка элементарного слоя
        /// </summary>
        public double LocalSettlement { get; set; }
        /// <summary>
        /// Осадка нарастающим итогом
        /// </summary>
        public double SumSettlement { get; set; }
    }
}
