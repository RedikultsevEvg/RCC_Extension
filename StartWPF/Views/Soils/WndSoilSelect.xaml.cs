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
using RDUIL.WPF_Windows.Foundations.Soils.ControlClasses;
using RDBLL.Entity.Soils;
using RDUIL.WPF_Windows.Foundations.Soils.UserControls;

namespace RDStartWPF.Views.Soils
{
    /// <summary>
    /// Логика взаимодействия для WndSoilSelect.xaml
    /// </summary>
    public partial class WndSoilSelect : Window
    {
        private List<SoilCard> _soilCards;

        public WndSoilSelect(List<SoilCard> soilCards)
        {
            _soilCards = soilCards;
            InitializeComponent();

            foreach (SoilCard soilCard in _soilCards)
            {
                SoilCardControl soilCardControl = new SoilCardControl(soilCard);
                wrpSoilCards.Children.Add(soilCardControl);
            }
        }
    }
}
