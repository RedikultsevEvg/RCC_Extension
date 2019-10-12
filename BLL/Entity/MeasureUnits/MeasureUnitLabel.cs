using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.MeasureUnits
{
    public class MeasureUnitLabel
    {
        public int Id { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
        public String UnitName { get; set; }
        public double AddKoeff { get; set; }
    }
}
