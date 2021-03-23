using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

namespace RDStartWPF
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WndAbout : Window
    {
        /// <summary>
        /// Конструктор окна "О программе"
        /// </summary>
        public WndAbout()
        {
            InitializeComponent();
            lbBuildText.Content = Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
