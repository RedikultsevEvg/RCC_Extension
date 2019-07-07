using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public SteelBasePart(SteelColumnBase columnBase)
        {
            ColumnBase = columnBase;
            SetDefault();
            columnBase.SteelBaseParts.Add(this);
        }
        #endregion
        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
