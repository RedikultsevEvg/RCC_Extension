using RDBLL.Entity.MeasureUnits;
using System;
using System.Windows.Data;

namespace RDUIL.Converters
{
    public class DistributedLoadConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value * MeasureUnitConverter.GetCoefficient(13);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return System.Convert.ToDouble(value) / MeasureUnitConverter.GetCoefficient(13);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
