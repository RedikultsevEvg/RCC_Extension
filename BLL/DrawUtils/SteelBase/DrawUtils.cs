using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace RDBLL.DrawUtils.SteelBase
{
    /// <summary>
    /// Утилиты рисования
    /// </summary>
    public class DrawUtils
    {
        /// <summary>
        /// Рисует изображение перекрестия осей
        /// </summary>
        /// <param name="canvas">Канвас</param>
        /// <param name="drawX">Флаг рисования оси по X</param>
        /// <param name="drawY">Флаг рисования оси по Y</param>
        public static void DrawAxis(Canvas canvas, bool drawX, bool drawY)
        {
            double gap = 0.05;
            //Рисуем ось X
            if(drawX)
            {
                Line axisX = new Line();
                axisX.X1 = canvas.Width * gap;
                axisX.Y1 = canvas.Height/2;
                axisX.X2 = canvas.Width * (1-gap);
                axisX.Y2 = canvas.Height / 2;
                axisX.Stroke = Brushes.Red;
                axisX.StrokeThickness = 1;
                axisX.StrokeDashArray = new DoubleCollection { 10, 4 };
                canvas.Children.Add(axisX);
            }

            //Рисуем ось Y
            if (drawY)
            {
                Line axisY = new Line();
                axisY.X1 = canvas.Width / 2;
                axisY.Y1 = canvas.Height * gap;
                axisY.X2 = canvas.Width / 2;
                axisY.Y2 = canvas.Height * (1-gap);
                axisY.Stroke = Brushes.Red;
                axisY.StrokeThickness = 1;
                axisY.StrokeDashArray = new DoubleCollection { 10, 4 };
                canvas.Children.Add(axisY);
            }

        }
    }
}
