using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.RCC.Foundations.Processors;

namespace RDBLL.DrawUtils.SteelBase
{
    public class DrawFoundation
    {
        public static void DrawScatch(Foundation foundation, Canvas canvas)
        {
            canvas.Children.Clear();
            double[] sizes = FoundationProcessor.GetContourSize(foundation);
            double zoom_factor_X = canvas.Width / sizes[0] / 1.2;
            double zoom_factor_Y = canvas.Height / sizes[1] / 1.2;
            double scale_factor;
            if (zoom_factor_X < zoom_factor_Y) { scale_factor = zoom_factor_X; } else { scale_factor = zoom_factor_Y; }
            double[] AxisCenter = new double[2] { canvas.Width / 2, canvas.Height / 2 };
            int count = foundation.Parts.Count;
            for (int i=0; i < count; i++)
            {
                DrawFoundationPart(foundation.Parts[count-i-1], canvas, AxisCenter, scale_factor);
            }
            // Рисуем оси координат
            DrawUtils.DrawAxis(canvas);
        }
        public static void DrawFoundationPart(FoundationPart foundationPart, Canvas canvas, double[] AxisCenter, double scale_factor, int koeffX = 1, int koeffY = 1, double opacity = 0.6, bool showName = true)
        {
            double[] basePartCenter = new double[2] { AxisCenter[0] + foundationPart.CenterX * scale_factor * koeffX, AxisCenter[1] - foundationPart.CenterY * scale_factor * koeffY };
            Rectangle basePartRect = new Rectangle();
            basePartRect.Width = foundationPart.Width * scale_factor;
            basePartRect.Height = foundationPart.Length * scale_factor;
            basePartRect.Fill = Brushes.LightBlue;
            basePartRect.Stroke = Brushes.Blue;
            basePartRect.StrokeThickness = 1;
            basePartRect.Opacity = opacity;
            canvas.Children.Add(basePartRect);
            Canvas.SetLeft(basePartRect, basePartCenter[0] - basePartRect.Width / 2);
            Canvas.SetTop(basePartRect, basePartCenter[1] - basePartRect.Height / 2);
        }
    }
}
