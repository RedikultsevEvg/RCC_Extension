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

namespace BLL.ErrorProcessing
{
    /// <summary>
    /// Логика взаимодействия для wndErrorMessage.xaml
    /// </summary>
    public partial class wndErrorMessage : Window
    {
        public wndErrorMessage(String mainText, String extendedText)
        {
            InitializeComponent();
            MainText.Text = mainText;
            ExtendedText.Text = extendedText;
        }
    }
}
