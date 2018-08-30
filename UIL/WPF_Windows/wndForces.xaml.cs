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
using RDBLL.Forces;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для wndForces.xaml
    /// </summary>
    public partial class wndForces : Window
    {
        private BarLoadSet _loadSet;
        public wndForces(BarLoadSet loadSet)
        {
            InitializeComponent();
            _loadSet = loadSet;
            tbxName.Text = _loadSet.Name;
            tbxForce_Nz.Text = Convert.ToString(_loadSet.Force_Nz/1000);
            tbxForce_Mx.Text = Convert.ToString(_loadSet.Force_Mx/1000);
            tbxForce_My.Text = Convert.ToString(_loadSet.Force_My/1000);
            tbxForce_Qx.Text = Convert.ToString(_loadSet.Force_Qx/1000);
            tbxForce_Qy.Text = Convert.ToString(_loadSet.Force_Qy/1000);
            tbxPartialSafetyFactor.Text = Convert.ToString(_loadSet.PartialSafetyFactor);
            cbIsDeadLoad.IsChecked = _loadSet.IsDeadLoad;
            cbBothSign.IsChecked = _loadSet.BothSign;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _loadSet.Name = tbxName.Text;
                _loadSet.Force_Nz = Convert.ToDouble(tbxForce_Nz.Text) * 1000;
                _loadSet.Force_Mx = Convert.ToDouble(tbxForce_Mx.Text) * 1000;
                _loadSet.Force_My = Convert.ToDouble(tbxForce_My.Text) * 1000;
                _loadSet.Force_Qx = Convert.ToDouble(tbxForce_Qx.Text) * 1000;
                _loadSet.Force_Qy = Convert.ToDouble(tbxForce_Qy.Text) * 1000;
                _loadSet.PartialSafetyFactor = Convert.ToDouble(tbxPartialSafetyFactor.Text);
                _loadSet.IsDeadLoad = Convert.ToBoolean(cbIsDeadLoad.IsChecked);
                _loadSet.BothSign = Convert.ToBoolean(cbBothSign.IsChecked);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
