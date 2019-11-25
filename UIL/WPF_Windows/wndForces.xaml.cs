using RDBLL.Common.Service;
using RDBLL.Forces;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Winforms = System.Windows.Forms;
using RDBLL.Common.Interfaces;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для wndForces.xaml
    /// </summary>
    public partial class wndForces : Window
    {
        private ForcesGroup _forcesGroup;
        private IHaveForcesGroups _haveForcesGroups;
        private ObservableCollection<LoadSet> _loadSets;
        public wndForces(IHaveForcesGroups haveForcesGroups)
        {
            InitializeComponent();
            _haveForcesGroups = haveForcesGroups;
            _forcesGroup = _haveForcesGroups.ForcesGroups[0];
            _loadSets = _forcesGroup.LoadSets;
            this.DataContext = _forcesGroup;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _forcesGroup.SetParentsNotActual();
                ProgrammSettings.IsDataChanged = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        private void btnAddLoad_Click(object sender, RoutedEventArgs e)
        {
            _forcesGroup.LoadSets.Add(new LoadSet(_forcesGroup));
        }
        private void stpLoadsBtns_MouseLeave(object sender, MouseEventArgs e)
        {
            ((StackPanel)sender).Opacity = 0.5;
        }
        private void lvLoadSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvLoadSet.SelectedIndex >= 0)
            {
                lvForcesList.ItemsSource = _forcesGroup.LoadSets[lvLoadSet.SelectedIndex].ForceParameters;
            }
            else
            {
                lvForcesList.ItemsSource = null;
            }
                
        }
        private void stpLoadsBtns_MouseMove(object sender, MouseEventArgs e)
        {
            ((StackPanel)sender).Opacity = 1;
        }
        private void btnDeleteLoad_Click(object sender, RoutedEventArgs e)
        {
            if (lvLoadSet.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = lvLoadSet.SelectedIndex;
                    if (lvLoadSet.Items.Count == 1) lvLoadSet.UnselectAll();
                    else if (a < (lvLoadSet.Items.Count - 1)) lvLoadSet.SelectedIndex = a + 1;
                    else lvLoadSet.SelectedIndex = a - 1;
                    _forcesGroup.LoadSets.RemoveAt(a);
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }              
        }
        private void btnAddForce_Click(object sender, RoutedEventArgs e)
        {
            if (lvLoadSet.SelectedIndex >= 0)
            {
                int a = lvLoadSet.SelectedIndex;
                LoadSet loadSet = _forcesGroup.LoadSets[a];
                loadSet.ForceParameters.Add(new ForceParameter(loadSet) { KindId = 1 });
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }
        private void btnDeleteForce_Click(object sender, RoutedEventArgs e)
        {
            if (lvForcesList.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = lvForcesList.SelectedIndex;
                    if (lvForcesList.Items.Count == 1) lvForcesList.UnselectAll();
                    else if (a < (lvForcesList.Items.Count - 1)) lvForcesList.SelectedIndex = a + 1;
                    else lvForcesList.SelectedIndex = a - 1;
                    _forcesGroup.LoadSets[lvLoadSet.SelectedIndex].ForceParameters.RemoveAt(a);
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }

        }
    }
}
