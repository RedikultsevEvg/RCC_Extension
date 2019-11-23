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
using RDBLL.Common.Service;
using RDUIL.WinForms;
using winForms = System.Windows.Forms;
using RDUIL.WPF_Windows.ControlClasses;
using RDBLL.Entity.SC.Column;
using System.Threading;
using RDUIL.WPF_Windows;
using RDUIL.WPF_Windows.BuildingsAndSites;

namespace StartWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор главного окная программы
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ProgrammSettings.InicializeNew();

            List<CalcType> calcTypes = new List<CalcType>(); 

            //CalcType calcTypeRCC = new CalcType();
            //calcTypeRCC.TypeName = "Железобетон";
            //calcTypeRCC.ImageName = "Bridge.jpg";
            //calcTypeRCC.RegisterDelegate(new CalcType.AddCommandDelegate(AddItemWrapPanel));
            //calcTypes.Add(calcTypeRCC);

            CalcType calcTypeSC = new CalcType();
            calcTypeSC.TypeName = "Металл";
            calcTypeSC.ImageName = "Steel.jpg";
            calcTypeSC.RegisterDelegate(new CalcType.AddCommandDelegate(AddItemWrapPanel));
            calcTypes.Add(calcTypeSC);

            CalcType calcTypeSoil = new CalcType();
            calcTypeSoil.TypeName = "Грунт";
            calcTypeSoil.ImageName = "Steel.jpg";
            calcTypeSoil.RegisterDelegate(new CalcType.AddCommandDelegate(AddItemWrapPanel));
            calcTypes.Add(calcTypeSoil);

            foreach (CalcType calcType in calcTypes)
            {
                CalcTypeControl calcTypeControl = new CalcTypeControl(calcType);
                stpCalcTypes.Children.Add(calcTypeControl);
            }
            
            //CalcKind calcKindWall = new CalcKind();
            //calcKindWall.KindName = "Расчет железобетонных стен";
            //calcKindWall.KindAddition = "Подсчет объема бетона для железобетонных стен";
            //calcKindWall.RegisterDelegate(new CalcKind.CommandDelegate(ShowWall));
            //calcTypeRCC.CalcKinds.Add(calcKindWall);

            CalcKind calcKindSteelBase = new CalcKind();
            calcKindSteelBase.KindName = "Расчет баз стальных колонн";
            calcKindSteelBase.KindAddition = "Расчет параметров баз колонн с учетом давления под подошвой";
            calcKindSteelBase.RegisterDelegate(new CalcKind.CommandDelegate(ShowSteelBase));
            calcTypeSC.CalcKinds.Add(calcKindSteelBase);

            CalcKind calcKindFoundation = new CalcKind();
            calcKindFoundation.KindName = "Расчет столбчатых фундаментов";
            calcKindFoundation.KindAddition = "Расчет параметров фундаментов с учетом давления под подошвой";
            calcKindFoundation.RegisterDelegate(new CalcKind.CommandDelegate(ShowFoundation));
            calcTypeSoil.CalcKinds.Add(calcKindFoundation);

            calcTypes[0].RunCommand();
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

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            WndAbout wndAbout = new WndAbout();
            wndAbout.Show();
        }

        private static void ShowWall()
        {
            var detailObjectList = new DetailObjectList("Levels", ProgrammSettings.BuildingSite.Buildings[0],
            ProgrammSettings.BuildingSite.Buildings[0].Levels, false);
            detailObjectList.BtnVisibilityList = new List<short>() { 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0 };
            frmDetailList DetailForm = new frmDetailList(detailObjectList);
            DetailForm.Show();
        }

        private static void ShowSteelBase()
        {
            wndLevels wndLevels = new wndLevels(ProgrammSettings.BuildingSite.Buildings[0], ProgrammSettings.BuildingSite.Buildings[0].Levels, "SteelBases");
            wndLevels.ShowDialog();
        }

        /// <summary>
        /// Вызов окна уровней для ввода фундаментов
        /// </summary>
        private static void ShowFoundation()
        {
            wndLevels wndLevels = new wndLevels(ProgrammSettings.BuildingSite.Buildings[0], ProgrammSettings.BuildingSite.Buildings[0].Levels, "Foundations");
            wndLevels.ShowDialog();
        }

        /// <summary>
        /// Добавляет тип расчета на панель раздела расчетов
        /// </summary>
        /// <param name="calcKinds"></param>
        public void AddItemWrapPanel(List<CalcKind> calcKinds)
        {
            wpCalcPanel.Children.Clear();

            foreach (CalcKind calcKind in calcKinds)
            {
                CalcKindControl calcKindControl = new CalcKindControl(calcKind);
                wpCalcPanel.Children.Add(calcKindControl);
            }
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            wndMeasureUnits newWindow = new wndMeasureUnits();
            newWindow.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ProgrammSettings.IsDataChanged)
            {
                winForms.DialogResult result = winForms.MessageBox.Show(
                "Сохранить данные перед закрытием?",
                "Файл не сохранен",
                winForms.MessageBoxButtons.YesNo,
                winForms.MessageBoxIcon.Information,
                winForms.MessageBoxDefaultButton.Button1,
                winForms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == winForms.DialogResult.Yes)
                {
                    ProgrammSettings.SaveProjectToFile(false);
                }

                if (result == winForms.DialogResult.No)
                {
                    //Ничего не делаем
                }
            }
        }
    }
}
