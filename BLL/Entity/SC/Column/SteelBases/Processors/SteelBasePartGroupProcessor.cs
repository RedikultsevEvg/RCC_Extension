using RDBLL.Common.Geometry;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Processors.SC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Processors
{
    public static class SteelBasePartGroupProcessor
    {
        public static void SolvePartGroup(SteelBasePartGroup partGroup)
        {
            List<string> reportList = new List<string>();
            reportList.Add($"**************************************************************************");
            reportList.Add($"Наименование группы: " + partGroup.Name);
            reportList.Add($"**************************************************************************");
            reportList.Add($"Толщина базы в группе t={partGroup.Height * 1000}мм");
            SteelUsing steel = partGroup.Steel;
            double Ry = (steel.MaterialKind as SteelKind).FstStrength;
            double Ry_R = MathOperation.Round(Ry * MeasureUnitConverter.GetCoefficient(3));
            string unitRy = Ry_R + MeasureUnitConverter.GetUnitLabelText(3);
            reportList.Add("Расчетное сопротивление стали без учета коэффициентов Ry=" + unitRy);
            Ry *= steel.TotalSafetyFactor.PsfFst;
            Ry_R = MathOperation.Round(Ry * MeasureUnitConverter.GetCoefficient(3));
            unitRy = Ry_R + MeasureUnitConverter.GetUnitLabelText(3);
            reportList.Add("Расчетное сопротивление стали с учетом коэффициентов Ry=" + unitRy);

            foreach (SteelBasePart part in partGroup.SteelBaseParts)
            {
                reportList.Add($"******************************************************");
                reportList.Add($"Наименование участка: " + part.Name);
                reportList.Add($"******************************************************");
                reportList.Add($"Ширина участка (размер вдоль оси X): {part.Width}");
                reportList.Add($"Длина участка (размер вдоль оси Y): {part.Length}");
                double moment = SteelBasePartProcessor.GetMoment(part, partGroup.Pressure, reportList);
                double stress = SteelBasePartProcessor.GetPlateStress(moment, partGroup.Height, reportList);
                double recHeight = 0;
                if (stress > 0) { recHeight = partGroup.Height * Math.Pow((stress / Ry), 0.5); }
                double recHeight_R = MathOperation.Round(recHeight * 1000);
                reportList.Add($"Рекомендуемая толщина t={recHeight_R}мм");
            }
            partGroup.ReportList = reportList;
        }
    }
}
