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
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["BuildingSites"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { Id, 0, Name };
            dataTable.Rows.Add(dataRow);
            foreach (Building building in Buildings)
            {
                building.SaveToDataSet(dataSet);
            }
        }
        /// <summary>
        /// Открытие из датасета по Id
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="Id"></param>
        public void OpenFromDataSet(DataSet dataSet, int Id)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables["BuildingSites"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (Convert.ToInt32(dataTable.Rows[i].ItemArray[0]) == Id)
                {
                    this.Id = Id;
                    this.Name = Convert.ToString(dataTable.Rows[i].ItemArray[2]);
                    this.Buildings=GetEntity.GetBuildings(dataSet, this);
                }
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
            Id = ProgrammSettings.CurrentId;
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
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["Buildings"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { Id, BuildingSiteId, Name, RelativeLevel, AbsoluteLevel };
            dataTable.Rows.Add(dataRow);
            foreach (Level level in Levels)
            {
                level.SaveToDataSet(dataSet);
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
        /// <summary>
        /// Обновляет запись в соответствии с сохраненной в датасете
        /// </summary>
        /// <param name="dataSet"></param>
        public void Revert(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["Buildings"];
            var building = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
            Name = building.Field<string>("Name");
            RelativeLevel = building.Field<double>("RelativeLevel");
            AbsoluteLevel = building.Field<double>("AbsoluteLevel");
        }

        public void OpenFromDataSet(DataSet dataSet, int Id)
        {
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
