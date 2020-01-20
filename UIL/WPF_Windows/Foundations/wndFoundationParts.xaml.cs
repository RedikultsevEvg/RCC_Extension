using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Foundations;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Winforms = System.Windows.Forms;
using RDBLL.DrawUtils.SteelBase;
using RDUIL.Validations;


namespace RDUIL.WPF_Windows.Foundations
{
    /// <summary>
    /// Логика взаимодействия для wndFoundationParts.xaml
    /// </summary>
    public partial class wndFoundationParts : Window
    {
        private Foundation _foundation;
        private ObservableCollection<RectFoundationPart> _collection;
        public wndFoundationParts(Foundation foundation)
        {
            _foundation = foundation;
            _collection = _foundation.Parts;
            InitializeComponent();
            this.DataContext = _collection;
            DrawFoundation.DrawTopScatch(_foundation, cvScetch);
        }

        private void StpPartBtns_MouseMove(object sender, MouseEventArgs e)
        {
            ((StackPanel)sender).Opacity = 1;
        }

        private void StpPartBtns_MouseLeave(object sender, MouseEventArgs e)
        {
            ((StackPanel)sender).Opacity = 0.5;
        }

        private void BtnAddPart_Click(object sender, RoutedEventArgs e)
        {
            RectFoundationPart foundationPart = new RectFoundationPart(_foundation);
            _collection.Add(foundationPart);
        }

        private void BtnDeletePart_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = LvMain.SelectedIndex;
                    if (LvMain.Items.Count == 1) LvMain.UnselectAll();
                    else if (a < (LvMain.Items.Count - 1)) LvMain.SelectedIndex = a + 1;
                    else LvMain.SelectedIndex = a - 1;
                    _collection.RemoveAt(a);
                    ProgrammSettings.IsDataChanged = true;
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            string message = ErrorProcessor.cmdGetErrorString(GridMain);
            if (message != "") { MessageBox.Show(message); }
            else
            {
                this.DialogResult = true;
                this.Close();
            }            
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DrawFoundation.DrawTopScatch(_foundation, cvScetch);
        }
    }
}
