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
        /// <summary>
        /// Возвращает коллекцию точек, с учетом заданного количества внутренних точек
        /// </summary>
        /// <param name="startPoint">Начальная точка</param>
        /// <param name="endPoint">Конечная точка</param>
        /// <param name="quant">Количество внутренних точек</param>
        /// <param name="addStart">Флаг добавления начальной точки</param>
        /// <param name="addEnd">Флаг добавления конечной точки</param>
        /// <returns></returns>
        public static List<Point2D> GetInternalPoints(Point2D startPoint, Point2D endPoint, int quant, bool addStart, bool addEnd)
        {
            List<Point2D> points = new List<Point2D>();
            if (addStart) points.Add(startPoint.Clone() as Point2D);
            double length = GetDistance(startPoint, endPoint);
            double spacing = length / (quant + 1);
            for (int i = 1; i <= quant; i++)
            {
                points.Add(GeometryProc.GetPointOfset(startPoint, endPoint, spacing * i));
            }
            if (addEnd) points.Add(endPoint.Clone() as Point2D);
            return points;
        }
        public static List<Point2D> GetInternalPoints(Point2D startPoint, Point2D endPoint, double maxSpacing, bool addStart, bool addEnd)
        {
            double length = GetDistance(startPoint, endPoint);
            int quant = Convert.ToInt32(Math.Ceiling(length / maxSpacing)) - 1;
            return GetInternalPoints(startPoint, endPoint, quant, addStart, addEnd);
        }
        public static List<Point2D> GetRectArrayPoints(Point2D center, double sizeX, double sizeY, int quantityX, int quantityY, bool fillArray)
        {
            List<Point2D> points = new List<Point2D>();
            double bottomPointY = center.Y - sizeY / 2;
            double topPointY = center.Y + sizeY / 2;
            double leftPointX = center.X - sizeX / 2;
            double rightPointX = center.X + sizeX / 2;
            Point2D bottomLeft = new Point2D(leftPointX, bottomPointY);
            Point2D bottomRight = new Point2D(rightPointX, bottomPointY);
            Point2D topLeft = new Point2D(leftPointX, topPointY);
            Point2D topRight = new Point2D(rightPointX, topPointY);
            List<Point2D> pointsLeftY = GeometryProc.GetInternalPoints(bottomLeft, topLeft, quantityY - 2, false, false);
            List<Point2D> pointsRightY = GeometryProc.GetInternalPoints(bottomRight, topRight, quantityY - 2, false, false);
            points.AddRange(GeometryProc.GetInternalPoints(bottomLeft, bottomRight, quantityX - 2, true, true));
            for (int i = 0; i < pointsLeftY.Count(); i++)
            {
                points.Add(pointsLeftY[i]);
                if (fillArray)
                {
                    points.AddRange(GeometryProc.GetInternalPoints(pointsLeftY[i], pointsRightY[i], quantityX - 2, false, false));
                }
                points.Add(pointsRightY[i]);
            }
            points.AddRange(GeometryProc.GetInternalPoints(topLeft, topRight, quantityX - 2, true, true));
            return points;
        }
    }
}
