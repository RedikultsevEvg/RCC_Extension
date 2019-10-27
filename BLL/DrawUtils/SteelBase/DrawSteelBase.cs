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

namespace RDBLL.DrawUtils.SteelBase
{
    public static class DrawSteelBase
    {
        public static void DrawBase(Entity.SC.Column.SteelBase steelColumnBase, Canvas canvas)
        {
            canvas.Children.Clear();
            double zoom_factor_X = canvas.Width / steelColumnBase.Width / 1.2;
            double zoom_factor_Y = canvas.Height / steelColumnBase.Length / 1.2;
            double scale_factor;
            double[] columnBaseCenter = new double[2] { canvas.Width / 2, canvas.Height / 2 };
            if (zoom_factor_X < zoom_factor_Y) { scale_factor = zoom_factor_X; } else { scale_factor = zoom_factor_Y; }
            #region Рисуем прямоугольник для базы
            Rectangle columnBaseRect = new Rectangle();
            columnBaseRect.Width = steelColumnBase.Width * scale_factor;
            columnBaseRect.Height = steelColumnBase.Length * scale_factor;
            columnBaseRect.Fill = Brushes.Gray;
            columnBaseRect.Opacity = 0.3;
            columnBaseRect.Stroke = Brushes.Black;
            columnBaseRect.StrokeThickness = 3;
            canvas.Children.Add(columnBaseRect);
            Canvas.SetLeft(columnBaseRect, columnBaseCenter[0] - columnBaseRect.Width / 2);
            Canvas.SetTop(columnBaseRect, columnBaseCenter[1] - columnBaseRect.Height / 2);
            #endregion
            #region Рисуем оси координат
            //Рисуем ось X
            Line axisX = new Line();
            axisX.X1 = columnBaseCenter[0] - columnBaseRect.Width / 2 * 1.2;
            axisX.Y1 = columnBaseCenter[1];
            axisX.X2 = columnBaseCenter[0] + columnBaseRect.Width / 2 * 1.2;
            axisX.Y2 = columnBaseCenter[1];
            axisX.Stroke = Brushes.Red;
            axisX.StrokeThickness = 1;
            axisX.StrokeDashArray = new DoubleCollection { 10, 4 };
            canvas.Children.Add(axisX);
            //Рисуем ось Y
            Line axisY = new Line();
            axisY.X1 = columnBaseCenter[0];
            axisY.Y1 = columnBaseCenter[1] - columnBaseRect.Height / 2 * 1.2;
            axisY.X2 = columnBaseCenter[0];
            axisY.Y2 = columnBaseCenter[1] + columnBaseRect.Height / 2 * 1.2;
            axisY.Stroke = Brushes.Red;
            axisY.StrokeThickness = 1;
            axisY.StrokeDashArray = new DoubleCollection { 10, 4 };
            canvas.Children.Add(axisY);
            #endregion
            //Рисуем участки
            foreach (SteelBasePart basePart in steelColumnBase.SteelBaseParts)
            {
                foreach (SteelBasePart steelBasePart in steelColumnBase.SteelBaseParts)
                {
                    foreach (SteelBasePart steelBasePartEh in SteelBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart))
                    {
                        DrawBasePart(steelBasePartEh, canvas, columnBaseCenter, scale_factor, 1, 1, 0.8, true);
                    }
                }
            }
            //Рисуем болты
            foreach (SteelBolt baseBolt in steelColumnBase.SteelBolts)
            {
                foreach (SteelBolt baseBoltEh in SteelBoltProcessor.GetSteelBoltsFromBolt(baseBolt))
                {
                    DrawBolts(baseBoltEh, canvas, columnBaseCenter, scale_factor, 1, 1, 1, true);
                }
            }
        }
        //метод отрисовки участков базы
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
        public static void DrawBolts(SteelBolt steelBolt, Canvas canvas, double[] columnBaseCenter, double scale_factor, int koeffX, int koeffY, double opacity, bool showName)
        {
            Ellipse centerEllipse = new Ellipse();
            centerEllipse.Width = steelBolt.Diameter * scale_factor;
            centerEllipse.Height = centerEllipse.Width;
            centerEllipse.Fill = Brushes.Black;
            centerEllipse.Opacity = opacity;
            canvas.Children.Add(centerEllipse);
            double leftCornerX = columnBaseCenter[0] + steelBolt.CenterX * scale_factor * koeffX- centerEllipse.Width/2;
            double topCornerY = columnBaseCenter[1] - steelBolt.CenterY * scale_factor * koeffY- centerEllipse.Width/2;
            Canvas.SetLeft(centerEllipse, leftCornerX);
            Canvas.SetTop(centerEllipse, topCornerY);
        }
    }
}
