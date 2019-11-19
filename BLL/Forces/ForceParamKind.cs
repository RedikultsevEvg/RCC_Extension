using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.MeasureUnits;

namespace RDBLL.Forces
{
    public class ForceParamKind
    {
        private string _longLabel;

        public int Id { get; set; }
        public string LongLabel { get { return _longLabel; } set { _longLabel = value; } }
        public string LongLabelInUnit
        { get
            {
                MeasureUnitLabel measureUnitLabel = MeasureUnit.GetCurrentLabel();
                return _longLabel + ", " + measureUnitLabel.UnitName;
            }
        }
        public string ShortLabel { get; set; }
        public string UnitLabelInUnit
        { get
            {
                MeasureUnitLabel measureUnitLabel = MeasureUnit.GetCurrentLabel();
                return measureUnitLabel.UnitName;
            }
        }
        public string Addition { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
    }
}
