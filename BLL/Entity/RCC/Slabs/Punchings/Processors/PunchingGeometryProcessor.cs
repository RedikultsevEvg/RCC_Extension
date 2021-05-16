using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors.Offsets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors
{
    public static class PunchingGeometryProcessor
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
                    length += GeometryProcessor.GetDistance(line.StartPoint, line.EndPoint);
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
        /// Возвращает центр тяжести контура
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
                        double lineLength = GeometryProcessor.GetDistance(line.StartPoint, line.EndPoint);
                        //Точка центра линии
                        Point2D lineCenter = GeometryProcessor.GetMiddlePoint(line.StartPoint, line.EndPoint);
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
            double dY = statMomentY / sumLineLength;
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
            //моммент инерции относительно осей X и Y
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
                        double[] moments = GeometryProcessor.GetLineMomentInertia(line.StartPoint, line.EndPoint, center);

                        double bearingCoef = GetLineWorkCoef(line, subContour.Height);
                        //моммент инерции относительно осей X и Y
                        Ix += moments[0] * subContour.Height * bearingCoef;
                        Iy += moments[1] * subContour.Height * bearingCoef;
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
                    double[] moments = GeometryProcessor.GetLineMomentInertia(line.StartPoint, line.EndPoint, center);
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

        internal static double GetSubContourArea(PunchingSubContour subContour)
        {
            double area = 0;
            foreach (PunchingLine line in subContour.Lines)
            {
                if (line.IsBearing)
                {
                    area += GeometryProcessor.GetDistance(line.StartPoint, line.EndPoint) * GetLineWorkCoef(line, subContour.Height) * subContour.Height;
                }
            }
            return area;
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
        /// <summary>
        /// Проверяет верно ли задано расположение краев у продавливания
        /// </summary>
        /// <param name="punching"></param>
        /// <returns></returns>
        public static bool CheckPunchingEdges(Punching punching)
        {
            //Если свободные края слева и справа, то получается перекрытие только с двух сторон
            //такое закрепление является неверным (перекрытие не может считаться на продавливание и должно считаться как балка
            //Если свободные края сверху и снизу, то закрепление также является неверным
            if ((punching.LeftEdge & punching.RightEdge) || (punching.TopEdge & punching.BottomEdge)) { return false; }
            //Иначе возвращаем, что края назначены верно
            else return true;
        }
        /// <summary>
        /// Возвращает прямоугольный субконтур с учетом отступов
        /// </summary>
        /// <param name="rectangle">Исходный прямоугольник</param>
        /// <param name="height">Высота субконтура</param>
        /// <param name="concrete">Бетон субконтура</param>
        /// <param name="offsetGroup">Коллекция отступов</param>
        /// <returns></returns>
        public static PunchingSubContour GetRectSubContour(IRectangle rectangle, double height, ConcreteUsing concrete, RectOffsetGroup offsetGroup = null)
        {
            #region Величина отступов
            double leftSize, rightSize, topSize, bottomSize;
            double leftCoef, rightCoef, topCoef, bottomCoef;
            //Если группа отступов не задана, считаем, что отступов нет
            if (offsetGroup is null)
            {
                leftSize = rightSize = topSize = bottomSize = height / 2;
                leftCoef = rightCoef = topCoef = bottomCoef = 2;
            }
            else
            {
                leftSize = offsetGroup.LeftOffset.Size;
                rightSize = offsetGroup.RightOffset.Size;
                topSize = offsetGroup.TopOffset.Size;
                bottomSize = offsetGroup.BottomOffset.Size;

                leftCoef = offsetGroup.LeftOffset.IsBearing ? 2 : 1;
                rightCoef = offsetGroup.RightOffset.IsBearing ? 2 : 1;
                topCoef = offsetGroup.TopOffset.IsBearing ? 2 : 1;
                bottomCoef = offsetGroup.BottomOffset.IsBearing ? 2 : 1;
            }
            double[] offsets = new double[4] { leftSize, rightSize, topSize, bottomSize };
            #endregion
            //Получаем прямоугольное сечение с учетом отступов
            IRectangle rect = GeometryProcessor.GetRectangleOffset(rectangle, offsets);
            //Получаем коллекцию угловых точек прямоугольного сечения
            List<Point2D> anglePoints = GeometryProcessor.GetAnglePointsFromRectangle(rect);
            //Создаем новый субконтур
            PunchingSubContour subContour = new PunchingSubContour();
            //Высота субконтура
            subContour.Height = height;
            //Бетон субконтура
            subContour.Concrete = concrete;
            //Линии субконтура
            #region left line
            PunchingLine leftLine;
            leftLine = new PunchingLine();
            leftLine.HorizontalProjection = leftSize * leftCoef;
            leftLine.IsBearing = offsetGroup.LeftOffset.IsBearing;
            leftLine.StartPoint = anglePoints[0];
            leftLine.EndPoint = anglePoints[1];
            subContour.Lines.Add(leftLine);
            #endregion
            #region right line
            PunchingLine rightLine;
            rightLine = new PunchingLine();
            rightLine.HorizontalProjection = rightSize * rightCoef;
            rightLine.IsBearing = offsetGroup.RightOffset.IsBearing;
            rightLine.StartPoint = anglePoints[2];
            rightLine.EndPoint = anglePoints[3];
            subContour.Lines.Add(rightLine);
            #endregion
            #region top line
            PunchingLine topLine;
            topLine = new PunchingLine();
            topLine.HorizontalProjection = topSize * topCoef;
            topLine.IsBearing = offsetGroup.TopOffset.IsBearing;
            topLine.StartPoint = anglePoints[1];
            topLine.EndPoint = anglePoints[3];
            subContour.Lines.Add(topLine);
            #endregion
            #region bottom line
            PunchingLine bottomLine;
            bottomLine = new PunchingLine();
            bottomLine.HorizontalProjection = bottomSize * bottomCoef;
            bottomLine.IsBearing = offsetGroup.BottomOffset.IsBearing;
            bottomLine.StartPoint = anglePoints[0];
            bottomLine.EndPoint = anglePoints[2];
            subContour.Lines.Add(bottomLine);
            #endregion
            return subContour;
        }
        /// <summary>
        /// Возвращает коллекцию слоев с истинной высотой (за вычетом защитных слоев)
        /// </summary>
        /// <param name="punching"></param>
        /// <returns></returns>
        public static List<PunchingLayer> GetTrueLayers(Punching punching)
        {
            List<PunchingLayer> layers = new List<PunchingLayer>();
            List<PunchingLayer> tmpPunchingLayers = new List<PunchingLayer>();
            foreach (IHasParent child in punching.Children)
            {
                if (child is PunchingLayer) { tmpPunchingLayers.Add(child as PunchingLayer); }
            }
            //Величина защитного слоя определяется как среднее значение величи вдоль осей X и Y
            double coveringLayer = (punching.CoveringLayerX + punching.CoveringLayerY) / 2;
            //Создаем самый верхний слой
            PunchingLayer fstLayer = tmpPunchingLayers[0].Clone() as PunchingLayer;
            fstLayer.Height -= coveringLayer;
            int count = tmpPunchingLayers.Count();
            layers.Add(fstLayer);
            for (int i=1; i<count; i++)
            {
                layers.Add(tmpPunchingLayers[0].Clone() as PunchingLayer);
            }
            return layers;
        }
        /// <summary>
        /// Меняет порядок слоев в коллекции (если слои были введены сверху вниз, то возвращает снизу вверх)
        /// </summary>
        /// <param name="startLayers"></param>
        /// <returns></returns>
        public static List<PunchingLayer> GetInvertLayers (List<PunchingLayer> startLayers)
        {
            List<PunchingLayer> layers = new List<PunchingLayer>();
            int count = startLayers.Count();
            for (int i = 1; i == count; i++)
            {
                layers.Add(startLayers[count - i].Clone() as PunchingLayer);
            }
            return layers;
        }
        /// <summary>
        /// Возвращает суммарную толщину слоев
        /// </summary>
        /// <param name="layers"></param>
        /// <returns></returns>
        public static double GetTotalLayerHeight(List<PunchingLayer> layers)
        {
            double total = 0;
            foreach (PunchingLayer layer in layers)
            {
                total += layer.Height;
            }
            return total;
        }
        private static double GetLineWorkCoef(PunchingLine line, double height)
        {
            //Коэффициент несущей способности линии, зависящий от соотношения горизонтальной проекции линии и высоты контура
            //Чем меньше горизонтальная проекция, тем выше несущая способность
            double bearingCoef;
            //При соотношении 0.4 достигается максимум несущей способности
            if (line.HorizontalProjection < height * 0.4) { bearingCoef = 2.5; }
            else { bearingCoef = height / line.HorizontalProjection; }
            return bearingCoef;
        }
    }
}
