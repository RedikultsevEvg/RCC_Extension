using RDBLL.Common.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Shapes
{
    public interface ILine2D
    {
        /// <summary>
        /// Точка начала линии
        /// </summary>
        Point2D StartPoint { get; set; }
        /// <summary>
        /// Точка конца линии
        /// </summary>
        Point2D EndPoint { get; set; }
    }
}
