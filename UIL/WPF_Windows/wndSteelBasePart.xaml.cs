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
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;
using RDBLL.Processors.SC;
using Winforms = System.Windows.Forms;
using RDBLL.DrawUtils.SteelBase;
using RDBLL.Entity.MeasureUnits;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для SteelBasePart.xaml
    /// </summary>
    public partial class WndSteelBasePart : Window
    {
        private SteelColumnBase _steelColumnBase;

        public WndSteelBasePart(SteelColumnBase steelColumnBase)
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
