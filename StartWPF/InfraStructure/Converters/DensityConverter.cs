using RDBLL.Entity.MeasureUnits;
using System.Windows.Data;
using System;
using RDBLL.Common.Service;

namespace RDStartWPF.InfraStructure.Converters
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
                string s = System.Convert.ToString(value);
                return CommonOperation.ConvertToDouble(s,8);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
