using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using System.Collections.ObjectModel;
using System.Data;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;

namespace RDBLL.Common.Service
{
    public static class GetEntity
    {
        /// <summary>
        /// Получает коллекцию зданий по датасету и строительному объекту
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="buildingSite">Строительный объект</param>
        /// <returns></returns>
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
                    Elevation = dataRow.Field<double>("FloorLevel"),
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
        /// <summary>
        /// Получает коллекцию стальных баз по датасету и уровню
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="level">Уровень</param>
        /// <returns></returns>
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
                    IsActual = false, //dataRow.Field<bool>("IsActual"), В любом случае при загрузке данные неактуальны
                    IsLoadCasesActual = false,
                    IsBasePartsActual = false,
                    IsBoltsActual = false,
                    Width = dataRow.Field<double>("Width"),
                    Length = dataRow.Field<double>("Length"),
                    Thickness = dataRow.Field<double>("Thickness"),
                    WorkCondCoef = dataRow.Field<double>("WorkCondCoef"),
                    UseSimpleMethod = dataRow.Field<bool>("UseSimpleMethod")
                };
                newObject.SteelBaseParts = GetSteelBaseParts(dataSet, newObject);
                newObject.SteelBolts = GetSteelBolts(dataSet, newObject);
                newObject.LoadsGroup = GetForcesGroups(dataSet, newObject);
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        /// <summary>
        /// Получает коллекцию частей по датасету и стальной базе 
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="steelBase">Стальная база</param>
        /// <returns></returns>
        public static ObservableCollection<SteelBasePart> GetSteelBaseParts(DataSet dataSet, SteelBase steelBase)
        {
            ObservableCollection<SteelBasePart> newObjects = new ObservableCollection<SteelBasePart>();
            DataTable dataTable = dataSet.Tables["SteelBaseParts"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("SteelBaseId") == steelBase.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SteelBasePart newObject = new SteelBasePart
                {
                    Id = dataRow.Field<int>("Id"),
                    SteelBaseId = dataRow.Field<int>("SteelBaseId"),
                    SteelBase = steelBase,
                    Name = dataRow.Field<string>("Name"),
                    Width = dataRow.Field<double>("Width"),
                    Length = dataRow.Field<double>("Length"),
                    CenterX = dataRow.Field<double>("CenterX"),
                    CenterY = dataRow.Field<double>("CenterY"),
                    LeftOffset = dataRow.Field<double>("LeftOffset"),
                    RightOffset = dataRow.Field<double>("RightOffset"),
                    TopOffset = dataRow.Field<double>("TopOffset"),
                    BottomOffset = dataRow.Field<double>("BottomOffset"),
                    FixLeft = dataRow.Field<bool>("FixLeft"),
                    FixRight = dataRow.Field<bool>("FixRight"),
                    FixTop = dataRow.Field<bool>("FixTop"),
                    FixBottom = dataRow.Field<bool>("FixBottom"),
                    AddSymmetricX = dataRow.Field<bool>("AddSymmetricX"),
                    AddSymmetricY = dataRow.Field<bool>("AddSymmetricY")
                };
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        /// <summary>
        /// Получает коллекцию болтов по датасету и стальной базе 
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="steelBase">Стальная база</param>
        /// <returns></returns>
        public static ObservableCollection<SteelBolt> GetSteelBolts(DataSet dataSet, SteelBase steelBase)
        {
            ObservableCollection<SteelBolt> newObjects = new ObservableCollection<SteelBolt>();
            DataTable dataTable = dataSet.Tables["SteelBolts"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("SteelBaseId") == steelBase.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SteelBolt newObject = new SteelBolt
                {
                    Id = dataRow.Field<int>("Id"),
                    SteelBaseId = dataRow.Field<int>("SteelBaseId"),
                    SteelBase = steelBase,
                    Name = dataRow.Field<string>("Name"),
                    Diameter = dataRow.Field<double>("Diameter"),
                    CenterX = dataRow.Field<double>("CenterX"),
                    CenterY = dataRow.Field<double>("CenterY"),
                    AddSymmetricX = dataRow.Field<bool>("AddSymmetricX"),
                    AddSymmetricY = dataRow.Field<bool>("AddSymmetricY")
                };
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        public static ObservableCollection<ForcesGroup> GetForcesGroups(DataSet dataSet, SteelBase steelBase)
        {
            ObservableCollection<ForcesGroup> newObjects = new ObservableCollection<ForcesGroup>();
            DataTable adjDataTable = dataSet.Tables["SteelBaseForcesGroups"];
            DataTable dataTable = dataSet.Tables["ForcesGroups"]; 
            var query = from adjDataRow in adjDataTable.AsEnumerable()
                        from dataRow in dataTable.AsEnumerable()
                        where adjDataRow.Field<int>("SteelBaseId") == steelBase.Id
                        where adjDataRow.Field<int>("ForcesGroupId") == dataRow.Field<int>("Id")
                        select dataRow;
            foreach (var dataRow in query)
            {
                ForcesGroup newObject = new ForcesGroup
                {
                    Id = dataRow.Field<int>("Id"),
                    Name = dataRow.Field<string>("Name"),
                    CenterX = dataRow.Field<double>("CenterX"),
                    CenterY = dataRow.Field<double>("CenterY"),
                };
                newObject.SteelBases.Add(steelBase);
                newObjects.Add(newObject);
            }
            foreach (ForcesGroup forcesGroup in newObjects)
            {
                forcesGroup.LoadSets = GetLoadSets(dataSet, forcesGroup);
            }
            return newObjects;
        }
        public static ObservableCollection<LoadSet> GetLoadSets(DataSet dataSet, ForcesGroup forcesGroup)
        {
            ObservableCollection<LoadSet> newObjects = new ObservableCollection<LoadSet>();
            DataTable adjDataTable = dataSet.Tables["ForcesGroupLoadSets"];
            DataTable dataTable = dataSet.Tables["LoadSets"];
            var query = from adjDataRow in adjDataTable.AsEnumerable()
                        from dataRow in dataTable.AsEnumerable()
                        where adjDataRow.Field<int>("ForcesGroupId") == forcesGroup.Id
                        where adjDataRow.Field<int>("LoadSetId") == dataRow.Field<int>("Id")
                        select dataRow;
            foreach (var dataRow in query)
            {
                LoadSet newObject = new LoadSet
                {
                    Id = dataRow.Field<int>("Id"),
                    Name = dataRow.Field<string>("Name"),
                    PartialSafetyFactor = dataRow.Field<double>("PartialSafetyFactor"),
                    IsLiveLoad = dataRow.Field<bool>("IsLiveLoad"),
                    IsCombination = dataRow.Field<bool>("IsCombination"),
                    BothSign = dataRow.Field<bool>("BothSign"),
                };
                newObject.ForcesGroups.Add(forcesGroup);
                newObjects.Add(newObject);
            }
            foreach (LoadSet loadSet in newObjects)
            {
                loadSet.ForceParameters = GetForceParameters(dataSet, loadSet);
            }
            return newObjects;
        }
        public static ObservableCollection<ForceParameter> GetForceParameters(DataSet dataSet, LoadSet loadSet)
        {
            ObservableCollection<ForceParameter> newObjects = new ObservableCollection<ForceParameter>();
            DataTable dataTable = dataSet.Tables["ForceParameters"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("LoadSetId") == loadSet.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                ForceParameter newObject = new ForceParameter
                {
                    Id = dataRow.Field<int>("Id"),
                    LoadSetId = dataRow.Field<int>("LoadSetId"),
                    LoadSet = loadSet,
                    KindId = dataRow.Field<int>("KindId"),
                    Name = dataRow.Field<string>("Name"),
                    CrcValue = dataRow.Field<double>("CrcValue")
            };
                newObjects.Add(newObject);
            }
            return newObjects;
        }
    }
}
