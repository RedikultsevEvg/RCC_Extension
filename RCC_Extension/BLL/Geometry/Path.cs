using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCC_Extension.BLL.Geometry
{
    public class Path
    {
        public Point2D StartPoint { get; set; }
        public List<PathPart> PartList { get; set; }

        public decimal GetLength ()
        {
            decimal Sum = 0;
            Point2D _StartPoint = this.StartPoint;
            foreach (PathPart _PathPart in PartList)
            {
                Sum += _PathPart.GetDistance(_StartPoint);
                _StartPoint = _PathPart.EndPoint;
            }
            return Sum;
        }

        public List<Point2D> Point2DList()
        {
            //Получение списка точек по пути
            List<Point2D> _Point2DList = new List<Point2D>();
            _Point2DList.Add(this.StartPoint);

            foreach (PathPart _PathPart in PartList)
            {
                _Point2DList.Add(_PathPart.EndPoint);
            }
            return _Point2DList;
        }

        
        public Path (Point2D StartPoint, Point2D EndPoint)
        {
            //Путь из одной линии по двум точкам на плоскости
            List<PathPart> PathParts = new List<PathPart>();
            this.PartList = PathParts;
            this.StartPoint = StartPoint;
            PathPart PathPart = new PathPart(EndPoint);
            PathParts.Add(PathPart);
        }

        public Path(Point2D StartPoint, decimal Angle, decimal Length)
        {
            //Путь из одной линии по начальной точке, длине, углу.
            List<PathPart> PathParts = new List<PathPart>();
            this.PartList = PathParts;
            Point2D EndPoint = new Point2D();
            EndPoint.coord_X = StartPoint.coord_X + Convert.ToDecimal(Math.Cos(Convert.ToDouble(Angle))) * Length;
            EndPoint.coord_Y = StartPoint.coord_Y + Convert.ToDecimal(Math.Sin(Convert.ToDouble(Angle))) * Length;
            this.StartPoint = StartPoint;
            PathPart PathPart = new PathPart(EndPoint);
            PathParts.Add(PathPart);
        }
    }

    public class PathPart
    {
        //Класс для отрезков пути
        public Point2D EndPoint { get; set; }

        public decimal GetDistance(Point2D StartPoint)
        {
            decimal dX = EndPoint.coord_X - StartPoint.coord_X;
            decimal dY = EndPoint.coord_Y - StartPoint.coord_Y;
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((dX + dY))));
        }

        public PathPart()
        {
            //Конструктор отрезков по умолчанию
            PathPart PathPart = new PathPart();
        }

        public PathPart (Point2D EndPoint)
        {
            //Конструктор отрезка по конечной точке отрезка
            PathPart PathPart = new PathPart();
            PathPart.EndPoint = EndPoint;
        }
    }
}
