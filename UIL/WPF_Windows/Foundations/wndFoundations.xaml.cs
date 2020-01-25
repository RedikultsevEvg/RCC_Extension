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
using System.Collections.ObjectModel;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Common.Service;
using RDUIL.Common.Reports;
using Winforms = System.Windows.Forms;
using System.Data;

namespace RDUIL.WPF_Windows.Foundations
{
    /// <summary>
    /// Логика взаимодействия для wndFoundations.xaml
    /// </summary>
    public partial class wndFoundations : Window
    {
        private Level _level;
        private ObservableCollection<Foundation> _collection;

        public wndFoundations(Level level)
        {
            _level = level;
            _collection = _level.Foundations;
            InitializeComponent();
            this.DataContext = _collection;
            if (_collection.Count > 0) { LvMain.SelectedIndex = 0; }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Foundation foundation = new Foundation(_level);
            wndFoundation wndChild = new wndFoundation(foundation);
            wndChild.ShowDialog();
            if (wndChild.DialogResult == true)
            {
                DataSet dataSet = ProgrammSettings.CurrentDataSet;
                foundation.SaveToDataSet(dataSet, true);
                _collection.Add(foundation);
                ProgrammSettings.IsDataChanged = true;
            }

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
                Foundation foundation = _collection[a];
                wndFoundation wndChild = new wndFoundation(foundation);
                wndChild.ShowDialog();
                if (wndChild.DialogResult == true) { foundation.SaveToDataSet(dataSet, false); }
                else { foundation.OpenFromDataSet(dataSet); }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            ShowReportProcessor.ShowFoundationsReport();
        }
    }
}
