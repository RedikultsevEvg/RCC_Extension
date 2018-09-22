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
        public ForcesGroup ForcesGroup { get; set; }
        public LoadSet LoadSet { get; set; }
        public Force Force { get; set; }
        #endregion
        //Constructors
        #region
        public void SetDefault()
        {
            LoadSet = new LoadSet();
            LoadSet.Name = "";
            LoadSet.PartialSafetyFactor = 1;
            LoadSet.IsDeadLoad = false;
            LoadSet.IsCombination = false;
            LoadSet.IsDesignLoad = false;
            LoadSet.BothSign = false;
            Force = new Force(1);
            Force.Force_Nz = 0;
            Force.Force_Mx = 0;
            Force.Force_My = 0;
            Force.Force_Qx = 0;
            Force.Force_Qy = 0;
        }
        public void SetDefault1()
        {
            LoadSet = new LoadSet();
            LoadSet.Name = "Новая нагрузка";
            LoadSet.PartialSafetyFactor = 1.1;
            LoadSet.IsDeadLoad = true;
            LoadSet.IsCombination = false;
            LoadSet.IsDesignLoad = false;
            LoadSet.BothSign = false;
            Force = new Force(1);
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
        public BarLoadSet(ForcesGroup forcesGroup)
        {
            SetDefault1();
            ForcesGroup = forcesGroup;
        }
        #endregion
        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
