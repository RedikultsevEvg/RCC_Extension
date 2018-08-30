using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using RDBLL.Forces;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.Results.SC;
using RDBLL.Processors.Forces;

namespace RDBLL.Processors.SC
{
    public class SteelColumnBaseProcessor : ISteelBaseProcessor
    {
        private List<BarLoadSet> GetLoadCases (List<BarLoadSet> columnLoadSets)
        {
            List<BarLoadSet> LoadCases = new List<BarLoadSet>();
            LoadCases.Add(new BarLoadSet(0));
            List<BarLoadSet> tmpLoadSets = new List<BarLoadSet>();
            foreach (BarLoadSet columnLoadSet in columnLoadSets )
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
                                ColumnLoadSetProcessor.SumForces(LoadCase, LoadSet, 1.0);
                            }
                            else
                            {
                                LoadCases.Add(ColumnLoadSetProcessor.SumForcesInNew(LoadCase, LoadSet, 1.0));
                                if (LoadSet.BothSign) { LoadCases.Add(ColumnLoadSetProcessor.SumForcesInNew(LoadCase, LoadSet, -1.0)); }
                            }
                        }
                    tmpLoadSets.Remove(LoadSet);
                 }
            return LoadCases;
        }

        public ColumnBaseResult GetResult(SteelColumnBase columnBase)
        {
            ColumnBaseResult columnBaseResult = new ColumnBaseResult();
            columnBaseResult.LoadCases = GetLoadCases(columnBase.Loads);
            double Nz;
            double Mx;
            double My;
            double A = columnBase.Width * columnBase.Length;
            double Wx = columnBase.Width * columnBase.Length * columnBase.Length / 6;
            double Wy = columnBase.Length * columnBase.Width * columnBase.Width / 6;
            double Bx = columnBase.WidthBoltDist / 2;
            double By = columnBase.LengthBoltDist / 2;
            double minStress = 10000000;
            double maxStress = -10000000;
            double minStressTmp;
            double maxStressTmp;
            foreach (BarLoadSet LoadCase in columnBaseResult.LoadCases)
            {
                Nz = LoadCase.Force_Nz;
                Mx = Math.Abs(LoadCase.Force_Mx);
                My = Math.Abs(LoadCase.Force_My);
                minStressTmp = Nz / A - Mx / Wx - My / Wy;
                maxStressTmp = Nz / A + Mx / Wx + My / Wy;
                if (minStressTmp < minStress) { minStress = minStressTmp; }
                if (maxStressTmp > maxStress) { maxStress = maxStressTmp; }
                //Console.WriteLine("Time = " + DateTime.Now);
                //Console.WriteLine("Name = " + LoadCase.Name);
                //Console.WriteLine("Force_Nz = " + Convert.ToString(LoadCase.Force_Nz));
                //Console.WriteLine("Force_Mx = " + Convert.ToString(LoadCase.Force_Mx));
                //Console.WriteLine("PartialSafetyFactor = " + Convert.ToString(LoadCase.PartialSafetyFactor));
                //Console.ReadLine();
            }
            columnBaseResult.MinStress = minStress;
            columnBaseResult.MaxStress = maxStress;
            return columnBaseResult;
        }

    }
}

