using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.Geometry;
using RCC_Extension.BLL.WallAndColumn;


namespace RCC_Extension.BLL.Building
{
    public class Level :ICloneable
    {
        public string Name { get; set; }
        public decimal FloorLevel { get; set; }
        public decimal Height { get; set; }
        public decimal TopFloorThickness { get; set; }
        public int Quant { get; set; }

        public Point3D BasePoint { get; set; }
        public List<Wall> WallList { get; set; }
        public List<Column> ColumnList { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
