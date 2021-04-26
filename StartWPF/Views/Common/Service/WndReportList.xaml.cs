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

namespace RDStartWPF.Views.Common.Service
{
    /// <summary>
    /// Логика взаимодействия для WndReportList.xaml
    /// </summary>
    public partial class WndReportList : Window
    {
        public WndReportList(List<string> reportList)
        {
            InitializeComponent();
            string protocol = "";
            foreach (string s in reportList)
            {
                protocol += s + "\n";
            }
            ProtocolText.Text = protocol;
        }
    }
}
