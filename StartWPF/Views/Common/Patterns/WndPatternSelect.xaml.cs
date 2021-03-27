using RDStartWPF.Views.Common.Patterns.ControlClasses;
using RDStartWPF.Views.SC.Columns.Bases.UserControls;
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

namespace RDStartWPF.Views.Common.Patterns
{
    /// <summary>
    /// Логика взаимодействия для WndPatternSelect.xaml
    /// </summary>
    public partial class WndPatternSelect : Window
    {
        private List<PatternCard> _PatternCards;
        public WndPatternSelect(List<PatternCard> patternCards)
        {
            _PatternCards = patternCards;
            InitializeComponent();
            foreach (PatternCard patternCard in _PatternCards)
            {
                PatternCardControl card = new PatternCardControl(patternCard);
                wrpPatternCards.Children.Add(card);
            }
        }
    }
}
