using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;

namespace RDBLL.Processors.Forces
{
    public class ColumnLoadSetProcessor
    {
        public static void SumForces(ColumnLoadSet OldLoadSet, ColumnLoadSet SecondLoadSet, double koeff)
        {
            if (! String.IsNullOrEmpty(OldLoadSet.Name)) { OldLoadSet.Name += " + "; }
            OldLoadSet.Name += SecondLoadSet.Name + "*(" + Convert.ToString(koeff)+")";
            OldLoadSet.Force_Nz += SecondLoadSet.Force_Nz * SecondLoadSet.PartialSafetyFactor * koeff;
            OldLoadSet.Force_Mx += SecondLoadSet.Force_Mx * SecondLoadSet.PartialSafetyFactor * koeff;
            OldLoadSet.Force_My += SecondLoadSet.Force_My * SecondLoadSet.PartialSafetyFactor * koeff;
            OldLoadSet.Force_Qx += SecondLoadSet.Force_Qx * SecondLoadSet.PartialSafetyFactor * koeff;
            OldLoadSet.Force_Qy += SecondLoadSet.Force_Qy * SecondLoadSet.PartialSafetyFactor * koeff;
            return;
        }

        public static ColumnLoadSet SumForcesInNew (ColumnLoadSet firstLoadSet, ColumnLoadSet secondLoadSet, double koeff)
        {
            ColumnLoadSet newLoadSet = new ColumnLoadSet();
            newLoadSet.Name = "";
            newLoadSet.Force_Nz = 0;
            SumForces(newLoadSet, firstLoadSet, 1.0);
            SumForces(newLoadSet, firstLoadSet, 1.0);
            return newLoadSet;
        }

    }
}
