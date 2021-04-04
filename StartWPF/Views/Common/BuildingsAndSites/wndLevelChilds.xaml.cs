using CSL.Reports;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.SC.Column;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Winforms = System.Windows.Forms;
using RDBLL.Entity.SC.Column.SteelBases.Builders;
using RDStartWPF.Infrasructure.Reports;
using RDBLL.Entity.SC.Column.SteelBases.Patterns;
using RDStartWPF.Views.Common.Patterns;
using RDStartWPF.Views.Common.Patterns.ControlClasses;
using RDBLL.Common.Interfaces;
using RDBLL.Entity.RCC.Foundations;
using RDStartWPF.Views.RCC.Foundations;
using RDStartWPF.Views.SC.Columns.Bases;

namespace RDStartWPF.Views.Common.BuildingsAndSites
{
    /// <summary>
    /// Логика взаимодействия для wndSteelBases.xaml
    /// </summary>
    public partial class wndLevelChilds : Window
    {
        private Level _level;
        private string _childName;
        private ObservableCollection<IHasParent> _collection;
        public wndLevelChilds(Level level, string childName)
        {
            _level = level;
            _childName = childName;
            _collection = _level.Children;
            InitializeComponent();
            this.DataContext = level.Children;           
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            if (_childName == "SteelBases") ShowReportProcessor.ShowSteelBasesReport();
            if (_childName == "Foundations") ShowReportProcessor.ShowFoundationsReport();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_childName == "SteelBases")
            {
                List<PatternCard> patternCards = SelectPatternProcessor.SelectSteelBasePattern(_level);
                WndPatternSelect wndPatternSelect = new WndPatternSelect(patternCards);
                wndPatternSelect.ShowDialog();
            }
            if (_childName == "Foundations")
            {
                Foundation foundation = new Foundation(_level);
                //Надо создать элемент, иначе некуда будет сохранять дочерние
                foundation.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                wndFoundation wndFoundation = new wndFoundation(foundation);
                wndFoundation.ShowDialog();
                if (wndFoundation.DialogResult == true)
                {
                    try
                    {
                        foundation.SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                        ProgrammSettings.IsDataChanged = true;
                    }
                    catch (Exception ex)
                    {
                        CommonErrorProcessor.ShowErrorMessage("Ошибка сохранения в элементе: " + foundation.Name, ex);
                    }

                }
                else //Если пользователь не подтвердил создание, то удаляем элемент
                {
                    foundation.UnRegisterParent();
                    foundation.DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
                }
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (LvMain.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = LvMain.SelectedIndex;
                    if (LvMain.Items.Count == 1) LvMain.UnselectAll();
                    else if (a < (LvMain.Items.Count - 1)) LvMain.SelectedIndex = a + 1;
                    else LvMain.SelectedIndex = a - 1;
                    _collection[a].DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
                    _collection.RemoveAt(a);
                    ProgrammSettings.IsDataChanged = true;
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                bool dialogResult = false;
                if (_collection[a] is SteelBase)
                {
                    SteelBase child = _collection[a] as SteelBase;
                    WndSteelColumnBase wnd = new WndSteelColumnBase(child);
                    wnd.ShowDialog();
                    if (wnd.DialogResult == true) dialogResult = true;
                }
                if (_collection[a] is Foundation)
                {
                    Foundation child = _collection[a] as Foundation;
                    wndFoundation wnd = new wndFoundation(child);
                    wnd.ShowDialog();
                    if (wnd.DialogResult == true) dialogResult = true;
                }
                if (dialogResult)
                {
                    try
                    {
                        _collection[a].SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                        //(_collection[a] as SteelBase).IsActual = false;
                        ProgrammSettings.IsDataChanged = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка сохранения :" + ex);
                    }
                }
                else
                {
                    _collection[a].OpenFromDataSet(ProgrammSettings.CurrentDataSet);
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void TbxName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ProgrammSettings.IsDataChanged = true;
        }

        private void BtnDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if (LvMain.SelectedIndex >= 0)
            {
                int a = LvMain.SelectedIndex;
                IHasParent child = _collection[a];
                if (child is ICloneable)
                {
                    ICloneable cloneable = child as ICloneable;
                    object newObj = cloneable.Clone();
                    IHasParent newChild = newObj as IHasParent;
                    newChild.RegisterParent(child.ParentMember);
                    try
                    {
                        newChild.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                        ProgrammSettings.IsDataChanged = true;
                    }
                    catch (Exception ex)
                    {
                        CommonErrorProcessor.ShowErrorMessage("Ошибка сохранения элемента: " + Name, ex);
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно клонировать элемент", "Элемент не поддерживает клонирование");
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }
    }
}
