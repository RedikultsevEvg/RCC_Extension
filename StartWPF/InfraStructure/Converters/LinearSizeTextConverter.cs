using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Service;

namespace RDStartWPF.InfraStructure.Converters
{
    //конвертер для подстановки префикса "к" к единицам измерения, например к+Н.
    public class LinearSizeTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (string)value + ", " + MeasureUnitConverter.GetUnitLabelText(0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
