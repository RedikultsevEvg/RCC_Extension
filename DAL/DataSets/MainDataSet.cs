using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL.Common;

namespace DAL.DataSets
{
    /// <summary>
    /// Класс датасета для хранения исходных данных элементов
    /// </summary>
    public class MainDataSet
    {
        /// <summary>
        /// Датасет
        /// </summary>
        public DataSet DataSet { get; private set; }

        /// <summary>
        /// Задает новый датасет с необходимыми свойствами
        /// </summary>
        public void GetNewDataSet()
        {
            DataSet dataSet = new DataSet();

            DataTable dataTable;
            DataColumn IdColumn, FkIdColumn, NameColumn;
            #region Generators
            dataTable = new DataTable("Generators");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
            #endregion
            #region Version
            dataTable = new DataTable("Versions");
            dataSet.Tables.Add(dataTable);
            DataColumn version = new DataColumn("Version", Type.GetType("System.Int32"));
            dataTable.Columns.Add(version);
            #endregion
            #region ProgrammSetting
            dataTable = new DataTable("MeasurementUnits");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
            #endregion
            #region SteelClasses
            #endregion
            #region ConcreteClasses
            #endregion
            #region BuildingSites
            dataTable = new DataTable("BuildingSites");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            IdColumn.Unique = true;
            dataTable.Columns.Add(IdColumn);
            FkIdColumn = new DataColumn("ParentId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            dataTable.Columns.Add(NameColumn);
            #endregion
            #region Soils
            dataTable = new DataTable("Soils");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddStringColumn(dataTable, "Type");
            DsOperation.AddFkIdColumn("BuildingSites", "BuildingSiteId", dataTable);
            DsOperation.AddStringColumn(dataTable, "Description");
            DsOperation.AddBoolColumn(dataTable, "IsDefinedFromTest", true);
            DsOperation.AddDoubleColumn(dataTable, "CrcDensity", 1950);
            DsOperation.AddDoubleColumn(dataTable, "FstDesignDensity", 1800);
            DsOperation.AddDoubleColumn(dataTable, "SndDesignDensity", 1900);
            DsOperation.AddDoubleColumn(dataTable, "CrcParticularDensity", 2700);
            DsOperation.AddDoubleColumn(dataTable, "FstParticularDensity", 2700);
            DsOperation.AddDoubleColumn(dataTable, "SndParticularDensity", 2700);
            DsOperation.AddDoubleColumn(dataTable, "PorousityCoef", 0.7);
            DsOperation.AddDoubleColumn(dataTable, "FiltrationCoeff", 0.0001);
            DsOperation.AddDoubleColumn(dataTable, "ElasticModulus", 2e7);
            DsOperation.AddDoubleColumn(dataTable, "SndElasticModulus", 1e8);
            DsOperation.AddDoubleColumn(dataTable, "PoissonRatio", 0.3);
            DsOperation.AddDoubleColumn(dataTable, "CrcFi", 20);
            DsOperation.AddDoubleColumn(dataTable, "FstDesignFi", 17);
            DsOperation.AddDoubleColumn(dataTable, "SndDesignFi", 18);
            DsOperation.AddDoubleColumn(dataTable, "CrcCohesion", 20000);
            DsOperation.AddDoubleColumn(dataTable, "FstDesignCohesion", 17000);
            DsOperation.AddDoubleColumn(dataTable, "SndDesignCohesion", 18000);
            DsOperation.AddDoubleColumn(dataTable, "CrcStrength", 2e7);
            DsOperation.AddDoubleColumn(dataTable, "FstDesignStrength", 1.8e7);
            DsOperation.AddDoubleColumn(dataTable, "SndDesignStrength", 1.6e7);
            #endregion
            #region SoilSections
            dataTable = new DataTable("SoilSections");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("BuildingSites", "BuildingSiteId", dataTable);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddBoolColumn(dataTable, "HasWater", false);
            DsOperation.AddDoubleColumn(dataTable, "NaturalWaterLevel", 200);
            DsOperation.AddDoubleColumn(dataTable, "WaterLevel", 200);
            DsOperation.AddDoubleColumn(dataTable, "CenterX", 0);
            DsOperation.AddDoubleColumn(dataTable, "CenterY", 0);
            #endregion
            #region SoilLayers
            dataTable = new DataTable("SoilLayers");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("Soils", "SoilId", dataTable);
            DsOperation.AddFkIdColumn("SoilSections", "SoilSectionId", dataTable);
            DsOperation.AddDoubleColumn(dataTable, "TopLevel", 0);
            #endregion
            #region Buildings
            dataTable = new DataTable("Buildings");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("BuildingSites", "BuildingSiteId", dataTable);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "RelativeLevel",0);
            DsOperation.AddDoubleColumn(dataTable, "AbsoluteLevel", 260);
            DsOperation.AddDoubleColumn(dataTable, "AbsolutePlaningLevel", 259.5);
            DsOperation.AddDoubleColumn(dataTable, "MaxFoundationSettlement", 0.08);
            DsOperation.AddBoolColumn(dataTable, "IsRigid", false);
            DsOperation.AddDoubleColumn(dataTable, "RigidRatio", 4);
            #endregion
            #region Levels
            dataTable = new DataTable("Levels");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            IdColumn.Unique = true;
            dataTable.Columns.Add(IdColumn);
            FkIdColumn = new DataColumn("BuildingId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            dataTable.Columns.Add(NameColumn);
            DsOperation.AddDoubleColumn(dataTable, "FloorLevel", 0);
            DsOperation.AddDoubleColumn(dataTable, "Height", 3);
            DsOperation.AddDoubleColumn(dataTable, "TopOffset", -0.2);
            DsOperation.AddDoubleColumn(dataTable, "BasePointX", 0);
            DsOperation.AddDoubleColumn(dataTable, "BasePointY", 0);
            DsOperation.AddDoubleColumn(dataTable, "BasePointZ", 0);
            #endregion
            #region SteelBases
            dataTable = new DataTable("SteelBases");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("Levels", "LevelId", dataTable);
            FkIdColumn = new DataColumn("SteelClassId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            FkIdColumn = new DataColumn("ConcreteClassId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "SteelStrength");
            DsOperation.AddDoubleColumn(dataTable, "ConcreteStrength");
            DsOperation.AddBoolColumn(dataTable, "IsActual");
            DsOperation.AddDoubleColumn(dataTable, "Width");
            DsOperation.AddDoubleColumn(dataTable, "Length");
            DsOperation.AddDoubleColumn(dataTable, "Thickness");
            DsOperation.AddDoubleColumn(dataTable, "WorkCondCoef");
            DsOperation.AddBoolColumn(dataTable, "UseSimpleMethod", true);
            #endregion
            #region SteelBaseParts
            dataTable = new DataTable("SteelBaseParts");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("SteelBases", "SteelBaseId", dataTable);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "Width");
            DsOperation.AddDoubleColumn(dataTable, "Length");
            DsOperation.AddDoubleColumn(dataTable, "CenterX");
            DsOperation.AddDoubleColumn(dataTable, "CenterY");
            DsOperation.AddDoubleColumn(dataTable, "LeftOffset");
            DsOperation.AddDoubleColumn(dataTable, "RightOffset");
            DsOperation.AddDoubleColumn(dataTable, "TopOffset");
            DsOperation.AddDoubleColumn(dataTable, "BottomOffset");
            DsOperation.AddBoolColumn(dataTable, "FixLeft");
            DsOperation.AddBoolColumn(dataTable, "FixRight");
            DsOperation.AddBoolColumn(dataTable, "FixTop");
            DsOperation.AddBoolColumn(dataTable, "FixBottom");
            DsOperation.AddBoolColumn(dataTable, "AddSymmetricX");
            DsOperation.AddBoolColumn(dataTable, "AddSymmetricY");
            #endregion
            #region SteelBolts
            dataTable = new DataTable("SteelBolts");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("SteelBases", "SteelBaseId", dataTable);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "Diameter");
            DsOperation.AddDoubleColumn(dataTable, "CenterX");
            DsOperation.AddDoubleColumn(dataTable, "CenterY");
            DsOperation.AddBoolColumn(dataTable, "AddSymmetricX");
            DsOperation.AddBoolColumn(dataTable, "AddSymmetricY");
            #endregion
            #region ForcesGroups
            dataTable = new DataTable("ForcesGroups");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "CenterX");
            DsOperation.AddDoubleColumn(dataTable, "CenterY");
            #endregion
            #region SteelBaseForcesGroups
            dataTable = new DataTable("SteelBaseForcesGroups");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("SteelBases", "SteelBaseId", dataTable);
            DsOperation.AddFkIdColumn("ForcesGroups", "ForcesGroupId", dataTable);
            #endregion
            #region LoadSets
            dataTable = new DataTable("LoadSets");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "PartialSafetyFactor");
            DsOperation.AddBoolColumn(dataTable, "IsLiveLoad");
            DsOperation.AddBoolColumn(dataTable, "IsCombination");
            DsOperation.AddBoolColumn(dataTable, "BothSign");
            #endregion
            #region ForcesGroupLoadSets
            dataTable = new DataTable("ForcesGroupLoadSets");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("ForcesGroups", "ForcesGroupId", dataTable);
            DsOperation.AddFkIdColumn("LoadSets", "LoadSetId", dataTable);
            #endregion
            #region ForceParameters
            dataTable = new DataTable("ForceParameters");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("LoadSets", "LoadSetId", dataTable);
            DsOperation.AddIntColumn(dataTable, "KindId");
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "CrcValue");
            #endregion
            #region Foundations
            dataTable = new DataTable("Foundations");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("Levels", "LevelId", dataTable);
            DsOperation.AddFkIdColumn("SoilSections", "SoilSectionId", dataTable, true);
            FkIdColumn = new DataColumn("ReinfSteelClassId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            FkIdColumn = new DataColumn("ConcreteClassId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "RelativeTopLevel", -0.2);
            DsOperation.AddDoubleColumn(dataTable, "SoilRelativeTopLevel", -0.2);
            DsOperation.AddDoubleColumn(dataTable, "SoilVolumeWeight", 18000);
            DsOperation.AddDoubleColumn(dataTable, "ConcreteVolumeWeight", 25000);
            DsOperation.AddDoubleColumn(dataTable, "FloorLoad", 0);
            DsOperation.AddDoubleColumn(dataTable, "FloorLoadFactor", 1.2);
            DsOperation.AddDoubleColumn(dataTable, "ConcreteFloorLoad", 0);
            DsOperation.AddDoubleColumn(dataTable, "ConcreteFloorLoadFactor", 1.2);
            DsOperation.AddDoubleColumn(dataTable, "CompressedLayerRatio", 0.2);
            #endregion
            #region FoundationParts
            dataTable = new DataTable("FoundationParts");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddStringColumn(dataTable, "Type");
            DsOperation.AddFkIdColumn("Foundations", "FoundationId", dataTable);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "Width");
            DsOperation.AddDoubleColumn(dataTable, "Length");
            DsOperation.AddDoubleColumn(dataTable, "Height");
            DsOperation.AddDoubleColumn(dataTable, "CenterX");
            DsOperation.AddDoubleColumn(dataTable, "CenterY");
            #endregion
            #region FoundationForcesGroups
            dataTable = new DataTable("FoundationForcesGroups");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("Foundations", "FoundationId", dataTable);
            DsOperation.AddFkIdColumn("ForcesGroups", "ForcesGroupId", dataTable);
            #endregion
            #region MaterialContainer
            dataTable = new DataTable("MaterialContainers");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddIntColumn(dataTable, "ParentId");
            DsOperation.AddStringColumn(dataTable, "Purpose");
            #endregion
            #region MaterialUsing
            dataTable = new DataTable("MaterialUsings");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddStringColumn(dataTable, "Purpose");
            DsOperation.AddIntColumn(dataTable, "ParentId");
            DsOperation.AddStringColumn(dataTable, "Materialkindname");
            DsOperation.AddIntColumn(dataTable, "SelectedId");
            DsOperation.AddDoubleColumn(dataTable, "Diameter", 0.012);
            DsOperation.AddDoubleColumn(dataTable, "Prestrain", 0);
            #endregion
            #region Safetyfactors
            dataTable = new DataTable("Safetyfactors");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddIntColumn(dataTable, "ParentId");
            DsOperation.AddDoubleColumn(dataTable, "PsfFst");
            DsOperation.AddDoubleColumn(dataTable, "PsfSnd");
            DsOperation.AddDoubleColumn(dataTable, "PsfFstLong");
            DsOperation.AddDoubleColumn(dataTable, "PsfSndLong");
            #endregion
            #region RFSpacings
            dataTable = new DataTable("RFSpacings");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddIntColumn(dataTable, "ParentId");
            DsOperation.AddStringColumn(dataTable, "Type");
            #endregion
            #region Placements
            dataTable = new DataTable("Placements");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddIntColumn(dataTable, "ParentId");
            DsOperation.AddStringColumn(dataTable, "Type");
            #endregion
            #region StoredParams
            #endregion
            dataTable = new DataTable("StoredParams");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddIntColumn(dataTable, "ParentId");
            DsOperation.AddStringColumn(dataTable, "Type");
            DsOperation.AddStringColumn(dataTable, "Value");
            #region
            #endregion
            DataSet = dataSet;
        }

    }
}
