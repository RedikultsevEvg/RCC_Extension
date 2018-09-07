using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using RDBLL.Entity.Results.Forces;
using System.Windows.Forms;
using RDBLL.Common.Geometry;

namespace RDBLL.Processors.Forces
{
    public class BarLoadSetProcessor
    {
        public static void SumForces(BarLoadSet oldLoadSet, BarLoadSet secondLoadSet, double koeff)
        {
            if (! String.IsNullOrEmpty(oldLoadSet.Name)) { oldLoadSet.Name += " + "; }
            oldLoadSet.Name += secondLoadSet.Name + "*(" + Convert.ToString(secondLoadSet.PartialSafetyFactor * koeff) + ")";
            oldLoadSet.Force.Force_Nz += secondLoadSet.Force.Force_Nz * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.Force.Force_Mx += secondLoadSet.Force.Force_Mx * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.Force.Force_My += secondLoadSet.Force.Force_My * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.Force.Force_Qx += secondLoadSet.Force.Force_Qx * secondLoadSet.PartialSafetyFactor * koeff;
            oldLoadSet.Force.Force_Qy += secondLoadSet.Force.Force_Qy * secondLoadSet.PartialSafetyFactor * koeff;
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
        public static StressInRect MInMaxStressInBarSection(BarLoadSet loadCase, MassProperty massProperty, double dx, double dy)
        {
            StressInRect stress = new StressInRect();
            double Nz = loadCase.Force.Force_Nz;
            double Mx = loadCase.Force.Force_Mx;
            double My = loadCase.Force.Force_My;
            double A = massProperty.A;
            double Ix = massProperty.Ix;
            double Iy = massProperty.Iy;
            stress.MinStress = Nz / A - Math.Abs(Mx / (Ix / dy)) - Math.Abs(My / (Iy / dx));
            stress.MaxStress = Nz / A + Math.Abs(Mx / (Ix / dy)) + Math.Abs(My / (Iy / dx));
            return stress;
        }
        public static StressInRect MinMaxStressInBarSection(BarLoadSet loadCase, MassProperty massProperty)
        {
            StressInRect stress = new StressInRect();
            double Nz = loadCase.Force.Force_Nz;
            double Mx = loadCase.Force.Force_Mx;
            double My = loadCase.Force.Force_My;
            double A = massProperty.A;
            double Wx = massProperty.Wx;
            double Wy = massProperty.Wy;
            stress.MinStress = Nz / A - Math.Abs(Mx / Wx) - Math.Abs(My / Wy);
            stress.MaxStress = Nz / A + Math.Abs(Mx / Wx) + Math.Abs(My / Wy);
            return stress;
        }

        public static double StressInBarSection(BarLoadSet loadCase, MassProperty massProperty, double dx, double dy)
        {
            double stress;
            double Nz = loadCase.Force.Force_Nz;
            double Mx = loadCase.Force.Force_Mx;
            double My = loadCase.Force.Force_My;
            double A = massProperty.A;
            double Ix = massProperty.Ix;
            double Iy = massProperty.Iy;
            stress = Nz / A + Mx / (Ix / dy) - My / (Iy / dx);
            return stress;
        }
    }
}
