using CSL.Reports;
using RDBLL.Common.Service;
using RDUIL.WPF_Windows;
using RDUIL.WPF_Windows.ControlClasses;
using System.Collections.Generic;
using System.IO;

namespace RDUIL.Common.Reports
{
    public static class ShowReportProcessor
    {
        public static void ShowSteelBasesReport()
        {
            List<ReportCard> reportCards = new List<ReportCard>();
            ReportCard newReportCard;
            string directory = Directory.GetCurrentDirectory() + "\\Reports\\SteelBases\\";
            newReportCard = new ReportCard
            {
                Name = "Основной отчет по базам колонн",
                FileName = directory + "SteelBases.frx",
                Description = "Краткий отчет с выводом давления под подошвой фундамента, напряжений в опорной плите и усилий в болтах",
                ImageName = directory + "SteelBases.png",
                ToolTip = ""
            };
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowSteelReport));
            reportCards.Add(newReportCard);

            newReportCard = new ReportCard
            {
                Name = "Задание на фундаменты",
                FileName = directory + "Assignment.frx",
                Description = "Вывод нагрузок на фундаменты в виде таблицы",
                ImageName = directory + "Assignment.png",
                ToolTip = ""
            };
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowSteelReport));
            reportCards.Add(newReportCard);

            wndReports wndReports = new wndReports(reportCards);
            wndReports.ShowDialog();
        }

        private static void ShowSteelReport(string reportFileName)
        {
            SteelBaseReport resultReport = new SteelBaseReport(ProgrammSettings.BuildingSite);
            resultReport.ShowReport(reportFileName);
        }
    }
}
