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
using RDBLL.Entity.RCC.BuildingAndSite;
using System.Collections.ObjectModel;
using RDBLL.Common.Service;
using Winforms = System.Windows.Forms;


namespace RDUIL.WPF_Windows.BuildingsAndSites
{
    /// <summary>
    /// Логика взаимодействия для wndLevels.xaml
    /// </summary>
    public partial class wndLevels : Window
    {
        private ObservableCollection<Level> _collections;
        public Building Building { get; set; }
        public string ChildName { get; set; }
        public wndLevels(ObservableCollection<Level> levels)
        {
            _collections = levels;
            InitializeComponent();
            if (ChildName == "SteelBases") { ChildPng.SetResourceReference(Image.SourceProperty, "IconBase40"); }
            else if (ChildName == "Foundations") { ChildPng.SetResourceReference(Image.SourceProperty, "IconBase40"); }
            this.DataContext = _collections;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Level level = new Level(Building);
            ProgrammSettings.IsDataChanged = true;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
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
                    _collections.RemoveAt(a);
                    ProgrammSettings.IsDataChanged = true;
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                wndLevel wndLevel = new wndLevel(_collections[a]);
                wndLevel.ShowDialog();
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnChildItem_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                wndSteelBases childWindow = new wndSteelBases(_collections[a]);
                childWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
            
        }
    }
}
