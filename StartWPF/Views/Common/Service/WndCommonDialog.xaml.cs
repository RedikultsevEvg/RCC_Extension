using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.SC.Column.SteelBases;
using RDStartWPF.ViewModels.Base;
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
                MinWidth = 600;
                MinHeight = 600;
            }
            else if (_Element is Punching)
            {
                page = new PgPunching(_Element as Punching);
                Title = "Продавливание плиты колонной прямоугольного сечения";
                MinWidth = 600;
                MinHeight = 400;
            }
            if (page !=null) ElementProps.Navigate(page);
        }
    }
}
