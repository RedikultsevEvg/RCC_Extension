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
using RDBLL.Entity.Soils;
using System.Collections.ObjectModel;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Service;
using Winforms = System.Windows.Forms;

namespace RDStartWPF.Views.Soils
{
    /// <summary>
    /// Логика взаимодействия для WndSoils.xaml
    /// </summary>
    public partial class WndSoils : Window
    {
        private BuildingSite _buildingSite;
        private ObservableCollection<Soil> _collection;
        public WndSoils(BuildingSite buildingSite)
        {
            _buildingSite = buildingSite;
            _collection = _buildingSite.Soils;
            this.DataContext = _collection;
            InitializeComponent();
            if (_collection.Count>0) { LvMain.SelectedIndex = 0; }
        }

        /// <summary>
        /// Добавить новый грунт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
           SelectSoilProcessor.SelectSoils(_buildingSite);       
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
                    //Проверка на содержание грунта в других элементах, например в скважинах
                    if (_collection[a].HasChild())
                    {
                        MessageBox.Show("Нельзя удалять грунт, содержащийся в других элементах");
                    }
                    else
                    {
                        if (LvMain.Items.Count == 1) LvMain.UnselectAll();
                        else if (a < (LvMain.Items.Count - 1)) LvMain.SelectedIndex = a + 1;
                        else LvMain.SelectedIndex = a - 1;
                        _collection.RemoveAt(a);
                        ProgrammSettings.IsDataChanged = true;
                    }
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
                Soil soil = _collection[a];
                WndClaySoil wndSoil = new WndClaySoil(soil);
                wndSoil.ShowDialog();
                if (wndSoil.DialogResult == true)
                {
                    try
                    {
                        soil.SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                        ProgrammSettings.IsDataChanged = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка сохранения :" + ex);
                    }
                }
                else { soil.OpenFromDataSet(ProgrammSettings.CurrentDataSet); }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            //ShowReportProcessor.ShowFoundationsReport();
        }
    }
}
