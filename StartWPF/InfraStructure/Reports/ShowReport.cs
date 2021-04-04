﻿using CSL.Reports;
using RDBLL.Common.Service;
using RDStartWPF.InfraStructure.ControlClasses;
using RDUIL.WPF_Windows;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using RDStartWPF.Views.Common.Service;

namespace RDStartWPF.Infrasructure.Reports
{
    /// <summary>
    /// Процессор показа отчетов
    /// </summary>
    public static class ShowReportProcessor
    {
        /// <summary>
        /// Отчеты для баз стальных колонн
        /// </summary>
        public static void ShowSteelBasesReport()
        {
            List<ReportCard> reportCards = new List<ReportCard>();
            ReportCard newReportCard;
            string directory = Directory.GetCurrentDirectory() + "\\Reports\\SteelBases\\";
            newReportCard = new ReportCard
            {
                Name = "Основной отчет по базам колонн",
                FileName = directory + "SteelBases.frx",
                Description = "Краткий отчет с выводом давления под подошвой базы колонны, напряжений в опорной плите и усилий в болтах",
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

        public static void ShowFoundationsReport()
        {
            List<ReportCard> reportCards = new List<ReportCard>();
            ReportCard newReportCard;
            string directory = Directory.GetCurrentDirectory() + "\\Reports\\Foundations\\";
            newReportCard = new ReportCard
            {
                Name = "Основной отчет по фундаментам",
                FileName = directory + "Foundations.frx",
                Description = "Краткий отчет с выводом давления под подошвой фундамента",
                ImageName = directory + "Foundations.png",
                ToolTip = ""
            };
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowFoundationReport));
            reportCards.Add(newReportCard);

            newReportCard = new ReportCard
            {
                Name = "Отчет по комбинациям нагрузок",
                FileName = directory + "ForceCombinations.frx",
                Description = "Полный отчет по нагрузкам, приведенным к подошве",
                ImageName = directory + "Foundations.png",
                ToolTip = ""
            };
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowFoundationReport));
            reportCards.Add(newReportCard);

            newReportCard = new ReportCard
            {
                Name = "Отчет по расчету осадки",
                FileName = directory + "Settlements.frx",
                Description = "Полный отчет по методу послойного суммирования",
                ImageName = directory + "Foundations.png",
                ToolTip = ""
            };
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowFoundationReport));
            reportCards.Add(newReportCard);

            newReportCard = new ReportCard
            {
                Name = "Отчет по напряжениям под подошвой",
                FileName = directory + "Stresses.frx",
                Description = "Полный отчет по напряжениям под подошвой",
                ImageName = directory + "Foundations.png",
                ToolTip = ""
            };
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowFoundationReport));
            reportCards.Add(newReportCard);

            wndReports wndReports = new wndReports(reportCards);
            wndReports.ShowDialog();
        }

        private static void ShowSteelReport(string reportFileName)
        {
            SteelBaseReport resultReport = new SteelBaseReport(ProgrammSettings.BuildingSite);
            resultReport.ShowReport(reportFileName);
        }

        private static void ShowFoundationReport(string reportFileName)
        {
            FoundationReport resultReport = new FoundationReport(ProgrammSettings.BuildingSite);
            //MessageBox.Show("Точка расчета 01");
            resultReport.ShowReport(reportFileName);
            //MessageBox.Show("Точка расчета 02");
        }
    }
}