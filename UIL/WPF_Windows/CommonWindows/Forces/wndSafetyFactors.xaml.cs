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
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using System.Collections.ObjectModel;

namespace RDUIL.WPF_Windows.CommonWindows.Forces
{
    /// <summary>
    /// Логика взаимодействия для wndPartialSafetyFactors.xaml
    /// </summary>
    public partial class wndSafetyFactors : Window
    {
        private MaterialUsing _materialUsing;
        private ObservableCollection<SafetyFactor> _collection;

        public wndSafetyFactors(MaterialUsing materialUsing)
        {
            _materialUsing = materialUsing;
            _collection = _materialUsing.SafetyFactors;
            InitializeComponent();
            this.DataContext = _collection;
            if (_collection.Count > 0) { LvMain.SelectedIndex = 0; }
        }
    }
}
