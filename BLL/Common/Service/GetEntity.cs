using RDBLL.Common.Interfaces;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.Soils;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using RDBLL.Entity.Common.Placements;
using RDBLL.Common.Params;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Common.Params;
using RDBLL.Entity.SC.Column.SteelBases.Patterns;

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
                        where dataRow.Field<int>("ParentId") == buildingSite.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                Building newObject = new Building();
                newObject.OpenFromDataSet(dataRow);
                newObject.RegisterParent(buildingSite);
                GetLevels(dataSet, newObject);
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
                                         where dataRow.Field<int>("ParentId") == building.Id
                                         select dataRow;
            foreach (var dataRow in query)
            {
                Level newObject = new Level();
                newObject.OpenFromDataSet(dataRow);
                newObject.RegisterParent(building);
                GetSteelBases(dataSet, newObject);
                GetFoundations(dataSet, newObject);
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
                        where dataRow.Field<int>("ParentId") == level.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SteelBase newObject = new SteelBase();
                newObject.OpenFromDataSet(dataRow);
                newObject.RegisterParent(level);
                PatternBase pattern = GetPapametricObject(dataSet, newObject) as PatternBase;
                
                if (pattern is null)
                {
                    newObject.SteelBaseParts = GetSteelBaseParts(dataSet, newObject);
                    newObject.SteelBolts = GetSteelBolts(dataSet, newObject);
                }
                else
                {
                    pattern.RegisterParent(newObject);
                    newObject.Pattern = pattern;
                }
                newObject.ForcesGroups = GetParentForcesGroups(dataSet, newObject);
                List<MaterialUsing> materials = GetMaterialUsings(dataSet, newObject);
                foreach (MaterialUsing material in materials)
                {
                    if (material is SteelUsing) newObject.Steel = material as SteelUsing;
                    if (material is ConcreteUsing) newObject.Concrete = material as ConcreteUsing;
                }
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
                        where dataRow.Field<int>("ParentId") == steelBase.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SteelBasePart newObject = new SteelBasePart();
                newObject.OpenFromDataSet(dataRow);
                newObject.RegisterParent(steelBase);
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
                        where dataRow.Field<int>("ParentId") == steelBase.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SteelBolt newObject = new SteelBolt();
                newObject.OpenFromDataSet(dataRow);
                newObject.RegisterParent(steelBase);
                //не очень корректно достаем единственный элемент
                newObject.Steel = GetMaterialUsings(dataSet, newObject)[0] as SteelUsing;
                newObject.Placement = GetPlacement(dataSet, newObject);
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
        //public static ObservableCollection<ForcesGroup> GetSteelBaseForcesGroups(DataSet dataSet, SteelBase steelBase)
        //{
        //    ObservableCollection<ForcesGroup> newObjects = new ObservableCollection<ForcesGroup>();
        //    DataTable adjDataTable = dataSet.Tables["SteelBaseForcesGroups"];
        //    DataTable dataTable = dataSet.Tables["ForcesGroups"]; 
        //    var query = from adjDataRow in adjDataTable.AsEnumerable()
        //                from dataRow in dataTable.AsEnumerable()
        //                where adjDataRow.Field<int>("SteelBaseId") == steelBase.Id
        //                where adjDataRow.Field<int>("ForcesGroupId") == dataRow.Field<int>("Id")
        //                select dataRow;
        //    foreach (var dataRow in query)
        //    {
        //        ForcesGroup newObject = new ForcesGroup
        //        {
        //            Id = dataRow.Field<int>("Id"),
        //            Name = dataRow.Field<string>("Name"),
        //            CenterX = dataRow.Field<double>("CenterX"),
        //            CenterY = dataRow.Field<double>("CenterY"),
        //        };
        //        newObject.Owners.Add(steelBase);
        //        newObjects.Add(newObject);
        //    }
        //    foreach (ForcesGroup forcesGroup in newObjects)
        //    {
        //        forcesGroup.LoadSets = GetLoadSets(dataSet, forcesGroup);
        //    }
        //    return newObjects;
        //}
        /// <summary>
        /// Получает коллекцию групп усилий по датасету и фундаменту
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static ObservableCollection<ForcesGroup> GetParentForcesGroups(DataSet dataSet, IHasForcesGroups parent)
        {
            ObservableCollection<ForcesGroup> newObjects = new ObservableCollection<ForcesGroup>();
            DataTable adjDataTable = dataSet.Tables["ParentForcesGroups"];
            DataTable dataTable = dataSet.Tables["ForcesGroups"];
            var query = from adjDataRow in adjDataTable.AsEnumerable()
                        from dataRow in dataTable.AsEnumerable()
                        where adjDataRow.Field<int>("ParentId") == parent.Id
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
                newObject.Owners.Add(parent);
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
                    LoadId = dataRow.Field<int>("LoadSetId"),
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
                        where dataRow.Field<int>("ParentId") == level.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                Foundation newObject = new Foundation();
                newObject.RegisterParent(level);
                newObject.OpenFromDataSet(dataRow);
                newObject.Parts = GetFoundationParts(dataSet, newObject);                
                newObject.ForcesGroups = GetParentForcesGroups(dataSet, newObject);
                GetSoilSection(dataSet, newObject);
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
                        where dataRow.Field<int>("ParentId") == foundation.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                RectFoundationPart newObject = new RectFoundationPart();
                newObject.OpenFromDataSet(dataRow);
                newObject.ParentMember = foundation;
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
                        where dataRow.Field<int>("ParentId") == buildingSite.Id
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
                        where dataRow.Field<int>("ParentId") == buildingSite.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SoilSection newObject = new SoilSection(buildingSite);
                newObject.OpenFromDataSet(dataRow);
                newObject.RegisterParent(buildingSite);
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
                foreach (Soil soil in (soilSection.ParentMember as BuildingSite).Soils)
                {
                    if (soil.Id == newObject.SoilId) { newObject.Soil = soil; }
                }
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        public static List<MaterialContainer> GetContainers (DataSet dataSet, IDsSaveable parent)
        {
            List<MaterialContainer> materialContainers = new List<MaterialContainer>();
            DataTable dataTable = dataSet.Tables["MaterialContainers"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("ParentId") == parent.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                MaterialContainer materialContainer = new MaterialContainer();
                materialContainer.OpenFromDataSet(dataRow);
                materialContainers.Add(materialContainer);
            }
            foreach (MaterialContainer materialContainer in materialContainers)
            {
                List<MaterialUsing> matUsings = GetEntity.GetMaterialUsings(dataSet, materialContainer);
                foreach (MaterialUsing materialUsing in matUsings)
                {
                    if (materialUsing is ReinforcementUsing)
                    { materialContainer.MaterialUsings.Add(materialUsing as ReinforcementUsing); }
                    else if (materialUsing is ConcreteUsing) { materialContainer.MaterialUsings.Add(materialUsing as ConcreteUsing); }
                    else materialContainer.MaterialUsings.Add(materialUsing);
                }
            }
            return materialContainers;
        }
        public static List<MaterialUsing> GetMaterialUsings(DataSet dataSet, IDsSaveable parent)
        {
            List<MaterialUsing> materialUsings = new List<MaterialUsing>();
            DataTable dataTable = dataSet.Tables["MaterialUsings"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("ParentId") == parent.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                string materialKindName = dataRow.Field<string>("Materialkindname");
                MaterialUsing materialUsing;
                if (string.Compare(materialKindName, "Concrete") == 0) { materialUsing = new ConcreteUsing(); }
                else if (string.Compare(materialKindName, "Reinforcement") == 0) { materialUsing = new ReinforcementUsing();}
                else if (string.Compare(materialKindName, "Steel") == 0) { materialUsing = new SteelUsing(); }
                else if (string.Compare(materialKindName, "SteelBolt") == 0) { materialUsing = new BoltUsing(); }
                else throw new Exception("Material type is not valid");
                materialUsing.RegisterParent(parent);
                materialUsing.OpenFromDataSet(dataRow);
                if (materialUsing is IHasPlacement) { GetPlacement(dataSet, materialUsing as IHasPlacement); }
                #region SafetyFActors
                List<SafetyFactor> safetyFactorsList = GetSafetyFactors(dataSet, materialUsing);
                ObservableCollection<SafetyFactor> safetyFactors = new ObservableCollection<SafetyFactor>();
                foreach (SafetyFactor safetyFactor in safetyFactorsList)
                {
                    safetyFactors.Add(safetyFactor);
                }
                materialUsing.SafetyFactors = safetyFactors;
                #endregion
                materialUsings.Add(materialUsing);
            }
            return materialUsings;
        }
        public static Placement GetPlacement(DataSet dataSet, IHasPlacement parent)
        {
            Placement placement;
            IDsSaveable savableParent;
            if (parent is IDsSaveable)
            {
                savableParent = parent as IDsSaveable;
            }
            else throw new Exception("Parent is not SavableToDataSet");
            DataTable dataTable = dataSet.Tables["ParametricObjects"];
            var row = (from dataRow in dataTable.AsEnumerable()
                       where dataRow.Field<int>("ParentId") == savableParent.Id
                       select dataRow).Single();

            string type = row.Field<string>("Type");
            if (string.Compare(type, "LineBySpacing") == 0) { placement = new LineBySpacing(); }
            else if (string.Compare(type, "RectArrayPlacement") == 0) { placement = new RectArrayPlacement(); }
            else throw new Exception("Spacing type is not valid");
            placement.OpenFromDataSet(row);
            parent.SetPlacement(placement);
            placement.RegisterParent(savableParent);
            placement.StoredParams = GetStoredParams(dataSet, placement);
            return placement;
        }
        private static List<SafetyFactor> GetSafetyFactors(DataSet dataSet, MaterialUsing parent)
        {
            List<SafetyFactor> newObjects = new List<SafetyFactor>();
            DataTable dataTable = dataSet.Tables["SafetyFactors"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("ParentId") == parent.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                SafetyFactor newObject = new SafetyFactor();
                newObject.OpenFromDataSet(dataRow);
                newObject.RegisterParent(parent);
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        private static List<StoredParam> GetStoredParams(DataSet dataSet, IDsSaveable parent)
        {
            List<StoredParam> newObjects = new List<StoredParam>();
            DataTable dataTable = dataSet.Tables["StoredParams"];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("ParentId") == parent.Id
                        select dataRow;
            foreach (var dataRow in query)
            {
                StoredParam newObject = new StoredParam(parent);
                newObject.OpenFromDataSet(dataRow);
                newObject.RegisterParent(parent);
                newObjects.Add(newObject);
            }
            return newObjects;
        }
        private static SoilSectionUsing GetSoilSection (DataSet dataSet, IHasSoilSection parent)
        {
            SoilSectionUsing soilSectionUsing;
            DataTable dataTable = dataSet.Tables["SoilSectionUsings"];
            var query = (from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("ParentId") == parent.Id
                        select dataRow).Single();
            soilSectionUsing = new SoilSectionUsing();
            soilSectionUsing.OpenFromDataSet(query);
            soilSectionUsing.RegisterParent(parent);
            return soilSectionUsing;
        }
        private static ParametriсBase GetPapametricObject(DataSet dataSet, IDsSaveable parent)
        {
            ParametriсBase newObj;
            DataTable dataTable = dataSet.Tables["ParametricObjects"];
            var query = (from dataRow in dataTable.AsEnumerable()
                         where dataRow.Field<int>("ParentId") == parent.Id
                         select dataRow).SingleOrDefault();
            if (query is null) return null;
            switch (query.Field<string>("Type"))
            {
                case "SteelBasePatternType1" :
                    {
                        newObj = new PatternType1();
                        newObj.OpenFromDataSet(query);
                        newObj.StoredParams = GetStoredParams(dataSet, newObj);
                        return newObj;
                    }
                case "SteelBasePatternType2":
                    {
                        newObj = new PatternType2();
                        newObj.OpenFromDataSet(query);
                        newObj.StoredParams = GetStoredParams(dataSet, newObj);
                        return newObj;
                    }
                case "SteelBasePatternType3":
                    {
                        newObj = new PatternType3();
                        newObj.OpenFromDataSet(query);
                        newObj.StoredParams = GetStoredParams(dataSet, newObj);
                        return newObj;
                    }
                default:
                    {
                        return null;
                    }
            }

        }
    }
}
