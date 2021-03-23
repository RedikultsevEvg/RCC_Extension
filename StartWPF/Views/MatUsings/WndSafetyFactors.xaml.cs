using RDBLL.Entity.Common.Materials;
using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.ViewModels.MatUsings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RDStartWPF.Views.MatUsings
{
    /// <summary>
    /// Логика взаимодействия для SafetyFactors.xaml
    /// </summary>
    public partial class WndSafetyFactors : Window
    {
        private ObservableCollection<SafetyFactor> _SafetyFactors;

        public WndSafetyFactors(MaterialUsing parentMember)
        {
            _SafetyFactors = parentMember.SafetyFactors;
            this.DataContext = new SafetyFactorsVM(parentMember);
            InitializeComponent();
        }
    }
}
