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
using System.Collections.ObjectModel;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Common.Service;
using RDUIL.Common.Reports;
using Winforms = System.Windows.Forms;
using System.Data;
using BLL.ErrorProcessing;

namespace RDUIL.WPF_Windows.Foundations
{
    /// <summary>
    /// Логика взаимодействия для wndFoundations.xaml
    /// </summary>
    public partial class wndFoundations : Window
    {
        private Level _level;
        private ObservableCollection<Foundation> _collection;

        public wndFoundations(Level level)
        {
            _level = level;
            _collection = _level.Foundations;
            InitializeComponent();
            this.DataContext = _collection;
            if (_collection.Count > 0) { LvMain.SelectedIndex = 0; }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Foundation foundation = new Foundation(_level);
            foundation.RenewAll();
            //Надо создать элемент, иначе некуда будет сохранять дочерние
            foundation.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
            wndFoundation wndFoundation = new wndFoundation(foundation);
            wndFoundation.ShowDialog();
            if (wndFoundation.DialogResult == true)
            {
                try
                {
                    foundation.SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                    _collection.Add(foundation);
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    CommonErrorProcessor.ShowErrorMessage("Ошибка сохранения в элементе: "+ foundation.Name, ex);
                }

            }
            else //Если пользователь не подтвердил создание, то удаляем элемент
            {
                foundation.DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
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
                    _collection[a].DeleteFromObservables();
                    _collection[a].DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
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
                Foundation foundation = _collection[a];
                wndFoundation wndChild = new wndFoundation(foundation);
                wndChild.ShowDialog();
                if (wndChild.DialogResult == true)
                {
                    foundation.RenewAll();
                    try
                    {
                        foundation.SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                        ProgrammSettings.IsDataChanged = true;
                    }
                    catch (Exception ex)
                    {
                        CommonErrorProcessor.ShowErrorMessage("Ошибка сохранения в элементе: " + foundation.Name, ex);
                    }
                }
                else { foundation.OpenFromDataSet(ProgrammSettings.CurrentDataSet); }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            ShowReportProcessor.ShowFoundationsReport();
        }

        private void BtnDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                Foundation foundation = _collection[a];
                Foundation newFoundation = foundation.Duplicate() as Foundation;
                newFoundation.LevelId = foundation.LevelId;
                newFoundation.Level = foundation.Level;
                try
                {
                    newFoundation.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    _collection.Add(newFoundation);
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    CommonErrorProcessor.ShowErrorMessage("Ошибка дублирования элемента: " + Name, ex);
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }
    }
}
