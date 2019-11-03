﻿using RDBLL.Common.Service;
using RDBLL.DrawUtils.SteelBase;
using RDBLL.Entity.SC.Column;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Winforms = System.Windows.Forms;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для SteelBasePart.xaml
    /// </summary>
    public partial class WndSteelBasePart : Window
    {
        private SteelBase _steelColumnBase;

        public WndSteelBasePart(SteelBase steelColumnBase)
        {
            InitializeComponent();
            _steelColumnBase = steelColumnBase;
            DrawSteelBase.DrawBase(_steelColumnBase, cvScetch);
            this.DataContext = _steelColumnBase;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _steelColumnBase.IsBoltsActual = false;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgrammSettings.IsDataChanged = true;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
        }
        

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DrawSteelBase.DrawBase(_steelColumnBase, cvScetch);
        }

        private void BtnAddPart_Click(object sender, RoutedEventArgs e)
        {
            _steelColumnBase.SteelBaseParts.Add(new SteelBasePart(_steelColumnBase));
        }

        private void BtnDeletePart_Click(object sender, RoutedEventArgs e)
        {
            if (lvPartsList.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = lvPartsList.SelectedIndex;
                    if (lvPartsList.Items.Count == 1) lvPartsList.UnselectAll();
                    else if (a < (lvPartsList.Items.Count - 1)) lvPartsList.SelectedIndex = a + 1;
                    else lvPartsList.SelectedIndex = a - 1;
                    _steelColumnBase.SteelBaseParts.RemoveAt(a);
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void StpPartBtns_MouseMove(object sender, MouseEventArgs e)
        {
            ((StackPanel)sender).Opacity = 1;
        }

        private void StpPartBtns_MouseLeave(object sender, MouseEventArgs e)
        {
            ((StackPanel)sender).Opacity = 0.5;
        }
    }
}
