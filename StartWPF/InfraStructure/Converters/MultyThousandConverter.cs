using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RDBLL.Common.Service;

namespace RDStartWPF.InfraStructure.Converters
{
    public class MultyThousandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value * 1000;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string s = System.Convert.ToString(value);
                return CommonOperation.ConvertToDouble(s) / 1000;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
