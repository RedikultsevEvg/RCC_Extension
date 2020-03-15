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
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.Soils;

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
                Building newObject = new Building();
                newObject.OpenFromDataSet(dataRow);
                newObject.BuildingSite = buildingSite;
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
                Level newObject = new Level();
                newObject.OpenFromDataSet(dataRow);
                newObject.Building = building;
                newObject.SteelBases = GetSteelBases(dataSet, newObject);
                newObject.Foundations = GetFoundations(dataSet, newObject);
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
                SteelBase newObject = new SteelBase();
                newObject.OpenFromDataSet(dataRow);
                newObject.Level = level;
                newObject.SteelBaseParts = GetSteelBaseParts(dataSet, newObject);
                newObject.SteelBolts = GetSteelBolts(dataSet, newObject);
                newObject.ForcesGroups = GetSteelBaseForcesGroups(dataSet, newObject);
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
                SteelBasePart newObject = new SteelBasePart();
                newObject.OpenFromDataSet(dataRow);
                newObject.SteelBase = steelBase;
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
                SteelBolt newObject = new SteelBolt();
                newObject.OpenFromDataSet(dataRow);
                newObject.SteelBase = steelBase;
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        /// <summary>
        /// Получает коллекцию групп усилий по датасету и стальной базе
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="steelBase"></param>
        /// <returns></returns>
        public static ObservableCollection<ForcesGroup> GetSteelBaseForcesGroups(DataSet dataSet, SteelBase steelBase)
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
        /// <summary>
        /// Получает коллекцию групп усилий по датасету и фундаменту
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static ObservableCollection<ForcesGroup> GetFoundationForcesGroups(DataSet dataSet, Foundation foundation)
        {
            ObservableCollection<ForcesGroup> newObjects = new ObservableCollection<ForcesGroup>();
            DataTable adjDataTable = dataSet.Tables["FoundationForcesGroups"];
            DataTable dataTable = dataSet.Tables["ForcesGroups"];
            var query = from adjDataRow in adjDataTable.AsEnumerable()
                        from dataRow in dataTable.AsEnumerable()
                        where adjDataRow.Field<int>("FoundationId") == foundation.Id
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
                newObject.Foundations.Add(foundation);
                newObjects.Add(newObject);
            }
            foreach (ForcesGroup forcesGroup in newObjects)
            {
                forcesGroup.LoadSets = GetLoadSets(dataSet, forcesGroup);
            }
            return newObjects;
        }
        /// <summary>
        /// Получает коллекцию наборов усилий по датасету и группе нагрузок
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="forcesGroup"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Получает коллекцию параметров усилий по датасету и набору усилий
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="loadSet"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Возвращает коллекцию фундаментов по датасету и уровню
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static ObservableCollection<Foundation> GetFoundations(DataSet dataSet, Level level)
        {
            ObservableCollection<Foundation> newObjects = new ObservableCollection<Foundation>();
            DataTable dataTable = dataSet.Tables["Foundations"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("LevelId") == level.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                Foundation newObject = new Foundation();
                newObject.Level = level;
                newObject.OpenFromDataSet(dataRow);
                newObject.Parts = GetFoundationParts(dataSet, newObject);                
                newObject.ForcesGroups = GetFoundationForcesGroups(dataSet, newObject);
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        /// <summary>
        /// Возвращает коллекцию ступеней фундамента по датасуту и фундаменту
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static ObservableCollection<RectFoundationPart> GetFoundationParts(DataSet dataSet, Foundation foundation)
        {
            ObservableCollection<RectFoundationPart> newObjects = new ObservableCollection<RectFoundationPart>();
            DataTable dataTable = dataSet.Tables["FoundationParts"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("FoundationId") == foundation.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                RectFoundationPart newObject = new RectFoundationPart();
                newObject.OpenFromDataSet(dataRow);
                newObject.Foundation = foundation;
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        /// <summary>
        /// Возвращает коллекцию грунтов по датасету и строительному объекту
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="buildingSite">Строительный объект</param>
        /// <returns></returns>
        public static ObservableCollection<Soil> GetSoils (DataSet dataSet, BuildingSite buildingSite)
        {
            ObservableCollection<Soil> newObjects = new ObservableCollection<Soil>();
            DataTable dataTable = dataSet.Tables["Soils"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("BuildingSiteId") == buildingSite.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                Soil newObject;
                if (dataRow.Field<string>("Type") == "ClaySoil")
                {
                    newObject = new ClaySoil(buildingSite);
                    newObject.OpenFromDataSet(dataRow);
                    newObject.BuildingSite = buildingSite;
                    newObjects.Add(newObject);
                }
                if (dataRow.Field<string>("Type") == "RockSoil")
                {
                    newObject = new RockSoil(buildingSite);
                    newObject.OpenFromDataSet(dataRow);
                    newObject.BuildingSite = buildingSite;
                    newObjects.Add(newObject);
                }
            }
            return newObjects;
        }
        /// <summary>
        /// Возвращает коллекцию скважин по датасету и строительному объекту
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="buildingSite">Строительный объект</param>
        /// <returns></returns>
        public static ObservableCollection<SoilSection> GetSoilSections(DataSet dataSet, BuildingSite buildingSite)
        {
            ObservableCollection<SoilSection> newObjects = new ObservableCollection<SoilSection>();
            DataTable dataTable = dataSet.Tables["SoilSections"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("BuildingSiteId") == buildingSite.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SoilSection newObject = new SoilSection(buildingSite);
                newObject.OpenFromDataSet(dataRow);
                newObject.BuildingSite = buildingSite;
                //Получаем коллекцию слоев грунта
                newObject.SoilLayers = GetSoilLayers(dataSet, newObject);
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        /// <summary>
        /// Возвращает коллекцию слоев грунта по датасету и скважине
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="soilSection">Скважина</param>
        /// <returns></returns>
        public static ObservableCollection<SoilLayer> GetSoilLayers (DataSet dataSet, SoilSection soilSection)
        {
            ObservableCollection<SoilLayer> newObjects = new ObservableCollection<SoilLayer>();
            DataTable dataTable = dataSet.Tables["SoilLayers"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("SoilSectionId") == soilSection.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SoilLayer newObject = new SoilLayer();
                newObject.OpenFromDataSet(dataRow);
                newObject.SoilSection = soilSection;
                foreach (Soil soil in soilSection.BuildingSite.Soils)
                {
                    if (soil.Id == newObject.SoilId) { newObject.Soil = soil; }
                }
                newObjects.Add(newObject);
            }
            return newObjects;
        }
    }
}
