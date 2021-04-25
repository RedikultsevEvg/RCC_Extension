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
using System.Data;
using RDStartWPF.Infrasructure.Reports;
using RDStartWPF.Views.SC.Columns.Bases;
using RDStartWPF.Views.RCC.Foundations;

namespace RDStartWPF.Views.Common.BuildingsAndSites
{
    /// <summary>
    /// Логика взаимодействия для wndLevels.xaml
    /// </summary>
    public partial class wndLevels : Window
    {
        private ObservableCollection<Level> _collection;
        private LvlChildType _ChildType;
        private Building _building;

        public wndLevels(Building building, ObservableCollection<Level> levels, LvlChildType childType)
        {
            _building = building;
            _collection = levels;
            _ChildType = childType;
            InitializeComponent();
            switch (_ChildType)
            {
                case LvlChildType.SteelBase:
                    {
                        BtnChildItem.ToolTip = "Новая база стальной колонны";
                        ChildPng.SetResourceReference(Image.SourceProperty, "IconBase40");
                        break;
                    }
                case LvlChildType.SteelBasePartGroup:
                    {
                        BtnChildItem.ToolTip = "Новая группа участвов базы стальной колонны";
                        ChildPng.SetResourceReference(Image.SourceProperty, "IconBase40");
                        break;
                    }
                case LvlChildType.Foundation:
                    {
                        BtnChildItem.ToolTip = "Новый столбчатый фундамент";
                        ChildPng.SetResourceReference(Image.SourceProperty, "IconFoundation40");
                        break;
                    }
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
            switch (_ChildType)
            {
                case LvlChildType.SteelBase:
                    {
                        ShowReportProcessor.ShowSteelBasesReport();
                        break;
                    }
                case LvlChildType.Foundation:
                    {
                        ShowReportProcessor.ShowFoundationsReport();
                        break;
                    }
            }
        }

        private void BtnChildItem_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                wndLevelChilds childWindow;
                childWindow = new wndLevelChilds(_collection[a], _ChildType);
                childWindow.ShowDialog();
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
