using System;
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
        public int Width { get; set; }
        public int Length { get; set; }
        public bool FixLeft { get; set; }
        public bool FixRight { get; set; }
        public bool FixTop { get; set; }
        public bool FixBottom { get; set; }
        #endregion
        //Constructors
        #region
        public void SetDefault()
        {
            Width = 200;
            Length = 200;
            FixLeft = true;
            FixRight = true;
            FixTop = true;
            FixBottom = true;
        }
        public SteelBasePart()
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
