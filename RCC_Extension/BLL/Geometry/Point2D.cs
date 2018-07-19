using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCC_Extension.BLL.Geometry

{
    public class Point2D
    {
        public decimal coord_X { get; set; }
        public decimal coord_Y { get; set; }

        public Point2D EndPoint (decimal Angle, decimal Length)
        {
            Point2D EndPoint = new Point2D();
            EndPoint.coord_X = this.coord_X + Convert.ToDecimal(Math.Cos(Convert.ToDouble(Angle))) * Length;
            EndPoint.coord_Y = this.coord_Y + Convert.ToDecimal(Math.Sin(Convert.ToDouble(Angle))) * Length;
            return EndPoint;
        }

        public Point2D()
        {
            //Point2D point2D = new Point2D();
        }

        public Point2D(decimal coord_X, decimal coord_Y)
        {
            Point2D point2D = new Point2D();
            point2D.coord_X = coord_X;
            point2D.coord_Y = coord_Y;
        }
    }

    public class Vector2D
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public decimal GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            return (StartPoint.coord_X - EndPoint.coord_X);
                //Math.Sqrt(Convert.ToDouble(StartPoint.coord_X - EndPoint.coord_X));
        }
    }

    public class Geometry2D
    {
        public decimal GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            decimal dX = EndPoint.coord_X - StartPoint.coord_X;
            decimal dY = EndPoint.coord_Y - StartPoint.coord_Y;
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(dX*dX + dY*dY)));
        }
    }
}
