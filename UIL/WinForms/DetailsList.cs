﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.WallAndColumn;
using RDBLL.Common.Service;
using RDBLL.Common.Geometry;
using RDBLL.Entity.RCC.Reinforcement;
using RDUIL.WinForms;
using RDUIL.WPF_Windows;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Entity.Results.SC;
using RDBLL.Processors.SC;

namespace RDUIL.WinForms
{
    public partial class frmDetailList : Form
    {
        const string ObjLevels = "Levels";
        const string ObjWalls = "Walls";
        const string ObjSteelColumnBases = "SteelColumnBases";
        const string ObjColumnForses = "ColumnForces";
        const string ObjColumnBaseParts = "ColumnBaseParts";

        private DetailObjectList _detailObjectList;
        private String _formType;
        private Object _parentObject;
        private Object _objectList;
        private bool _isSelectable;

        public frmDetailList(DetailObjectList detailObjectList)
        {
            #region
            InitializeComponent();
            _detailObjectList = detailObjectList;
            _formType = _detailObjectList.DataType;
            _parentObject = _detailObjectList.ParentObject;
            _objectList = _detailObjectList.ObjectList;
            _isSelectable = _detailObjectList.IsSelectable;
            lvDetails.Columns.Clear();

            List<String> ColumnName = new List<String>();
            List<Int32> ColumnWidth = new List<Int32>();
            #endregion
            switch (_formType) //
            {
                case "Buildings": //Создаем окно для списка зданий
                    {
                        this.Text = "Здания";
                        List<String> _ColumnName = new List<String>() { "Наименование"};
                        List<Int32> _ColumnWidth = new List<Int32>() { 600 };
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        break;
                    }
                case ObjLevels: //Создаем окно для списка уровней
                    {
                        this.Text = "Уровни";
                        List<String> _ColumnName = new List<String>() { "Наименование", "Отметка, м", "Высота, мм", "Перекрытие, мм", "Кол-во, шт.", "V_нетто (на этаж)", "V_нетто (всего)" };
                        List<Int32> _ColumnWidth = new List<Int32>() { 100, 100, 100, 100, 100, 150, 150 };
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);

                        var levelList = (List<Level>)_objectList;
                        foreach (Level level in levelList)
                        {
                            NewItemFromLevel(level);
                        }

                        tsbSteelColumnBase.Visible = true;
                        tsbColumnForces.Visible = false;
                        tsbSteelColumnBaseParts.Visible = false;
                        tsbWalls.Visible = true;
                        tsbWallTypes.Visible = true;
                        tsbOpeningTypes.Visible = true;
                        tsbOpenings.Visible = false;
                        tsbReport.Visible = true;
                        break;
                    }
                //Create window for Openings
                case "OpeningPlacings":
                    {
                        this.Text = "Проемы";
                        List<String> _ColumnName = new List<String>() { "Марка", "Привязка от начала, мм", "Привязка снизу, мм", "S_нетто, кв.м" };
                        List<Int32> _ColumnWidth = new List<Int32>() { 250, 150, 150, 80};
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);

                        var openingPlacingList = (List<OpeningPlacing>)_objectList;
                        foreach (OpeningPlacing openingPlacing in openingPlacingList)
                        {
                            NewItemFromOpeningPlacing(openingPlacing);
                        }
                        tsbWalls.Visible = false;
                        tsbWallTypes.Visible = false;
                        tsbOpeningTypes.Visible = false;
                        tsbOpenings.Visible = false;
                        tsbReport.Visible = false;
                        break;
                    }
                //Create window for OpeningType
                case "OpeningTypes": 
                    {
                        this.Text = "Типы проемов";
                        List<String> _ColumnName = new List<String>() { "Марка", "Назначение", "Ширина", "Высота",  "Привязка снизу, мм", "Привязка сверху, мм", "Площадь, кв.м" };
                        List<Int32> _ColumnWidth = new List<Int32>() { 250, 100, 100, 100, 100, 100, 100 };
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        var openingTypeList = (List<OpeningType>)_objectList;
                        foreach (OpeningType openingType in openingTypeList)
                        {
                            NewItemFromOpeningType(openingType);
                        }
                        tsbWalls.Visible = false;
                        tsbWallTypes.Visible = false;
                        tsbOpeningTypes.Visible = false;
                        tsbOpenings.Visible = false;
                        tsbReport.Visible = false;
                        break;
                    }
                case ObjColumnBaseParts:
                    {
                        #region
                        this.Text = "Участки базы стальных колонн";
                        List<String> _ColumnName = new List<String>() { "Наименование", "Размеры, мм", "Макс. напряжения, МПа"};
                        List<Int32> _ColumnWidth = new List<Int32>() { 150, 100, 100};
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        #endregion
                        var basePartList = (List<SteelBasePart>)_objectList;
                        foreach (SteelBasePart basePart in basePartList)
                        {
                            NewItemFromColumnBasePart(basePart);
                        }

                        tsbSteelColumnBase.Visible = false;
                        tsbColumnForces.Visible = false;
                        tsbSteelColumnBaseParts.Visible = false;
                        tsbWalls.Visible = false;
                        tsbWallTypes.Visible = false;
                        tsbOpeningTypes.Visible = false;
                        tsbOpenings.Visible = false;
                        tsbReport.Visible = false;
                        break;
                    }
                case ObjColumnForses:
                    {
                        #region
                        this.Text = "Нагрузки на базы стальных колонн";
                        List<String> _ColumnName = new List<String>() { "Наименование", "Nz, кН", "Mx, кН*м", "My, кН*м", "Qx, кН", "Qx, кН" };
                        List<Int32> _ColumnWidth = new List<Int32>() { 150, 100, 100, 100, 100, 100 };
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        #endregion
                        var LoadSetList = (List<ColumnLoadSet>)_objectList;
                        foreach (ColumnLoadSet loadSet in LoadSetList)
                        {
                            NewItemFromColumnLoadSet(loadSet);
                        }

                        tsbSteelColumnBase.Visible = false;
                        tsbColumnForces.Visible = false;
                        tsbSteelColumnBaseParts.Visible = false;
                        tsbWalls.Visible = false;
                        tsbWallTypes.Visible = false;
                        tsbOpeningTypes.Visible = false;
                        tsbOpenings.Visible = false;
                        tsbReport.Visible = false;
                        break;
                    }
                case ObjSteelColumnBases:
                    {
                        this.Text = "Базы стальных колонн";
                        List<String> _ColumnName = new List<String>() { "Марка", "Размеры, мм", "Привязки болтов, мм", "Напряжения макс/мин"};
                        List<Int32> _ColumnWidth = new List<Int32>() { 150, 100, 150, 150};
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);

                        var SteelColumnBaseList = (List<SteelColumnBase>)_objectList;
                        foreach (SteelColumnBase columnBase in SteelColumnBaseList)
                        {
                            NewItemFromSteelColumnBase(columnBase);
                        }

                        tsbSteelColumnBase.Visible = false;
                        tsbColumnForces.Visible = true;
                        tsbSteelColumnBaseParts.Visible = true;
                        tsbWalls.Visible = false;
                        tsbWallTypes.Visible = false;
                        tsbOpeningTypes.Visible = false;
                        tsbOpenings.Visible = false;
                        tsbReport.Visible = false;
                        break;
                    }
                //Создаем окно для списка стен
                case ObjWalls: 
                {
                        this.Text = "Стены";
                        List<String> _ColumnName = new List<String>() { "Марка", "Проемы", "Размеры, мм", "Верт.стерж.", "Гор.стерж.", "Диаг.стерж.", "S_брутто, кв.м", "S_нетто, кв.м", "V_брутто, куб.м", "V_нетто, куб.м"};
                        List<Int32> _ColumnWidth = new List<Int32>() { 150, 100, 150, 100, 100, 100, 100, 100, 100, 100 };
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);

                        var wallList = (List<Wall>)_objectList;
                        foreach (Wall wall in wallList)
                        {
                            NewItemFromWall(wall);
                        }

                        tsbWalls.Visible = false;
                        tsbWallTypes.Visible = false;
                        tsbOpeningTypes.Visible = false;
                        tsbOpenings.Visible = true;
                        tsbReport.Visible = true;
                        break;
                }
                case "WallTypes": //Создаем окно для списка стен
                    {
                        this.Text = "Типы стен";
                        List<String> _ColumnName = new List<String>() { "Марка", "Толщина, мм", "Шаг верт, мм", "Шаг гор, мм"};
                        List<Int32> _ColumnWidth = new List<Int32>() { 300, 80, 80, 80};
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        var wallTypeList = (List<WallType>)_objectList;
                        foreach (WallType wallType in wallTypeList)
                        {
                            NewItemFromWallType(wallType);
                        }
                        tsbWalls.Visible = false;
                        tsbWallTypes.Visible = false;
                        tsbOpeningTypes.Visible = false;
                        tsbOpenings.Visible = false;
                        tsbReport.Visible = false;
                        break;
                    }
            }
            #region
            foreach (String S in ColumnName)
            {
                lvDetails.Columns.Add(S);
            }
            for (int i = 0; i < lvDetails.Columns.Count; i++)
            {
                lvDetails.Columns[i].Width= ColumnWidth[i];
            }
            for (int i=1; i<lvDetails.Columns.Count; i++)
            {
                lvDetails.Columns[i].TextAlign = HorizontalAlignment.Center;
            }
            if (_isSelectable) { btnClose.Name = "Выбрать"; } else { btnClose.Name = "Закрыть"; }
            lvDetails.MultiSelect = !_isSelectable;
            #endregion
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tsbNew_Click(object sender, EventArgs e)
        {
            ProgrammSettings.IsDataChanged = true;
            switch (_formType) //
            {
                case "Buildings":
                    {
                        break;
                    }
                case "Levels":
                    {
                        Level level = new Level((Building)_parentObject);
                        NewItemFromLevel(level);
                        break;
                    }
                //Add new Opening
                case "OpeningPlacings":
                    {
                        OpeningPlacing openingPlacing = new OpeningPlacing((Wall)_parentObject);
                        NewItemFromOpeningPlacing(openingPlacing);
                        break;
                    }
                //Add new OpeningType
                case "OpeningTypes":
                    {
                        OpeningType openingType = new OpeningType((Building)_parentObject);
                        NewItemFromOpeningType(openingType);
                        break;
                    }
                case ObjColumnBaseParts:
                    {
                        SteelBasePart basePart = new SteelBasePart((SteelColumnBase)_parentObject);
                        NewItemFromColumnBasePart(basePart);
                        break;
                    }
                case ObjColumnForses:
                    {
                        ColumnLoadSet columnLoadSet = new ColumnLoadSet((SteelColumnBase)_parentObject);
                        NewItemFromColumnLoadSet(columnLoadSet);
                        break;
                    }
                case ObjSteelColumnBases:
                    {
                        SteelColumnBase steelColumnBase = new SteelColumnBase((Level)_parentObject);
                        NewItemFromSteelColumnBase(steelColumnBase);
                        break;
                    }
                case "Walls":
                    {
                        Wall wall = new Wall((Level)_parentObject);
                        NewItemFromWall(wall);
                        break;
                    }
                case "WallTypes":
                    {
                        WallType wallType = new WallType((Building)_parentObject);
                        NewItemFromWallType(wallType);
                        break;
                    }
            }

        }
        private void tsbEdit_Click(object sender, EventArgs e)
        {
            switch (_formType) //
            {
                case "Buildings":
                    {
                       break;
                    }
                case "Levels":
                    {
                        var objList = (List<Level>)_objectList;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            frmLevel frmObj = new frmLevel(objList[i]);
                            frmObj.ShowDialog();
                            if (frmObj.DialogResult == DialogResult.OK)
                            {
                                EditItemFromLevel(lvDetails.Items[i], objList[i]);
                            }
                        }
                        break;
                    }
                case "OpeningPlacings":
                    {
                        var OpeningPlacingList = (List<OpeningPlacing>)_objectList;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            var frmOpening = new frmOpening(OpeningPlacingList[i]);
                            frmOpening.ShowDialog();
                            if (frmOpening.DialogResult == DialogResult.OK)
                            {
                                EditItemFromOpeningPlacing(lvDetails.Items[i], OpeningPlacingList[i]);
                            }
                        }
                        break;
                    }
                case "OpeningTypes":
                    {
                        var objList = (List<OpeningType>)_objectList;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            var frmObj = new frmOpeningType(objList[i]);
                            frmObj.ShowDialog();
                            if (frmObj.DialogResult == DialogResult.OK)
                            {
                                EditItemFromOpeningType(lvDetails.Items[i], objList[i]);
                            }
                        }
                        break;
                    }
                case ObjColumnBaseParts:
                    {
                        var BasePartList = (List<SteelBasePart>)_objectList;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            WndSteelBasePart wndSteelBasePart = new WndSteelBasePart(BasePartList[i]);
                            wndSteelBasePart.ShowDialog();
                            EditItemFromColumnBasePart(lvDetails.Items[i], BasePartList[i]);
                        }
                        break;
                    }
                case ObjColumnForses:
                    {
                        var ColumnLoadSetList = (List<ColumnLoadSet>)_objectList;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            wndForces wndForces = new wndForces(ColumnLoadSetList[i]);
                            wndForces.ShowDialog();
                            EditItemFromColumnLoadSet(lvDetails.Items[i], ColumnLoadSetList[i]);
                        }
                        break;
                    }      
                case ObjSteelColumnBases:
                    {
                        var steelColumnBaseList = (List<SteelColumnBase>)_objectList;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            WndSteelColumnBase wndSteelColumnBase = new WndSteelColumnBase(steelColumnBaseList[i]);
                            wndSteelColumnBase.ShowDialog();
                            EditItemFromSteelColumnBase(lvDetails.Items[i], steelColumnBaseList[i]);
                        }
                        break;
                    }
                case "Walls":
                    {
                        var wallList = (List<Wall>)_objectList;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            var frmWall = new frmWall(wallList[i]);
                            frmWall.ShowDialog();
                            if (frmWall.DialogResult == DialogResult.OK)
                            {
                                EditItemFromWall(lvDetails.Items[i], wallList[i]);
                            }
                        }
                        break;
                    }
                case "WallTypes":
                    {
                        var objList = (List<WallType>)_objectList;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            var frmObj = new frmWallType(objList[i]);
                            frmObj.ShowDialog();
                            if (frmObj.DialogResult == DialogResult.OK)
                            {
                                EditItemFromWallType(lvDetails.Items[i], objList[i]);
                            }
                        }
                        break;
                    }
            }
        }
        //Add New ListView Item from Level instance
        private ListViewItem NewItemFromLevel(Level obj)
        {
            ListViewItem NewItem = new ListViewItem();
            EditItemFromLevel(NewItem, obj);
            lvDetails.Items.Add(NewItem);
            return NewItem;
        }
        private ListViewItem NewItemFromOpeningPlacing(OpeningPlacing obj)
        {
            ListViewItem NewItem = new ListViewItem();
            EditItemFromOpeningPlacing(NewItem, obj);
            lvDetails.Items.Add(NewItem);
            return NewItem;
        }
        //Add New ListView Item from OpeningType instance
        private ListViewItem NewItemFromOpeningType(OpeningType obj)
        {
            ListViewItem NewItem = new ListViewItem();
            EditItemFromOpeningType(NewItem, obj);
            lvDetails.Items.Add(NewItem);
            return NewItem;
        }
        private ListViewItem NewItemFromColumnBasePart(SteelBasePart obj)
        {
            ListViewItem NewItem = new ListViewItem();
            EditItemFromColumnBasePart(NewItem, obj);
            lvDetails.Items.Add(NewItem);
            return NewItem;
        }
        private ListViewItem NewItemFromColumnLoadSet(ColumnLoadSet obj)
        {
            ListViewItem NewItem = new ListViewItem();
            EditItemFromColumnLoadSet(NewItem, obj);
            lvDetails.Items.Add(NewItem);
            return NewItem;
        }
        private ListViewItem NewItemFromSteelColumnBase(SteelColumnBase obj)
        {
            ListViewItem NewItem = new ListViewItem();
            EditItemFromSteelColumnBase(NewItem, obj);
            lvDetails.Items.Add(NewItem);
            return NewItem;
        }
        private ListViewItem NewItemFromWall(Wall obj)
        {
            ListViewItem NewItem = new ListViewItem();
            EditItemFromWall(NewItem, obj);
            lvDetails.Items.Add(NewItem);
            return NewItem;
        }
        private ListViewItem NewItemFromWallType(WallType obj)
        {
            ListViewItem NewItem = new ListViewItem();
            EditItemFromWallType(NewItem, obj);
            lvDetails.Items.Add(NewItem);
            return NewItem;
        }
        //Edit item of listView from Level
        private void EditItemFromLevel(ListViewItem Item, Level level)
        {
            Item.SubItems.Clear();
            Item.Text = level.Name;
            Item.SubItems.Add(Convert.ToString(level.FloorLevel));
            Item.SubItems.Add(Convert.ToString(level.Height));
            Item.SubItems.Add(Convert.ToString(level.TopOffset));
            Item.SubItems.Add(Convert.ToString(level.Quant));
            Item.SubItems.Add(Convert.ToString(Math.Round(level.GetConcreteVolumeNetto()/1000000)/1000));
            Item.SubItems.Add(Convert.ToString(Math.Round(level.GetConcreteVolumeNetto() * level.Quant / 1000000) / 1000));
        }
        private void EditItemFromOpeningPlacing(ListViewItem Item, OpeningPlacing openingPlacing)
        {
            Item.SubItems.Clear();
            Item.Text = openingPlacing.OpeningType.FullName();
            Item.SubItems.Add(Convert.ToString(openingPlacing.Left ));
            Item.SubItems.Add(Convert.ToString(openingPlacing.GetBottom()));
            Item.SubItems.Add(Convert.ToString(Math.Round(openingPlacing.OpeningType.GetArea() / 1000) / 1000));
        }
        private void EditItemFromOpeningType(ListViewItem Item, OpeningType openingType)
        {
            Item.SubItems.Clear();
            Item.Text = openingType.Name;
            Item.SubItems.Add(Convert.ToString(openingType.Purpose));
            Item.SubItems.Add(Convert.ToString(openingType.Width));
            Item.SubItems.Add(Convert.ToString(openingType.Height));
            Item.SubItems.Add(Convert.ToString(openingType.Bottom));
            Item.SubItems.Add(Convert.ToString(openingType.Bottom+openingType.Height));
            Item.SubItems.Add(Convert.ToString(Math.Round(openingType.GetArea()/1000)/1000));
        }
        private void EditItemFromColumnBasePart(ListViewItem Item, SteelBasePart basePart)
        {
            Item.SubItems.Clear();
            Item.Text = basePart.Name;
            Item.SubItems.Add(Convert.ToString(basePart.Width * 1000) +"x" + Convert.ToString(basePart.Length * 1000));
            ColumnBasePartResult result = SteelColumnBasePartProcessor.GetResult(basePart);
            double maxStress = result.MaxStress;
            maxStress = Math.Round(maxStress / 1000) / 1000;
            Item.SubItems.Add(Convert.ToString(maxStress));
        }
        private void EditItemFromColumnLoadSet(ListViewItem Item, ColumnLoadSet loadSet)
        {
            Item.SubItems.Clear();
            Item.Text = loadSet.Name;
            Item.SubItems.Add(Convert.ToString(loadSet.Force_Nz/1000));
            Item.SubItems.Add(Convert.ToString(loadSet.Force_Mx / 1000));
            Item.SubItems.Add(Convert.ToString(loadSet.Force_My / 1000));
            Item.SubItems.Add(Convert.ToString(loadSet.Force_Qx / 1000));
            Item.SubItems.Add(Convert.ToString(loadSet.Force_Qy / 1000));
        }
        private void EditItemFromSteelColumnBase(ListViewItem Item, SteelColumnBase columnBase)
        {
            Item.SubItems.Clear();
            Item.Text = columnBase.Name;
            Item.SubItems.Add(Convert.ToString(columnBase.Width) + "x" + Convert.ToString(columnBase.Length));
            Item.SubItems.Add(Convert.ToString(columnBase.WidthBoltDist) + "x" + Convert.ToString(columnBase.LengthBoltDist));
            SteelColumnBaseProcessor columBaseProcessor = new SteelColumnBaseProcessor();
            ColumnBaseResult baseResult = columBaseProcessor.GetResult(columnBase);
            double maxStress = baseResult.MaxStress;
            maxStress = Math.Round(maxStress / 1000) / 1000;
            double minStress = baseResult.MinStress;
            minStress = Math.Round(minStress / 1000) / 1000;
            Item.SubItems.Add(Convert.ToString(maxStress) + " / " + Convert.ToString(minStress));
        }
        private void EditItemFromWall (ListViewItem Item, Wall wall)
        {
            Item.SubItems.Clear();
            Item.Text = wall.WallType.Name + " - "+ wall.Name + " ("+ Convert.ToString(wall.WallType.Thickness)+"мм)";
            Item.SubItems.Add(wall.GetStringOpenings());
            Item.SubItems.Add(wall.GetStringSizes());
            Item.SubItems.Add(Convert.ToString(wall.VertBarQuantity()));
            Item.SubItems.Add(Convert.ToString(wall.HorBarQuantity()));
            Item.SubItems.Add(Convert.ToString(wall.IncBarQuantity()));
            Item.SubItems.Add(Convert.ToString(Math.Round(wall.GetConcreteAreaBrutto() / 1000)/1000));
            Item.SubItems.Add(Convert.ToString(Math.Round(wall.GetConcreteAreaNetto() / 1000)/1000));
            Item.SubItems.Add(Convert.ToString(Math.Round(wall.GetConcreteVolumeBrutto() / 1000000)/1000));
            Item.SubItems.Add(Convert.ToString(Math.Round(wall.GetConcreteVolumeNetto() / 1000000)/1000));
        }
        private void EditItemFromWallType(ListViewItem Item, WallType wallType)
        {
            Item.SubItems.Clear();
            Item.Text = wallType.Name;
            Item.SubItems.Add(Convert.ToString(wallType.Thickness));
            Item.SubItems.Add(Convert.ToString(wallType.VertSpacingSetting.MainSpacing));
            Item.SubItems.Add(Convert.ToString(wallType.HorSpacingSetting.MainSpacing));
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            ProgrammSettings.IsDataChanged = true;
            //Show promt Dialog
            if (lvDetails.SelectedIndices.Count == 0)
            { MessageBox.Show("Выберите элемент из списка", "Ничего не выбрано"); }
            //При удалении нескольких объектов учитываем сдвижку номеров в коллекции вызванную удалении
            int i = 0;
            switch (_formType) //
            {
                case "Buildings":
                    {
                        break;
                    }
                case "Levels":
                    {
                        foreach (int j in lvDetails.SelectedIndices)
                        {
                            if (((List<Level>)_objectList)[j-i].WallList.Count == 0)
                            {
                                ((List<Level>)_objectList).RemoveAt(j - i);
                                lvDetails.Items.RemoveAt(j - i);
                                i++;
                            }
                            else { MessageBox.Show("Удалите стены, принадлежащие уровню", "Уровень содержит стены"); }        
                        }
                        break;
                    }
                case "OpeningPlacings":
                    {
                        foreach (int j in lvDetails.SelectedIndices)
                        {
                            ((List<OpeningPlacing>)_objectList).RemoveAt(j - i);
                            lvDetails.Items.RemoveAt(j - i);
                            i++;
                        }
                        break;
                    }
                case "OpeningTypes":
                    {
                        //Необходимо добавить проверку на существование стен имеющих данный тип стены
                        foreach (int j in lvDetails.SelectedIndices)
                        {
                            ((List<OpeningType>)_objectList).RemoveAt(j - i);
                            lvDetails.Items.RemoveAt(j - i);
                            i++;
                        }
                        break;
                    }
                case ObjColumnBaseParts:
                    {
                        //Необходимо добавить проверку на существование стен имеющих данный тип стены
                        foreach (int j in lvDetails.SelectedIndices)
                        {
                            ((List<SteelBasePart>)_objectList).RemoveAt(j - i);
                            lvDetails.Items.RemoveAt(j - i);
                            i++;
                        }
                        break;
                    }
                case ObjColumnForses:
                    {
                        //Необходимо добавить проверку на существование стен имеющих данный тип стены
                        foreach (int j in lvDetails.SelectedIndices)
                        {
                            ((List<ColumnLoadSet>)_objectList).RemoveAt(j - i);
                            lvDetails.Items.RemoveAt(j - i);
                            i++;
                        }
                        break;
                    }
                case ObjSteelColumnBases:
                    {
                        //Необходимо добавить проверку на существование стен имеющих данный тип стены
                        foreach (int j in lvDetails.SelectedIndices)
                        {
                            ((List<SteelColumnBase>)_objectList).RemoveAt(j - i);
                            lvDetails.Items.RemoveAt(j - i);
                            i++;
                        }
                        break;
                    }
                case "Walls":
                    {
                        foreach (int j in lvDetails.SelectedIndices)
                        {
                            ((List<Wall>)_objectList).RemoveAt(j - i);
                            lvDetails.Items.RemoveAt(j - i);
                            i++;
                        }
                    break;
                    }
                case "WallTypes":
                    {
                        //Необходимо добавить проверку на существование стен имеющих данный тип стены
                        foreach (int j in lvDetails.SelectedIndices)
                        {
                            ((List<WallType>)_objectList).RemoveAt(j - i);
                            lvDetails.Items.RemoveAt(j - i);
                            i++;
                        }
                        break;
                    }       
            }
        }
        private void tsbWalls_Click(object sender, EventArgs e)
        {
            if (lvDetails.SelectedIndices.Count == 1)
            {
                Level level;
                foreach (int i in lvDetails.SelectedIndices)
                {
                    level = ((List<Level>)_objectList)[i];
                    var detailObjectList = new DetailObjectList("Walls", level, level.WallList, false);
                    frmDetailList DetailForm = new frmDetailList(detailObjectList);
                    this.Visible = false;
                    DetailForm.ShowDialog();
                    this.Visible = true;
                    EditItemFromLevel(lvDetails.Items[i], level);
                }
            }
            else
            { MessageBox.Show("Выберите один элемент из списка", "Неверный выбор");}         
        }
        private void tsbWallType_Click(object sender, EventArgs e)
        {
            Building building = (Building)_parentObject;
            var detailObjectList = new DetailObjectList("WallTypes", building, building.WallTypeList, false);
            frmDetailList DetailForm = new frmDetailList(detailObjectList);
            this.Visible = false;
            DetailForm.ShowDialog();
            this.Visible = true;
            foreach (ListViewItem i in lvDetails.Items)
            {
                EditItemFromLevel(i, building.LevelList[i.Index]);
            }
            
        }
        private void tsbOpeningTypes_Click(object sender, EventArgs e)
        {
            Building building = (Building)_parentObject;
            var detailObjectList = new DetailObjectList("OpeningTypes", building, building.OpeningTypeList, false);
            frmDetailList DetailForm = new frmDetailList(detailObjectList);
            this.Visible = false;
            DetailForm.ShowDialog();
            this.Visible = true;
        }
        private void tsbOpenings_Click(object sender, EventArgs e)
        {
            if (lvDetails.SelectedIndices.Count == 1)
            {
                Wall wall;
                foreach (int i in lvDetails.SelectedIndices)
                {
                    wall = ((List<Wall>)_objectList)[i];
                    var detailObjectList = new DetailObjectList("OpeningPlacings", wall, wall.OpeningPlacingList , false);
                    frmDetailList DetailForm = new frmDetailList(detailObjectList);
                    this.Visible = false;
                    DetailForm.ShowDialog();
                    this.Visible = true;
                    EditItemFromWall(lvDetails.Items[i], wall);
                }
            }
            else
            { MessageBox.Show("Выберите один элемент из списка", "Неверный выбор"); }
        }
        private void tsbReport_Click(object sender, EventArgs e)
        {
            if (lvDetails.SelectedIndices.Count == 0)
            { MessageBox.Show("Выберите элемент из списка", "Ничего не выбрано"); }
            //При удалении нескольких объектов учитываем сдвижку номеров в коллекции вызванную удалении
            decimal sumVolume = 0;
            switch (_formType) //
            {
                case "Levels":
                    {
                        Level level;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            level = ((List<Level>)_objectList)[i];
                            sumVolume += level.GetConcreteVolumeNetto();
                        }
                        sumVolume = (Math.Round(sumVolume / 1000000)) / 1000;
                        { MessageBox.Show(Convert.ToString(sumVolume) + "куб.м.", "Суммарный объем бетона по уровням"); }
                        break;
                    }
                case "Walls":
                    {
                        List<Wall> objectList = new List<Wall>();
                        Wall wall;
                        foreach (int i in lvDetails.SelectedIndices)
                        {
                            wall = ((List<Wall>)_objectList)[i];
                            objectList.Add(wall); 
                        }
                        var frmResult = new frmResult(objectList);
                        frmResult.ShowDialog();
                        break;
                    }
            }
            
        }
        private void tsbSteelColumnBase_Click(object sender, EventArgs e)
        {
            if (lvDetails.SelectedIndices.Count == 1)
            {
                Level level;
                foreach (int i in lvDetails.SelectedIndices)
                {
                    level = ((List<Level>)_objectList)[i];
                    var detailObjectList = new DetailObjectList("SteelColumnBases", level, level.SteelColumnBaseList, false);
                    frmDetailList DetailForm = new frmDetailList(detailObjectList);
                    this.Visible = false;
                    DetailForm.ShowDialog();
                    this.Visible = true;
                    EditItemFromLevel(lvDetails.Items[i], level);
                }
            }
            else
            { MessageBox.Show("Выберите один элемент из списка", "Неверный выбор"); }
        }
        private void tsbColumnForces_Click(object sender, EventArgs e)
        {
            if (lvDetails.SelectedIndices.Count == 1)
            {
                SteelColumnBase steelColumnBase;
                foreach (int i in lvDetails.SelectedIndices)
                {
                    steelColumnBase = ((List<SteelColumnBase>)_objectList)[i];
                    var detailObjectList = new DetailObjectList("ColumnForces", steelColumnBase, steelColumnBase.Loads, false);
                    frmDetailList DetailForm = new frmDetailList(detailObjectList);
                    this.Visible = false;
                    DetailForm.ShowDialog();
                    this.Visible = true;
                    EditItemFromSteelColumnBase(lvDetails.Items[i], steelColumnBase);
                }
            }
            else
            { MessageBox.Show("Выберите один элемент из списка", "Неверный выбор"); }
        }
        private void tsbSteelBaseParts_Click(object sender, EventArgs e)
        {
            if (lvDetails.SelectedIndices.Count == 1)
            {
                SteelColumnBase steelColumnBase;
                foreach (int i in lvDetails.SelectedIndices)
                {
                    steelColumnBase = ((List<SteelColumnBase>)_objectList)[i];
                    var detailObjectList = new DetailObjectList("ColumnBaseParts", steelColumnBase, steelColumnBase.SteelBaseParts, false);
                    frmDetailList DetailForm = new frmDetailList(detailObjectList);
                    this.Visible = false;
                    DetailForm.ShowDialog();
                    this.Visible = true;
                    EditItemFromSteelColumnBase(lvDetails.Items[i], steelColumnBase);
                }
            }
            else
            { MessageBox.Show("Выберите один элемент из списка", "Неверный выбор"); }
        }
    }
}
