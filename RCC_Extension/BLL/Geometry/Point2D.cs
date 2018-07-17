using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCC_Extension.BLL.Geometry

{
    public class Point2D
    {
        public decimal coord_X { get; set; }
        public decimal coord_Y { get; set; }

        
    }

    public class Vector2D
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public decimal GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            return (StartPoint.coord_X - EndPoint.coord_X);
                //Math.Sqrt(Convert.ToDouble(StartPoint.coord_X - EndPoint.coord_X));
        }
    }
}
