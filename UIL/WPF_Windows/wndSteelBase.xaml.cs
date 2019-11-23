using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.Results.NDM;
using RDBLL.Entity.SC.Column;
using RDBLL.Processors.SC;
using System;
using System.Collections.Generic;
using System.Windows;


namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WndSteelColumnBase : Window
    {
        private SteelBase _steelColumnBase;

        public WndSteelColumnBase(SteelBase steelColumnBase)
        {
            InitializeComponent();
            _steelColumnBase = steelColumnBase;
            this.DataContext = _steelColumnBase;
            tbWidthMeasure.Text = MeasureUnitConverter.GetUnitLabelText(0);
            tbLengthMeasure.Text = MeasureUnitConverter.GetUnitLabelText(0);
            tbThicknessMeasure.Text = MeasureUnitConverter.GetUnitLabelText(0);
            tbSteelStrengthMeasure.Text = MeasureUnitConverter.GetUnitLabelText(3);
            tbConcreteStrengthMeasure.Text = MeasureUnitConverter.GetUnitLabelText(3);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.DialogResult = OK;
                ProgrammSettings.IsDataChanged = true;
                _steelColumnBase.IsActual = false;
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
        private void BtnParts_Click(object sender, RoutedEventArgs e)
        {
            WndSteelBasePart wndSteelBasePart = new WndSteelBasePart(_steelColumnBase);
            wndSteelBasePart.ShowDialog();
        }

        private void BtnBolts_Click(object sender, RoutedEventArgs e)
        {
            wndSteelBaseBolts wndSteelBaseBolts = new wndSteelBaseBolts(_steelColumnBase);
            wndSteelBaseBolts.ShowDialog();
        }

        private void BtnStresses_Click(object sender, RoutedEventArgs e)
        {
            //Коллекция комбинаций нагрузок и значений по прямоугольникам
            List<LoadCaseRectangleValue> loadCaseRectangleValues = SteelBaseProcessor.GetRectangleValues(_steelColumnBase);
            //Здесь необходимо вызывать окно с построением изополей и передавать в него коллекцию комбинаций и значений
        }
    }
}
