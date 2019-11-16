using CSL.Reports;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.SC.Column;
using RDUIL.WPF_Windows.ControlClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Winforms = System.Windows.Forms;


namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для wndSteelBases.xaml
    /// </summary>
    public partial class wndSteelBases : Window
    {
        private Level _level;
        private ObservableCollection<SteelBase> _steelBases;
        public wndSteelBases(Level level)
        {
            _level = level;
            _steelBases = _level.SteelBases;
            InitializeComponent();
            this.DataContext = level.SteelBases;           
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
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
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowReport));
            reportCards.Add(newReportCard);

            newReportCard = new ReportCard
            {
                Name = "Задание на фундаменты",
                FileName = directory + "Assignment.frx",
                Description = "Вывод нагрузок на фундаменты в виде таблицы",
                ImageName = directory + "Assignment.png",
                ToolTip = ""
            };
            newReportCard.RegisterDelegate(new ReportCard.CommandDelegate(ShowReport));
            reportCards.Add(newReportCard);

            wndReports wndReports = new wndReports(reportCards);
            wndReports.ShowDialog();
        }

        private void ShowReport(string reportFileName)
        {
            ResultReport resultReport = new ResultReport(_level.Building.BuildingSite);
            resultReport.PrepareReport();
            resultReport.ShowReport(reportFileName);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            SteelBase steelBase = new SteelBase(_level);
            ProgrammSettings.IsDataChanged = true;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (LvSteelBases.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = LvSteelBases.SelectedIndex;
                    if (LvSteelBases.Items.Count == 1) LvSteelBases.UnselectAll();
                    else if (a < (LvSteelBases.Items.Count - 1)) LvSteelBases.SelectedIndex = a + 1;
                    else LvSteelBases.SelectedIndex = a - 1;
                    _steelBases.RemoveAt(a);
                    ProgrammSettings.IsDataChanged = true;
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvSteelBases.SelectedIndex >= 0)
            {
                int a = LvSteelBases.SelectedIndex;
                WndSteelColumnBase wndSteelColumnBase = new WndSteelColumnBase(_steelBases[a]);
                wndSteelColumnBase.ShowDialog();
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void TbxName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ProgrammSettings.IsDataChanged = true;
        }
    }
}
