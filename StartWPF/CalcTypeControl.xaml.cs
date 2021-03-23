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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RDStartWPF;
using RDStartWPF.Infrasructure.ControlClasses;

namespace RDStartWPF
{
    /// <summary>
    /// Логика взаимодействия для CalcTypeControl.xaml
    /// </summary>
    public partial class CalcTypeControl : UserControl
    {
        private CalcType _calcType;
        /// <summary>
        /// Конструктор контрола выбора раздела расчета, например
        /// железобетонные конструкции, металлические конструкции
        /// </summary>
        /// <param name="calcType"></param>
        public CalcTypeControl(CalcType calcType)
        {
            InitializeComponent();
            _calcType = calcType;
            lbCalcTypeName.Content = _calcType.TypeName;
            imgCalcTypeImage.Source = new BitmapImage(new Uri(_calcType.ImageName, UriKind.Relative));
        }

        private void btnRCC_Click(object sender, RoutedEventArgs e)
        {
            _calcType.RunCommand();
        }
    }
}
