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
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            _collection.Add(new Foundation(_level));
            ProgrammSettings.IsDataChanged = true;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ProgrammSettings.IsDataChanged = true;
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
    }
}
