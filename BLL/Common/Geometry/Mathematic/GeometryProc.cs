using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;

namespace RDBLL.Common.Geometry.Mathematic
{
    public static class GeometryProc
    { 
        public static double GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            double dX = EndPoint.X - StartPoint.X;
            double dY = EndPoint.Y - StartPoint.Y;
            return Convert.ToDouble(Math.Sqrt(Convert.ToDouble(dX * dX + dY * dY)));
        }

        public static Point2D GetPointOfset(Point2D point, double angle, double dist)
        {
            return new Point2D(point.X + Math.Cos(angle) * dist, point.Y + Math.Sin(angle) * dist);
        }

        public static Point2D GetPointOfset(Point2D startPoint, Point2D endPoint, double dist)
        {
            double angle = Math.Atan2((endPoint.Y - startPoint.Y), (endPoint.X - startPoint.X)); 
            return GetPointOfset(startPoint, angle, dist);
        }
    }
}
