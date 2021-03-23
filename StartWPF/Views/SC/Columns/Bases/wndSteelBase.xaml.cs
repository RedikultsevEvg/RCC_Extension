using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.Results.NDM;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using RDBLL.Entity.SC.Column.SteelBases.Processors;
using RDBLL.Common.Service.DsOperations;
using System.Data;
using RDStartWPF.Views.Common.Forces;

namespace RDStartWPF.Views.SC.Columns.Bases
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
            tbThicknessMeasure.Text = MeasureUnitConverter.GetUnitLabelText(0);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnForces_Click(object sender, RoutedEventArgs e)
        {
            wndForces wndForces = new wndForces(_steelColumnBase.ForcesGroups[0]);
            wndForces.ShowDialog();
            if (wndForces.DialogResult == true)
            {
                try
                {
                    _steelColumnBase.ForcesGroups[0].DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
                    _steelColumnBase.ForcesGroups[0].SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
            }
            else
            {
                _steelColumnBase.ForcesGroups = GetEntity.GetParentForcesGroups(ProgrammSettings.CurrentDataSet, _steelColumnBase);
            }
        }
        private void BtnParts_Click(object sender, RoutedEventArgs e)
        {
            WndSteelBasePart wndSteelBasePart = new WndSteelBasePart(_steelColumnBase);
            wndSteelBasePart.ShowDialog();
            if (wndSteelBasePart.DialogResult == true)
            {
                try
                {
                    _steelColumnBase.IsActual = false;
                    DsOperation.DeleteRow(ProgrammSettings.CurrentDataSet, "SteelBaseParts", "ParentId", _steelColumnBase.Id);
                    foreach (SteelBasePart steelBasePart in _steelColumnBase.SteelBaseParts)
                    {
                        steelBasePart.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    }
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
            }
            else
            {
                _steelColumnBase.SteelBaseParts = GetEntity.GetSteelBaseParts(ProgrammSettings.CurrentDataSet, _steelColumnBase);
            }
        }

        private void BtnBolts_Click(object sender, RoutedEventArgs e)
        {
            wndSteelBaseBolts wndSteelBaseBolts = new wndSteelBaseBolts(_steelColumnBase);
            wndSteelBaseBolts.ShowDialog();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            if (wndSteelBaseBolts.DialogResult == true)
            {
                try
                {
                    _steelColumnBase.IsActual = false;
                    foreach (SteelBolt bolt in _steelColumnBase.SteelBolts)
                    {
                        bolt.DeleteFromDataSet(dataSet);
                    }
                    foreach (SteelBolt steelBolt in _steelColumnBase.SteelBolts)
                    {
                        steelBolt.SaveToDataSet(dataSet, true);
                    }
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
            }
            else
            {
                _steelColumnBase.SteelBolts = GetEntity.GetSteelBolts(dataSet, _steelColumnBase);
            }
        }

        private void BtnStresses_Click(object sender, RoutedEventArgs e)
        {
            SteelBaseProcessor.SolveSteelColumnBase(_steelColumnBase);
            SteelBaseViewProcessor.ShowPartStress(_steelColumnBase);
        }

        private void BtnConcreteStresses_Click(object sender, RoutedEventArgs e)
        {
            SteelBaseProcessor.SolveSteelColumnBase(_steelColumnBase);
            SteelBaseViewProcessor.ShowStress(_steelColumnBase);
        }
    }
}
