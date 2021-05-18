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
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors;

namespace RDBLL.DrawUtils.RCC.Slabs.Punchings
{
    /// <summary>
    /// Процессор создания эскиза узла продавливания
    /// </summary>
    public class PunchingDrawProcessor : IDrawScatch
    {
        public void DrawTopScatch(Canvas canvas, IDsSaveable item)
        {
            //Если переданный объект не является панчингом, выдаем исключение
            if (! (item is Punching)) { throw new Exception("Item type isn't valid"); }
            canvas.Children.Clear();
            Punching punching = item as Punching;
            double width = Math.Max(punching.Width * 3, punching.Length * 3);
            //double width = punching.Width * 3;
            //double length = punching.Length * 3;

            double zoom_factor_X = canvas.Width / width / 1.2;
            double zoom_factor_Y = canvas.Height / width / 1.2;
            //Принимаем минимальный масштаб
            double scale_factor = Math.Min(zoom_factor_X, zoom_factor_Y);
            Point2D center = new Point2D(canvas.Width / 2, canvas.Height / 2);


            // Рисуем оси координат
            CommonDrawUtils.DrawAxis(canvas, true, true);
            //Рисуем изображение колонны
            DrawColumn(canvas, punching, center, scale_factor);
            //Рисуем прямоугольник расчетной области
            DrawSlab(canvas, punching, center, scale_factor);
        }
        public void DrawPunchingContour(Canvas canvas, Punching punching, PunchingContour contour)
        {
            List<ILine2D> lines = new List<ILine2D>();
            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                lines.AddRange(subContour.Lines);
            }
            double[] sizes = GeometryProcessor.GetTotalSizesOfLines(lines);
            double width = Math.Max(Math.Abs(sizes[0]), Math.Abs(sizes[1])) * 2;
            double length = Math.Max(Math.Abs(sizes[2]), Math.Abs(sizes[3])) * 2;

            double zoom_factor_X = canvas.Width / width / 1.2;
            double zoom_factor_Y = canvas.Height / length / 1.2;
            //Принимаем минимальный масштаб
            double scale_factor = Math.Min(zoom_factor_X, zoom_factor_Y);
            Point2D center = new Point2D(canvas.Width / 2, canvas.Height / 2);
            // Рисуем оси координат
            CommonDrawUtils.DrawAxis(canvas, true, true);
            //Рисуем изображение колонны
            DrawColumn(canvas, punching, center, scale_factor);
            //Рисуем прямоугольник расчетной области
            DrawSlab(canvas, punching, center, scale_factor);
            //Рисуем центр тяжести
            DrawGravityCenter(canvas, contour, center, scale_factor);
            //Рисуем субконтуры
            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                DrawSubContour(canvas, subContour, center, scale_factor);
            }
        }

        private void DrawGravityCenter(Canvas canvas, PunchingContour contour, Point2D center, double scale_factor)
        {
            double size = 0.05 * scale_factor;
            Point2D gravCenter = PunchingGeometryProcessor.GetContourCenter(contour);
            Ellipse ellipse = new Ellipse();
            ellipse.Width = ellipse.Height = size;
            ellipse.Fill = Brushes.Black;
            canvas.Children.Add(ellipse);
            double leftCornerX = gravCenter.X * scale_factor + center.X - size / 2;
            double topCornerY = gravCenter.Y * scale_factor * (-1D) + center.Y - size / 2;
            Canvas.SetLeft(ellipse, leftCornerX); 
            Canvas.SetTop(ellipse, topCornerY);
        }

        private void DrawSubContour(Canvas canvas, PunchingSubContour subContour, Point2D center, double scale_factor)
        {
            //Для каждой линии субконтура
            foreach (PunchingLine line in subContour.Lines)
            {
                //создаем новую линию
                Line LineShape = new Line();
                LineShape.X1 = line.StartPoint.X * scale_factor + center.X;
                //Координаты по Y переворачиваем, так как у канваса отсчет идет сверху вниз
                LineShape.Y1 = line.StartPoint.Y * scale_factor * (-1D) + center.Y;
                LineShape.X2 = line.EndPoint.X * scale_factor + center.X;
                //Координаты по Y переворачиваем, так как у канваса отсчет идет сверху вниз
                LineShape.Y2 = line.EndPoint.Y * scale_factor * (-1D) + center.Y;
                //Если линия является несущей
                if (line.IsBearing)
                {
                    //Линия будет красной и толстой
                    LineShape.Stroke = Brushes.Red;
                    LineShape.StrokeThickness = 1;
                }
                else
                {
                    //Иначе линия будет серой и более тонкой
                    LineShape.Stroke = Brushes.Gray;
                    LineShape.StrokeThickness = 0.5;
                }
                //Линя будет пунктирной
                LineShape.StrokeDashArray = new DoubleCollection { 10, 4 };
                canvas.Children.Add(LineShape);
            }
        }

        private void DrawSlab(Canvas canvas, Punching punching, Point2D center, double scale_factor, double opacity = 0.6)
        {
            //Если края перекрытия не указаны, то просто рисуем область в 3 раза больше размера колонны
            double size = Math.Max(punching.Width * 3, punching.Length * 3);
            //Изначально линии ограничивающие прямоугольник перекрытия определяем из его размера
            double posX = size / 2, posY = size / 2;
            double negX = -size / 2, negY = - size / 2;
            //Если для перекрытия указаны края, то указываем четкие размеры до краев
            if (punching.LeftEdge) { negX = - (punching.Width /2 + punching.LeftEdgeDist) + punching.Center.X; }
            if (punching.RightEdge) { posX = punching.RightEdgeDist + punching.Width / 2 + punching.Center.X; }
            if (punching.TopEdge) { posY = punching.TopEdgeDist + punching.Length / 2 + punching.Center.Y; }
            if (punching.BottomEdge) { negY = - (punching.Length / 2 + punching.BottomEdgeDist) + punching.Center.Y; }
            //Ширина и длина прямоугольника перекрытия
            double width = - negX + posX;
            double length = - negY + posY;
            //Центр прямоугольника перекрытия
            Point2D rectCenter = new Point2D((posX + negX) / 2, (posY + negY) / 2);
            //Рисуем прямоугольник перекрытия
            Rectangle rect = new Rectangle();
            rect.Width = width * scale_factor;
            rect.Height = length * scale_factor;
            rect.Fill = Brushes.LightBlue;
            rect.Opacity = opacity;
            //Добавляем прямоугольник перекрытия на канвас
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, center.X + rectCenter.X * scale_factor - rect.Width / 2);
            Canvas.SetTop(rect, center.Y - rectCenter.Y * scale_factor - rect.Height / 2);
            double[] coordinates = new double[] { negX, posX, negY, posY };
            //Рисуем линии контура перекрытия
            DrawSlabLines(punching, coordinates, scale_factor, canvas, center);
        }
        /// <summary>
        /// Добавляет линии для перекрытия при продавливании на канвас
        /// </summary>
        /// <param name="punching"></param>
        /// <param name="coordinates"></param>
        /// <param name="scale_factor"></param>
        /// <param name="canvas"></param>
        /// <param name="center"></param>
        private void DrawSlabLines(Punching punching, double[] coordinates, double scale_factor, Canvas canvas, Point2D center)
        {
            //Координаты линии
            double posX = coordinates[1], posY = - coordinates[3];
            double negX = coordinates[0], negY = - coordinates[2];
            //Точки для построения линий
            Point2D leftBottomPoint = new Point2D(negX * scale_factor + center.X, negY * scale_factor + center.Y);
            Point2D leftTopPoint = new Point2D(negX * scale_factor + center.X, posY * scale_factor + center.Y);
            Point2D rightBottomPoint = new Point2D(posX * scale_factor + center.X, negY * scale_factor + center.Y);
            Point2D rightTopPoint = new Point2D(posX * scale_factor + center.X, posY * scale_factor + center.Y);
            //Если контур перекрытия ограничен, добавляем соответствующие линии
            if (punching.LeftEdge) { DrawSlabLine(leftBottomPoint, leftTopPoint, canvas);}
            if (punching.RightEdge) { DrawSlabLine(rightBottomPoint, rightTopPoint, canvas);}
            if (punching.TopEdge) { DrawSlabLine(leftTopPoint, rightTopPoint, canvas);}
            if (punching.BottomEdge) { DrawSlabLine(leftBottomPoint, rightBottomPoint, canvas); }
        }
        /// <summary>
        /// Добавляет линию перекрытия на канвас
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="brush"></param>
        /// <param name="thickness"></param>
        /// <param name="canvas"></param>
        private void DrawSlabLine(Point2D startPoint, Point2D endPoint, Canvas canvas)
        {
            Line LineShape = new Line();
            LineShape.X1 = startPoint.X;
            LineShape.Y1 = startPoint.Y;
            LineShape.X2 = endPoint.X;
            LineShape.Y2 = endPoint.Y;
            LineShape.Stroke = Brushes.Blue;
            LineShape.StrokeThickness = 1;
            canvas.Children.Add(LineShape);
        }
        /// <summary>
        /// Добавляет прямоугольник перекрытия на канвас
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="punching"></param>
        /// <param name="center"></param>
        /// <param name="scale_factor"></param>
        /// <param name="opacity"></param>
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
