using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces.Shapes;

namespace RDBLL.Common.Geometry.Mathematic
{
    /// <summary>
    /// Класс геометрических операций
    /// </summary>
    public static class GeometryProcessor
    {
        /// <summary>
        /// Получение расстояния между двумя точками
        /// </summary>
        /// <param name="StartPoint">Начальная точка</param>
        /// <param name="EndPoint">Конечная точка</param>
        /// <returns></returns>
        public static double GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            double dX = EndPoint.X - StartPoint.X;
            double dY = EndPoint.Y - StartPoint.Y;
            return Convert.ToDouble(Math.Sqrt(Convert.ToDouble(dX * dX + dY * dY)));
        }
        /// <summary>
        /// Возвращает длину линии
        /// </summary>
        /// <param name="line">Линия</param>
        /// <returns></returns>
        public static double GetLineLength(ILine2D line)
        {
            return GetDistance(line.StartPoint, line.EndPoint);
        }
        /// <summary>
        /// Возвращает минимальные и максимальные координаты вершин по коллекции линий
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static double[] GetTotalSizesOfLines(List<ILine2D> lines)
        {
            double maxX, minX, maxY, minY;
            maxX = minX = maxY = minY = 0;
            //Максимальные координаты по X
            maxX = lines.Max(x =>
            {
                var res = Math.Max(x.StartPoint.X, x.EndPoint.X);
                return res;
            });
            //Минимальные координаты по X
            minX = lines.Min(x =>
            {
                var res = Math.Min(x.StartPoint.X, x.EndPoint.X);
                return res;
            });
            //Максимальные координаты по Y
            maxY = lines.Max(x =>
            {
                var res = Math.Max(x.StartPoint.Y, x.EndPoint.Y);
                return res;
            });
            //Минимальные координаты по Y
            minY = lines.Min(x =>
            {
                var res = Math.Min(x.StartPoint.Y, x.EndPoint.Y);
                return res;
            });
            return new double[4] { maxX, minX, maxY, minY };
        }
        /// <summary>
        /// Возвращает значение угла между осью X и заданным отрезком
        /// </summary>
        /// <param name="startPoint">Начальная точка отрезка</param>
        /// <param name="endPoint">Конечная точка отрезка</param>
        /// <returns>Значение угла в радианах</returns>
        public static double GetAngle(Point2D startPoint, Point2D endPoint)
        {
            double dX = endPoint.X - startPoint.X;
            double dY = endPoint.Y - startPoint.Y;
            return Math.Atan2(dY, dX);
        }
        /// <summary>
        /// Возвращает новую точку
        /// </summary>
        /// <param name="point">Начальная точка</param>
        /// <param name="angle">Угол вдоль которого строится новая точка</param>
        /// <param name="dist">Расстояние, на котором строится новая точка</param>
        /// <returns></returns>
        public static Point2D GetPointOfset(Point2D point, double angle, double dist)
        {
            return new Point2D(point.X + Math.Cos(angle) * dist, point.Y + Math.Sin(angle) * dist);
        }
        /// <summary>
        /// Возвращает новую точку как центр отрезка между заданными точками
        /// </summary>
        /// <param name="startPoint">Начальная точка</param>
        /// <param name="endPoint">Конечная точка</param>
        /// <returns></returns>
        public static Point2D GetMiddlePoint(Point2D startPoint, Point2D endPoint)
        {
            //Расстояние между точками
            double lineLength = GetDistance(startPoint, endPoint);
            //Точка центра линии
            Point2D lineCenter = GetPointOfset(startPoint, endPoint, lineLength / 2);
            return lineCenter;
        }
        /// <summary>
        /// Возвращает новую точку
        /// </summary>
        /// <param name="startPoint">Начальная точка</param>
        /// <param name="endPoint">Конечная точка</param>
        /// <param name="dist">Расстояние от начальной точки</param>
        /// <returns></returns>
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
                points.Add(GeometryProcessor.GetPointOfset(startPoint, endPoint, spacing * i));
            }
            if (addEnd) points.Add(endPoint.Clone() as Point2D);
            return points;
        }
        /// <summary>
        /// Возвращает коллекцию точек между существующими
        /// </summary>
        /// <param name="startPoint">Начальная точка</param>
        /// <param name="endPoint">Конечная точка</param>
        /// <param name="maxSpacing">Максимальное расстояние между точками</param>
        /// <param name="addStart">Флаг добавления начальной точки</param>
        /// <param name="addEnd">Флаг добавления конечной точки</param>
        /// <returns></returns>
        public static List<Point2D> GetInternalPoints(Point2D startPoint, Point2D endPoint, double maxSpacing, bool addStart, bool addEnd)
        {
            double length = GetDistance(startPoint, endPoint);
            int quant = Convert.ToInt32(Math.Ceiling(length / maxSpacing)) - 1;
            return GetInternalPoints(startPoint, endPoint, quant, addStart, addEnd);
        }
        /// <summary>
        /// Возвращает коллекцию точек внутри прямоугольного массива
        /// </summary>
        /// <param name="center">Центр массива</param>
        /// <param name="sizeX">Размер массива по X</param>
        /// <param name="sizeY">Размер массива по Y</param>
        /// <param name="quantityX">Количество по X</param>
        /// <param name="quantityY">Количество по Y</param>
        /// <param name="fillArray">Флаг заполнения массива</param>
        /// <returns></returns>
        public static List<Point2D> GetRectArrayPoints(Point2D center, double sizeX, double sizeY, int quantityX, int quantityY, bool fillArray)
        {
            List<Point2D> points = new List<Point2D>();
            double bottomPointY = center.Y - sizeY / 2;
            double topPointY = center.Y + sizeY / 2;
            double leftPointX = center.X - sizeX / 2;
            double rightPointX = center.X + sizeX / 2;

            Point2D bottomCenter = new Point2D(center.X, bottomPointY);
            Point2D topCenter = new Point2D(center.X, topPointY);
            Point2D leftCenter = new Point2D(leftPointX, center.Y);
            Point2D rightCenter = new Point2D(rightPointX, center.Y);

            Point2D bottomLeft = new Point2D(leftPointX, bottomPointY);
            Point2D bottomRight = new Point2D(rightPointX, bottomPointY);
            Point2D topLeft = new Point2D(leftPointX, topPointY);
            Point2D topRight = new Point2D(rightPointX, topPointY);
            //Если по X есть точки
            if (quantityX > 1)
            {
                //Если по Y есть точки
                if (quantityY > 1)
                {
                    List<Point2D> pointsLeftY = GeometryProcessor.GetInternalPoints(bottomLeft, topLeft, quantityY - 2, false, false);
                    List<Point2D> pointsRightY = GeometryProcessor.GetInternalPoints(bottomRight, topRight, quantityY - 2, false, false);
                    points.AddRange(GeometryProcessor.GetInternalPoints(bottomLeft, bottomRight, quantityX - 2, true, true));
                    for (int i = 0; i < pointsLeftY.Count(); i++)
                    {
                        points.Add(pointsLeftY[i]);
                        if (fillArray)
                        {
                            points.AddRange(GeometryProcessor.GetInternalPoints(pointsLeftY[i], pointsRightY[i], quantityX - 2, false, false));
                        }
                        points.Add(pointsRightY[i]);
                    }
                    points.AddRange(GeometryProcessor.GetInternalPoints(topLeft, topRight, quantityX - 2, true, true));
                }
                //Если по Y только одна точка
                else
                {
                    points.AddRange(GeometryProcessor.GetInternalPoints(leftCenter, rightCenter, quantityX - 2, true, true));
                }
            }
            //Если точка по X только одна
            else
            {
                //Если по Y есть точки
                if (quantityY > 1)
                {
                    points.AddRange(GeometryProcessor.GetInternalPoints(bottomCenter, topCenter, quantityY - 2, true, true));
                }
                //Если необходимо вывести только одну точку
                else points.Add(center);
                    
            }
            return points;
        }
        /// <summary>
        /// Возвращает суммарную площадь фигур по коллекции фигур
        /// </summary>
        /// <param name="shapes"></param>
        /// <returns></returns>
        public static double GetArea(List<IShape> shapes)
        {
            double area = 0;
            foreach (IShape shape in shapes)
            {
                area += shape.GetArea();
            }        
            return area;
        }
        /// <summary>
        /// Возвращает момент инерции для коллекции фигур
        /// </summary>
        /// <param name="shapes"></param>
        /// <returns></returns>
        public static double[] GetMomInertia(List<IShape> shapes)
        {
            double[] moments = new double[] { 0, 0 };
            Point2D center = GetGravCenter(shapes);
            foreach (IShape shape in shapes)
            {
                double[] locMoments = GetMomInertia(shape, center);
                moments[0] += locMoments[0];
                moments[1] += locMoments[1];
            }
            return moments;
        }
        /// <summary>
        /// Возвращает моменты инерции линии относительно заданного центра
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="gravityCenter"></param>
        /// <returns></returns>
        public static double[] GetLineMomentInertia(Point2D startPoint, Point2D endPoint, Point2D gravityCenter)
        {
            double Ix, Iy;
            //Находим длину отрезка
            double lineLength = GetDistance(startPoint, endPoint);
            //Находим центр отрезка
            Point2D lineCenter = GetMiddlePoint(startPoint, endPoint);
            //Находим угол между осью X и отрезком
            double angle = GetAngle(startPoint, endPoint);
            //расстояние от заданного центра тяжести до центра отрезка (проекция на ось X)
            double dX = gravityCenter.X - lineCenter.X;
            //то же, проекция на ось Y
            double dY = gravityCenter.Y - lineCenter.Y;
            //Вспомогательный коэффициент для экономии ресурсов
            double k1 = lineLength * lineLength * lineLength / 12;
            //Находим моменты инерции
            Ix = k1 * Math.Pow(Math.Sin(angle), 2) + lineLength * dY * dY;
            Iy = k1 * Math.Pow(Math.Cos(angle), 2) + lineLength * dX * dX;
            return new double[] { Ix, Iy };
        }
        /// <summary>
        /// Возвращает точку центра тяжести по коллекции фигур
        /// </summary>
        /// <param name="shapes">Коллекция фигур</param>
        /// <returns>Точка центра тяжести</returns>
        public static Point2D GetGravCenter(List<IShape> shapes)
        {
            double[] s = new double[] { 0, 0, };
            double totArea = GetArea(shapes);
            Point2D center = new Point2D();
            foreach (IShape shape in shapes)
            {
                s[0] += shape.GetArea() * (shape.Center.X - center.X);
                s[1] += shape.GetArea() * (shape.Center.Y - center.Y);
            }
            Point2D newCenter = new Point2D(s[0] / totArea, s[1] / totArea);
            return newCenter;
        }
        /// <summary>
        /// Возвращает момент инерции фигуры относительно указанного центра
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static double[] GetMomInertia(IShape shape, Point2D center)
        {
            double[] moments = new double[] { 0, 0 };
            if (shape is IRectangle)
            {
                IRectangle rectangle = shape as IRectangle;
                moments[0] += rectangle.Width * Math.Pow(rectangle.Length, 3) / 12;
                moments[1] += rectangle.Length * Math.Pow(rectangle.Width, 3) / 12;
            }
            if (shape is ICircle)
            {
                ICircle circle = shape as ICircle;
                double mom = Math.Pow(circle.Diameter, 4) / 64;
                moments[0] += mom;
                moments[1] += mom;
            }
            double dX = shape.Center.X-center.X;
            double dY = shape.Center.Y - center.Y;

            moments[0] += shape.GetArea() * dY * dY;
            moments[1] += shape.GetArea() * dX * dX;

            return moments;
        }
        /// <summary>
        /// Возвращает краевые расстояния для коллекции фигур
        /// </summary>
        /// <param name="shapes"></param>
        /// <returns></returns>
        public static double[] GetEdgeDist(List<IShape> shapes)
        {
            List<double> coordX = new List<double>();
            List<double> coordY = new List<double>();
            foreach (IShape shape in shapes)
            {
                if (shape is IRectangle)
                {
                    IRectangle rectangle = shape as IRectangle;
                    coordX.Add(rectangle.Center.X + rectangle.Width / 2);
                    coordX.Add(rectangle.Center.X - rectangle.Width / 2);
                    coordY.Add(rectangle.Center.Y + rectangle.Length / 2);
                    coordY.Add(rectangle.Center.Y - rectangle.Length / 2);
                }
                if (shape is ICircle)
                {
                    ICircle circle = shape as ICircle;
                    coordX.Add(circle.Center.X + circle.Diameter / 2);
                    coordX.Add(circle.Center.X - circle.Diameter / 2);
                    coordY.Add(circle.Center.Y + circle.Diameter / 2);
                    coordY.Add(circle.Center.Y - circle.Diameter / 2);
                }
            }
            double[] dists = new double[] { coordX.Max(), coordX.Min(), coordY.Max(), coordY.Min() };
            return dists;
        }
        /// <summary>
        /// Возвращает моменты сопротивления по коллекции фигур
        /// </summary>
        /// <param name="shapes">Коллекция фигур</param>
        /// <returns>WxPos, WxNeg, WyPos, WyNeg,</returns>
        public static double[] GetSecMomentInertia(List<IShape> shapes)
        {
            double[] inertia = GetMomInertia(shapes);
            double[] edgDist = GetEdgeDist(shapes);
           
            double[] moms = new double[] { inertia[0] / Math.Abs(edgDist[2]), inertia[0] / Math.Abs(edgDist[3]), inertia[1] / Math.Abs(edgDist[0]), inertia[1] / Math.Abs(edgDist[1]) };
            return moms;
        }
        /// <summary>
        /// Возвращает минимальные момент сопротивления относительно осей X и Y
        /// </summary>
        /// <param name="shapes"></param>
        /// <returns>Wx, Wy</returns>
        public static double[] GetMinSecMomentInertia(List<IShape> shapes)
        {
            double[] moms = GetSecMomentInertia(shapes);
            double[] edgDist = GetEdgeDist(shapes);

            double[] momsMin = new double[] { Math.Min(moms[0], moms[1]), Math.Min(moms[2], moms[3]) };
            return momsMin;
        }
        /// <summary>
        /// Возвращает прямоугольное сечение по исходному прямоугольному сечению и массиву офсетов
        /// </summary>
        /// <param name="rectangle">Исходный прямоугольник</param>
        /// <param name="ofssets">Массив отступов слева, справа, сверху, снизу</param>
        /// <returns>Прямоугольное сечение</returns>
        public static RectCrossSection GetRectangleOffset(IRectangle rectangle, double[] ofssets)
        {
            if (ofssets.Count() != 4) throw new Exception("Count of members of array is not valid");
            Point2D center = new Point2D(rectangle.Center.X, rectangle.Center.Y);
            double width = rectangle.Width;
            double length = rectangle.Length;
            width += ofssets[0] + ofssets[1];
            length += ofssets[2] + ofssets[3];
            center.X += (ofssets[1] - ofssets[0]) / 2;
            center.Y += (ofssets[2] - ofssets[3]) / 2;
            RectCrossSection section = new RectCrossSection(width, length, center);
            return section;
        }
        /// <summary>
        /// Возвращает прямоугольное сечение по исходному прямоугольному сечению и массиву офсетов
        /// </summary>
        /// <param name="rectangle">Исходный прямоугольник</param>
        /// <param name="ofsset">Величина офсета (одинаковый для всех сторон)</param>
        /// <returns>Прямоугольное сечение</returns>
        public static RectCrossSection GetRectangleOffset(IRectangle rectangle, double ofsset)
        {
            double[] ofssets = new double[] { ofsset, ofsset, ofsset, ofsset };
            return GetRectangleOffset(rectangle, ofssets);
        }
        /// <summary>
        /// Возвращает угловые точки по исходному прямоугольному сечению
        /// </summary>
        /// <param name="rectangle">Исходный прямоугольник</param>
        /// <returns>Коллекция угловых точек</returns>
        public static List<Point2D> GetAnglePointsFromRectangle(IRectangle rectangle)
        {
            List<Point2D> points = new List<Point2D>();
            Point2D center = rectangle.Center;
            double width = rectangle.Width;
            double length = rectangle.Length;
            Point2D point;
            //левая нижняя точка
            point = new Point2D(center.X - width / 2, center.Y - length / 2);
            points.Add(point);
            //левая верхняя точка
            point = new Point2D(center.X - width / 2, center.Y + length / 2);
            points.Add(point);
            //правая нижняя точка
            point = new Point2D(center.X + width / 2, center.Y - length / 2);
            points.Add(point);
            //правая верхняя точка
            point = new Point2D(center.X + width / 2, center.Y + length / 2);
            points.Add(point);
            return points;
        }
    }
}
