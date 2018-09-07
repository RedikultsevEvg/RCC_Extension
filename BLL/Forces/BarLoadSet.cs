using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.SC.Column;

namespace RDBLL.Forces
{
    public class BarLoadSet : ICloneable
    {
        //Properties
        #region 
        public SteelColumnBase SteelColumnBase { get; set; }
        public String Name { get; set; } //Наименование
        public double PartialSafetyFactor { get; set; } //Коэффициент надежности по нагрузке
        public bool IsDeadLoad { get; set; }
        public bool IsCombination { get; set;}
        public bool IsDesignLoad { get; set; }
        public bool BothSign { get; set; }
        public Force Force { get; set; }
        #endregion
        //Constructors
        #region
        public void SetDefault()
        {
            Name = "";
            PartialSafetyFactor = 1;
            IsDeadLoad = false;
            IsCombination = false;
            IsDesignLoad = false;
            BothSign = false;
            Force = new Force();
            Force.Force_Nz = 0;
            Force.Force_Mx = 0;
            Force.Force_My = 0;
            Force.Force_Qx = 0;
            Force.Force_Qy = 0;
        }
        public void SetDefault1()
        {
            Name = "Новая нагрузка";
            PartialSafetyFactor = 1.1;
            IsDeadLoad = true;
            IsCombination = false;
            IsDesignLoad = false;
            BothSign = false;
            Force = new Force();
            Force.Force_Nz = -100000;
            Force.Force_Mx = 0;
            Force.Force_My = 0;
            Force.Force_Qx = 0;
            Force.Force_Qy = 0;
        }
        public BarLoadSet(int setDefault)
        {
            if (setDefault == 0) { SetDefault(); } else { SetDefault1(); }
        }
        public BarLoadSet(SteelColumnBase steelColumnBase)
        {
            SetDefault1();
            steelColumnBase.Loads.Add(this);
            SteelColumnBase = steelColumnBase;
        }
        #endregion
        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
