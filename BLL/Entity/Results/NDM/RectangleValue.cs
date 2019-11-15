using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Results.NDM
{
    /// <summary>
    /// Прямоугольный элемент со значением
    /// </summary>
    public class RectangleValue
    {
        /// <summary>
        /// Центр прямоугольника вдоль оси X
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Центр прямоугольника вдоль оси Y
        /// </summary>
        public double CenterY { get; set; }
        /// <summary>
        /// Ширина прямоугольника (вдоль оси X)
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина прямоугольника (вдоль оси Y)
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public double Value { get; set; }
    }
}
