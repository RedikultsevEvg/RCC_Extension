﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using RDBLL.Entity.Results.SC;

namespace RDBLL.Entity.SC.Column
{
    public class SteelColumnBase : ICloneable
    {
        //Properties
        #region 
        //public ColumnBaseResult ColumnBaseResult { get; set; }
        public int SteelColumnBaseID { get; set; }
        public String Name { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Thickness { get; set; }
        public int WidthBoltDist { get; set; }
        public int LengthBoltDist { get; set; }
        public double ConcreteStrength { get; set; }
        public double BoltPrestressForce { get; set; }
        public List<ColumnLoadSet> Loads { get; set; }
        public List<SteelBasePart> SteelBaseParts { get; set; }

        #endregion
        
        //Constructors
        #region
        public void SetDefault()
        {
            Name = "Новая база";
            Width = 600;
            Length = 900;
            Thickness = 60;
            WidthBoltDist = 150;
            LengthBoltDist = 150;
            ConcreteStrength = 1;
            BoltPrestressForce = 0;
            Loads = new List<ColumnLoadSet>();
            Loads.Add(new ColumnLoadSet());
            SteelBaseParts = new List<SteelBasePart>();
            SteelBaseParts.Add(new SteelBasePart());
        }
        public SteelColumnBase()
        {
            SetDefault();
        }
        #endregion
        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
