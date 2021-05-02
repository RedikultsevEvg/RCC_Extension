using RDBLL.Common.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Shapes
{
    public interface IPoligone
    {
        /// <summary>
        /// Коллекция точек контура
        /// </summary>
        List<Point2D> Points { get; set; }
        /// <summary>
        /// Признак замкнутости контура
        /// </summary>
        bool IsClosed { get; set; }
    }
}
