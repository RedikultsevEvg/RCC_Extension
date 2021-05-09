using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Results
{
    public class PunchingCalcResult
    {
        public int Id { get; set; }
        public Punching Punching { get; set; }
        public LoadSet LoadSet { get; set; }
        public PunchingContour PunchingContour {get;set;}
        public double BearingCoef { get; set; }
    }
}
