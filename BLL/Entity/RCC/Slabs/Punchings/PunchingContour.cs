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
    public class PunchingContour
    {
        public List<PunchingSubContour> SubContours { get; set; }
        public PunchingContour()
        {
            SubContours = new List<PunchingSubContour>();
        }
    }
}
