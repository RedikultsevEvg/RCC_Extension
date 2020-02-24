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
using RDUIL.Validations;

namespace RDUIL.WPF_Windows.Foundations.Soils
{
    /// <summary>
    /// Логика взаимодействия для WndClaySoil.xaml
    /// </summary>
    public partial class WndClaySoil : Window
    {
        private Soil _element;
        public WndClaySoil(Soil soil)
        {
            _element = soil;
            this.DataContext = _element;
            InitializeComponent();
            if (_element is BearingSoil) AddContentControl(_element, "MechanicalSoilProps");
            if (_element is DispersedSoil) AddContentControl(_element, "MCSoilProps");
            if (_element is RockSoil) AddContentControl(_element, "RockSoilProps");
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void AddContentControl(Soil soil, string name)
        {
            ContentControl contentControl = new ContentControl();
            contentControl.SetResourceReference(ContentControl.ContentTemplateProperty, name);
            Binding binding = new Binding();
            binding.Source = soil;
            contentControl.SetBinding(ContentControl.ContentProperty, binding);
            StpProperties.Children.Add(contentControl);
        }
    }
}
