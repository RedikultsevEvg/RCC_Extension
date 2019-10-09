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
using Winforms = System.Windows.Forms;
using RDBLL.DrawUtils.SteelBase;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для wndSteelBaseBolts.xaml
    /// </summary>
    public partial class wndSteelBaseBolts : Window
    {
        private SteelColumnBase _steelColumnBase;

        public wndSteelBaseBolts(SteelColumnBase steelColumnBase)
        {
            InitializeComponent();
            _steelColumnBase = steelColumnBase;
            DrawSteelBase.DrawBase(_steelColumnBase, cvScetch);
            this.DataContext = _steelColumnBase;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DrawSteelBase.DrawBase(_steelColumnBase, cvScetch);
        }

        private void BtnAddBolt_Click(object sender, RoutedEventArgs e)
        {
            _steelColumnBase.SteelBolts.Add(new SteelBolt(_steelColumnBase));
        }

        private void BtnDeleteBolt_Click(object sender, RoutedEventArgs e)
        {
            if (lvBoltsList.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = lvBoltsList.SelectedIndex;
                    if (lvBoltsList.Items.Count == 1) lvBoltsList.UnselectAll();
                    else if (a < (lvBoltsList.Items.Count - 1)) lvBoltsList.SelectedIndex = a + 1;
                    else lvBoltsList.SelectedIndex = a - 1;
                    _steelColumnBase.SteelBolts.RemoveAt(a);
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }
    }
}
