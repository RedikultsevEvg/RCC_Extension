using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.WallAndColumn;
using System.Collections.ObjectModel;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Entity.Soils;
using DAL.Common;
using RDBLL.Entity.MeasureUnits;
using System.ComponentModel;
using System.Windows;

namespace RDBLL.Entity.RCC.BuildingAndSite
{
    /// <summary>
    /// Класс строительного объекта
    /// </summary>
    public class BuildingSite :ICloneable, IDsSaveable
    {
        /// <summary>
        /// Код строительного объекта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Коллекция зданий
        /// </summary>
        public ObservableCollection<Building> Buildings { get; set; }
        /// <summary>
        /// Коллекция грунтов строительного объекта
        /// </summary>
        public ObservableCollection<Soil> Soils { get; set; }
        /// <summary>
        /// Коллекция геологических разрезов
        /// </summary>
        public ObservableCollection<SoilSection> SoilSections { get; set; }
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "BuildingSites"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables[GetTableName()];
            if (createNew)
            {
                row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            else
            {
                var tmpRow = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
                row = tmpRow;
            }
            //Задаем свойства элемента
            row["Id"] = Id;
            row["ParentId"] = 0;
            row["Name"] = Name;
            dataTable.AcceptChanges();
            //Добавляем вложенные элементы
            foreach (Soil soil in Soils)
            {
                soil.SaveToDataSet(dataSet, createNew);
            }
            foreach (SoilSection soilSection in SoilSections)
            {
                soilSection.SaveToDataSet(dataSet, createNew);
            }
            //Здание надо добавлять позже грунтов, так как грунты нужны для фундаментов
            foreach (Building building in Buildings)
            {
                building.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Открытие из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[GetTableName()];
            var row = (from dataRow in dataTable.AsEnumerable()
                           where dataRow.Field<int>("Id") == Id
                           select dataRow).Single();
            OpenFromDataSet(row);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            this.Id = dataRow.Field<int>("Id");
            this.Name = dataRow.Field<string>("Name");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        /// <summary>
        /// Возвращает грунт по умолчанию
        /// </summary>
        /// <returns></returns>
        public Soil AddDefaultSoil()
        {
            if (Soils.Count > 0) { return Soils[0]; } //Если в коллекции грунтов есть хотя бы один грунт, то возвращаем его
            else
            {
                MessageBox.Show("Для объекта не задано ни одного грунта", "Будет создан грунт по умолчанию");
                Soil soil = new ClaySoil(this);
                Soils.Add(soil);
                soil.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                ProgrammSettings.IsDataChanged = true;
                return soil;
            }
        }
        /// <summary>
        /// Возвращает скважину по умолчанию
        /// </summary>
        /// <param name="building"></param>
        /// <returns></returns>
        public SoilSection AddDefaultSoilSection(Building building)
        {
            SoilLayer AddSoilLayer(SoilSection soilSection)
            {
                Soil soil = AddDefaultSoil();
                SoilLayer soilLayer = new SoilLayer(soil, soilSection, building);
                return soilLayer;
            }
            ProgrammSettings.IsDataChanged = true;
            if (SoilSections.Count > 0)  //Если в коллекции есть хоть одна скважина, возвращаем ее
            {
                SoilSection section = SoilSections[0];
                if (section.SoilLayers.Count == 0)
                {
                    MessageBox.Show("Для скважины не задано ни одного грунта", "Будет создан грунт по умолчанию");
                    SoilLayer soilLayer = AddSoilLayer(section);
                    //Сохраняем слой в датасет, так как скважина уже есть
                    soilLayer.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                }
                return section;
            }
            else //Если ни одной скважины в коллекции нет, то добавляем ее
            {
                MessageBox.Show("Для объекта не задано ни одной скважины", "Будет создана скважина по умолчанию");
                SoilSection section = new SoilSection(this);
                AddSoilLayer(section);
                SoilSections.Add(section);
                //Сохраняем в датасет новую скважину
                section.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                return section;
            }
        }
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public BuildingSite()
        {
            Name = "Мой объект";
            Buildings = new ObservableCollection<Building>();
            Soils = new ObservableCollection<Soil>();
            SoilSections = new ObservableCollection<SoilSection>();
        }
    }
    /// <summary>
    /// Класс здания
    /// </summary>
    public class Building : ICloneable, IHasParent, IDataErrorInfo
    {
        /// <summary>
        /// Код здания
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на строительный объект
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Относительная отметка
        /// </summary>
        public double RelativeLevel { get; set; }
        /// <summary>
        /// Абсолютная отметка, соответствующая указанной относительной отметке
        /// </summary>
        public double AbsoluteLevel { get; set; }
        /// <summary>
        /// Абсолютная отметка планировки
        /// </summary>
        public double AbsolutePlaningLevel { get; set; }
        /// <summary>
        /// Предельное значение осадки фундамента
        /// </summary>
        public double MaxFoundationSettlement { get; set; }
        /// <summary>
        /// Флаг отнесения здания к жесткой системе
        /// </summary>
        public bool IsRigid { get; set; }
        /// <summary>
        /// Отношение, характеризующее жесткость здания
        /// </summary>
        public double RigidRatio { get; set; }
        /// <summary>
        /// Коллекция уровней
        /// </summary>
        public ObservableCollection<Level> Levels { get; set; }
        /// <summary>
        /// Коллекция типов стен
        /// </summary>
        public ObservableCollection<WallType> WallTypeList { get; set; }
        /// <summary>
        /// Коллекция отверстий
        /// </summary>
        public ObservableCollection<OpeningType> OpeningTypeList { get; set; }
        /// <summary>
        /// Наименование линейных единиц измерения
        /// </summary>
        public string LinearMeasure { get { return MeasureUnitConverter.GetUnitLabelText(0); } }
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Building()
        {
            Levels = new ObservableCollection<Level>();
            WallTypeList = new ObservableCollection<WallType>();
            OpeningTypeList = new ObservableCollection<OpeningType>();
        }
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public Building(BuildingSite buildingSite)
        {
            Id = ProgrammSettings.CurrentId;
            Name = "Мое здание";
            RelativeLevel = 0;
            AbsoluteLevel = 260;
            AbsolutePlaningLevel = 260;
            RegisterParent(buildingSite);
            MaxFoundationSettlement = 0.08;
            IsRigid = false;
            RigidRatio = 4;
            Levels = new ObservableCollection<Level>();
            WallTypeList = new ObservableCollection<WallType>();
            OpeningTypeList = new ObservableCollection<OpeningType>();
        }
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "Buildings"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="createNew">Флаг инсерт/апдейт</param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables[GetTableName()];
            if (createNew)
            {
                row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            else
            {
                var tmpRow = (from dataRow in dataTable.AsEnumerable()
                              where dataRow.Field<int>("Id") == Id
                              select dataRow).Single();
                row = tmpRow;
            }
            row.SetField("Id", Id);
            row.SetField("ParentId", ParentMember.Id);
            row.SetField("Name", Name);
            row.SetField("RelativeLevel", RelativeLevel);
            row.SetField("AbsoluteLevel", AbsoluteLevel);
            row.SetField("AbsolutePlaningLevel", AbsolutePlaningLevel);
            row.SetField("MaxFoundationSettlement", MaxFoundationSettlement);
            row.SetField("IsRigid", IsRigid);
            row.SetField("RigidRatio", RigidRatio);
            dataTable.AcceptChanges();
            foreach (Level level in Levels)
            {
                level.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Открытие из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[GetTableName()];
            var building = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
            OpenFromDataSet(building);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            RelativeLevel = dataRow.Field<double>("RelativeLevel");
            AbsoluteLevel = dataRow.Field<double>("AbsoluteLevel");
            AbsolutePlaningLevel = dataRow.Field<double>("AbsolutePlaningLevel");
            MaxFoundationSettlement = dataRow.Field<double>("MaxFoundationSettlement");
            IsRigid = dataRow.Field<bool>("IsRigid");
            RigidRatio = dataRow.Field<double>("RigidRatio");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// Регистрирует родительский элемент
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            BuildingSite buildingSite;
            try
            {
                buildingSite = parent as BuildingSite;
            }
            catch
            {
                throw new Exception("Parent type is not valid");
            }
            buildingSite.Buildings.Add(this);
            ParentMember = buildingSite;
        }

        public void UnRegisterParent()
        {
            BuildingSite buildingSite = ParentMember as BuildingSite;
            buildingSite.Buildings.Remove(this);
            ParentMember = null;
        }

        /// <summary>
        /// Поле для ошибки
        /// </summary>
        public string Error { get { throw new NotImplementedException(); } }
        /// <summary>
        /// Поле для проверки на ошибки
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "RelativeLevel":
                        {
                            if (RelativeLevel < -5)
                            {
                                error = "Ошибка относительной отметки";
                            }
                        }
                        break;
                    case "AbsoluteLevel":
                        {
                            if (AbsoluteLevel <= 0)
                            {
                                error = "Абсолютная отметка не может быть меньше нуля";
                            }
                            if (AbsoluteLevel >= 1000)
                            {
                                error = "Абсолютная отметка не может быть больше 1000м";
                            }
                        }
                        break;
                    case "AbsolutePlaningLevel":
                        {
                            if (AbsolutePlaningLevel <= 0)
                            {
                                error = "Абсолютная отметка планировки не может быть меньше нуля";
                            }
                            if (AbsolutePlaningLevel < AbsoluteLevel - 20)
                            {
                                error = "Абсолютная отметка планировки назначена неверно";
                            }
                        }
                        break;
                    case "MaxFoundationSettlement":
                        {
                            if (MaxFoundationSettlement < 0)
                            {
                                error = "Введите корректное значение осадки";
                            }
                        }
                        break;
                    case "RigidRatio":
                        {
                            if (RigidRatio < 0)
                            {
                                error = "Отношение длины к высоте не может быть меньше нуля";
                            }
                        }
                        break;
                }
                return error;
            }
        }
    }
}
