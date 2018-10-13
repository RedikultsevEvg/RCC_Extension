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
            if (! String.IsNullOrEmpty(oldLoadSet.LoadSet.Name)) { oldLoadSet.LoadSet.Name += " + "; }
            oldLoadSet.LoadSet.Name += secondLoadSet.LoadSet.Name + "*(" + Convert.ToString(secondLoadSet.LoadSet.PartialSafetyFactor * koeff) + ")";
            //oldLoadSet.Force.Force_Nz += secondLoadSet.Force.Force_Nz * secondLoadSet.LoadSet.PartialSafetyFactor * koeff;
            //oldLoadSet.Force.Force_Mx += secondLoadSet.Force.Force_Mx * secondLoadSet.LoadSet.PartialSafetyFactor * koeff;
            //oldLoadSet.Force.Force_My += secondLoadSet.Force.Force_My * secondLoadSet.LoadSet.PartialSafetyFactor * koeff;
            //oldLoadSet.Force.Force_Qx += secondLoadSet.Force.Force_Qx * secondLoadSet.LoadSet.PartialSafetyFactor * koeff;
            //oldLoadSet.Force.Force_Qy += secondLoadSet.Force.Force_Qy * secondLoadSet.LoadSet.PartialSafetyFactor * koeff;
            //oldLoadSet.LoadSet.PartialSafetyFactor = 1;
            return;
        }

        public static BarLoadSet SumForcesInNew (BarLoadSet firstLoadSet, BarLoadSet secondLoadSet, double koeff)
        {
            BarLoadSet newLoadSet = new BarLoadSet(0);
            SumForces(newLoadSet, firstLoadSet, 1.0);
            SumForces(newLoadSet, secondLoadSet, koeff);
            return newLoadSet;
        }

        public void SumLoadSets(LoadSet fstLoadSet, LoadSet secLoadSet, double koeff)
        {
            int I;
            for (I = 0; I <= 5; I++)
            {
                fstLoadSet.ForceParameters[I].Value += secLoadSet.ForceParameters[I].Value*koeff;
            }
        }

        public static List<LoadCase> LoadCases(List<ForcesGroup> forcesGroups)
        {
            List<LoadCase> LoadCases = new List<LoadCase>();

            foreach (ForcesGroup forcesGroup in forcesGroups)
            {
                
                LoadCase loadCase = new LoadCase();
                List<LoadCase> tmpLoadCases = new List<LoadCase>();
                //На первом проходе суммируем нагрузки одного, если таковые имеются
                List<LoadSet> tmpLoadSets = new List<LoadSet>(); 
                foreach (BarLoadSet LoadSet in forcesGroup.Loads)
                {
                    LoadSet tmpLoadSet = new LoadSet();
                    tmpLoadSet.Name = LoadSet.LoadSet.Name;
                    tmpLoadSet.IsDeadLoad = LoadSet.LoadSet.IsDeadLoad;
                    tmpLoadSet.PartialSafetyFactor = LoadSet.LoadSet.PartialSafetyFactor;
                    int I;
                    for (I = 1; I <= 6; I++)
                    {
                        tmpLoadSet.ForceParameters.Add(new ForceParameter { Value = 0, Kind_id = I });
                    }
                    foreach (ForceParameter forceParameter in LoadSet.LoadSet.ForceParameters)
                    {
                        tmpLoadSet.ForceParameters[forceParameter.Kind_id].Value += forceParameter.Value;
                    }
                    tmpLoadSets.Add(tmpLoadSet);
                }

                while (tmpLoadSets.Count>0)
                {
                    LoadSet curLoadSet = tmpLoadSets[0];
                    foreach (LoadSet loadSet in tmpLoadSets)
                    {
                        if (loadSet.IsDeadLoad)
                        {

                        }
                    }
                    LoadCase tmpLoadCase = new LoadCase();

                }


                while (tmpLoadSets.Count > 0)
                {
                    BarLoadSet LoadSet = tmpLoadSets[0];
                    List<BarLoadSet> tmpLoadCases = new List<BarLoadSet>();
                    foreach (BarLoadSet LoadCase in forcesGroup.Loads)
                    {
                        tmpLoadCases.Add(LoadCase);
                    }
                    foreach (BarLoadSet LoadCase in tmpLoadCases)
                    {
                        if (LoadSet.LoadSet.IsDeadLoad)
                        {
                            BarLoadSetProcessor.SumForces(LoadCase, LoadSet, 1.0);
                        }
                        else
                        {
                            LoadCases.Add(BarLoadSetProcessor.SumForcesInNew(LoadCase, LoadSet, 1.0));
                            if (LoadSet.LoadSet.BothSign) { LoadCases.Add(BarLoadSetProcessor.SumForcesInNew(LoadCase, LoadSet, -1.0)); }
                        }
                    }
                    tmpLoadSets.Remove(LoadSet);
                }
                LoadCases.Add(loadCase);
            }
            

            //LoadCases.Add(new BarLoadSet(0));
            //List<BarLoadSet> tmpLoadSets = new List<BarLoadSet>();
            //foreach (BarLoadSet columnLoadSet in columnLoadSets)
            //{
            //    tmpLoadSets.Add(columnLoadSet);
            //}
            //while (tmpLoadSets.Count > 0)
            //{
            //    BarLoadSet LoadSet = tmpLoadSets[0];
            //    List<BarLoadSet> tmpLoadCases = new List<BarLoadSet>();
            //    foreach (BarLoadSet LoadCase in LoadCases)
            //    {
            //        tmpLoadCases.Add(LoadCase);
            //    }
            //    foreach (BarLoadSet LoadCase in tmpLoadCases)
            //    {
            //        if (LoadSet.LoadSet.IsDeadLoad)
            //        {
            //            BarLoadSetProcessor.SumForces(LoadCase, LoadSet, 1.0);
            //        }
            //        else
            //        {
            //            LoadCases.Add(BarLoadSetProcessor.SumForcesInNew(LoadCase, LoadSet, 1.0));
            //            if (LoadSet.LoadSet.BothSign) { LoadCases.Add(BarLoadSetProcessor.SumForcesInNew(LoadCase, LoadSet, -1.0)); }
            //        }
            //    }
            //    tmpLoadSets.Remove(LoadSet);
            //}
            return LoadCases;
        }
        public static StressInRect MInMaxStressInBarSection(BarLoadSet loadCase, MassProperty massProperty, double dx, double dy)
        {
            StressInRect stress = new StressInRect();
            double Nz = 0; // loadCase.Force.Force_Nz;
            double Mx = 0; //loadCase.Force.Force_Mx;
            double My = 0; //loadCase.Force.Force_My;
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
            double Nz = 0; //loadCase.Force.Force_Nz;
            double Mx = 0; //loadCase.Force.Force_Mx;
            double My = 0; //loadCase.Force.Force_My;
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
            double Nz = 0; //loadCase.Force.Force_Nz;
            double Mx = 0; //loadCase.Force.Force_Mx;
            double My = 0; //loadCase.Force.Force_My;
            double A = massProperty.A;
            double Ix = massProperty.Ix;
            double Iy = massProperty.Iy;
            stress = Nz / A + Mx / (Ix / dy) - My / (Iy / dx);
            return stress;
        }
    }
}
