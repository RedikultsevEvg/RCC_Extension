using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RDStartWPF.InfraStructure.ControlClasses;

namespace RDUIL.WPF_Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ReportCard.xaml
    /// </summary>
    public partial class ReportCardControl : UserControl
    {
        private ReportCard _reportCard { get; set; }
        public ReportCardControl(ReportCard reportCard)
        {
            InitializeComponent();
            _reportCard = reportCard;
            this.DataContext = _reportCard;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _reportCard.RunCommand();
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
