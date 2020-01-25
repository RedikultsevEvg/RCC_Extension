using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RDBLL.Entity.RCC.BuildingAndSite;
using System.Collections.ObjectModel;
using RDBLL.Common.Service;
using Winforms = System.Windows.Forms;
using RDUIL.WPF_Windows.Foundations;
using RDUIL.Common.Reports;
using RDUIL.WPF_Windows.BuildingsAndSites;
using System.Data;


namespace RDUIL.WPF_Windows.BuildingsAndSites
{
    /// <summary>
    /// Логика взаимодействия для wndLevels.xaml
    /// </summary>
    public partial class wndLevels : Window
    {
        private ObservableCollection<Level> _collection;
        private string _childName;
        private Building _building;

        public wndLevels(Building building, ObservableCollection<Level> levels, string childName)
        {
            _building = building;
            _collection = levels;
            _childName = childName;
            InitializeComponent();
            if (_childName == "SteelBases")
            {
                BtnChildItem.ToolTip = "Новая база стальной колонны";
                ChildPng.SetResourceReference(Image.SourceProperty, "IconBase40");
            }
            else if (_childName == "Foundations")
            {
                BtnChildItem.ToolTip = "Новый столбчатый фундамент";
                ChildPng.SetResourceReference(Image.SourceProperty, "IconFoundation40");
            }
            this.DataContext = _collection;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Level level = new Level(_building);
            wndLevel wndLevel = new wndLevel(level);
            wndLevel.ShowDialog();
            if (wndLevel.DialogResult == true)
            {
                DataSet dataSet = ProgrammSettings.CurrentDataSet;
                level.SaveToDataSet(dataSet, true);
                _collection.Add(level);
                ProgrammSettings.IsDataChanged = true;
            }
            ProgrammSettings.IsDataChanged = true;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = LvMain.SelectedIndex;
                    if (LvMain.Items.Count == 1) LvMain.UnselectAll();
                    else if (a < (LvMain.Items.Count - 1)) LvMain.SelectedIndex = a + 1;
                    else LvMain.SelectedIndex = a - 1;
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
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                DataSet dataSet = ProgrammSettings.CurrentDataSet;
                Level level = _collection[a];
                wndLevel wndLevel = new wndLevel(level);
                wndLevel.ShowDialog();
                if (wndLevel.DialogResult == true) { level.SaveToDataSet(dataSet, false); }
                else { level.OpenFromDataSet(dataSet); }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            if (_childName == "SteelBases") { ShowReportProcessor.ShowSteelBasesReport(); }
            if (_childName == "Foundations") { ShowReportProcessor.ShowFoundationsReport(); }
        }

        private void BtnChildItem_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                
                if (_childName == "SteelBases")
                {
                    wndSteelBases childWindow;
                    childWindow = new wndSteelBases(_collection[a]);
                    childWindow.ShowDialog();
                }
                else if (_childName == "Foundations")
                {
                    wndFoundations childWindow;
                    childWindow = new wndFoundations(_collection[a]);
                    childWindow.ShowDialog();
                }              
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
            
        }
        private void BtnBuilding_Click(object sender, RoutedEventArgs e)
        {
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            WndBuilding wndBuilding = new WndBuilding(_building);
            wndBuilding.ShowDialog();
            if (wndBuilding.DialogResult == true) { _building.SaveToDataSet(dataSet, false); }
            else { _building.OpenFromDataSet(dataSet); }
        }
    }
}
