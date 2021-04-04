using CSL.Reports;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.SC.Column;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Winforms = System.Windows.Forms;
using RDBLL.Entity.SC.Column.SteelBases.Builders;
using RDStartWPF.Infrasructure.Reports;
using RDBLL.Entity.SC.Column.SteelBases.Patterns;
using RDStartWPF.Views.Common.Patterns;
using RDStartWPF.Views.Common.Patterns.ControlClasses;
using RDBLL.Common.Interfaces;
using RDBLL.Entity.RCC.Foundations;
using RDStartWPF.Views.RCC.Foundations;

namespace RDStartWPF.Views.SC.Columns.Bases
{
    /// <summary>
    /// Логика взаимодействия для wndSteelBases.xaml
    /// </summary>
    public partial class wndSteelBases : Window
    {
        private Level _level;
        private ObservableCollection<IHasParent> _collection;
        public wndSteelBases(Level level)
        {
            _level = level;
            _collection = _level.SteelBases;
            InitializeComponent();
            this.DataContext = level.SteelBases;           
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            ShowReportProcessor.ShowSteelBasesReport();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            List<PatternCard> patternCards = SelectPatternProcessor.SelectSteelBasePattern(_level);
            WndPatternSelect wndPatternSelect = new WndPatternSelect(patternCards);
            wndPatternSelect.ShowDialog();
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
                    _collection[a].DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
                    _collection.RemoveAt(a);
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
                bool dialogResult = false;
                if (_collection[a] is SteelBase)
                {
                    SteelBase child = _collection[a] as SteelBase;
                    WndSteelColumnBase wnd = new WndSteelColumnBase(child);
                    wnd.ShowDialog();
                    if (wnd.DialogResult == true) dialogResult = true;
                }
                if (_collection[a] is Foundation)
                {
                    Foundation child = _collection[a] as Foundation;
                    wndFoundation wnd = new wndFoundation(child);
                    wnd.ShowDialog();
                    if (wnd.DialogResult == true) dialogResult = true;
                }
                if (dialogResult)
                {
                    try
                    {
                        _collection[a].SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                        (_collection[a] as SteelBase).IsActual = false;
                        ProgrammSettings.IsDataChanged = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка сохранения :" + ex);
                    }
                }
                else
                {
                    _collection[a].OpenFromDataSet(ProgrammSettings.CurrentDataSet);
                }
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
