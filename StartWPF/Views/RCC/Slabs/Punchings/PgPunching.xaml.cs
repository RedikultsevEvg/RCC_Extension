using RDBLL.DrawUtils.Interfaces;
using RDBLL.DrawUtils.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDStartWPF.ViewModels.RCC.Slabs.Punchings;
using RDStartWPF.Views.Bases.Pages;
using RDStartWPF.Views.Common.Service.Pages.Scatches;
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

namespace RDStartWPF.Views.RCC.Slabs.Punchings
{
    /// <summary>
    /// Логика взаимодействия для Punching.xaml
    /// </summary>
    public partial class PgPunching : Page
    {
        private Punching _Punching;
        internal PunchingVM PunchingVM { get; set; }

        public PgPunching()
        {
            InitializeComponent();
        }

        public PgPunching(Punching punching)
        {
            InitializeComponent();
            this._Punching = punching;
            PunchingVM = new PunchingVM(_Punching);
            this.DataContext = PunchingVM;
            IDrawScatch drawScatch = new PunchingDrawProcessor();
            Page page = new PgScatch(_Punching, drawScatch);
            FrmScatch.Navigate(page);
        }
    }
}
