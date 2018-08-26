using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Geometry
{
    public class Point3D
    {
        public decimal coord_X { get; set; }
        public decimal coord_Y { get; set; }
        public decimal coord_Z { get; set; }

        public Point3D()
        {
           // Point3D point3D = new Point3D();
        }

        public Point3D(decimal coord_X, decimal coord_Y, decimal coord_Z)
        {
            Point3D point3D = new Point3D();
            point3D.coord_X = coord_X;
            point3D.coord_Y = coord_Y;
            point3D.coord_Z = coord_Y;
        }
    }
}
