using RDBLL.Common.Interfaces;
using RDBLL.DrawUtils.Interfaces;
using RDBLL.Entity.RCC.Slabs.Punchings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RDBLL.DrawUtils.SteelBases;
using RDBLL.Common.Geometry;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RDBLL.DrawUtils.RCC.Slabs.Punchings
{
    public class PunchingDrawProcessor : IDrawScatch
    {
        public void DrawTopScatch(Canvas canvas, IDsSaveable item)
        {
            canvas.Children.Clear();
            if (item is Punching)
            {
                Punching punching = item as Punching;
                double width = Math.Max(punching.Width * 3, punching.Length * 3);
                //double width = punching.Width * 3;
                //double length = punching.Length * 3;

                double zoom_factor_X = canvas.Width / width / 1.2;
                double zoom_factor_Y = canvas.Height / width / 1.2;
                double scale_factor;
                Point2D center = new Point2D(canvas.Width / 2, canvas.Height / 2 );
                //Принимаем минимальный масштаб
                scale_factor = Math.Min(zoom_factor_X, zoom_factor_Y);
                // Рисуем оси координат
                CommonDrawUtils.DrawAxis(canvas, true, true);
                //Рисуем изображение колонны
                DrawColumn(canvas, punching, center, scale_factor);
                //Рисуем прямоугольник расчетной области
                DrawSlab(canvas, punching, center, scale_factor);
            }
            else { throw new Exception("Item type isn't valid"); }
        }

        private void DrawSlab(Canvas canvas, Punching punching, Point2D center, double scale_factor, double opacity = 0.6)
        {
            Rectangle columnRect = new Rectangle();
            double width = Math.Max(punching.Width * 3, punching.Length * 3) * scale_factor;
            columnRect.Width = width;
            columnRect.Height = width;
            columnRect.Fill = Brushes.LightBlue;
            columnRect.Stroke = Brushes.Blue;
            columnRect.StrokeThickness = 1;
            columnRect.Opacity = opacity;
            canvas.Children.Add(columnRect);
            Canvas.SetLeft(columnRect, center.X - columnRect.Width / 2);
            Canvas.SetTop(columnRect, center.Y - columnRect.Height / 2);
        }

        private void DrawColumn(Canvas canvas, Punching punching, Point2D center, double scale_factor, double opacity = 0.6)
        {
            Rectangle columnRect = new Rectangle();
            columnRect.Width = punching.Width * scale_factor;
            columnRect.Height = punching.Length * scale_factor;
            columnRect.Fill = Brushes.LightBlue;
            columnRect.Stroke = Brushes.Red;
            columnRect.StrokeThickness = 1;
            columnRect.Opacity = opacity;
            canvas.Children.Add(columnRect);
            Canvas.SetLeft(columnRect, center.X - columnRect.Width / 2);
            Canvas.SetTop(columnRect, center.Y - columnRect.Height / 2);
        }
    }
}
