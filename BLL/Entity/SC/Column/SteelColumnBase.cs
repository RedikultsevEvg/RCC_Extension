using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using RDBLL.Entity.Results.SC;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDBLL.Entity.SC.Column
{
    public class SteelColumnBase : ICloneable
    {
        //Properties
        #region 
        //public ColumnBaseResult ColumnBaseResult { get; set; }
        public Level Level { get; set; }
        public int SteelColumnBaseID { get; set; }
        public String Name { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }
        public double WidthBoltDist { get; set; }
        public double LengthBoltDist { get; set; }
        public double ConcreteStrength { get; set; }
        public double BoltPrestressForce { get; set; }
        public List<BarLoadSet> Loads { get; set; }
        public List<SteelBasePart> SteelBaseParts { get; set; }

        #endregion
        
        //Constructors
        #region
        public void SetDefault()
        {
            Name = "Новая база";
            Width = 0.6;
            Length = 0.9;
            Thickness = 0.06;
            WidthBoltDist = 0.15;
            LengthBoltDist = 0.60;
            ConcreteStrength = 1000000;
            BoltPrestressForce = 0;
            Loads = new List<BarLoadSet>();
            //ColumnLoadSet columnLoadSet = new ColumnLoadSet(this);
            SteelBaseParts = new List<SteelBasePart>();
            //SteelBasePart steelBasePart = new SteelBasePart(this);
        }
        public SteelColumnBase(Level level)
        {
            SetDefault();
            Level = level;
            level.SteelColumnBaseList.Add(this);
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
