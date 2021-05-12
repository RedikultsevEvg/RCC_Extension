using Microsoft.Win32;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.SC.Column.SteelBases;
using RDStartWPF.ViewModels.Base;
using RDStartWPF.ViewModels.RCC.Slabs.Punchings;
using RDStartWPF.ViewModels.SC.Columns.Bases;
using RDStartWPF.Views.RCC.Slabs.Punchings;
using RDStartWPF.Views.SC.Columns.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RDStartWPF.Views.Common.Service
{
    /// <summary>
    /// Логика взаимодействия для WndCommonDialog.xaml
    /// </summary>
    public partial class WndCommonDialog : Window
    {
        private ViewModelDialog DialogVM;
        private IDsSaveable _Element;
        /// <summary>
        /// Конструктор диалогового окна
        /// </summary>
        /// <param name="element"></param>
        public WndCommonDialog(IDsSaveable element)
        {
            InitializeComponent();
            _Element = element;
            DialogVM = new ViewModelDialog(this);
            this.DataContext = DialogVM;
            Page page = null;
            if (_Element is SteelBasePartGroup)
            {
                page = new PgSteelBasePartGroup(_Element as SteelBasePartGroup);
                Title = "Группа участков базы стальной колонны";
            }
            else if (_Element is Punching)
            {
                Punching punching = _Element as Punching;
                page = new PgPunching(punching);
                (page as PgPunching).PunchingVM.ParentVM = DialogVM;
                DialogVM.Children.Add((page as PgPunching).PunchingVM);
                Title = "Продавливание плиты колонной прямоугольного сечения";
                MinWidth = 600;
                MinHeight = 400;
            }
            if (page != null) { ElementProps.Navigate(page); }
            try
            {
                if (_Element != null)
                {
                    Type type = _Element.GetType();
                    RegistryKey currentUserKey = Registry.CurrentUser;
                    RegistryKey windowsKey = currentUserKey.CreateSubKey("SOFTWARE\\RDCalculator\\Controls\\Windows\\Dialogs\\Window\\" + type.Name);
                    Width = Convert.ToDouble(windowsKey.GetValue("Width") ?? 800);
                    Height = Convert.ToDouble(windowsKey.GetValue("Height") ?? 800);
                    Left = Convert.ToDouble(windowsKey.GetValue("Left") ?? (System.Windows.SystemParameters.PrimaryScreenWidth /2 - Width / 2));
                    Top = Convert.ToDouble(windowsKey.GetValue("Top") ?? (System.Windows.SystemParameters.PrimaryScreenHeight /2 - Height / 2));
                    windowsKey.Close();
                }
            }
            catch(Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage("Ошибка загрузки параметров из реестра", ex);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window window = sender as Window;
            if (_Element != null)
            {
                Type type = _Element.GetType();
                RegistryKey currentUserKey = Registry.CurrentUser;
                RegistryKey windowsKey = currentUserKey.CreateSubKey("SOFTWARE\\RDCalculator\\Controls\\Windows\\Dialogs\\Window\\" + type.Name);
                windowsKey.SetValue("Width", window.Width);
                windowsKey.SetValue("Height", window.Height);
                windowsKey.SetValue("Left", window.Left);
                windowsKey.SetValue("Top", window.Top);
                windowsKey.Close();
            }
        }
    }
}
