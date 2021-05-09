using CSL.Reports;
using RDBLL.Common.Service;
using RDStartWPF.InfraStructure.ControlClasses;
using RDUIL.WPF_Windows;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using RDStartWPF.Views.Common.Service;
using CSL.Reports.RCC.Slabs.Punchings;
using CSL.Reports.Interfaces;
using System;

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

        public static void ShowPunchingsReport()
        {
            //Строка пути к файлу отчета
            string directory = Directory.GetCurrentDirectory() + "\\Reports\\RCC\\Slabs\\Punchings\\";
            //Создаем карточки отчетов
            List<ReportCard> reportCards = PunchingsCardsFactory(directory);
            //Показываем окно для выбора карточки расчета
            if (reportCards.Count > 1)
            {
                wndReports wndReports = new wndReports(reportCards);
                wndReports.ShowDialog();
            }
            //Если карточка только одна, то сразу выводим отчет
            else if  (reportCards.Count == 1)
            {
                reportCards[0].RunCommand();
            }
        }

        private static List<ReportCard> PunchingsCardsFactory(string directory)
        {
            List<ReportCard> reportCards = new List<ReportCard>();
            ReportCard newReportCard;
            newReportCard = new ReportCard
            {
                Name = "Основной отчет по расчету на продавливание",
                FileName = directory + "Punchings.frx",
                Description = "Основной отчет по результатам расчета на продавливание",
                ImageName = directory + "Punchings.png",
                ToolTip = ""
            };
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowPunchingReport));
            reportCards.Add(newReportCard);
            return reportCards;
        }

        private static void ShowSteelReport(string reportFileName)
        {
            SteelBaseReport resultReport = new SteelBaseReport(ProgrammSettings.BuildingSite);
            resultReport.ShowReport(reportFileName);
        }

        private static void ShowFoundationReport(string reportFileName)
        {
            IReport report = new FoundationReport(ProgrammSettings.BuildingSite);
            report.ShowReport(reportFileName);
        }

        private static void ShowPunchingReport(string reportFileName)
        {
            IReport report = new PunchingReport(ProgrammSettings.BuildingSite);
            report.ShowReport(reportFileName);
        }
    }
}
