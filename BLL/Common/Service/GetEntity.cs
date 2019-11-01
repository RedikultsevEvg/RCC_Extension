using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using System.Collections.ObjectModel;
using System.Data;
using RDBLL.Entity.SC.Column;

namespace RDBLL.Common.Service
{
    public static class GetEntity
    {
        public static ObservableCollection<Building> GetBuildings(DataSet dataSet, BuildingSite buildingSite)
        {
            ObservableCollection<Building> newObjects = new ObservableCollection<Building>();
            DataTable dataTable = dataSet.Tables["Buildings"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("BuildingSiteId") == buildingSite.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                Building newObject = new Building
                {
                    Id = dataRow.Field<int>("Id"),
                    BuildingSiteId = dataRow.Field<int>("BuildingSiteId"),
                    BuildingSite = buildingSite,
                    Name = dataRow.Field<string>("Name")
                };
                newObject.Levels = GetLevels(dataSet, newObject);
                newObjects.Add(newObject);
            }
            return newObjects;
        }

        /// <summary>
        /// Получает коллекцию уровней по датасету и зданию
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="building">ссылка на здание</param>
        /// <returns></returns>
        public static ObservableCollection<Level> GetLevels(DataSet dataSet, Building building)
        {
            ObservableCollection<Level> newObjects = new ObservableCollection<Level>();
            DataTable dataTable = dataSet.Tables["Levels"];
            var query = from dataRow in dataTable.AsEnumerable()
                                         where dataRow.Field<int>("BuildingId") == building.Id
                                         select dataRow;
            foreach (var dataRow in query)
            {
                Level newObject = new Level
                {
                    Id = dataRow.Field<int>("Id"),
                    BuildingId = dataRow.Field<int>("BuildingId"),
                    Building = building,
                    Name = dataRow.Field<string>("Name"),
                    FloorLevel = dataRow.Field<double>("FloorLevel"),
                    Height = dataRow.Field<double>("Height"),
                    TopOffset = dataRow.Field<double>("TopOffset"),
                    BasePointX = dataRow.Field<double>("BasePointX"),
                    BasePointY = dataRow.Field<double>("BasePointY"),
                    BasePointZ = dataRow.Field<double>("BasePointZ")
                };
                newObject.SteelBases = GetSteelBases(dataSet, newObject);
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        public static ObservableCollection<SteelBase> GetSteelBases(DataSet dataSet, Level level)
        {
            ObservableCollection<SteelBase> newObjects = new ObservableCollection<SteelBase>();
            DataTable dataTable = dataSet.Tables["SteelBases"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("LevelId") == level.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SteelBase newObject = new SteelBase
                {
                    Id = dataRow.Field<int>("Id"),
                    LevelId = dataRow.Field<int>("LevelId"),
                    Level = level,
                    SteelClassId = dataRow.Field<int>("SteelClassId"),
                    ConcreteClassId = dataRow.Field<int>("ConcreteClassId"),
                    //Надо получить ссылки на сталь и бетон

                    Name = dataRow.Field<string>("Name"),
                    SteelStrength = dataRow.Field<double>("SteelStrength"),
                    ConcreteStrength = dataRow.Field<double>("ConcreteStrength"),
                    IsActual = dataRow.Field<bool>("IsActual"),
                    Width = dataRow.Field<double>("Width"),
                    Length = dataRow.Field<double>("Length"),
                    Thickness = dataRow.Field<double>("Thickness"),
                    WorkCondCoef = dataRow.Field<double>("WorkCondCoef")
                };
                newObjects.Add(newObject);
            }
            return newObjects;
        }
    }
}
