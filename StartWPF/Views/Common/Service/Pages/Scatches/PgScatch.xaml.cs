using RDBLL.Common.Interfaces;
using RDBLL.DrawUtils.Interfaces;
using RDBLL.DrawUtils.RCC.Slabs.Punchings;
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

namespace RDStartWPF.Views.Common.Service.Pages.Scatches
{
    /// <summary>
    /// Логика взаимодействия для PgScatch.xaml
    /// </summary>
    public partial class PgScatch : Page
    {
        private IDsSaveable _DrawableItem;
        private IDrawScatch _DrawScatchProc;

        public PgScatch(IDsSaveable drawableItem, IDrawScatch drawScatchProc)
        {
            InitializeComponent();
            _DrawableItem = drawableItem;
            _DrawScatchProc = drawScatchProc;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (!(cvScetch.Width > 1 & cvScetch.Height > 1))
            { 
                cvScetch.Width = cvScetch.ActualWidth;
                cvScetch.Height = cvScetch.ActualHeight;
            }
            _DrawScatchProc.DrawTopScatch(cvScetch, _DrawableItem);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            cvScetch.Width = (sender as Grid).Width;
            cvScetch.Height = (sender as Grid).Height - 80;
            _DrawScatchProc.DrawTopScatch(cvScetch, _DrawableItem);
        }
    }
}
