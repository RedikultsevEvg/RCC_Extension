using RDBLL.Common.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Results
{
    public class ContourResult
    {
        public int Id { get; set; }
        public PunchingContour PunchingContour { get; set; }
        public Point2D Center { get; set; }
    }
}
