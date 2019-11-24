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
using RDBLL.Entity.RCC.Foundations;
using RDBLL.DrawUtils.SteelBase;
using RDBLL.Entity.MeasureUnits;

namespace RDUIL.WPF_Windows.Foundations
{
    /// <summary>
    /// Логика взаимодействия для wndFoundation.xaml
    /// </summary>
    public partial class wndFoundation : Window
    {
        private Foundation _element;
        public wndFoundation(Foundation foundation)
        {
            _element = foundation;
            InitializeComponent();
            this.DataContext = _element;
            tbSoilWeightMeasure.Text = MeasureUnitConverter.GetUnitLabelText(9);
            tbConcreteWeightMeasure.Text = MeasureUnitConverter.GetUnitLabelText(9);
            DrawFoundation.DrawTopScatch(_element, cvScetch);
        }

        private void BtnForces_Click(object sender, RoutedEventArgs e)
        {
            wndForces wndForces = new wndForces(_element);
            wndForces.ShowDialog();
        }

        private void BtnParts_Click(object sender, RoutedEventArgs e)
        {
            wndFoundationParts wndFoundationParts = new wndFoundationParts(_element);
            wndFoundationParts.ShowDialog();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DrawFoundation.DrawTopScatch(_element, cvScetch);
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
