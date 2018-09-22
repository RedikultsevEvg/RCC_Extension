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
        private Force _force;
        private Force _tmpForce;
        private BarLoadSet _loadSet;
        public wndForces(BarLoadSet loadSet)
        {
            InitializeComponent();
            _loadSet = loadSet;
            tbxName.Text = _loadSet.LoadSet.Name;
            tbxForce_Nz.Text = Convert.ToString(_loadSet.Force.Force_Nz /1000);
            tbxForce_Mx.Text = Convert.ToString(_loadSet.Force.Force_Mx /1000);
            tbxForce_My.Text = Convert.ToString(_loadSet.Force.Force_My /1000);
            tbxForce_Qx.Text = Convert.ToString(_loadSet.Force.Force_Qx /1000);
            tbxForce_Qy.Text = Convert.ToString(_loadSet.Force.Force_Qy /1000);
            tbxPartialSafetyFactor.Text = Convert.ToString(_loadSet.LoadSet.PartialSafetyFactor);
            cbIsDeadLoad.IsChecked = _loadSet.LoadSet.IsDeadLoad;
            cbBothSign.IsChecked = _loadSet.LoadSet.BothSign;
            _force = _loadSet.Force;
            _tmpForce = (Force)_loadSet.Force.Clone();
            lvForcesList.ItemsSource = _force.ForceParameters;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _loadSet.LoadSet.Name = tbxName.Text;
                _loadSet.Force.Force_Nz = Convert.ToDouble(tbxForce_Nz.Text) * 1000;
                _loadSet.Force.Force_Mx = Convert.ToDouble(tbxForce_Mx.Text) * 1000;
                _loadSet.Force.Force_My = Convert.ToDouble(tbxForce_My.Text) * 1000;
                _loadSet.Force.Force_Qx = Convert.ToDouble(tbxForce_Qx.Text) * 1000;
                _loadSet.Force.Force_Qy = Convert.ToDouble(tbxForce_Qy.Text) * 1000;
                _loadSet.LoadSet.PartialSafetyFactor = Convert.ToDouble(tbxPartialSafetyFactor.Text);
                _loadSet.LoadSet.IsDeadLoad = Convert.ToBoolean(cbIsDeadLoad.IsChecked);
                _loadSet.LoadSet.BothSign = Convert.ToBoolean(cbBothSign.IsChecked);
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
