using RDStartWPF.Views.Common.Patterns.ControlClasses;
using RDStartWPF.Views.Common.Patterns.UserControls;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RDStartWPF.Views.SC.Columns.Bases.UserControls
{
    /// <summary>
    /// Логика взаимодействия для PatternCard.xaml
    /// </summary>
    public partial class PatternCardControl : UserControl
    {
        private PatternCard _PatternCard;
        public PatternCardControl(PatternCard patternCard)
        {
            _PatternCard = patternCard;
            InitializeComponent();
            this.DataContext = new PatternCardControlVM(_PatternCard);
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
