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
using RDBLL.Common.Service;

namespace RDUIL.WPF_Windows.BuildingsAndSites
{
    /// <summary>
    /// Логика взаимодействия для wndLevel.xaml
    /// </summary>
    public partial class wndLevel : Window
    {
        private Level _item;
        public wndLevel(Level level)
        {
            _item = level;
            InitializeComponent();
            this.DataContext = _item;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
