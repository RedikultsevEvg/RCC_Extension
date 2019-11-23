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
    public class DrawUtils
    {
        public static void DrawAxis(Canvas canvas)
        {
            double gap = 0.05;
            //Рисуем ось X
            Line axisX = new Line();
            axisX.X1 = canvas.Width * gap;
            axisX.Y1 = canvas.Height/2;
            axisX.X2 = canvas.Width * (1-gap);
            axisX.Y2 = canvas.Height / 2;
            axisX.Stroke = Brushes.Red;
            axisX.StrokeThickness = 1;
            axisX.StrokeDashArray = new DoubleCollection { 10, 4 };
            canvas.Children.Add(axisX);
            //Рисуем ось Y
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
