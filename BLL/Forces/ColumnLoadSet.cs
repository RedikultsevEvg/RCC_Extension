using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Forces
{
    public class ColumnLoadSet : ICloneable
    {
        //Properties
        #region 
        public String Name { get; set; } //Наименование
        public double PartialSafetyFactor { get; set; } //Коэффициент надежности по нагрузке
        public bool IsDeadLoad { get; set; }
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
            Name = "Новая нагрузка";
            PartialSafetyFactor = 1.1;
            IsDeadLoad = true;
            BothSign = false;
            Force_Nz = -100000;
            Force_Mx = 0;
            Force_My = 0;
            Force_Qx = 0;
            Force_Qy = 0;
        }
        public ColumnLoadSet()
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
