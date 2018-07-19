using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.Geometry;

namespace RCC_Extension.BLL.Reinforcement
{
    public class Bar
    {
        public String Name { get; set; }
        public decimal Diametr { get; set;}
        public String ClassName { get; set; }
        public Path Path { get; set; }
    }

    public class BarSpacingSetting
    {
        public bool AddBarsLeft { get; set; }
        public bool AddBarsRight { get; set; }

        public decimal AddBarsLeftSpacing { get; set; }
        public decimal AddBarsRightSpacing { get; set; }

        public Int32 AddBarsLeftQuant { get; set; }
        public Int32 AddBarsRightQuant { get; set; }

        public decimal MainSpacing { get; set; }

        public Bar Bar { get; set; }
    }

    public class BarLineSpacing
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public bool AddStartBar { get; set; }
        public bool AddEndBar { get; set; }

        public Int32 BarQuantity (BarSpacingSetting barSpacingSetting)
        {
            Geometry2D geometry2D = new Geometry2D();
            decimal length = geometry2D.GetDistance(this.StartPoint, this.EndPoint);
            int Quant = Convert.ToInt16(Math.Floor(length / barSpacingSetting.MainSpacing));
            if (barSpacingSetting.AddBarsLeft) Quant++;
            if (barSpacingSetting.AddBarsRight) Quant++;
            return Quant;
        }
    }
}
