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
            tbxWidth.Text = Convert.ToString(_steelColumnBase.Width);
            tbxLength.Text = Convert.ToString(_steelColumnBase.Length);
            tbxThickness.Text = Convert.ToString(_steelColumnBase.Thickness);
            tbxWidthBoltDist.Text = Convert.ToString(_steelColumnBase.WidthBoltDist);
            tbxLengthBoltDist.Text = Convert.ToString(_steelColumnBase.LengthBoltDist);
            tbxConcreteStrength.Text = Convert.ToString(_steelColumnBase.ConcreteStrength);
            #endregion
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _steelColumnBase.Name = tbxName.Text;
                _steelColumnBase.Width = Convert.ToInt16(tbxWidth.Text);
                _steelColumnBase.Length = Convert.ToInt16(tbxLength.Text);
                _steelColumnBase.Thickness = Convert.ToInt16(tbxThickness.Text);
                _steelColumnBase.WidthBoltDist = Convert.ToInt16(tbxWidthBoltDist.Text);
                _steelColumnBase.LengthBoltDist = Convert.ToInt16(tbxLengthBoltDist.Text);
                _steelColumnBase.ConcreteStrength = Convert.ToDouble(tbxConcreteStrength.Text);
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
    }
}
