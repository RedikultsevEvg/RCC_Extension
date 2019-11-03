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
using RDBLL.Common.Service;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для wndMeasureUnits.xaml
    /// </summary>
    public partial class wndMeasureUnits : Window
    {
        public wndMeasureUnits()
        {
            InitializeComponent();
            //this.DataContext = ProgrammSettings.MeasureUnits;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            ProgrammSettings.IsDataChanged = true;
            this.Close();
        }
    }
}
