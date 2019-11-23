using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using RDBLL.Entity.RCC.Foundations;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RDUIL.Converters
{
    public class FoundationToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Foundation item)
            {
                Path path = new Path();
                GeometryGroup geometryGroup = new GeometryGroup();
                foreach (FoundationPart foundationPart in item.Parts)
                {
                    RectangleGeometry rectangleGeometry = new RectangleGeometry();
                    rectangleGeometry.Rect = new Rect(0, 0, foundationPart.Width*100, foundationPart.Length*100);
                    geometryGroup.Children.Add(rectangleGeometry);
                }
                path.Data = geometryGroup;
                return path.Data;
            }
            throw new ArgumentException();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
