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
        public ForcesGroup ForcesGroup { get; set; }
        public LoadSet LoadSet { get; set; }
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
        }
        public BarLoadSet(int setDefault)
        {
            if (setDefault == 0) { SetDefault(); } else { SetDefault1(); }
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
