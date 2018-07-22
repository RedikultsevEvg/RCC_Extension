using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCC_Extension.BLL.Geometry

{
    public class Point2D : ICloneable
    {
        public decimal Coord_X { get; set; }
        public decimal Coord_Y { get; set; }

        public String PointText()
        {
            return "(" + Convert.ToString(Coord_X) + ";" + Convert.ToString(Coord_Y) + ")";
        }

        public Point2D EndPoint (decimal Angle, decimal Length)
        {
            Point2D EndPoint = new Point2D(0,0);
            EndPoint.Coord_X = this.Coord_X + Convert.ToDecimal(Math.Cos(Convert.ToDouble(Angle))) * Length;
            EndPoint.Coord_Y = this.Coord_Y + Convert.ToDecimal(Math.Sin(Convert.ToDouble(Angle))) * Length;
            return EndPoint;
        }

        public Point2D(decimal coord_X, decimal coord_Y)
        {
            Coord_X = coord_X;
            Coord_Y = coord_Y;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Vector2D
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public decimal GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            return (StartPoint.Coord_X - EndPoint.Coord_X);
                //Math.Sqrt(Convert.ToDouble(StartPoint.coord_X - EndPoint.coord_X));
        }
    }

    public class Geometry2D
    {
        public decimal GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            decimal dX = EndPoint.Coord_X - StartPoint.Coord_X;
            decimal dY = EndPoint.Coord_Y - StartPoint.Coord_Y;
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(dX*dX + dY*dY)));
        }
    }
}
