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
    public class ColumBaseProcessor : ISteelBaseProcessor
    {
        private List<ColumnLoadSet> GetLoadCases (List<ColumnLoadSet> columnLoadSets)
        {
            List<ColumnLoadSet> LoadCases = new List<ColumnLoadSet>();
            List<ColumnLoadSet> tmpLoadSets = new List<ColumnLoadSet>();
            foreach (ColumnLoadSet columnLoadSet in columnLoadSets )
                {
                    tmpLoadSets.Add(columnLoadSet);
                }
            while (tmpLoadSets.Count > 0)
                {
                    ColumnLoadSet LoadSet = tmpLoadSets[1];
                    if (LoadCases.Count == 0) { LoadCases.Add(LoadSet); }
                    else
                    {
                        List<ColumnLoadSet> tmpLoadCases = new List<ColumnLoadSet>();
                        foreach (ColumnLoadSet LoadCase in LoadCases)
                        {
                            tmpLoadCases.Add(LoadCase);
                        }
                        foreach (ColumnLoadSet LoadCase in tmpLoadCases)
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
            double minStress = 0;
            double maxStress = 0;
            double minStressTmp;
            double maxStressTmp;
            foreach (ColumnLoadSet LoadCase in columnBaseResult.LoadCases)
            {
                Nz = LoadCase.Force_Nz;
                Mx = Math.Abs(LoadCase.Force_Mx);
                My = Math.Abs(LoadCase.Force_My);
                minStressTmp = Nz / A - Mx / Wx - My / Wy;
                maxStressTmp = Nz / A + Mx / Wx + My / Wy;
                if (minStressTmp < minStress) { minStress = minStressTmp; }
                if (maxStressTmp > maxStress) { maxStress = maxStressTmp; }
            }
            columnBaseResult.MinStress = minStress;
            columnBaseResult.MaxStress = maxStress;
            return columnBaseResult;
        }

    }
}

