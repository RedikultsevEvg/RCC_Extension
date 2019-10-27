using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CSL.DataSets.SC
{
    public static class SteelBaseDataSet
    {
        public static DataSet GetDataSet()
        {
            DataSet dataSet = new DataSet();
            #region SteelBases
            //Базы стальных колонн
            DataTable SteelBases = new DataTable("SteelBases");
            dataSet.Tables.Add(SteelBases);
            DataColumn SteelBaseId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBasePicture = new DataColumn("Picture", Type.GetType("System.Byte[]"));
            DataColumn SteelBaseName = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn SteelBaseWidth = new DataColumn("Width", Type.GetType("System.Double"));
            DataColumn SteelBaseLength = new DataColumn("Length", Type.GetType("System.Double"));
            DataColumn SteelBaseArea = new DataColumn("Area", Type.GetType("System.Double"));
            DataColumn SteelBaseWx = new DataColumn("Wx", Type.GetType("System.Double"));
            DataColumn SteelBaseWy = new DataColumn("Wy", Type.GetType("System.Double"));

            SteelBases.Columns.Add(SteelBaseId);
            SteelBases.Columns.Add(SteelBasePicture);
            SteelBases.Columns.Add(SteelBaseName);
            SteelBases.Columns.Add(SteelBaseWidth);
            SteelBases.Columns.Add(SteelBaseLength);
            SteelBases.Columns.Add(SteelBaseArea);
            SteelBases.Columns.Add(SteelBaseWx);
            SteelBases.Columns.Add(SteelBaseWy);
            #endregion
            #region Loads

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
            DataTable ForceParameters = new DataTable("ForceParameters");
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
            DataColumn SteelBasesPartCenterX = new DataColumn("CenterX", Type.GetType("System.Double"));
            DataColumn SteelBasesPartCenterY = new DataColumn("CenterY", Type.GetType("System.Double"));
            DataColumn SteelBasesPartWidth = new DataColumn("Width", Type.GetType("System.Double"));
            DataColumn SteelBasesPartLength = new DataColumn("Length", Type.GetType("System.Double"));
            DataColumn SteelBasesPartMaxBedStress = new DataColumn("MaxBedStress", Type.GetType("System.Double"));
            DataColumn SteelBasesPartMaxSteelStress = new DataColumn("MaxSteelStress", Type.GetType("System.Double"));

            SteelBasesParts.Columns.Add(SteelBasesPartId);
            SteelBasesParts.Columns.Add(SteelBaseIdInPart);
            SteelBasesParts.Columns.Add(SteelBasePartPicture);
            SteelBasesParts.Columns.Add(SteelBasesPartName);
            SteelBasesParts.Columns.Add(SteelBasesPartCenterX);
            SteelBasesParts.Columns.Add(SteelBasesPartCenterY);
            SteelBasesParts.Columns.Add(SteelBasesPartWidth);
            SteelBasesParts.Columns.Add(SteelBasesPartLength);
            SteelBasesParts.Columns.Add(SteelBasesPartMaxBedStress);
            SteelBasesParts.Columns.Add(SteelBasesPartMaxSteelStress);
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
