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
    /// Логика взаимодействия для SteelBasePart.xaml
    /// </summary>
    public partial class WndSteelBasePart : Window
    {
        private SteelBasePart _steelBasePart;

        public WndSteelBasePart(SteelBasePart steelBasePart)
        {
            InitializeComponent();
            _steelBasePart = steelBasePart;
            #region
            cbFixLeft.IsChecked = _steelBasePart.FixLeft;
            cbFixRight.IsChecked = _steelBasePart.FixRight;
            cbFixTop.IsChecked = _steelBasePart.FixTop;
            cbFixBottom.IsChecked = _steelBasePart.FixBottom;
            tbxWidth.Text = Convert.ToString(_steelBasePart.Width);
            tbxLength.Text = Convert.ToString(_steelBasePart.Length);
            #endregion
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _steelBasePart.FixLeft = Convert.ToBoolean(cbFixLeft.IsChecked);
                _steelBasePart.FixRight = Convert.ToBoolean(cbFixRight.IsChecked);
                _steelBasePart.FixTop = Convert.ToBoolean(cbFixTop.IsChecked);
                _steelBasePart.FixBottom = Convert.ToBoolean(cbFixBottom.IsChecked);
                _steelBasePart.Width = Convert.ToInt16(tbxWidth.Text);
                _steelBasePart.Length = Convert.ToInt16(tbxLength.Text);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
        }
    }
}
