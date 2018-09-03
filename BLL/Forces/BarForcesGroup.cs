using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;

namespace RDBLL.Forces
{
    public class BarForcesGroup
    {
        public List<BarLoadSet> Loads { get; set; }
        public Point2D Excentricity { get; set; }
    }
}
