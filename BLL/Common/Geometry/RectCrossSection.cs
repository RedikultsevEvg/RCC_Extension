using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Geometry
{
    /// <summary>
    /// Класс прямоугольного поперечного сечения
    /// </summary>
    public class RectCrossSection : IRectangle
    {
        /// <summary>
        /// Ширина сечения
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина сечения
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Точка центра
        /// </summary>
        public Point2D Center { get; set; }

        /// <summary>
        /// Конструктор по длине, ширине и точке центра
        /// </summary>
        /// <param name="width"></param>
        /// <param name="length"></param>
        /// <param name="center"></param>
        public RectCrossSection(double width, double length, Point2D center = null)
        {
            if (center is null) center = new Point2D();
            Width = width;
            Length = length;
            Center = center;
        }
        /// <summary>
        /// Возвращает площадь
        /// </summary>
        /// <returns></returns>
        public double GetArea()
        {
            return Width * Length;
        }
        /// <summary>
        /// Возвращает периметр
        /// </summary>
        /// <returns></returns>
        public double GetPerimeter()
        {
            return (Width + Length) * 2;
        }
    }

    //public static class RectProcessor
    //{
    //    public static MassProperty GetRectMassProperty(RectCrossSection rect)
    //    {
    //        MassProperty massProperty = new MassProperty();
    //        massProperty.A = rect.Width * rect.Length;
    //        massProperty.Wx = rect.Width * Math.Pow(rect.Length, 2) / 6;
    //        massProperty.Wy = rect.Length * Math.Pow(rect.Width, 2) / 6;
    //        massProperty.Ix = rect.Width * Math.Pow(rect.Length, 3) / 12;
    //        massProperty.Iy = rect.Length * Math.Pow(rect.Width, 3) / 12;
    //        massProperty.Xmax = rect.Width / 2;
    //        massProperty.Ymax = rect.Length / 2;
    //        return massProperty;
    //    }
    //}
}
