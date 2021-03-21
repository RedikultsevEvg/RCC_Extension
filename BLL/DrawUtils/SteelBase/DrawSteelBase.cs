using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.SC.Column;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using RDBLL.Processors.SC;
using RDBLL.Common.Geometry;

namespace RDBLL.DrawUtils.SteelBase
{
    /// <summary>
    /// Рисует стальную базу на канвасе
    /// </summary>
    public static class DrawSteelBase
    {
        /// <summary>
        /// Рисует стальную базу на канваса
        /// </summary>
        /// <param name="steelBase">Стальная база</param>
        /// <param name="canvas">Канвас</param>
        public static void DrawBase(Entity.SC.Column.SteelBase steelBase, Canvas canvas)
        {
            canvas.Children.Clear();
            double width = 1.0;
            double length = 1.0;
            double zoom_factor_X = canvas.Width / width / 1.2;
            double zoom_factor_Y = canvas.Height / length / 1.2;
            double scale_factor;
            double[] columnBaseCenter = new double[2] { canvas.Width / 2, canvas.Height / 2 };
            if (zoom_factor_X < zoom_factor_Y) { scale_factor = zoom_factor_X; } else { scale_factor = zoom_factor_Y; }
            // Рисуем оси координат
            DrawUtils.DrawAxis(canvas, true, true);

            //Рисуем участки
            foreach (SteelBasePart basePart in steelBase.SteelBaseParts)
            {
                foreach (SteelBasePart part in steelBase.SteelBaseParts)
                {
                    DrawBasePart(part, canvas, columnBaseCenter, scale_factor, 1, 1, 0.8, true);
                }
            }
            //Рисуем болты
            foreach (SteelBolt baseBolt in steelBase.SteelBolts)
            {
                DrawBolts(baseBolt, canvas, columnBaseCenter, scale_factor, 1, 1, 1, true);
            }
        }
        /// <summary>
        /// Рисует участок стальной базы на канвасе
        /// </summary>
        /// <param name="basePart">Участок стальной базы</param>
        /// <param name="canvas">Канвас</param>
        /// <param name="columnBaseCenter">Массив координат центра базы</param>
        /// <param name="scale_factor">масштабный фактор</param>
        /// <param name="koeffX">Коэффициент по X</param>
        /// <param name="koeffY">Коэффициент по Y</param>
        /// <param name="opacity">Значение прозрачности 1 - непрозрачный</param>
        /// <param name="showName">Флаг отрисовки имени участка</param>
        public static void DrawBasePart(SteelBasePart basePart, Canvas canvas, double[] columnBaseCenter, double scale_factor, int koeffX, int koeffY, double opacity, bool showName)
        {
            double[] basePartCenter = new double[2] { columnBaseCenter[0] + basePart.CenterX * scale_factor * koeffX, columnBaseCenter[1] - basePart.CenterY * scale_factor * koeffY };
            Rectangle basePartRect = new Rectangle();
            basePartRect.Width = basePart.Width * scale_factor;
            basePartRect.Height = basePart.Length * scale_factor;
            basePartRect.Fill = Brushes.LightBlue;
            basePartRect.Opacity = opacity;
            canvas.Children.Add(basePartRect);
            Canvas.SetLeft(basePartRect, basePartCenter[0] - basePartRect.Width / 2);
            Canvas.SetTop(basePartRect, basePartCenter[1] - basePartRect.Height / 2);
            Ellipse centerEllipse = new Ellipse();
            centerEllipse.Width = 5;
            centerEllipse.Height = 5;
            centerEllipse.Fill = Brushes.Red;
            centerEllipse.Opacity = opacity;
            canvas.Children.Add(centerEllipse);
            Canvas.SetLeft(centerEllipse, basePartCenter[0] - centerEllipse.Width / 2);
            Canvas.SetTop(centerEllipse, basePartCenter[1] - centerEllipse.Height / 2);
            #region Если требуются закрепления по сторонам, рисуем соответствующие линии
            if (basePart.FixLeft)
            {
                Line borderLeft = new Line();
                borderLeft.X1 = basePartCenter[0] - basePartRect.Width / 2 * koeffX;
                borderLeft.Y1 = basePartCenter[1] + basePartRect.Height / 2 * koeffY;
                borderLeft.X2 = basePartCenter[0] - basePartRect.Width / 2 * koeffX;
                borderLeft.Y2 = basePartCenter[1] - basePartRect.Height / 2 * koeffY;
                borderLeft.Stroke = Brushes.Blue;
                borderLeft.StrokeThickness = 2;
                borderLeft.StrokeDashArray = new DoubleCollection { 5, 2 };
                canvas.Children.Add(borderLeft);
            }
            if (basePart.FixRight)
            {
                Line borderLeft = new Line();
                borderLeft.X1 = basePartCenter[0] + basePartRect.Width / 2 * koeffX;
                borderLeft.Y1 = basePartCenter[1] + basePartRect.Height / 2 * koeffY;
                borderLeft.X2 = basePartCenter[0] + basePartRect.Width / 2 * koeffX;
                borderLeft.Y2 = basePartCenter[1] - basePartRect.Height / 2 * koeffY;
                borderLeft.Stroke = Brushes.Blue;
                borderLeft.StrokeThickness = 2;
                borderLeft.StrokeDashArray = new DoubleCollection { 5, 2 };
                canvas.Children.Add(borderLeft);
            }
            if (basePart.FixTop)
            {
                Line borderLeft = new Line();
                borderLeft.X1 = basePartCenter[0] - basePartRect.Width / 2 * koeffX;
                borderLeft.Y1 = basePartCenter[1] - basePartRect.Height / 2 * koeffY;
                borderLeft.X2 = basePartCenter[0] + basePartRect.Width / 2 * koeffX;
                borderLeft.Y2 = basePartCenter[1] - basePartRect.Height / 2 * koeffY;
                borderLeft.Stroke = Brushes.Blue;
                borderLeft.StrokeThickness = 2;
                borderLeft.StrokeDashArray = new DoubleCollection { 5, 2 };
                canvas.Children.Add(borderLeft);
            }
            if (basePart.FixBottom)
            {
                Line borderLeft = new Line();
                borderLeft.X1 = basePartCenter[0] - basePartRect.Width / 2 * koeffX;
                borderLeft.Y1 = basePartCenter[1] + basePartRect.Height / 2 * koeffY;
                borderLeft.X2 = basePartCenter[0] + basePartRect.Width / 2 * koeffX;
                borderLeft.Y2 = basePartCenter[1] + basePartRect.Height / 2 * koeffY;
                borderLeft.Stroke = Brushes.Blue;
                borderLeft.StrokeThickness = 2;
                borderLeft.StrokeDashArray = new DoubleCollection { 5, 2 };
                canvas.Children.Add(borderLeft);
            }
            #endregion
            //Если требуется указать имя участка, рисуем его
            if (showName)
            {
                TextBlock basePartNameText = new TextBlock();
                basePartNameText.Text = basePart.Name;
                basePartNameText.Foreground = Brushes.Black;
                canvas.Children.Add(basePartNameText);
                Canvas.SetLeft(basePartNameText, basePartCenter[0] - basePartNameText.FontSize);
                Canvas.SetTop(basePartNameText, basePartCenter[1] - basePartNameText.FontSize);
            }
        }
        /// <summary>
        /// Рисует болт стальной базы на канвасе
        /// </summary>
        /// <param name="steelBolt">Болт стальной базы</param>
        /// <param name="canvas">Кпнвас</param>
        /// <param name="columnBaseCenter">Массив координат центра базы</param>
        /// <param name="scale_factor">Масштабный фактор</param>
        /// <param name="koeffX">Коэффициент по X</param>
        /// <param name="koeffY">Коэффициент по Y</param>
        /// <param name="opacity">Значение прозрачности 1 - непрозрачный</param>
        /// <param name="showName">Флаг отображения имени болта</param>
        public static void DrawBolts(SteelBolt steelBolt, Canvas canvas, double[] columnBaseCenter, double scale_factor, int koeffX, int koeffY, double opacity, bool showName)
        {
            foreach (Point2D point in steelBolt.Placement.GetElementPoints())
            {
                Ellipse centerEllipse = new Ellipse();
                centerEllipse.Width = steelBolt.Diameter * scale_factor;
                centerEllipse.Height = centerEllipse.Width;
                centerEllipse.Fill = Brushes.Black;
                centerEllipse.Opacity = opacity;
                canvas.Children.Add(centerEllipse);
                double leftCornerX = columnBaseCenter[0] + point.X * scale_factor * koeffX - centerEllipse.Width / 2;
                double topCornerY = columnBaseCenter[1] - point.Y * scale_factor * koeffY - centerEllipse.Width / 2;
                Canvas.SetLeft(centerEllipse, leftCornerX);
                Canvas.SetTop(centerEllipse, topCornerY);
            }
        }
    }
}
