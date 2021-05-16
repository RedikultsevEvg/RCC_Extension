using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors.Offsets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors
{
    public class MultiLayersProcessor : ILayerProcessor
    {
        public List<PunchingContour> GetPunchingContours(Punching punching)
        {
            List<PunchingContour> contours = new List<PunchingContour>();
            //Коллекция истинных (за вычитом защитного слоя) слоев продавливания
            List<PunchingLayer> layers = PunchingGeometryProcessor.GetTrueLayers(punching);
            //Коллекция слоев снизу вверх
            List<PunchingLayer> invLayer = PunchingGeometryProcessor.GetInvertLayers(layers);
            //Суммарная толщина контура продавливания
            double totalHeight = PunchingGeometryProcessor.GetTotalLayerHeight(layers);
            #region Коллекции отступов для составления контуров продавливания
            //Отступы слева
            List<Offset> contourLeftSizes = GetOffsets(punching.LeftEdge, totalHeight, punching.LeftEdgeDist);
            //Отступы справа
            List<Offset> contourRightSizes = GetOffsets(punching.RightEdge, totalHeight, punching.RightEdgeDist);
            //Отступы сверху
            List<Offset> contourTopSizes = GetOffsets(punching.TopEdge, totalHeight, punching.TopEdgeDist);
            //Отступы снизу
            List<Offset> contourBottomSizes = GetOffsets(punching.BottomEdge, totalHeight, punching.BottomEdgeDist);
            #endregion
            //Проходим по всем возможным левым сторонам контуров
            foreach (Offset leftSize in contourLeftSizes)
            {
                //То же по правым сторонам
                foreach (Offset rightSize in contourRightSizes)
                {
                    //То же по верхним сторонам
                    foreach (Offset topSize in contourTopSizes)
                    {
                        //То же по нижним сторонам
                        foreach (Offset bottomSize in contourBottomSizes)
                        {
                            PunchingContour contour = new PunchingContour();
                            double sumLeftDist = 0,  sumRightDist = 0, sumTopDist = 0, sumBottomDist = 0;
                            IRectangle rectangle = punching;
                            //Проходимо по коллекции слоев панчинга для составления контуров
                            foreach (PunchingLayer layer in invLayer)
                            {
                                rectangle = GeometryProcessor.GetRectangleOffset(rectangle, new double[4] { sumLeftDist, sumRightDist, sumTopDist, sumBottomDist });
                                double height = layer.Height;
                                ConcreteUsing concrete = layer.Concrete;
                                Offset leftOffset = GetOffset(leftSize.Size, totalHeight, height + sumLeftDist, leftSize.IsBearing);
                                sumLeftDist = leftOffset.Size;
                                Offset rightOffset = GetOffset(rightSize.Size, totalHeight, height + sumRightDist, rightSize.IsBearing);
                                sumRightDist = rightOffset.Size;
                                Offset topOffset = GetOffset(topSize.Size, totalHeight, height + sumTopDist, topSize.IsBearing);
                                sumTopDist = topOffset.Size;
                                Offset bottomOffset = GetOffset(bottomSize.Size, totalHeight, height + sumBottomDist, bottomSize.IsBearing);
                                sumBottomDist = bottomOffset.Size;
                                RectOffsetGroup offsetGroup = new RectOffsetGroup { LeftOffset = leftOffset, RightOffset = rightOffset, TopOffset = topOffset, BottomOffset = bottomOffset};
                                PunchingSubContour subContour = PunchingGeometryProcessor.GetRectSubContour(rectangle, height, concrete, offsetGroup);
                                contour.SubContours.Add(subContour);
                            }
                            contours.Add(contour);
                        }
                    }
                }
            }
            return contours;
        }
        /// <summary>
        /// Возвращает отступ от исходного контура
        /// </summary>
        /// <param name="size"></param>
        /// <param name="totalHeight"></param>
        /// <param name="height"></param>
        /// <param name="isBearing"></param>
        /// <returns></returns>
        private Offset GetOffset(double size, double totalHeight, double height, bool isBearing)
        {
            double dist;
            //Если линия несущая, то значит это не край контура и тогда оффсет определяем с учетом толщины конкретного слоя
            // по линейной интерполяции
            if (isBearing)
            {
                dist = size / totalHeight * height;
            }
            else //Иначе размер субконтура будет определяться до края
            {
                dist = size;
            }
            Offset offset = new Offset { Size = dist, IsBearing = isBearing };
            return offset;
        }
        /// <summary>
        /// Возвращает группу офсетов для построения расчетных контуров
        /// </summary>
        /// <param name="hasEdge">Признак наличия ребра вблизи исходного контура</param>
        /// <param name="height">Высота контура</param>
        /// <param name="edgeDistance">Расстояние до края</param>
        /// <returns></returns>
        private List<Offset> GetOffsets(bool hasEdge, double height, double edgeDistance)
        {
            List<Offset> contourSizes = new List<Offset>();
            //Если для стороны не указано наличие края
            if (! hasEdge)
            {
                //Добавляем край контура, расположенный на половине рабочей высоты
                double dist = height / 2;
                bool isBearing = true;
                contourSizes.Add(new Offset { Size = dist, IsBearing = isBearing });
                return contourSizes;
            }
            else //Если по соответствующей стороне указано наличие края
            {
                //Контур до края создаем в любом случае
                double dist = edgeDistance;
                bool isBearing = false;
                contourSizes.Add(new Offset { Size = dist, IsBearing = isBearing });
                //Если край находится дальше чем, половина толщины плиты
                if (edgeDistance > height / 2)
                {
                    //Добавляем край контура, расположенный на половине рабочей высоты
                    dist = height / 2;
                    isBearing = true;
                    contourSizes.Add(new Offset { Size = dist, IsBearing = isBearing });
                }
            }
            return contourSizes;
        }
        private List<PunchingLine> GetPunchingLines(RectOffsetGroup offsetGroup, IRectangle rectangle, double totalHeight, double height)
        {
            List<PunchingLine> lines = new List<PunchingLine>();
            double width = rectangle.Width;
            double length = rectangle.Length;
            Point2D center = rectangle.Center;

            Offset leftOffset = offsetGroup.LeftOffset;
            return lines;
        }
    }
}
