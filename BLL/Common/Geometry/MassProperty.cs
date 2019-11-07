using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Geometry
{
    /// <summary>
    /// Класс геометрических характеристик для сечения
    /// </summary>
    public class MassProperty
    {
        /// <summary>
        /// Площадь сечения
        /// </summary>
        public double A { get; set; }
        /// <summary>
        /// Момент сопротивления относительно оси X
        /// </summary>
        public double Wx { get; set; }
        /// <summary>
        /// Момент сопротивления относительно оси Y
        /// </summary>
        public double Wy { get; set; }
        /// <summary>
        /// Момент инерции относительно оси X
        /// </summary>
        public double Ix { get; set; }
        /// <summary>
        /// Момент инерции относительно оси Y
        /// </summary>
        public double Iy { get; set; }
        /// <summary>
        /// Максимальное расстояние вдоль оси X
        /// </summary>
        public double Xmax { get; set; }
        /// <summary>
        /// Максимальное расстояние вдоль оси Y
        /// </summary>
        public double Ymax { get; set; }
    }
}
