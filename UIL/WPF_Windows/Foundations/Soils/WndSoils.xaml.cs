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
            DispersedSoil dispersedSoil = new DispersedSoil(_buildingSite);
            WndClaySoil wndSoil = new WndClaySoil(dispersedSoil);
            wndSoil.ShowDialog();
            if (wndSoil.DialogResult == true)
            {
                try
                {
                    dispersedSoil.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    ProgrammSettings.IsDataChanged = true;
                    _buildingSite.Soils.Add(dispersedSoil);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
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

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                Soil soil = _collection[a];
                if (soil is DispersedSoil)
                {
                    WndClaySoil wndSoil = new WndClaySoil(soil as DispersedSoil);
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
