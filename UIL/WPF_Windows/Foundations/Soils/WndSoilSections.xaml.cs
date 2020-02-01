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
using RDUIL.Common.Reports;
using Winforms = System.Windows.Forms;

namespace RDUIL.WPF_Windows.Foundations.Soils
{
    /// <summary>
    /// Логика взаимодействия для WndSoilCrossSections.xaml
    /// </summary>
    public partial class WndSoilSections : Window
    {
        private BuildingSite _buildingSite;
        private ObservableCollection<SoilSection> _collection;
        public WndSoilSections(BuildingSite buildingSite)
        {
            _buildingSite = buildingSite;
            _collection = _buildingSite.SoilSections;
            this.DataContext = _collection;
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            SoilSection soilSection = new SoilSection(_buildingSite);
            WndSoilSection wndSoilSection = new WndSoilSection(soilSection);
            wndSoilSection.ShowDialog();
            if (wndSoilSection.DialogResult == true)
            {
                try
                {
                    soilSection.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    _collection.Add(soilSection);
                    ProgrammSettings.IsDataChanged = true;
                    LvMain.SelectedIndex = _collection.Count - 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
                
            }
            else
            {
                soilSection.OpenFromDataSet(ProgrammSettings.CurrentDataSet);
            }
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
                    DeleteSoilSection();
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
                SoilSection soilSection = _collection[a];
                WndSoilSection wndSoilSection = new WndSoilSection(soilSection);
                wndSoilSection.ShowDialog();
                if (wndSoilSection.DialogResult == true)
                {
                    try
                    {
                        soilSection.SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                        ProgrammSettings.IsDataChanged = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка сохранения :" + ex);
                    }
                }
                else
                {
                    soilSection.OpenFromDataSet(ProgrammSettings.CurrentDataSet);
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }
        private void DeleteSoilSection()
        {
            bool canDelete = true;
            int a = LvMain.SelectedIndex;
            //Если скважина содержит слои грунта
            if (_collection[a].SoilLayers.Count > 0)
            {
                //то задаем запрос на удаление слоев вместе со скважиной
                Winforms.DialogResult result2 = Winforms.MessageBox.Show("Элемент содержит слои грунта", "Все равно удалить?",
                Winforms.MessageBoxButtons.YesNo,
                Winforms.MessageBoxIcon.Information,
                Winforms.MessageBoxDefaultButton.Button1,
                Winforms.MessageBoxOptions.DefaultDesktopOnly);
                //Если ответ отрицательный, то удаление невозможно
                if (!(result2 == Winforms.DialogResult.Yes))
                {
                    canDelete = false;
                }
            }
            //Если скважину можно удалить
            if (canDelete)
            {//удаляем
                if (LvMain.Items.Count == 1) LvMain.UnselectAll();
                else if (a < (LvMain.Items.Count - 1)) LvMain.SelectedIndex = a + 1;
                else LvMain.SelectedIndex = a - 1;
                _collection[a].DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
                _collection.RemoveAt(a);
                ProgrammSettings.IsDataChanged = true;
                LvMain.SelectedIndex = _collection.Count - 1;
            }
        }
    }
}
