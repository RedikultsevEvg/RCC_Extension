using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using System.Windows.Forms;

namespace RDBLL.Processors.Forces
{
    public class ColumnLoadSetProcessor
    {
        public static void SumForces(ColumnLoadSet oldLoadSet, ColumnLoadSet secondLoadSet, double koeff)
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

        public static ColumnLoadSet SumForcesInNew (ColumnLoadSet firstLoadSet, ColumnLoadSet secondLoadSet, double koeff)
        {
            ColumnLoadSet newLoadSet = new ColumnLoadSet(0);
            SumForces(newLoadSet, firstLoadSet, 1.0);
            SumForces(newLoadSet, secondLoadSet, koeff);
            return newLoadSet;
        }

    }
}
