using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Entity.MeasureUnits
{
    /// <summary>
    /// 
    /// </summary>
    public static class MeasureUnitConverter
    {
        public static double GetCoefficient(int index)
        {
            return GetUnitLabel(index).AddKoeff;
        }

        public static string GetUnitLabelText(int index)
        {
            return GetUnitLabel(index).UnitName;
        }

        public static MeasureUnitLabel GetUnitLabel(int index)
        {
            var measureUnitLabels = from t in ProgrammSettings.MeasureUnits[index].UnitLabels where t.Id == ProgrammSettings.MeasureUnits[index].CurrentUnitLabelId select t;
            return measureUnitLabels.First();
        }
    }
}
