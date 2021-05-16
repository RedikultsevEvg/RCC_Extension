using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Geometry
{
    /// <summary>
    /// Класс точки в трехмерном пространстве
    /// </summary>
    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        /// <summary>
        /// Конструктор точки с нулевыми коорединатами
        /// </summary>
        public Point3D()
        {
            X = Y = Z = 0;
        }
        /// <summary>
        /// Конструктор точки с указанными координатами
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = y;
        }
        /// <summary>
        /// Конструктор точки 3D по точке 2D, координата Z принимается равной нулю
        /// </summary>
        /// <param name="point2D"></param>
        public Point3D(Point2D point2D)
        {
            X = point2D.X;
            Y = point2D.Y;
            Z = 0;
        }
    }
}
