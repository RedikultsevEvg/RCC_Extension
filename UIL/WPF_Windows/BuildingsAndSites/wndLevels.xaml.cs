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

namespace RDUIL.WPF_Windows.BuildingsAndSites
{
    /// <summary>
    /// Логика взаимодействия для wndLevels.xaml
    /// </summary>
    public partial class wndLevels : Window
    {
        private ObservableCollection<Level> _levels;
        public wndLevels(ObservableCollection<Level> levels)
        {
            _levels = levels;
            InitializeComponent();
            this.DataContext = _levels;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
