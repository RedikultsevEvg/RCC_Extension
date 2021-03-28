using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDBLL.Common.Geometry
{
    /// <summary>
    /// Класс для математических операций
    /// </summary>
    public class MathOperation
    {
        /// <summary>
        /// Линейная интерполяция между двумя точками
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="xn"></param>
        /// <returns></returns>
        public static double InterpolateNumber(double x1, double y1, double x2, double y2, double xn)
        {
            double result = y1 + (y2 - y1) / (x2 - x1) * (xn - x1);
            return result;
        }
        /// <summary>
        /// Линейная интерполяция по диапазону
        /// </summary>
        /// <param name="xValue">Значения по оси абсцисс</param>
        /// <param name="yValue">Значения по оси ординат</param>
        /// <param name="xn">Известное значение по оси абсцисс</param>
        /// <returns></returns>
        public static double InterpolateList(List<double> xValue, List<double> yValue, double xn)
        {
            double result;
            int i = 0;
            if ((xn < xValue[0]) || (xn > xValue[xValue.Count - 1]))
            {
                MessageBox.Show("Значение за пределами диапазона " + Convert.ToString(xn) + " / " + Convert.ToString(xValue[0]) + " / " + Convert.ToString(xValue[xValue.Count - 1]), "Неверные значения");
            }
            if (xn == xValue[0]) { result = yValue[0]; return result; }
            while (xn > xValue[i])
            {
                i++;
            }
            double x1 = xValue[i - 1];
            double x2 = xValue[i];
            double y1 = yValue[i - 1];
            double y2 = yValue[i];
            result = InterpolateNumber(x1, y1, x2, y2, xn);
            return result;
        }
        public static double Round(double value, int quant = 3)
        {
            if (value == 0) return 0;
            int i = Convert.ToInt32(Math.Ceiling(Math.Log10(Math.Abs(value))));
            return Math.Round(Math.Abs(value) * Math.Pow(10, quant - i)) / Math.Pow(10, quant - i) * Math.Sign(value);
        }
        public static double[] Round(double[] values, int quant = 3)
        {
            double[] result = new double[values.Count()];
            for (int i = 0; i < values.Count(); i++)
            {
                result[i] = Round(values[i], quant);
            }
            return result;
        }
        public static List<double> Round(List<double> values, int quant = 3)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < values.Count(); i++)
            {
                result.Add(Round(values[i], quant));
            }
            return result;
        }
        public static List<double[]> Round(List<double[]> values, int quant = 3)
        {
            List<double[]> result = new List<double[]>();
            for (int i = 0; i < values.Count(); i++)
            {
                result.Add(Round(values[i], quant));
            }
            return result;
        }
    }
}
