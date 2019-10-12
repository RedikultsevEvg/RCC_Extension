using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.MeasureUnits
{
    public class MeasureUnit
    {
        public int Id { get; set; }
        public int CurrentUnitLabelId { get; set; }
        public List<MeasureUnitLabel> UnitLabels { get; set; }

        public MeasureUnit()
        {
            UnitLabels = new List<MeasureUnitLabel>();
        }

        public MeasureUnitLabel GetCurrentLabel()
        {
            var currentLabel = from t in UnitLabels where t.Id == CurrentUnitLabelId select t;
            return currentLabel.First();
        }
    }
}
