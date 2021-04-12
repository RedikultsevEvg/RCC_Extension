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
using RDStartWPF.InfraStructure.ControlClasses;
using RDUIL.WPF_Windows.UserControls;

namespace RDStartWPF.Views.Common.Service
{
    
    /// <summary>
    /// Логика взаимодействия для wndReports.xaml
    /// </summary>
    public partial class wndReports : Window
    {
        private List<ReportCard> _reportCards;

        public wndReports(List<ReportCard> reportCards)
        {
            InitializeComponent();
            _reportCards = reportCards;
            foreach (ReportCard reportCard in _reportCards)
            {
                ReportCardControl reportCardControl = new ReportCardControl(reportCard);
                wrpReportCards.Children.Add(reportCardControl);
            }
        }
    }
}
