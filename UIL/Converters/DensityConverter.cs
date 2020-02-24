using RDBLL.Entity.MeasureUnits;
using System.Windows.Data;
using System;

namespace RDUIL.Converters
{
    public class DensityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value * MeasureUnitConverter.GetCoefficient(8);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return System.Convert.ToDouble(value) / MeasureUnitConverter.GetCoefficient(8);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
