using RDBLL.Common.Service;
using RDBLL.DrawUtils.SteelBase;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.RCC.Foundations;
using System.Windows;
using RDUIL.Validations;

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
            DrawFoundation.DrawTopScatch(_element, cvScetch);
        }

        private void BtnParts_Click(object sender, RoutedEventArgs e)
        {
            wndFoundationParts wndFoundationParts = new wndFoundationParts(_element);
            wndFoundationParts.ShowDialog();
            DrawFoundation.DrawTopScatch(_element, cvScetch);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DrawFoundation.DrawTopScatch(_element, cvScetch);
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            string message = ErrorProcessor.cmdGetErrorString(GridMain);
            if (message != "") { MessageBox.Show(message); }
            else
            {
                _element.IsLoadCasesActual = false;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
