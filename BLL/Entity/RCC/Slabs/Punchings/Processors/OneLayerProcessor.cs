using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors.Offsets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors
{
    public class OneLayerProcessor : ILayerProcessor
    {

        /// <summary>
        /// Возвращает коллекцию контуров продавливания
        /// </summary>
        /// <param name="punching"></param>
        /// <returns></returns>
        public List<PunchingContour> GetPunchingContours(Punching punching)
        {
            //Если количество слоев равно нулю или больше одного, то выдаем исключение
            if (punching.Children.Count() != 1) { throw new Exception("Count of layers is invalid"); }
            List<PunchingContour> contours = new List<PunchingContour>();
            contours.AddRange(GetFstContours(punching));
            return contours;
        }
        /// <summary>
        /// Определяет рабочую высоту для самого верхнего слоя
        /// </summary>
        /// <param name="punching"></param>
        /// <returns></returns>
        private double GetFstLayerDepth(Punching punching)
        {
            //Определяем высоту самого верхнего слоя
            double fullDepth = (punching.Children[0] as PunchingLayer).Height;
            //Рабочая высота для арматуры вдоль оси X
            double depthX = fullDepth - punching.CoveringLayerX;
            //Рабочая высота для арматуры вдоль оси Y
            double depthY = fullDepth - punching.CoveringLayerY;
            //Возвращаем рабочую высоту как среднюю по осям X и Y
            return (depthX + depthY) / 2;
        }
        /// <summary>
        /// Возвращает прямоугольный контур продавливания с 4-мя несущими сторонами
        /// </summary>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="depth"></param>
        /// <param name="concrete"></param>
        /// <param name="centerPoint"></param>
        /// <returns></returns>
        private PunchingSubContour GetSubContour(double sizeX, double sizeY, double depth, ConcreteUsing concrete, Point2D centerPoint = null)
        {
            PunchingSubContour subContour = new PunchingSubContour();
            Point2D center = centerPoint ?? new Point2D();
            subContour.Concrete = concrete;
            subContour.Height = depth;
            PunchingLine line;
            #region LeftLine
            line = new PunchingLine();
            line.StartPoint = new Point2D(-sizeX / 2 + center.X, - sizeY / 2 + center.Y);
            line.EndPoint = new Point2D(-sizeX / 2 + center.X, sizeY / 2 + center.Y);
            line.HorizontalProjection = depth;
            line.IsBearing = true;
            subContour.Lines.Add(line);
            #endregion
            #region RightLine
            line = new PunchingLine();
            line.StartPoint = new Point2D(sizeX / 2 + center.X, -sizeY / 2 + center.Y);
            line.EndPoint = new Point2D(sizeX / 2 + center.X, sizeY / 2 + center.Y);
            line.HorizontalProjection = depth;
            line.IsBearing = true;
            subContour.Lines.Add(line);
            #endregion
            #region TopLine
            line = new PunchingLine();
            line.StartPoint = new Point2D(- sizeX / 2 + center.X, sizeY / 2 + center.Y);
            line.EndPoint = new Point2D(sizeX / 2 + center.X, sizeY / 2 + center.Y);
            line.HorizontalProjection = depth;
            line.IsBearing = true;
            subContour.Lines.Add(line);
            #endregion
            #region BottomLine
            line = new PunchingLine();
            line.StartPoint = new Point2D(-sizeX / 2 + center.X, - sizeY / 2 + center.Y);
            line.EndPoint = new Point2D(sizeX / 2 + center.X, - sizeY / 2 + center.Y);
            line.HorizontalProjection = depth;
            line.IsBearing = true;
            subContour.Lines.Add(line);
            #endregion
            return subContour;
        }
        /// <summary>
        /// Возвращает контур первого (самого верхнего) слоя
        /// </summary>
        /// <param name="punching"></param>
        /// <returns></returns>
        private List<PunchingContour> GetFstContours(Punching punching)
        {
            List<PunchingContour> contours = new List<PunchingContour>();
            //Для верхнего слоя рабочая высота определяется за минусом защитных слоев
            double fstLayerDepth = GetFstLayerDepth(punching);
            PunchingContour contour;
            contour = new PunchingContour();
            contours.Add(contour);
            //Если для колонны не задано краев, то задаем один замкнутый контур
            if (!punching.LeftEdge & !punching.RightEdge & !punching.TopEdge & !punching.BottomEdge)
            {
                double width = punching.Width + fstLayerDepth;
                double length = punching.Length + fstLayerDepth;
                //Первый контур составляем как замкнутый контур отступая половину рабочей высоты от центра
                PunchingSubContour fstContour = GetSubContour(width, length, fstLayerDepth, (punching.Children[0] as PunchingLayer).Concrete);
                //Так как рассматривается только один слой, то добавляем созданный контур
                contour.SubContours.Add(fstContour);
            }
            else
            {
                //double width = punching.Width;
                //double length = punching.Length;
                //if (punching.LeftEdge & )
            }
            return contours;
        }
        /// <summary>
        /// Возвращает прямоугольный субконтур продавливания, отстоящий от заданных размеров на величину, заданную группой отступов
        /// </summary>
        /// <param name="width">Ширина основания</param>
        /// <param name="length">Длина основания</param>
        /// <param name="offsetGroup">Группа отступов</param>
        /// <param name="height">Высота субконтура</param>
        /// <param name="concrete">Бетон субконтура</param>
        /// <param name="center">Точка центра исходного контура</param>
        /// <returns></returns>
        private PunchingSubContour GetSubContour (double width, double length, RectOffsetGroup offsetGroup, double height, ConcreteUsing concrete, Point2D center = null)
        {
            if (center is null) center = new Point2D();
            width += offsetGroup.LeftOffset.Size + offsetGroup.RightOffset.Size;
            length += offsetGroup.TopOffset.Size + offsetGroup.BottomOffset.Size;
            //полуразмеры исходного контура
            double halfWidth = width / 2;
            double halfLength = length / 2;
            #region Величина отступов
            double leftSize = offsetGroup.LeftOffset.Size;
            double rightSize = offsetGroup.RightOffset.Size;
            double topSize = offsetGroup.TopOffset.Size;
            double bottomSize = offsetGroup.BottomOffset.Size;
            #endregion
            //Создаем новый субконтур
            PunchingSubContour subContour = new PunchingSubContour();
            //Высота субконтура
            subContour.Height = height;
            //Бетон субконтура
            subContour.Concrete = concrete;
            //Линии субконтура
            #region left line
            PunchingLine leftLine;
            leftLine = new PunchingLine();
            leftLine.HorizontalProjection = leftSize;
            leftLine.IsBearing = offsetGroup.LeftOffset.IsBearing;
            leftLine.StartPoint = new Point2D(center.X - (halfWidth + leftSize), -(halfLength + bottomSize));
            leftLine.EndPoint = new Point2D(center.X - (halfWidth + leftSize), (halfLength + topSize));
            subContour.Lines.Add(leftLine);
            #endregion
            #region right line
            PunchingLine rightLine;
            rightLine = new PunchingLine();
            rightLine.HorizontalProjection = rightSize;
            rightLine.IsBearing = offsetGroup.RightOffset.IsBearing;
            rightLine.StartPoint = new Point2D(center.X + (halfWidth + rightSize), -(halfLength + bottomSize));
            rightLine.EndPoint = new Point2D(center.X + (halfWidth + rightSize), (halfLength + topSize));
            subContour.Lines.Add(rightLine);
            #endregion
            #region top line
            PunchingLine topLine;
            topLine = new PunchingLine();
            topLine.HorizontalProjection = topSize;
            topLine.IsBearing = offsetGroup.TopOffset.IsBearing;
            topLine.StartPoint = new Point2D(center.X - (halfWidth + leftSize), (halfLength + topSize));
            topLine.EndPoint = new Point2D(center.X + (halfWidth + rightSize), (halfLength + topSize));
            subContour.Lines.Add(topLine);
            #endregion
            #region bottom line
            PunchingLine bottomLine;
            bottomLine = new PunchingLine();
            bottomLine.HorizontalProjection = bottomSize;
            bottomLine.IsBearing = offsetGroup.BottomOffset.IsBearing;
            bottomLine.StartPoint = new Point2D(center.X - (halfWidth + leftSize), -(halfLength + bottomSize));
            bottomLine.EndPoint = new Point2D(center.X + (halfWidth + rightSize), -(halfLength + bottomSize));
            subContour.Lines.Add(bottomLine);
            #endregion
            return subContour;
        }
    }
}
