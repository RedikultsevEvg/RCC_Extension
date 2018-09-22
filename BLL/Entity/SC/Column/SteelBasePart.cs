﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column
{
    public class SteelBasePart : ICloneable
    {
        //Properties
        #region 
        public SteelColumnBase ColumnBase { get; set; }
        public String Name { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double[] Center { get; set; }
        public double LeftOffset { get; set; }
        public double RightOffset { get; set; }
        public double TopOffset { get; set; }
        public double BottomOffset { get; set; }
        public bool FixLeft { get; set; }
        public bool FixRight { get; set; }
        public bool FixTop { get; set; }
        public bool FixBottom { get; set; }
        public bool AddSymmetricX { get; set; }
        public bool AddSymmetricY { get; set; }

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