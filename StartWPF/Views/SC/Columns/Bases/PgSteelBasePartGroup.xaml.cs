using RDBLL.Entity.SC.Column.SteelBases;
using RDStartWPF.ViewModels.SC.Columns.Bases;
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

namespace RDStartWPF.Views.SC.Columns.Bases
{
    /// <summary>
    /// Логика взаимодействия для PgSteelBasePartGroup.xaml
    /// </summary>
    public partial class PgSteelBasePartGroup : Page
    {
        private SteelBasePartGroup _PartGroup;
        private SteelBasePartsPgVM _SteelBasePartsPgVM;

        public PgSteelBasePartGroup(SteelBasePartGroup partGroup)
        {
            InitializeComponent();
            _PartGroup = partGroup;
            _SteelBasePartsPgVM = new SteelBasePartsPgVM(_PartGroup.SteelBaseParts, _PartGroup, false);
            this.DataContext = new SteelBasePartGroupPgVM(_PartGroup);
        }

        private void Parts_Navigated(object sender, NavigationEventArgs e)
        {
            pgSteelBaseParts pgSteelBaseParts = new pgSteelBaseParts();
            Frame frame = sender as Frame;
            var page = frame.Content as Page;
            page.DataContext = _SteelBasePartsPgVM;
        }
    }
}
