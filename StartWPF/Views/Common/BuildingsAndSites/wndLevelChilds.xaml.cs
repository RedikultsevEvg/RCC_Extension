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
using FoundBuilder = RDBLL.Entity.RCC.Foundations.Builders;
using RDBLL.Entity.SC.Column.SteelBases.Factories;
using RDBLL.Entity.SC.Column.SteelBases;
using RDStartWPF.Views.Common.Service;
using RDBLL.Common.Service.DsOperations;
using System.Data;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings.Factories;

namespace RDStartWPF.Views.Common.BuildingsAndSites
{
    /// <summary>
    /// Логика взаимодействия для wndSteelBases.xaml
    /// </summary>
    public partial class wndLevelChilds : Window
    {
        private Level _level;
        private LvlChildType _ChildType;
        private string _childName;
        private ObservableCollection<IHasParent> _collection;
        /// <summary>
        /// Конструктор окна
        /// </summary>
        /// <param name="level"></param>
        /// <param name="childType"></param>
        public wndLevelChilds(Level level, LvlChildType childType)
        {
            _level = level;
            _ChildType = childType;
            _collection = _level.Children;
            InitializeComponent();
            this.DataContext = level.Children;           
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (_ChildType)
                {
                    case LvlChildType.SteelBase:
                        {
                            ShowReportProcessor.ShowSteelBasesReport();
                            break;
                        }
                    case LvlChildType.Foundation:
                        {
                            ShowReportProcessor.ShowFoundationsReport();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage("Ошибка вывода отчета: ", ex);
            }
            
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

            switch (_ChildType)
            {
                case LvlChildType.SteelBase:
                    {
                        List<PatternCard> patternCards = SelectPatternProcessor.SelectSteelBasePattern(_level);
                        WndPatternSelect wndPatternSelect = new WndPatternSelect(patternCards);
                        wndPatternSelect.ShowDialog();
                        break;
                    }
                case LvlChildType.SteelBasePartGroup:
                    {
                        SteelBasePartGroup newItem =  GroupFactory.GetSteelBasePartGroup(GroupType.Type1);
                        AddOrEditLevelChild(newItem, true);
                        break;
                    }
                case LvlChildType.Foundation:
                    {
                        FoundBuilder.BuilderBase builder = new FoundBuilder.BuilderTemplate1();
                        Foundation foundation = FoundBuilder.FoundMaker.MakeFoundation(builder);
                        foundation.RegisterParent(_level);
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
                        break;
                    }
                case LvlChildType.Punching:
                    {
                        IPunchingFactory punchingFactory = new CentralColumnPunching();
                        IHasParent newItem = punchingFactory.CreatePunching();
                        AddOrEditLevelChild(newItem, true);
                    }
                    break;
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
                IHasParent item = _collection[a];
                    
                if (_collection[a] is SteelBase)
                {
                    SteelBase child = _collection[a] as SteelBase;
                    WndSteelColumnBase wnd = new WndSteelColumnBase(child);
                    wnd.ShowDialog();
                    if (wnd.DialogResult == true) dialogResult = true;
                }
                else if ((item is SteelBasePartGroup) || (item is Punching))
                {
                    //WndCommonDialog dialog = new WndCommonDialog(item);
                    //dialog.ShowDialog();
                    //if (dialog.DialogResult == true)
                    dialogResult = AddOrEditLevelChild(item, false);
                }
                else if (_collection[a] is Foundation)
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
                    DataRow row = DsOperation.OpenFromDataSetById(ProgrammSettings.CurrentDataSet, item);
                    _collection[a].OpenFromDataSet(row);
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

        private bool AddOrEditLevelChild(IHasParent newItem, bool createNew)
        {
            if (createNew)
            {
                newItem.RegisterParent(_level);
                newItem.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
            }
            WndCommonDialog dialog = new WndCommonDialog(newItem);
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                try
                {
                    newItem.SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                    ProgrammSettings.IsDataChanged = true;
                    return true;
                }
                catch (Exception ex)
                {
                    CommonErrorProcessor.ShowErrorMessage($"Ошибка сохранения в элементе: {newItem.Name}", $"Тип элемента: {newItem.GetType().Name}, \n Код элемента: Id={newItem.Id}, \n Имя элемента: {newItem.Name}", ex);
                    return false;
                }
            }
            else
            {
                if (createNew)
                {
                    newItem.UnRegisterParent();
                    newItem.DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
                }
                return false;
            }
        }
    }
}
