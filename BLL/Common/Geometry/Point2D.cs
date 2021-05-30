using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;
using System.Xml;
using RDBLL.Common.Interfaces.Geometry.Points;

namespace RDBLL.Common.Geometry

{
    /// <summary>
    /// Класс для точки на плоскости
    /// </summary>
    public class Point2D : IPoint2D, ICloneable
    {
        /// <summary>
        /// Координата X, метры
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Координата Y, метры
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Конструктор точки с координатами 0,0
        /// </summary>
        public Point2D()
        {
            X = 0;
            Y = 0;
        }
        /// <summary>
        /// Конструтор точки с указанными координатами
        /// </summary>
        /// <param name="coord_X"></param>
        /// <param name="coord_Y"></param>
        public Point2D(double coord_X, double coord_Y)
        {
            X = coord_X;
            Y = coord_Y;
        }
        /// <summary>
        /// Клонирование точки
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Vector2D
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public double GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            return (StartPoint.X - EndPoint.X);
                //Math.Sqrt(Convert.ToDouble(StartPoint.coord_X - EndPoint.coord_X));
        }
    }
}
