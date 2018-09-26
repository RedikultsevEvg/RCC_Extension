using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RDUIL.Converters
{
    //Конвертер для перевода чисел из Н в кН и т.п. и обратно
    public class MultThous : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value / 1000.0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            return System.Convert.ToDouble(value) * 1000.0;
        }
    }
}
