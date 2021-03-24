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
using RDStartWPF.ViewModels.SC.Columns.Bases;

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
            this.DataContext = new SteelBaseVM(_steelColumnBase);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (_steelColumnBase.Pattern != null) _steelColumnBase.Pattern.GetBaseParts();
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
