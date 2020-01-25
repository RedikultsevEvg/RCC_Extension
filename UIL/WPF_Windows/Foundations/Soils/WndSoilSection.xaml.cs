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
using RDUIL.Validations;

namespace RDUIL.WPF_Windows.Foundations.Soils
{
    /// <summary>
    /// Логика взаимодействия для WndSoilSection.xaml
    /// </summary>
    public partial class WndSoilSection : Window
    {
        private SoilSection _element;
        private ObservableCollection<SoilLayer> _collection;
        public WndSoilSection(SoilSection soilSection)
        {
            _element = soilSection;
            _collection = _element.SoilLayers;
            this.DataContext = _element;
            InitializeComponent();
            if (_element.BuildingSite.Soils.Count > 0) { LvSoils.SelectedIndex = 0; }
            if (_collection.Count > 0) { LvAssignedSoils.SelectedIndex = 0; }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            bool result = true;
            int count = _collection.Count;
            //Уровень грунтовых вод должен быть ниже уровня первого слоя
            if ((count>0) & (_element.HasWater))
            {
                if (_element.NaturalWaterLevel >= _collection[0].TopLevel)
                {
                    MessageBox.Show("Неверно назначены отметки кровли слоев");
                    result = false;
                }
            }
            //Проверяем назначение отметок слоев грунта
            if (count>1)
            {
                for (int i = 1; i <= count - 1; i++)
                {
                    //Отметка каждого следующего слоя должна быть меньше отметки предыдущего слоя
                    if (!(_collection[i].TopLevel < _collection[i - 1].TopLevel))
                    {
                        MessageBox.Show("Неверно назначены отметки кровли слоев");
                        result = false;
                    }
                }
            }

            string message = ErrorProcessor.cmdGetErrorString(GridMain);
            if (message != "") { MessageBox.Show(message); result = false; }

            if (result)
            {
                ProgrammSettings.IsDataChanged = true;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnDeleteSoil_Click(object sender, RoutedEventArgs e)
        {
            if (LvAssignedSoils.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                Winforms.MessageBoxButtons.YesNo,
                Winforms.MessageBoxIcon.Information,
                Winforms.MessageBoxDefaultButton.Button1,
                Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = LvAssignedSoils.SelectedIndex;
                    if (LvAssignedSoils.Items.Count == 1) LvAssignedSoils.UnselectAll();
                    else if (a < (LvAssignedSoils.Items.Count - 1)) LvAssignedSoils.SelectedIndex = a + 1;
                    else LvAssignedSoils.SelectedIndex = a - 1;
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

        private void BtnAddSoil_Click(object sender, RoutedEventArgs e)
        {
            if (LvSoils.SelectedIndex >= 0)
            {
                int a = LvSoils.SelectedIndex;
                Soil soil = _element.BuildingSite.Soils[a];
                double topLevel =250;
                int count = _element.SoilLayers.Count;
                if (count>0) { topLevel = _element.SoilLayers[count - 1].TopLevel - 2;}
                SoilLayer soilLayer = new SoilLayer()
                { Id = ProgrammSettings.CurrentId,
                    SoilId = soil.Id, Soil = soil,
                    SoilSectionId = _element.Id, SoilSection = _element,
                    TopLevel = topLevel };
                soilLayer.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                _element.SoilLayers.Add(soilLayer);
                LvAssignedSoils.SelectedIndex = _element.SoilLayers.Count - 1;
                ProgrammSettings.IsDataChanged = true;
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }
    }
}
