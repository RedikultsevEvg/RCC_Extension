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
using RDBLL.Entity.SC.Column;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WndSteelColumnBase : Window
    {
        private SteelColumnBase _steelColumnBase;

        public WndSteelColumnBase(SteelColumnBase steelColumnBase)
        {
            InitializeComponent();
            _steelColumnBase = steelColumnBase;
            #region Initial parametrs
            tbxName.Text = _steelColumnBase.Name;
            tbxWidth.Text = Convert.ToString(_steelColumnBase.Width*1000);
            tbxLength.Text = Convert.ToString(_steelColumnBase.Length*1000);
            tbxThickness.Text = Convert.ToString(_steelColumnBase.Thickness*1000);
            tbxWidthBoltDist.Text = Convert.ToString(_steelColumnBase.WidthBoltDist*1000);
            tbxLengthBoltDist.Text = Convert.ToString(_steelColumnBase.LengthBoltDist*1000);
            tbxConcreteStrength.Text = Convert.ToString(_steelColumnBase.ConcreteStrength/1000000);
            #endregion
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _steelColumnBase.Name = tbxName.Text;
                _steelColumnBase.Width = Convert.ToDouble(tbxWidth.Text)/1000;
                _steelColumnBase.Length = Convert.ToDouble(tbxLength.Text)/1000;
                _steelColumnBase.Thickness = Convert.ToDouble(tbxThickness.Text)/1000;
                _steelColumnBase.WidthBoltDist = Convert.ToDouble(tbxWidthBoltDist.Text)/1000;
                _steelColumnBase.LengthBoltDist = Convert.ToDouble(tbxLengthBoltDist.Text)/1000;
                _steelColumnBase.ConcreteStrength = Convert.ToDouble(tbxConcreteStrength.Text)*1000000;
                //this.DialogResult = OK;
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

        private void btnForces_Click(object sender, RoutedEventArgs e)
        {
            wndForces wndForces = new wndForces(_steelColumnBase);
            wndForces.ShowDialog();
        }
    }
}
