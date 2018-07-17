using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.Building;

namespace RCC_Extension.BLL.WallAndColumn
{
    public class WallType :ICloneable
    {
        public decimal Thickness { get; set; }
        public decimal TopOffset { get; set; }
        public decimal BottomOffset { get; set; }

        public decimal GetHeight(Level _level)
        {
            return _level.Height + TopOffset + BottomOffset;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }


    }
}
