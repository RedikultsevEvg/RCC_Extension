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

namespace RDBLL.Entity.RCC.BuildingAndSite
{
    /// <summary>
    /// Класс строительного объекта
    /// </summary>
    public class BuildingSite :ICloneable, ISavableToDataSet
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
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables["BuildingSites"];
            if (createNew) { row = dataTable.NewRow(); }
            else
            {
                var soil = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
                row = soil;
            }
            //Задаем свойства элемента
            row["Id"] = Id;
            row["ParentId"] = 0;
            row["Name"] = Name;
            dataTable.Rows.Add(row);
            //Добавляем вложенные элементы
            foreach (Building building in Buildings)
            {
                building.SaveToDataSet(dataSet, createNew);
            }
            foreach (Soil soil in Soils)
            {
                soil.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Открытие из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["BuildingSites"];
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
            DsOperation.DeleteRow(dataSet, "BuildingSites", Id);
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
    public class Building : ICloneable, ISavableToDataSet
    {
        /// <summary>
        /// Код здания
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код строительного объекта
        /// </summary>
        public int BuildingSiteId { get; set; }
        /// <summary>
        /// Обратная ссылка на строительный объект
        /// </summary>
        public BuildingSite BuildingSite { get; set; }
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
            BuildingSiteId = buildingSite.Id;
            Name = "Мое здание";
            RelativeLevel = 0;
            AbsoluteLevel = 260;
            BuildingSite = buildingSite;
            Levels = new ObservableCollection<Level>();
            WallTypeList = new ObservableCollection<WallType>();
            OpeningTypeList = new ObservableCollection<OpeningType>();
        }
        public Building(BuildingSite buildingSite, XmlNode xmlNode)
        {
        }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["Buildings"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { Id, BuildingSiteId, Name, RelativeLevel, AbsoluteLevel };
            dataTable.Rows.Add(dataRow);
            foreach (Level level in Levels)
            {
                level.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Обновляет датасет в соответствии с записью
        /// </summary>
        /// <param name="dataSet"></param>
        public void Save (DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["Buildings"];
            var building = (from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("Id") == Id
                        select dataRow).Single();
            building.SetField("Name", Name);
            building.SetField("RelativeLevel", RelativeLevel);
            building.SetField("AbsoluteLevel", AbsoluteLevel);
            dataTable.AcceptChanges();
        }

        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["Buildings"];
            var building = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
            Name = building.Field<string>("Name");
            RelativeLevel = building.Field<double>("RelativeLevel");
            AbsoluteLevel = building.Field<double>("AbsoluteLevel");
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
