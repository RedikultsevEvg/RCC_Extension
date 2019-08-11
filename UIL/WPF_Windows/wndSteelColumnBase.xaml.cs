﻿using System;
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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WndSteelColumnBase : Window
    {
        private SteelColumnBase _steelColumnBase;

        public WndSteelColumnBase(SteelColumnBase steelColumnBase)
        {
            InitializeComponent();
            _steelColumnBase = steelColumnBase;
            this.DataContext = _steelColumnBase;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.DialogResult = OK;
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
    }
}
