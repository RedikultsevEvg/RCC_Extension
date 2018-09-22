﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDBLL.Common.Geometry
{
    public class MathOperation
    {
        public static double InterpolateNumber(double x1, double y1, double x2, double y2, double xn)
        {
            double result = y1 + (y2-y1)/(x2-x1)*(xn-x1);
            return result;
        }

        public static double InterpolateList(List<double> xValue, List<double> yValue, double xn)
        {
            double result;
            int i = 0 ;
            if ((xn<xValue[0]) || (xn > xValue[xValue.Count - 1]))
            {
                MessageBox.Show("Значение за пределами диапазона " + Convert.ToString(xn) + " / " + Convert.ToString(xValue[0]) + " / "  + Convert.ToString(xValue[xValue.Count-1]), "Неверные значения");
            }
            if (xn == xValue[0]) { result = yValue[0]; return result; }
            while (xn> xValue[i])
            {
                i++;
            }
            double x1 = xValue[i-1];
            double x2 = xValue[i];
            double y1 = yValue[i-1];
            double y2 = yValue[i];
            result = InterpolateNumber(x1, y1, x2, y2,  xn);
            return result;
        }
    }
}