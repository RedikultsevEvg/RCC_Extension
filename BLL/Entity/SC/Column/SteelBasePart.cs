using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM;

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// Класс участков баз стальных колонн
    /// </summary>
    public class SteelBasePart : ICloneable
    {
        //Properties
        #region 
        public SteelColumnBase ColumnBase { get; set; } //База стальной колонны к которой относится участок
        public int Id { get; set; } //Код участка
        public String Name { get; set; } //Имя участка
        public double Width { get; set; } //Ширина участка
        public double Length { get; set; } //Длина участка
        public double[] Center { get; set; } //Привязка центра
        public double LeftOffset { get; set; } //Смещение левой границы 
        public double RightOffset { get; set; } //Смещение правой границы
        public double TopOffset { get; set; } //Смещение верхней границы
        public double BottomOffset { get; set; } //Смещение нижней границы
        public bool FixLeft { get; set; } //Опора по левой границе
        public bool FixRight { get; set; } //Опора по правой границе
        public bool FixTop { get; set; } //Опора по верхней границе
        public bool FixBottom { get; set; } //Опора по нижней границе
        public bool AddSymmetricX { get; set; } //Наличие симметричного участка относительно оси X
        public bool AddSymmetricY { get; set; } //Наличие симметричного участка по оси Y

        public List<NdmConcreteArea> SubParts { get; set; }

        #endregion
        //Constructors
        #region
        public void SetDefault()
        {
            Name = "Новый участок";
            Width = 0.2;
            Length = 0.2;
            Center = new double[2] { 0, 0 };
            FixLeft = true;
            FixRight = true;
            FixTop = true;
            FixBottom = true;
            AddSymmetricX = true;
            AddSymmetricY = true;
        }
        public SteelBasePart()
        {
            SetDefault();
        }
        public SteelBasePart(SteelColumnBase columnBase)
        {
            ColumnBase = columnBase;
            SetDefault();
        }
        #endregion
        public void GetSubParts()
        {
            SubParts = new List<NdmConcreteArea>();
            double elementSize = 0.02;
            int numX = Convert.ToInt32(Width / elementSize);
            int numY = Convert.ToInt32(Length / elementSize);
   
            double stepX = Width / numX;
            double stepY = Length / numY;

            double startCenterX = Center[0] - Width/2 + stepX / 2;
            double startCenterY = Center[1] - Length/2 + stepY / 2;

            for ( int i = 0; i < numX; i++)
            {
                for (int j = 0; j < numY; j++)
                {
                    NdmConcreteArea subPart = new NdmConcreteArea();
                    subPart.Width = stepX;
                    subPart.Length = stepY;
                    subPart.ConcreteArea.CenterX = startCenterX + stepX * i;
                    subPart.ConcreteArea.CenterY = startCenterY + stepY * j;
                    SubParts.Add(subPart);
                }
            }
        }



        //IClonable
        public object Clone()
        {
            SteelBasePart steelBasePart = this.MemberwiseClone() as SteelBasePart;
            steelBasePart.Center = new double[2] { 0, 0 };
            steelBasePart.Center[0] = this.Center[0];
            steelBasePart.Center[1] = this.Center[1];
            return steelBasePart;
        }
    }
}
