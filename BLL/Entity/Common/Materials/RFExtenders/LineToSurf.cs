using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Placements;
using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;

namespace RDBLL.Entity.Common.Materials.RFExtenders
{
    /// <summary>
    /// Абстрактный класс адаптера от раскладки по линии к раскладки по грани
    /// </summary>
    public abstract class LineToSurf : CoveredExtender
    {
        /// <summary>
        /// Величина защитного слоя арматуры, м
        /// </summary>
        public override double CoveringLayer
        { get
            {
                LinePlacement linePlacement = Placement as LinePlacement;
                return linePlacement.StartPoint.Y;
            }
            set
            {
                LinePlacement linePlacement = Placement as LinePlacement;
                linePlacement.StoredParams[2].SetDoubleValue(value);
                linePlacement.StoredParams[4].SetDoubleValue(value);
                //Point2D startPoint = new Point2D(linePlacement.StartPoint.X, value);
                //Point2D endPoint = new Point2D(linePlacement.EndPoint.X, value);
                //linePlacement.StartPoint = startPoint;
                //linePlacement.EndPoint = endPoint;
            }
        }
        /// <summary>
        /// Устанавливает начало и конец раскладки арматуры
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        public void SetPointsX(double startX, double endX)
        {
            LinePlacement linePlacement = Placement as LinePlacement;
            Point2D startPoint = new Point2D(startX, linePlacement.StartPoint.Y);
            Point2D endPoint = new Point2D(endX, linePlacement.EndPoint.Y);
            linePlacement.StartPoint = startPoint;
            linePlacement.EndPoint = endPoint;
        }
    }
}
