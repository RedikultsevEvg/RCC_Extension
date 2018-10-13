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
        public double Value { get; set; }
        public int Kind_id { get; set; }
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
