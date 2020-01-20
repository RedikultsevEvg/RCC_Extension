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
            _collection.Add(new Foundation(_level));
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
                wndFoundation wndChild = new wndFoundation(_collection[a]);
                wndChild.ShowDialog();
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
