using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Forces
{
    public class Force : ICloneable
    {
        //Properties
        #region 
        public double Force_Nz { get; set; }
        public double Force_Mx { get; set; }
        public double Force_My { get; set; }
        public double Force_Qx { get; set; }
        public double Force_Qy { get; set; }
        #endregion
        #region Constructors

        #endregion
        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
