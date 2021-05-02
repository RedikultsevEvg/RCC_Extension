using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings
{
    /// <summary>
    /// Класс контура продавливания для расчета
    /// </summary>
    public class PunchingContour : IPoligone
    {
        /// <summary>
        /// Коллекция точек контура
        /// </summary>
        public List<Point2D> Points { get; set; }
        /// <summary>
        /// Признак замкнутости контура
        /// </summary>
        public bool IsClosed { get; set; }

        public PunchingContour()
        {
            Points = new List<Point2D>();
        }
    }
}
