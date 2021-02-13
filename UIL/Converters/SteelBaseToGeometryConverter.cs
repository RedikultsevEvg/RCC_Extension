using RDBLL.Entity.SC.Column;
using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using RDBLL.DrawUtils.SteelBase;
using System.Windows;
using RDBLL.Common.Service;


namespace RDUIL.Converters
{
    public class SteelBaseToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is SteelBase item)
            {
                Canvas canvas = new Canvas();
                canvas.Background = Brushes.White;
                canvas.Width = 150;
                canvas.Height = 120;
                //canvas.VerticalAlignment = VerticalAlignment.Stretch;
                DrawSteelBase.DrawBase(item, canvas);
                return canvas;
            }
            throw new ArgumentException();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
