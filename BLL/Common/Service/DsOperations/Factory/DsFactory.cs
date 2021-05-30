﻿using RDBLL.Entity.RCC.Slabs.Punchings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations.Factory
{
    /// <summary>
    /// Фабрика для настройки главного датасета
    /// </summary>
    public static class DsFactory
    {
        public static DataSet GetDataSet()
        {
            DataSet dataSet = new DataSet();

            DataTable dataTable;
            DataColumn FkIdColumn;
            #region Generators
            dataTable = new DataTable("Generators");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
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
            DsOperation.AddIdColumn(dataTable);
            #endregion
            #region BuildingSites
            dataTable = new DataTable("BuildingSites");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            FkIdColumn = new DataColumn("ParentId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            #endregion
            #region Soils
            dataTable = new DataTable("Soils");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable, "BuildingSites");
            DsOperation.AddStringColumn(dataTable, "Type");
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
            DsOperation.AddCommonColumns(dataTable, "BuildingSites");
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
            DsOperation.AddCommonColumns(dataTable, "BuildingSites");
            DsOperation.AddDoubleColumn(dataTable, "RelativeLevel", 0);
            DsOperation.AddDoubleColumn(dataTable, "AbsoluteLevel", 260);
            DsOperation.AddDoubleColumn(dataTable, "AbsolutePlaningLevel", 259.5);
            DsOperation.AddDoubleColumn(dataTable, "MaxFoundationSettlement", 0.08);
            DsOperation.AddBoolColumn(dataTable, "IsRigid", false);
            DsOperation.AddDoubleColumn(dataTable, "RigidRatio", 4);
            #endregion
            #region Levels
            dataTable = new DataTable("Levels");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable, "Buildings");
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
            DsOperation.AddCommonColumns(dataTable, "Levels");
            FkIdColumn = new DataColumn("SteelClassId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            FkIdColumn = new DataColumn("ConcreteClassId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            DsOperation.AddDoubleColumn(dataTable, "SteelStrength");
            DsOperation.AddDoubleColumn(dataTable, "ConcreteStrength");
            DsOperation.AddBoolColumn(dataTable, "IsActual");
            DsOperation.AddDoubleColumn(dataTable, "Width");
            DsOperation.AddDoubleColumn(dataTable, "Length");
            DsOperation.AddDoubleColumn(dataTable, "Height");
            DsOperation.AddDoubleColumn(dataTable, "WorkCondCoef");
            DsOperation.AddBoolColumn(dataTable, "UseSimpleMethod", true);
            #endregion
            #region SteelBasePartGroups
            dataTable = new DataTable("SteelBasePartGroups");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable, "Levels");
            DsOperation.AddDoubleColumn(dataTable, "Height");
            DsOperation.AddDoubleColumn(dataTable, "Pressure");
            #endregion
            #region SteelBaseParts
            dataTable = new DataTable("SteelBaseParts");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable);
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
            DsOperation.AddCommonColumns(dataTable, "SteelBases");
            DsOperation.AddDoubleColumn(dataTable, "Diameter");
            DsOperation.AddDoubleColumn(dataTable, "CenterX");
            DsOperation.AddDoubleColumn(dataTable, "CenterY");
            DsOperation.AddBoolColumn(dataTable, "AddSymmetricX");
            DsOperation.AddBoolColumn(dataTable, "AddSymmetricY");
            #endregion
            #region ForcesGroups
            dataTable = new DataTable("ForcesGroups");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddDoubleColumn(dataTable, "CenterX");
            DsOperation.AddDoubleColumn(dataTable, "CenterY");
            #endregion
            #region LoadSets
            dataTable = new DataTable("LoadSets");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddDoubleColumn(dataTable, "PartialSafetyFactor");
            DsOperation.AddBoolColumn(dataTable, "IsLiveLoad");
            DsOperation.AddBoolColumn(dataTable, "IsCombination");
            DsOperation.AddBoolColumn(dataTable, "BothSign");
            #endregion
            #region ForcesGroupLoadSets
            dataTable = new DataTable("ForcesGroupLoadSets");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddFkIdColumn("ForcesGroups", "ForcesGroupId", dataTable);
            DsOperation.AddFkIdColumn("LoadSets", "LoadSetId", dataTable);
            #endregion
            #region ForceParameters
            dataTable = new DataTable("ForceParameters");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable, true);
            DsOperation.AddFkIdColumn("LoadSets", "LoadSetId", dataTable);
            DsOperation.AddIntColumn(dataTable, "KindId");
            DsOperation.AddDoubleColumn(dataTable, "CrcValue");
            #endregion
            #region Foundations
            dataTable = new DataTable("Foundations");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable, "Levels");
            DsOperation.AddFkIdColumn("SoilSections", "SoilSectionId", dataTable, true);
            #endregion
            #region FoundationParts
            dataTable = new DataTable("FoundationParts");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable, "Foundations");
            DsOperation.AddStringColumn(dataTable, "Type");
            DsOperation.AddDoubleColumn(dataTable, "Width");
            DsOperation.AddDoubleColumn(dataTable, "Length");
            DsOperation.AddDoubleColumn(dataTable, "Height");
            DsOperation.AddDoubleColumn(dataTable, "CenterX");
            DsOperation.AddDoubleColumn(dataTable, "CenterY");
            #endregion
            #region ParentForcesGroups
            dataTable = new DataTable("ParentForcesGroups");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddIdColumn(dataTable);
            DsOperation.AddFkIdColumn("ParentId", dataTable);
            DsOperation.AddFkIdColumn("ForcesGroups", "ForcesGroupId", dataTable);
            #endregion
            #region MaterialContainer
            dataTable = new DataTable("MaterialContainers");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable);
            DsOperation.AddStringColumn(dataTable, "Purpose");
            #endregion
            #region MaterialUsing
            dataTable = new DataTable("MaterialUsings");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable);
            DsOperation.AddStringColumn(dataTable, "Purpose");
            DsOperation.AddStringColumn(dataTable, "Materialkindname");
            DsOperation.AddIntColumn(dataTable, "SelectedId");
            DsOperation.AddDoubleColumn(dataTable, "Diameter", 0.012);
            DsOperation.AddDoubleColumn(dataTable, "Prestrain", 0);
            #endregion
            #region Safetyfactors
            dataTable = new DataTable("Safetyfactors");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable);
            for (int i = 0; i <= 11; i++) { DsOperation.AddDoubleColumn(dataTable, Convert.ToString(i)); }
            #endregion
            #region RFSpacings
            dataTable = new DataTable("RFSpacings");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable);
            DsOperation.AddStringColumn(dataTable, "Type");
            #endregion
            #region ParametricObjects
            dataTable = new DataTable("ParametricObjects");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable);
            DsOperation.AddStringColumn(dataTable, "Type");
            #endregion
            #region StoredParams
            dataTable = new DataTable("StoredParams");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable);
            DsOperation.AddStringColumn(dataTable, "Type");
            DsOperation.AddStringColumn(dataTable, "Value");
            #endregion
            #region SoilSectionUsing
            dataTable = new DataTable("SoilSectionUsings");
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable);
            DsOperation.AddFkIdColumn("SoilSections", "SelectedId", dataTable, true);
            #endregion
            //Добавление таблицы узлов на продавливания
            dataTable = DsOperation.AddTableToDataset(typeof(Punching));
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable, "Levels");
            //Добавление таблицы слоев продавливания
            dataTable = DsOperation.AddTableToDataset(typeof(PunchingLayer));
            dataSet.Tables.Add(dataTable);
            DsOperation.AddCommonColumns(dataTable, "Punchings");
            return dataSet;
        }
    }
}
