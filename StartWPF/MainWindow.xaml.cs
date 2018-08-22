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
using RCC_Extension.BLL.Service;
using RCC_Extension.UI;

namespace StartWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ProgrammSettings.InicializeNew();
            ProgrammSettings.IsDataChanged = true;
            ProgrammSettings.FilePath = "bla-bla-bla";
        }

        private void btnWall_Click(object sender, RoutedEventArgs e)
        {
            var detailObjectList =
            new DetailObjectList("Levels", ProgrammSettings.BuildingSite.BuildingList[0],
            ProgrammSettings.BuildingSite.BuildingList[0].LevelList, false);
            frmDetailList DetailForm = new frmDetailList(detailObjectList);
            DetailForm.Show();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ProgrammSettings.SaveProjectToFile(false);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            ProgrammSettings.OpenProjectFromFile();
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            ProgrammSettings.SaveProjectToFile(true);
        }
    }
}
