using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors
{
    public static class GeomProcessor
    {
        /// <summary>
        /// Возвращает суммарную длину субконтура
        /// </summary>
        /// <param name="subContour"></param>
        /// <returns></returns>
        public static double GetSubContourLength(PunchingSubContour subContour)
        {
            double length = 0;
            foreach (PunchingLine line in subContour.Lines)
            {
                if (line.IsBearing)
                {
                    length += GeometryProc.GetDistance(line.StartPoint, line.EndPoint);
                }
            }
            return length;
        }
        /// <summary>
        /// Возвращает суммарную высоту контура
        /// </summary>
        /// <param name="contour"></param>
        /// <returns></returns>
        public static double GetContourHeight(PunchingContour contour)
        {
            double totalHeight = 0;
            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                totalHeight += subContour.Height;
            }
            return totalHeight;
        }
        /// <summary>
        /// Возвращает момент сопротивления
        /// </summary>
        /// <param name="contour"></param>
        /// <returns></returns>
        public static double[] GetMomentResistance(PunchingContour contour)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Возвращает центр тяжести субконтура
        /// </summary>
        /// <param name="сontour"></param>
        /// <returns></returns>
        public static Point2D GetContourCenter(PunchingContour сontour)
        {
            //Заданный начальный центр
            Point2D initCenter = new Point2D();
            //Статический момент сопротивления (статический момент площади)
            double statMomentX = 0;
            double statMomentY = 0;
            double sumLineLength = 0;
            //Для каждого субконтура в контуре
            foreach (PunchingSubContour subContour in сontour.SubContours)
            {
                //Для каждой линии в субконтуре
                foreach (PunchingLine line in subContour.Lines)
                {
                    //Если линия субконтура несущая, то учитываем ее при подсчете центра тяжести
                    if (line.IsBearing)
                    {
                        //Длина линии
                        double lineLength = GeometryProc.GetDistance(line.StartPoint, line.EndPoint);
                        //Точка центра линии
                        Point2D lineCenter = GeometryProc.GetMiddlePoint(line.StartPoint, line.EndPoint);
                        //Расстояние от центра линии до заданного центра
                        double dXLoc = lineCenter.X - initCenter.X;
                        double dYLoc = lineCenter.Y - initCenter.Y;
                        //Статический момент площади
                        //Определяется как площадь умноженная на расстояние до центра тяжести
                        statMomentX += lineLength * subContour.Height * dXLoc;
                        statMomentY += lineLength * subContour.Height * dYLoc;
                        //Суммарная площадь всех линий
                        sumLineLength += lineLength * subContour.Height;
                    }
                }
            }
            //Если суммарная площадь по всем линиям равна нулю, то выдаем исключение, что контур не является несущим
            if (sumLineLength == 0) { throw new Exception("Countour isn't bearing"); }
            //Получаем сдвижку центра тяжести расчетного контура относительно заданного начального контура
            double dX = statMomentX / sumLineLength;
            double dY = statMomentX / sumLineLength;
            Point2D newCenter = new Point2D(initCenter.X + dX, initCenter.Y + dY);
            return newCenter;
        }
        /// <summary>
        /// Возвращает суммарный момент инерции расчетного контура с учетом высоты
        /// </summary>
        /// <param name="contour"></param>
        /// <returns></returns>
        public static double[] GetMomentOfInertiaHeight(PunchingContour contour)
        {
            double Ix = 0;
            double Iy = 0;
            Point2D center = GetContourCenter(contour);
            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                foreach (PunchingLine line in subContour.Lines)
                {
                    //Если линия субконтура несущая, то учитываем ее при подсчете момента инерции контура
                    if (line.IsBearing)
                    {
                        double[] moments = GeometryProc.GetLineMomentInertia(line.StartPoint, line.EndPoint, center);
                        Ix += moments[0] * subContour.Height;
                        Iy += moments[1] * subContour.Height;
                    }
                }
            }
            return new double[] { Ix, Iy };
        }
        /// <summary>
        /// Возвращает суммарный момент инерции расчетного субконтура контура
        /// </summary>
        /// <param name="subContour"></param>
        /// <param name="center"></param>
        /// <param name="contour"></param>
        /// <returns></returns>
        public static double[] GetMomentOfInertia(PunchingSubContour subContour, Point2D center)
        {
            double Ix = 0;
            double Iy = 0;
            foreach (PunchingLine line in subContour.Lines)
            {
                //Если линия субконтура несущая, то учитываем ее при подсчете момента инерции контура
                if (line.IsBearing)
                {
                    double[] moments = GeometryProc.GetLineMomentInertia(line.StartPoint, line.EndPoint, center);
                    Ix += moments[0];
                    Iy += moments[1];
                }
            }
            return new double[] { Ix, Iy };
        }
        /// <summary>
        /// Возвращает минимальные и максимальные расстояния от заданного центра до точек линий
        /// </summary>
        /// <param name="lines">Коллекция линий</param>
        /// <param name="center">Заданный центр</param>
        /// <returns></returns>
        public static double[] GetMaxDistFromLineList(List<PunchingLine> lines, Point2D center)
        {
            //Коллекции для хранения координат линий контура
            List<double> coordsX = new List<double>();
            List<double> coordsY = new List<double>();
            foreach (PunchingLine line in lines)
            {
                if (line.IsBearing)
                {
                    coordsX.Add(line.StartPoint.X);
                    coordsX.Add(line.EndPoint.X);
                    coordsY.Add(line.StartPoint.Y);
                    coordsY.Add(line.EndPoint.Y);
                }
            }
            double maxX = coordsX.Max() - center.X;
            double minX = coordsX.Min() - center.X;
            double maxY = coordsY.Max() - center.Y;
            double minY = coordsY.Min() - center.Y;

            return new double[] { maxX, minX, maxY, minY };
        }
        /// <summary>
        /// Возвращает минимальные и максимальные расстояния от заданного центра до точек линий
        /// </summary>
        /// <param name="contour">Расчетный контур</param>
        /// <returns></returns>
        public static double[] GetMaxDistFromContour(PunchingContour contour)
        {
            Point2D center = GetContourCenter(contour);
            //Коллекции для хранения координат линий контура
            List<double> coordsX = new List<double>();
            List<double> coordsY = new List<double>();
            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                double[] coords = GetMaxDistFromLineList(subContour.Lines, center);
                coordsX.Add(coords[0]);
                coordsX.Add(coords[1]);
                coordsY.Add(coords[2]);
                coordsY.Add(coords[3]);
            }
            double maxX = coordsX.Max();
            double minX = coordsX.Min();
            double maxY = coordsY.Max();
            double minY = coordsY.Min();

            return new double[] { maxX, minX, maxY, minY };
        }
        /// <summary>
        /// Возвращает суммарную толщину плиты
        /// </summary>
        /// <param name="punching"></param>
        /// <returns></returns>
        public static double GetTotalHeight(Punching punching)
        {
            double total = 0;
            foreach (PunchingLayer layer in punching.Children)
            {
                total += layer.Height;
            }
            return total;
        }
    }
}
