using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;

namespace RDBLL.Entity.Common.NDM
{
    public class ForceCurvature
    {
        public LoadSet LoadSet { get; set; }
        public SumForces SumForces { get; set; }
        public Curvature Curvature { get; set; }

        public ForceCurvature(LoadSet loadSet, SumForces sumForces, Curvature curvature)
        {
            this.LoadSet = loadSet;
            this.SumForces = sumForces;
            this.Curvature = curvature;
        }

        public ForceCurvature(LoadSet loadSet, Curvature curvature)
        {
            this.LoadSet = loadSet;
            this.SumForces = new SumForces(loadSet);
            this.Curvature = curvature;
        }
    }
}
