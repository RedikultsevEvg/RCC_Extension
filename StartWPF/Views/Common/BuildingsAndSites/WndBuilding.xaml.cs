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
using RDBLL.Common.Service;
using RDUIL.Validations;
using RDStartWPF.Views.Soils;

namespace RDStartWPF.Views.Common.BuildingsAndSites
{
    /// <summary>
    /// Логика взаимодействия для WndBuilding.xaml
    /// </summary>
    public partial class WndBuilding : Window
    {
        Building _element;
        public WndBuilding(Building building)
        {
            _element = building;
            InitializeComponent();
            this.DataContext = _element;
        }

        private void BtnSoils_Click(object sender, RoutedEventArgs e)
        {
            WndSoils wndSoils = new WndSoils(_element.ParentMember as BuildingSite);
            wndSoils.ShowDialog();
        }

        private void BtnSoilSections_Click(object sender, RoutedEventArgs e)
        {
            WndSoilSections wndSoilSections = new WndSoilSections(_element.ParentMember as BuildingSite);
            wndSoilSections.ShowDialog();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            string message = ErrorProcessor.cmdGetErrorString(GridMain);
            if (message != "") { MessageBox.Show(message); }
            else
            {
                ProgrammSettings.IsDataChanged = true;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
