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
        public Curvature ConcreteCurvature { get; set; }
        public Curvature SteelCurvature { get; set; }

        public ForceCurvature(LoadSet loadSet, SumForces sumForces, Curvature concreteCurvature, Curvature steelCurvature)
        {
            this.LoadSet = loadSet;
            this.SumForces = sumForces;
            this.ConcreteCurvature = concreteCurvature;
            this.SteelCurvature = steelCurvature;
        }

        public ForceCurvature(LoadSet loadSet, Curvature concreteCurvature, Curvature steelCurvature)
        {
            this.LoadSet = loadSet;
            this.SumForces = new SumForces(loadSet);
            this.ConcreteCurvature = concreteCurvature;
            this.SteelCurvature = steelCurvature;
        }
        public ForceCurvature(LoadSet loadSet, Curvature curvature)
        {
            this.LoadSet = loadSet;
            this.SumForces = new SumForces(loadSet);
            this.ConcreteCurvature = curvature;
            this.SteelCurvature = curvature;
        }
    }
}
