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
using RDUIL.Common.Reports;
using RDBLL.Entity.SC.Column.SteelBases.Builders;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для wndSteelBases.xaml
    /// </summary>
    public partial class wndSteelBases : Window
    {
        private Level _level;
        private ObservableCollection<SteelBase> _collection;
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
            BuilderTempate1 builder = new BuilderTempate1();
            SteelBase steelBase = BaseMaker.MakeSteelBase(builder);
            steelBase.RegisterParent(_level);
            steelBase.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
            WndSteelColumnBase wndSteelColumnBase = new WndSteelColumnBase(steelBase);
            wndSteelColumnBase.ShowDialog();
            if (wndSteelColumnBase.DialogResult == true)
            {
                try
                {
                    steelBase.SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                    steelBase.IsActual = false;
                    _level.SteelBases.Add(steelBase);
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
            }
            else
            {
                steelBase.DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
            }
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
                WndSteelColumnBase wndSteelColumnBase = new WndSteelColumnBase(_collection[a]);
                wndSteelColumnBase.ShowDialog();
                if (wndSteelColumnBase.DialogResult == true)
                {
                    try
                    {
                        _collection[a].SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                        _collection[a].IsActual = false;
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
