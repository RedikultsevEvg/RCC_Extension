﻿using System;
using System.IO;
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
using RDBLL.Entity.Results.NDM;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Service;
using RDBLL.Processors.SC;


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
            wndForces wndForces = new wndForces(_steelColumnBase.LoadsGroup[0]);
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
