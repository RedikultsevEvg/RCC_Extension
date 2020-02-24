using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RDBLL.Entity.MeasureUnits;

namespace RDUIL.Converters
{
    public class VolumeWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value * MeasureUnitConverter.GetCoefficient(9);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return System.Convert.ToDouble(value) / MeasureUnitConverter.GetCoefficient(9);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
