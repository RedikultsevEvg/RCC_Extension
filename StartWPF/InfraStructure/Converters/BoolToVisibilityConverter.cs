using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RDStartWPF.InfraStructure.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        // Прямое конвертирование
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility ReturnValue = Visibility.Collapsed;

            switch ((bool)value)
            {
                case true: ReturnValue = Visibility.Visible; break;
                case false: ReturnValue = Visibility.Collapsed; break;
            }

            return ReturnValue;
        }

        // Обратное
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool ReturnValue = false;

            switch ((Visibility)value)
            {
                case Visibility.Visible: ReturnValue = true; break;
                case Visibility.Collapsed: ReturnValue = false; break;
                case Visibility.Hidden: ReturnValue = false; break;
            }

            return ReturnValue;
        }
    }
}
