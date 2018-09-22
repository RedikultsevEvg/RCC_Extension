using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Forces
{
    public class LoadSet
    {
        #region 
        public String Name { get; set; } //Наименование
        public double PartialSafetyFactor { get; set; } //Коэффициент надежности по нагрузке
        public bool IsDeadLoad { get; set; }
        public bool IsCombination { get; set; }
        public bool IsDesignLoad { get; set; }
        public bool BothSign { get; set; }
        #endregion
    }
}
