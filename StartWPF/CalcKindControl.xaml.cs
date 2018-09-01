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
using System.Windows.Media.Effects;
using RDBLL.Common.Service;
using RDUIL.WinForms;
using RDUIL.WPF_Windows.ControlClasses;
using StartWPF;

namespace StartWPF
{
    /// <summary>
    /// Логика взаимодействия для CalcKindControl.xaml
    /// </summary>
    public partial class CalcKindControl : UserControl
    {
        private CalcKind _calcKind;

        public CalcKindControl(CalcKind calcKind)
        {
            InitializeComponent();
            _calcKind = calcKind;
            tbCommandName.Text = _calcKind.KindName;
            tbAddition.Text = _calcKind.KindAddition;
            Width = 200;
            Height = 100;
            tbCommandName.Background = Brushes.LightBlue;
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Opacity = 0.6;
            dropShadowEffect.BlurRadius = 5;
            dropShadowEffect.ShadowDepth = 5;
            Effect = dropShadowEffect;
            tbCommandName.Background = Brushes.Cyan;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Effect = null;
            tbCommandName.Background = Brushes.LightBlue;
        }

        private void btnMainButton_Click(object sender, RoutedEventArgs e)
        {
            _calcKind.RunCommand();
        }
    }
}
