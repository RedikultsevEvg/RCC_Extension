﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using RDBLL.Entity.MeasureUnits;

namespace RDUIL.Converters
{
    /// <summary>
    /// Конвертер линейных размеров
    /// </summary>
    public class LinearSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value * MeasureUnitConverter.GetCoefficient(0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToDouble(value) / MeasureUnitConverter.GetCoefficient(0);
        }
    }
}
