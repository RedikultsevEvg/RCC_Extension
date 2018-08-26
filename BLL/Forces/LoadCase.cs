using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Forces
{
    class LoadCase : ICloneable
    {
        //Properties
        #region 
        public int LoadCaseID { get; set; }
        public String Name { get; set; }
        public double Force_N { get; set; }
        public double Force_Mx { get; set; }
        public double Force_My { get; set; }
        public double Force_Qx { get; set; }
        public double Force_Qy { get; set; }
        #endregion
        //Constructors
        #region
        public void SetDefault()
        {
            Name = "Новая комбинация";
            Force_N = 100;
            Force_Mx = 0;
            Force_My = 0;
            Force_Qx = 0;
            Force_Qy = 0;
        }
        public LoadCase()
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
