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

namespace StartWPF
{
    /// <summary>
    /// Логика взаимодействия для CalcKindControl.xaml
    /// </summary>
    public partial class CalcKindControl : UserControl
    {
        public CalcKindControl()
        {
            InitializeComponent();
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Opacity = 0.6;
            dropShadowEffect.BlurRadius = 5;
            dropShadowEffect.ShadowDepth = 5;
            Effect = dropShadowEffect;
            btnMainButton.Opacity = 1;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Effect = null;
            btnMainButton.Opacity = 0.7;
        }
    }
}
