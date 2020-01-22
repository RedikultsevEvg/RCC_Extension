﻿using System;
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
            DataColumn IdColumn, FkIdColumn, NameColumn, NewColumn;
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
            #region Buildings
            dataTable = new DataTable("Buildings");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("BuildingSites", "BuildingSiteId", dataTable);
            DsOperation.AddNameColumn(dataTable);
            DsOperation.AddDoubleColumn(dataTable, "RelativeLevel",0);
            DsOperation.AddDoubleColumn(dataTable, "AbsoluteLevel", 260);
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
            NewColumn = new DataColumn("FloorLevel", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Height", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("TopOffset", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("BasePointX", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("BasePointY", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("BasePointZ", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
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
            DsOperation.AddDoubleColumn(dataTable, "CoveringLayerX", 0.09);
            DsOperation.AddDoubleColumn(dataTable, "CoveringLayerY", 0.07);
            DsOperation.AddDoubleColumn(dataTable, "CompressedLayerRatio", 0.2);
            #endregion
            #region FoundationParts
            dataTable = new DataTable("FoundationParts");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
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
            DsOperation.AddFkIdColumn("Foundations", "FoundationId", dataTable);
            DsOperation.AddFkIdColumn("ForcesGroups", "ForcesGroupId", dataTable);
            #endregion
            #region
            #endregion
            DataSet = dataSet;
        }

    }
}
