using RDBLL.Common.Service;
using RDBLL.DrawUtils.SteelBases;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.RCC.Foundations;
using System.Windows;
using RDUIL.Validations;
using System;
using RDBLL.Entity.RCC.Foundations.Processors;
using RDStartWPF.Views.Common.Forces;

namespace RDStartWPF.Views.RCC.Foundations
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
            wndForces wndForces = new wndForces(_element.ForcesGroups[0]);
            wndForces.ShowDialog();
            if (wndForces.DialogResult == true)
            {
                try
                {
                    _element.ForcesGroups[0].DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
                    _element.ForcesGroups[0].SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
                DrawFoundation.DrawTopScatch(_element, cvScetch);            
            }
            else
            {
                _element.ForcesGroups = GetEntity.GetParentForcesGroups(ProgrammSettings.CurrentDataSet, _element);
            }          
        }

        private void BtnParts_Click(object sender, RoutedEventArgs e)
        {
            wndFoundationParts wndFoundationParts = new wndFoundationParts(_element);
            wndFoundationParts.ShowDialog();
            if (wndFoundationParts.DialogResult == true)
            {
                try
                {
                    _element.DeleteSubElements(ProgrammSettings.CurrentDataSet, "FoundationParts");
                    foreach (FoundationPart foundationPart in _element.Parts)
                    {
                        foundationPart.Change();
                        foundationPart.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    }
                    ProgrammSettings.IsDataChanged = true;
                    DrawFoundation.DrawTopScatch(_element, cvScetch);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
            }
            else
            {
                _element.Parts = GetEntity.GetFoundationParts(ProgrammSettings.CurrentDataSet, _element);
            }
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
                DialogResult = true;
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnSoilStresses_Click(object sender, RoutedEventArgs e)
        {
            FoundationProcessor.SolveFoundation(_element);
            FoundationProcessor.ShowSoilStress(_element);
        }
    }
}
