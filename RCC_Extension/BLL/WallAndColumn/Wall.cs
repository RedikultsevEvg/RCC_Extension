using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.BuildingAndSite;
using RCC_Extension.BLL.Geometry;


namespace RCC_Extension.BLL.WallAndColumn
{
    public class Wall :ICloneable
    {
        public String Name { get; set; }
        public WallType WallType { get; set; }
        
        public Level Level { get; set; }
        public List<Opening> OpeningList { get; set; }
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public bool ReWriteHeight { get; set; }
        public decimal Height { get; set; }

        public decimal ConcreteStartOffset { get; set; }
        public decimal ConcreteEndOffset { get; set; }

        public decimal ReiforcementStartOffset { get; set; }
        public decimal ReiforcementEndOffset { get; set; }

        //Start of methods

        public decimal GetLength()
        {
            Geometry2D Geometry2D = new Geometry2D();
            return Geometry2D.GetDistance(this.StartPoint, this.EndPoint);
        }

        public decimal GetConcreteLength()
        {
            return GetLength() + ConcreteStartOffset + ConcreteEndOffset;
        }

        public decimal GetHeight()
        {
            if (ReWriteHeight) { return Height; }
            else { return WallType.GetHeight(Level); }
        }

        public decimal GetConcreteAreaBrutto()
        {
            decimal Area = 0;
            Area = GetConcreteLength() * GetHeight();
            return Area;
        }
        public decimal GetConcreteAreaNetto()
        {
            decimal Area = 0;
            Area = GetConcreteAreaBrutto() - GetOpeningsArea();
            return Area;
        }
        public decimal GetOpeningsArea()
        {
            decimal Area = 0;
            //Возвращает суммарную площадь всех проемов
            foreach (Opening _Opening in OpeningList)
            {
                Area += _Opening.GetArea();
            }
            return Area;
        }
        public decimal GetConcreteVolumeBrutto()
        {
            decimal Volume = GetConcreteAreaBrutto() * WallType.Thickness;
            return Volume;
        }

        public decimal GetConcreteVolumeNetto()
        {
            decimal Volume = GetConcreteAreaNetto() * WallType.Thickness;
            return Volume;
        }

        //Конструктор стены по уровню (выбирается первый из списка или создается)
        public Wall(Level level)
        {
            Building building = level.Building;
            WallType wallType;
            if (building.WallTypeList.Count == 0)
            { wallType = new WallType(building); }
            else { wallType = building.WallTypeList[0]; }
            Name = "Новая стена";
            StartPoint = new Point2D(0, 0);
            EndPoint = new Point2D(6000, 0);
            ReWriteHeight = false;
            Height = 3000;
            ConcreteStartOffset = 0;
            ConcreteEndOffset = 0;
            ReiforcementStartOffset = 0;
            ReiforcementEndOffset = 0;
            Level = level;
            WallType = wallType;
            OpeningList = new List<Opening>();
            level.WallList.Add(this);

        }
            //Конструктор стены по уровню и типу стены
        public Wall(Level level, WallType wallType)
        {  
            Name = "Новая стена";
            StartPoint = new Point2D(0, 0);
            EndPoint = new Point2D(6, 0);
            Level = level;
            WallType = wallType;
            level.WallList.Add(this);
        }
        //Построение стены по двум точкам
        public Wall(Point2D StartPoint, Point2D EndPoint)
        {    
            Name = "Новая стена";
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;

        }
        //Построение стены по начальной точке, углу и длине
        public Wall(Point2D StartPoint, decimal Angle, decimal Length)
        {       
            Name = "Новая стена";
            this.StartPoint = StartPoint;
            this.EndPoint = StartPoint.EndPoint(Angle, Length);
        }
        //Клонирование объекта
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
