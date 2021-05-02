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
                        Point2D lineCenter = GeometryProc.GetPointOfset(line.StartPoint, line.EndPoint, lineLength / 2);
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

        public static double[] GetMomentOfInertia(PunchingContour contour)
        {
            double Ix = 0;
            double Iy = 0;
            Point2D center = GetContourCenter(contour);
            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                foreach (PunchingLine line in subContour.Lines)
                {
                    //Если линия субконтура несущая, то учитываем ее при подсчете центра тяжести
                    if (line.IsBearing)
                    {
                        //Длина линии
                        double lineLength = GeometryProc.GetDistance(line.StartPoint, line.EndPoint);
                        throw new NotImplementedException();
                    }
                }
            }
            return new double[] { Ix, Iy };
        }
    }
}
