using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Geometry.Sections
{
    /// <summary>
    /// Класс круглого сечения
    /// </summary>
    public class CircleSection : ICircle
    {
        /// <summary>
        /// Диаметр
        /// </summary>
        public double Diameter { get; set; }
        /// <summary>
        /// Точка центра
        /// </summary>
        public Point2D Center { get; set; }
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public CircleSection()
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
            return Math.PI * Diameter * Diameter / 4;
        }
        /// <summary>
        /// Возвращает периметр фигуры
        /// </summary>
        /// <returns></returns>
        public double GetPerimeter()
        {
            return Math.PI * Diameter;
        }
    }
}
