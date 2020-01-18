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
using RDBLL.Entity.Soils;

namespace RDUIL.WPF_Windows.Foundations.Soils
{
    /// <summary>
    /// Логика взаимодействия для WndClaySoil.xaml
    /// </summary>
    public partial class WndClaySoil : Window
    {
        private DispersedSoil _dispersedSoil;
        public WndClaySoil(DispersedSoil dispersedSoil)
        {
            _dispersedSoil = dispersedSoil;
            this.DataContext = _dispersedSoil;
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
