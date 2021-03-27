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

namespace RDBLL.DrawUtils.SteelBases
{
    /// <summary>
    /// Рисование эскиза фундамента
    /// </summary>
    public class DrawFoundation
    {
        /// <summary>
        /// Рисование эскиза в плане
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <param name="canvas">Канвас, на котором происходит рисование</param>
        public static void DrawTopScatch(Foundation foundation, Canvas canvas)
        {
            canvas.Children.Clear();
            double[] sizes = FoundationProcessor.GetContourSize(foundation);
            //Проверка на действительность размеров необходима для правильного вычисления масштаба
            //Если ступеней нет, то ничего не рисуем
            if (sizes[0] > 0 & sizes[1] > 0)
            {
                double zoom_factor_X = canvas.Width / sizes[0] / 1.2;
                double zoom_factor_Y = canvas.Height / sizes[1] / 1.2;
                double scale_factor = Math.Min(zoom_factor_X, zoom_factor_Y);
                double[] AxisCenter = new double[2] { canvas.Width / 2, canvas.Height / 2 };
                int count = foundation.Parts.Count;
                for (int i = 0; i < count; i++)
                {
                    double opacity = 0.6;
                    if (i == count - 1) { opacity = 1.0; }
                    DrawTopFoundationPart(foundation.Parts[count - i - 1], canvas, AxisCenter, scale_factor, 1, 1, opacity);
                }
            }
            // Рисуем оси координат
            DrawUtils.DrawAxis(canvas, true, true);
        }
        /// <summary>
        /// Рисование ступеней фундамента в плане
        /// </summary>
        /// <param name="foundationPart"></param>
        /// <param name="canvas"></param>
        /// <param name="AxisCenter">Массив с координатами осей</param>
        /// <param name="scale_factor"></param>
        /// <param name="koeffX"></param>
        /// <param name="koeffY"></param>
        /// <param name="opacity"> Прозрачность</param>
        /// <param name="showName"></param>
        public static void DrawTopFoundationPart(RectFoundationPart foundationPart, Canvas canvas, double[] AxisCenter, double scale_factor, int koeffX = 1, int koeffY = 1, double opacity = 0.6, bool showName = true)
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
