using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using System.Windows.Forms;

namespace RDBLL.Processors.Forces
{
    public class BarLoadSetProcessor
    {
        public static void SumForces(BarLoadSet oldLoadSet, BarLoadSet secondLoadSet, double koeff)
        {
            if (! String.IsNullOrEmpty(oldLoadSet.Name)) { oldLoadSet.Name += " + "; }
            oldLoadSet.Name += secondLoadSet.Name + "*(" + Convert.ToString(secondLoadSet.PartialSafetyFactor * koeff) + ")";
            oldLoadSet.Force_Nz += secondLoadSet.Force_Nz * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.Force_Mx += secondLoadSet.Force_Mx * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.Force_My += secondLoadSet.Force_My * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.Force_Qx += secondLoadSet.Force_Qx * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.Force_Qy += secondLoadSet.Force_Qy * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.PartialSafetyFactor = 1;
            return;
        }

        public static BarLoadSet SumForcesInNew (BarLoadSet firstLoadSet, BarLoadSet secondLoadSet, double koeff)
        {
            BarLoadSet newLoadSet = new BarLoadSet(0);
            SumForces(newLoadSet, firstLoadSet, 1.0);
            SumForces(newLoadSet, secondLoadSet, koeff);
            return newLoadSet;
        }

        public static List<BarLoadSet> GetLoadCases(List<BarLoadSet> columnLoadSets)
        {
            List<BarLoadSet> LoadCases = new List<BarLoadSet>();
            LoadCases.Add(new BarLoadSet(0));
            List<BarLoadSet> tmpLoadSets = new List<BarLoadSet>();
            foreach (BarLoadSet columnLoadSet in columnLoadSets)
            {
                tmpLoadSets.Add(columnLoadSet);
            }
            while (tmpLoadSets.Count > 0)
            {
                BarLoadSet LoadSet = tmpLoadSets[0];
                List<BarLoadSet> tmpLoadCases = new List<BarLoadSet>();
                foreach (BarLoadSet LoadCase in LoadCases)
                {
                    tmpLoadCases.Add(LoadCase);
                }
                foreach (BarLoadSet LoadCase in tmpLoadCases)
                {
                    if (LoadSet.IsDeadLoad)
                    {
                        BarLoadSetProcessor.SumForces(LoadCase, LoadSet, 1.0);
                    }
                    else
                    {
                        LoadCases.Add(BarLoadSetProcessor.SumForcesInNew(LoadCase, LoadSet, 1.0));
                        if (LoadSet.BothSign) { LoadCases.Add(BarLoadSetProcessor.SumForcesInNew(LoadCase, LoadSet, -1.0)); }
                    }
                }
                tmpLoadSets.Remove(LoadSet);
            }
            return LoadCases;
        }

    }
}
