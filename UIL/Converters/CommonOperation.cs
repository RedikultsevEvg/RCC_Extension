using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using RDBLL.Entity.MeasureUnits;

namespace RDUIL.Converters
{
    public static class CommonOperation
    {
        public static double ConvertToDouble(string s)
        {
            double result;
            if (! double.TryParse(s, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                !double.TryParse(s, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                !double.TryParse(s, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                throw new Exception("Неверное число");
            }
            return result;
        }

        public static double ConvertToDouble(string s, int index)
        {
            return ConvertToDouble(s) / MeasureUnitConverter.GetCoefficient(index);
        }

    }
}
