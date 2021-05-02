using RDBLL.Common.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings
{
    public class PunchingLine
    {
        /// <summary>
        /// Точка начала линии
        /// </summary>
        public Point2D StartPoint { get; set; }
        /// <summary>
        /// Точка конца линии
        /// </summary>
        public Point2D EndPoint { get; set; }
        /// <summary>
        /// Признак несущей линии
        /// </summary>
        public bool IsBearing { get; set; }
    }
}
