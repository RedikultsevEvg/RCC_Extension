using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Entity.Common.Materials;
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
            if (punching.Layers.Count() != 1) { throw new Exception("Count of layers is invalid"); }
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
            double fullDepth = punching.Layers[0].Height;
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
            line.IsBearing = true;
            subContour.Lines.Add(line);
            #endregion
            #region RightLine
            line = new PunchingLine();
            line.StartPoint = new Point2D(sizeX / 2 + center.X, -sizeY / 2 + center.Y);
            line.EndPoint = new Point2D(sizeX / 2 + center.X, sizeY / 2 + center.Y);
            line.IsBearing = true;
            subContour.Lines.Add(line);
            #endregion
            #region TopLine
            line = new PunchingLine();
            line.StartPoint = new Point2D(- sizeX / 2 + center.X, sizeY / 2 + center.Y);
            line.EndPoint = new Point2D(sizeX / 2 + center.X, sizeY / 2 + center.Y);
            line.IsBearing = true;
            subContour.Lines.Add(line);
            #endregion
            #region BottomLine
            line = new PunchingLine();
            line.StartPoint = new Point2D(-sizeX / 2 + center.X, - sizeY / 2 + center.Y);
            line.EndPoint = new Point2D(sizeX / 2 + center.X, - sizeY / 2 + center.Y);
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
            double width = punching.Width + fstLayerDepth;
            double length = punching.Length + fstLayerDepth;
            //Первый контур составляем как замкнутый контур отступая половину рабочей высоты от центра
            PunchingSubContour fstContour = GetSubContour(width, length, fstLayerDepth, punching.Layers[0].Concrete);
            //Так как рассматривается только один слой, то добавляем созданный контур
            PunchingContour contour;
            contour = new PunchingContour();
            contour.SubContours.Add(fstContour);
            contours.Add(contour);
            return contours;
        }
    }
}
