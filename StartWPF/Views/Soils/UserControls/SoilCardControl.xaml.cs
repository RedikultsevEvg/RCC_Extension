using RDUIL.WPF_Windows.Foundations.Soils.ControlClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace RDUIL.WPF_Windows.Foundations.Soils.UserControls
{
    /// <summary>
    /// Логика взаимодействия для SoilCardControl.xaml
    /// </summary>
    public partial class SoilCardControl : UserControl
    {
        private SoilCard _soilCard;
        public SoilCardControl(SoilCard soilCard)
        {
            _soilCard = soilCard;
            InitializeComponent();
            this.DataContext = _soilCard;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _soilCard.RunCommand();
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Opacity = 0.6;
            dropShadowEffect.BlurRadius = 5;
            dropShadowEffect.ShadowDepth = 5;
            Effect = dropShadowEffect;
            Thickness thickness = new Thickness();
            thickness.Top = 0;
            thickness.Bottom = 10;
            thickness.Left = 0;
            thickness.Right = 10;
            Margin = thickness;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Effect = null;
            Thickness thickness = new Thickness();
            thickness.Top = 5;
            thickness.Bottom = 5;
            thickness.Left = 5;
            thickness.Right = 5;
            Margin = thickness;
        }
    }
}
