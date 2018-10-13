using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using RDBLL.Forces;
using RDBLL.Entity.Results.Forces;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.Results.SC;
using RDBLL.Processors.Forces;
using RDBLL.Common.Geometry;

namespace RDBLL.Processors.SC
{
    public class SteelColumnBaseProcessor : ISteelBaseProcessor
    {
        public ColumnBaseResult GetResult(SteelColumnBase columnBase)
        {
            ColumnBaseResult columnBaseResult = new ColumnBaseResult();
            //columnBaseResult.LoadCases = BarLoadSetProcessor.GetLoadCases(columnBase.Loads);
            RectCrossSection rect = new RectCrossSection(columnBase.Width, columnBase.Length);
            MassProperty massProperty = RectProcessor.GetRectMassProperty(rect);
            double Bx = columnBase.WidthBoltDist / 2;
            double By = columnBase.LengthBoltDist / 2;
            //double minStress = double.PositiveInfinity;
            //double maxStress = double.NegativeInfinity;
            //double minStressTmp;
            //double maxStressTmp;
            //foreach (BarLoadSet LoadCase in columnBaseResult.LoadCases)
            //{
            //    StressInRect stress = BarLoadSetProcessor.MinMaxStressInBarSection(LoadCase, massProperty);
            //    minStressTmp = stress.MinStress;
            //    maxStressTmp = stress.MaxStress;
            //    if (minStressTmp < minStress) { minStress = minStressTmp; }
            //    if (maxStressTmp > maxStress) { maxStress = maxStressTmp; }
            //}
            columnBaseResult.MinStress = 0;// minStress;
            columnBaseResult.MaxStress = 0;// maxStress;
            return columnBaseResult;
        }

    }
}

