using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL.Common;

namespace CSL.DataSets.SC
{
    public static class SteelBaseDataSet
    {
        public static DataSet GetDataSet()
        {
            DataSet dataSet = new DataSet();
            DataTable newTable;
            #region SteelBases
            //Базы стальных колонн
            DataTable SteelBases = new DataTable("SteelBases");
            dataSet.Tables.Add(SteelBases);
            DsOperation.AddIdColumn(SteelBases);
            DataColumn SteelBasePicture = new DataColumn("Picture", Type.GetType("System.Byte[]"));
            SteelBases.Columns.Add(SteelBasePicture);
            DsOperation.AddNameColumn(SteelBases);
            DsOperation.AddDoubleColumn(SteelBases, "SteelStrength");
            DsOperation.AddDoubleColumn(SteelBases, "ConcreteStrength");
            DsOperation.AddDoubleColumn(SteelBases, "Width");
            DsOperation.AddDoubleColumn(SteelBases, "Length");
            DsOperation.AddDoubleColumn(SteelBases, "Thickness");
            DsOperation.AddDoubleColumn(SteelBases, "Area");
            DsOperation.AddDoubleColumn(SteelBases, "Wx");
            DsOperation.AddDoubleColumn(SteelBases, "Wy");
            #endregion
            #region Loads
            newTable = new DataTable("LoadSets");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn("SteelBases", "SteelBaseId", newTable);
            DsOperation.AddNameColumn(newTable);
            DsOperation.AddDoubleColumn(newTable, "PartialSafetyFactor");
            DsOperation.AddStringColumn(newTable, "CrcForceDescription");
            DsOperation.AddStringColumn(newTable, "DesignForceDescription");
            #endregion
            #region LoadForceParameters
            newTable = new DataTable("LoadSetsForceParameters");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIntColumn(newTable, "Id");
            DsOperation.AddFkIdColumn("SteelBases", "SteelBaseId", newTable);
            DsOperation.AddStringColumn(newTable, "SteelBaseName");
            DsOperation.AddIntColumn(newTable, "LoadSetId");
            DsOperation.AddStringColumn(newTable, "LoadSetName");
            DsOperation.AddStringColumn(newTable, "Description");
            DsOperation.AddStringColumn(newTable, "LongLabel");
            DsOperation.AddStringColumn(newTable, "ShortLabel");
            DsOperation.AddStringColumn(newTable, "Unit");
            DsOperation.AddDoubleColumn(newTable, "CrcValue");
            DsOperation.AddDoubleColumn(newTable, "DesignValue");
            #endregion
            #region LoadCases
            DataTable LoadCases = new DataTable("LoadCases");
            dataSet.Tables.Add(LoadCases);

            DataColumn LoadCaseId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBaseIdInLoadCase = new DataColumn("SteelBaseId", Type.GetType("System.Int32"));
            DataColumn LoadCaseDescription = new DataColumn("Description", Type.GetType("System.String"));
            DataColumn LoadCasePartialSafetyFactor = new DataColumn("PartialSafetyFactor", Type.GetType("System.Double"));
            DataColumn LoadCaseForceDescription = new DataColumn("ForceDescription", Type.GetType("System.String"));

            LoadCases.Columns.Add(LoadCaseId);
            LoadCases.Columns.Add(SteelBaseIdInLoadCase);
            LoadCases.Columns.Add(LoadCaseDescription);
            LoadCases.Columns.Add(LoadCasePartialSafetyFactor);
            LoadCases.Columns.Add(LoadCaseForceDescription);
            #endregion
            #region ForceParameters
            DataTable ForceParameters = new DataTable("LoadCasesForceParameters");
            dataSet.Tables.Add(ForceParameters);

            DataColumn ForceParameterId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn LoadCaseIdInForceParameter = new DataColumn("LoadCaseId", Type.GetType("System.Int32"));
            DataColumn ForceParameterLongLabel = new DataColumn("LongLabel", Type.GetType("System.String"));
            DataColumn ForceParameterShortLabel = new DataColumn("ShortLabel", Type.GetType("System.String"));
            DataColumn ForceParameterUnit = new DataColumn("Unit", Type.GetType("System.String"));
            DataColumn ForceParameterCrcValue = new DataColumn("CrcValue", Type.GetType("System.Double"));
            DataColumn ForceParameterDesignValue = new DataColumn("DesignValue", Type.GetType("System.Double"));

            ForceParameters.Columns.Add(ForceParameterId);
            ForceParameters.Columns.Add(LoadCaseIdInForceParameter);
            ForceParameters.Columns.Add(ForceParameterLongLabel);
            ForceParameters.Columns.Add(ForceParameterShortLabel);
            ForceParameters.Columns.Add(ForceParameterUnit);
            ForceParameters.Columns.Add(ForceParameterCrcValue);
            ForceParameters.Columns.Add(ForceParameterDesignValue);
            #endregion
            #region SteelBasesParts
            DataTable SteelBasesParts = new DataTable("SteelBasesParts");
            dataSet.Tables.Add(SteelBasesParts);

            DataColumn SteelBasesPartId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBaseIdInPart = new DataColumn("SteelBaseId", Type.GetType("System.Int32"));
            DataColumn SteelBasePartPicture = new DataColumn("Picture", Type.GetType("System.Byte[]"));
            DataColumn SteelBasesPartName = new DataColumn("Name", Type.GetType("System.String"));
            SteelBasesParts.Columns.Add(SteelBasesPartId);
            SteelBasesParts.Columns.Add(SteelBaseIdInPart);
            SteelBasesParts.Columns.Add(SteelBasePartPicture);
            SteelBasesParts.Columns.Add(SteelBasesPartName);
            DsOperation.AddDoubleColumn(SteelBasesParts, "CenterX");
            DsOperation.AddDoubleColumn(SteelBasesParts, "CenterY");
            DsOperation.AddDoubleColumn(SteelBasesParts, "Width");
            DsOperation.AddDoubleColumn(SteelBasesParts, "Length");
            DsOperation.AddDoubleColumn(SteelBasesParts, "MaxBedStress");
            DsOperation.AddDoubleColumn(SteelBasesParts, "MaxSteelStress");
            DsOperation.AddDoubleColumn(SteelBasesParts, "RecomendedThickness");
            #endregion
            #region SteelBolts
            DataTable SteelBasesBolts = new DataTable("SteelBasesBolts");
            dataSet.Tables.Add(SteelBasesBolts);

            DataColumn SteelBasesBoltId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBaseIdInBolt = new DataColumn("SteelBaseId", Type.GetType("System.Int32"));
            DataColumn SteelBasesBoltName = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn SteelBasesBoltDiameter = new DataColumn("Diameter", Type.GetType("System.Double"));
            DataColumn SteelBasesBoltCenterX = new DataColumn("CenterX", Type.GetType("System.Double"));
            DataColumn SteelBasesBoltCenterY = new DataColumn("CenterY", Type.GetType("System.Double"));
            DataColumn SteelBasesBoltMaxStress = new DataColumn("MaxStress", Type.GetType("System.Double"));
            DataColumn SteelBasesBoltMaxForce = new DataColumn("MaxForce", Type.GetType("System.Double"));

            SteelBasesBolts.Columns.Add(SteelBasesBoltId);
            SteelBasesBolts.Columns.Add(SteelBaseIdInBolt);
            SteelBasesBolts.Columns.Add(SteelBasesBoltName);
            SteelBasesBolts.Columns.Add(SteelBasesBoltDiameter);
            SteelBasesBolts.Columns.Add(SteelBasesBoltCenterX);
            SteelBasesBolts.Columns.Add(SteelBasesBoltCenterY);
            SteelBasesBolts.Columns.Add(SteelBasesBoltMaxStress);
            SteelBasesBolts.Columns.Add(SteelBasesBoltMaxForce);
            #endregion
            #region Relations
            dataSet.Relations.Add("LoadCases", SteelBases.Columns["Id"], LoadCases.Columns["SteelBaseId"]);
            dataSet.Relations.Add("SteelPart", SteelBases.Columns["Id"], SteelBasesParts.Columns["SteelBaseId"]);
            dataSet.Relations.Add("SteelBolt", SteelBases.Columns["Id"], SteelBasesBolts.Columns["SteelBaseId"]);
            #endregion

            return dataSet;
        }
    }
}
