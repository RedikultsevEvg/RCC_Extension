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

    public class BarSpacingSettings :ICloneable
    {
        public bool AddBarsLeft { get; set; }
        public bool AddBarsRight { get; set; }

        public decimal AddBarsLeftSpacing { get; set; }
        public decimal AddBarsRightSpacing { get; set; }

        public Int32 AddBarsLeftQuant { get; set; }
        public Int32 AddBarsRightQuant { get; set; }

        public decimal MainSpacing { get; set; }
        public Bar Bar { get; set; }

        public String SpacingText()
        {
            String S="Шаг ";
            if (AddBarsLeft) S += Convert.ToString(AddBarsLeftQuant - 1) + "*" + Convert.ToString(AddBarsLeftSpacing) + ";";
            S += Convert.ToString(MainSpacing) + ";";
            if (AddBarsRight) S += Convert.ToString(AddBarsRightQuant - 1) + "*" + Convert.ToString(AddBarsRightSpacing) + ";";
            return S;
        }

        public BarSpacingSettings(int Type)
        {
            switch (Type) //
            {
                case 1: //Вертикальная раскладка
                    {
                        AddBarsLeft = true;
                        AddBarsLeftQuant = 2;
                        AddBarsLeftSpacing = 100;
                        AddBarsRight = true;
                        AddBarsRightQuant = 2;
                        AddBarsRightSpacing = 100;
                        MainSpacing = 200;
                        break;
                    }
                case 2: //Горизонтальная раскладка
                    {
                        AddBarsLeft = false;
                        AddBarsLeftQuant = 2;
                        AddBarsLeftSpacing = 100;
                        AddBarsRight = false;
                        AddBarsRightQuant = 2;
                        AddBarsRightSpacing = 100;
                        MainSpacing = 200;
                        break;
                    }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class BarLineSpacing
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public decimal StartOffset { get; set; }
        public decimal EndOffset { get; set; }

        public bool AddStartBar { get; set; }
        public bool AddEndBar { get; set; }

        public BarSpacingSettings barSpacingSettings { get; set; }

        public Int32 BarQuantity ()
        {
            Geometry2D geometry2D = new Geometry2D();
            int Quant = 0;
            decimal length = geometry2D.GetDistance(this.StartPoint, this.EndPoint);
            decimal mainLength = length - this.StartOffset-this.EndOffset;
             
            if (this.barSpacingSettings.AddBarsLeft)
            {
                Quant += this.barSpacingSettings.AddBarsLeftQuant;
                mainLength -= this.barSpacingSettings.AddBarsLeftSpacing * this.barSpacingSettings.AddBarsLeftQuant;
            }
            if (this.barSpacingSettings.AddBarsRight)
            {
                Quant += this.barSpacingSettings.AddBarsRightQuant;
                mainLength -= this.barSpacingSettings.AddBarsRightSpacing * this.barSpacingSettings.AddBarsRightQuant;
            }

            Quant += Convert.ToInt16(Math.Ceiling(mainLength / barSpacingSettings.MainSpacing));
            return Quant;
        }
    }
}
