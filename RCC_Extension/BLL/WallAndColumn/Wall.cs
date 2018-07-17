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
        public Level Level { get; set; }
        public List<Opening> OpeningList { get; set; }
        
        public Path WallPath { get; set; }
              
        public decimal GetSurfaceAreaBrutto()
        {
            //
            return 0;
        }

        public Wall()
        {
            //Конструктор стены по умолчанию
            Wall Wall = new Wall();
        }

        public Wall(Point2D StartPoint, Point2D EndPoint)
        {
            //Построение стены по двум точкам
            Wall Wall = new Wall();
            Path Path = new Path(StartPoint, EndPoint);
            Wall.WallPath = Path;
        }

        public Wall(Point2D StartPoint, decimal Angle, decimal Length)
        {
            //Построение стены по начальной точке, углу и длине
            Wall Wall = new Wall();
            Path Path = new Path(StartPoint, Angle, Length);
            Wall.WallPath = Path;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
