using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Geometry.Sections
{
    /// <summary>
    /// Класс прямоугольного сечения
    /// </summary>
    public class RectangleSection : IRectangle
    {
        /// <summary>
        /// Ширина сечения
        /// </summary>
        public double Width {get; set; }
        /// <summary>
        /// Длина сечения
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Точка центра
        /// </summary>
        public Point2D Center { get; set; }
        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public RectangleSection()
        {
            Center = new Point2D();
        }
        #endregion
        /// <summary>
        /// Возвращает площадь фигуры
        /// </summary>
        /// <returns></returns>
        public double GetArea()
        {
            return Width * Length;
        }
        /// <summary>
        /// Возвращает периметр фигуры
        /// </summary>
        /// <returns></returns>
        public double GetPerimeter()
        {
            return (Width + Length) * 2;
        }
        
    }
}
