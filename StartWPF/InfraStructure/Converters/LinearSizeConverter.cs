﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Service;

namespace RDStartWPF.InfraStructure.Converters
{
    /// <summary>
    /// Конвертер линейных размеров
    /// </summary>
    public class LinearSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = System.Convert.ToString(value);
            double d = CommonOperation.ConvertToDouble(s);
            return d * MeasureUnitConverter.GetCoefficient(0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string s = System.Convert.ToString(value);
                return CommonOperation.ConvertToDouble(s) / MeasureUnitConverter.GetCoefficient(0);
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
