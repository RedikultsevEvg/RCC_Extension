using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.Building;
using RCC_Extension.BLL.Geometry;


namespace RCC_Extension.BLL.WallAndColumn
{
    public class Wall :ICloneable
    {
        public WallType WallType { get; set; }
        public String Name { get; set; }
        public Level Level { get; set; }
        public List<Opening> OpeningList { get; set; }
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public bool RewriteHeight { get; set; }
        public decimal Height { get; set; }

        public decimal ConcreteStartOffset { get; set; }
        public decimal ConcreteEndOffset { get; set; }

        public decimal ReiforcementStartOffset { get; set; }
        public decimal ReiforcementEndOffset { get; set; }

        //Start of methods

        public decimal GetLength()
        {
            Geometry2D Geometry2D = new Geometry2D();
            return Geometry2D.GetDistance(this.StartPoint, this.EndPoint); ;
        }

        public decimal GetHeight()
        {
            //В дальнейшем предусмотреть определение высоты стены по заданным уровням
            return Height;
        }

        public decimal GetOpeningsArea()
        {
            decimal Area = 0;
            //Возвращает суммарную площадь всех проемов
            foreach (Opening _Opening in this.OpeningList)
            {
                Area += _Opening.GetArea();
            }
            return Area;
        }

        public decimal GetConcreteVolume()
        {
            decimal Length = this.GetLength()-this.ConcreteStartOffset-this.ConcreteEndOffset;
            decimal Heigth = GetHeight();
            decimal Volume = Math.Ceiling(Length * Height*100)/100; //С округлением до 0.01 куб.м.

            return Volume;
        }

        public decimal GetSurfaceAreaBrutto()
        {
            //
            return 0;
        }

        public Wall()
        {
            //Конструктор стены по умолчанию
            //Wall Wall = new Wall();
        }

        public Wall(Point2D StartPoint, Point2D EndPoint)
        {
            //Построение стены по двум точкам
            Wall Wall = new Wall();
            Wall.StartPoint = StartPoint;
            Wall.EndPoint = EndPoint;

        }

        public Wall(Point2D StartPoint, decimal Angle, decimal Length)
        {
            //Построение стены по начальной точке, углу и длине
            Wall Wall = new Wall();
            Wall.StartPoint = StartPoint;
            Wall.EndPoint = StartPoint.EndPoint(Angle, Length);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
