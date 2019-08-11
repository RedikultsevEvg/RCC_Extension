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
            columnBaseResult.LoadCases = BarLoadSetProcessor.GetLoadCases(columnBase.LoadsGroup);
            RectCrossSection rect = new RectCrossSection(columnBase.Width, columnBase.Length);
            MassProperty massProperty = RectProcessor.GetRectMassProperty(rect);
            double Bx = columnBase.WidthBoltDist / 2;
            double By = columnBase.LengthBoltDist / 2;
            double minStress = double.PositiveInfinity;
            double maxStress = double.NegativeInfinity;

            double dx = massProperty.Xmax;
            double dy = massProperty.Ymax;
            double stress;

            foreach (BarLoadSet loadCase in columnBaseResult.LoadCases)
            {
                for (int i = -1; i <= 1; i+=2)
                {
                    for (int j = -1; j <= 1; j+=2)
                    {
                        stress = BarLoadSetProcessor.StressInBarSection(loadCase, massProperty, dx * i, dy * j);
                        if (stress > maxStress) { maxStress = stress; }
                        if (stress < minStress) { minStress = stress; }
                    }
                }
            }
            columnBaseResult.Stresses.MinStress = minStress;
            columnBaseResult.Stresses.MaxStress = maxStress;
            return columnBaseResult;
        }

    }
}

