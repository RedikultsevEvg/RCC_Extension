﻿using System;
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
using winForms = System.Windows.Forms;

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
            if (ProgrammSettings.IsDataChanged)
            {
                winForms.DialogResult result = winForms.MessageBox.Show(
                "Сохранить данные перед открытием нового файла?",
                "Файл не сохранен",
                winForms.MessageBoxButtons.YesNoCancel,
                winForms.MessageBoxIcon.Information,
                winForms.MessageBoxDefaultButton.Button1,
                winForms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == winForms.DialogResult.Yes)
                {
                    ProgrammSettings.SaveProjectToFile(false);
                    if (ProgrammSettings.OpenProjectFromFile())
                    this.Title = "RD-Калькулятор - " + ProgrammSettings.FilePath;
                }

                if (result == winForms.DialogResult.No)
                {
                    if (ProgrammSettings.OpenProjectFromFile())
                        this.Title = "RD-Калькулятор - " + ProgrammSettings.FilePath;
                }
            }
            else
            {
                if (ProgrammSettings.OpenProjectFromFile())
                    this.Title = "RD-Калькулятор - " + ProgrammSettings.FilePath;
            }
            
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            ProgrammSettings.SaveProjectToFile(true);
        }
    }
}
