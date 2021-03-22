using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Params;
using RDBLL.Common.Service;
using System.Drawing;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Geometry;


namespace RDBLL.Entity.Common.Placements
{
    public class LineBySpacing : LinePlacement
    {
        new public int ParamQuant { get { return 7; } }
        /// <summary>
        /// Шаг элементов вдоль линии (максимальный)
        /// </summary>
        public double Spacing
        {
          get { return StoredParams[0 + ParamQuant].GetDoubleValue(); }
          set { StoredParams[0 + ParamQuant].SetDoubleValue( value); }
        }
        public LineBySpacing(bool genId = false) : base (genId)
        {
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Spacing" });
            StoredParams[0 + ParamQuant].SetDoubleValue(0.2);
        }

        /// <summary>
        /// Возвращает коллекцию точек расположения элементов
        /// </summary>
        /// <returns></returns>
        public override List<Point2D> GetElementPoints()
        {
            return GeometryProc.GetInternalPoints(StartPoint, EndPoint, Spacing, AddStart, AddEnd);
        }
    }
}
