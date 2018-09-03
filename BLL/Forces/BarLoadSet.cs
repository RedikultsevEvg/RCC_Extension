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
        public double Force_Nz { get; set; }
        public double Force_Mx { get; set; }
        public double Force_My { get; set; }
        public double Force_Qx { get; set; }
        public double Force_Qy { get; set; }
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
            Force_Nz = 0;
            Force_Mx = 0;
            Force_My = 0;
            Force_Qx = 0;
            Force_Qy = 0;
        }
        public void SetDefault1()
        {
            Name = "Новая нагрузка";
            PartialSafetyFactor = 1.1;
            IsDeadLoad = true;
            IsCombination = false;
            IsDesignLoad = false;
            BothSign = false;
            Force_Nz = -100000;
            Force_Mx = 0;
            Force_My = 0;
            Force_Qx = 0;
            Force_Qy = 0;
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
