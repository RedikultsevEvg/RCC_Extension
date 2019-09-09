using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastReport;
using System.Data;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.SC.Column;
using RDBLL.Processors.SC;
using RDBLL.Processors.Forces;
using RDBLL.Entity.Results.SC;
using RDBLL.Forces;

namespace CSL.Reports
{
    public class ResultReport
    {
        public DataSet dataSet { get; set; }
        private BuildingSite _buildingSite;


        public void ShowReport()
        {
            using (Report report = new Report())
            {
                report.Load("C:\\Repos\\Reports\\SteelBases.frx");
                report.RegisterData(dataSet);
                //report.Design();
                report.Show();
            }
        }
        public void PrepareReport()
        {
            int steelBaseId = 1;
            int LoadCaseId = 1;
            int ForceParameterId = 1;
            int steelBasePartId = 1;

            foreach (Building building in _buildingSite.BuildingList)
            {
                foreach (Level level in building.LevelList)
                {
                    DataTable SteelBases = dataSet.Tables[0];
                    foreach (SteelColumnBase steelColumnBase in level.SteelColumnBaseList)
                    {
                        DataRow newSteelBase = SteelBases.NewRow();
                        Double A = steelColumnBase.Width * steelColumnBase.Length;
                        Double Wx = steelColumnBase.Width * steelColumnBase.Length * steelColumnBase.Length / 6;
                        Double Wy = steelColumnBase.Length * steelColumnBase.Width * steelColumnBase.Width / 6;
                        newSteelBase.ItemArray = new object[] { steelBaseId, steelColumnBase.Name, steelColumnBase.Width, steelColumnBase.Length, A, Wx, Wy };
                        SteelBases.Rows.Add(newSteelBase);
                        DataTable LoadCases = dataSet.Tables[1];                       
                        foreach (BarLoadSet barLoadSet in BarLoadSetProcessor.GetLoadCases(steelColumnBase.LoadsGroup))
                        {
                            DataRow newLoadCase = LoadCases.NewRow();
                            newLoadCase.ItemArray = new object[] { LoadCaseId, steelBaseId, barLoadSet.LoadSet.Name, barLoadSet.LoadSet.PartialSafetyFactor};
                            LoadCases.Rows.Add(newLoadCase);

                            DataTable ForceParameters = dataSet.Tables[2];
                            foreach (ForceParameter forceParameter in barLoadSet.LoadSet.ForceParameters)
                            {
                                DataRow newForceParameter = ForceParameters.NewRow();
                                newForceParameter.ItemArray = new object[] { ForceParameterId, LoadCaseId, "", "", "", 0, forceParameter.Value };
                                ForceParameters.Rows.Add(newForceParameter);
                                ForceParameterId++;
                            }
                            LoadCaseId++;
                        }
                        DataTable SteelBasesParts = dataSet.Tables[3];
                        foreach (SteelBasePart steelBasePart in steelColumnBase.SteelBaseParts)
                        {
                            foreach (SteelBasePart steelBasePartEh in SteelColumnBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart))
                            {
                                DataRow newSteelBasePart = SteelBasesParts.NewRow();
                                ColumnBasePartResult columnBasePartResult = SteelColumnBasePartProcessor.GetResult(steelBasePartEh);
                                double maxBedStress = columnBasePartResult.MaxBedStress;
                                double maxStress = columnBasePartResult.MaxStress;
                                newSteelBasePart.ItemArray = new object[] { steelBasePartId, steelBaseId, steelBasePartEh.Name, steelBasePartEh.Center[0], steelBasePartEh.Center[1], steelBasePartEh.Width, steelBasePart.Length, Math.Round(maxBedStress/1000000,3), Math.Round(maxStress /1000000,3) };
                                SteelBasesParts.Rows.Add(newSteelBasePart);
                                steelBasePartId++;
                            }
                        }
                        steelBaseId++;
                    }
                }
            }


        }

        #region Constructors
        public ResultReport(BuildingSite buildingSite)
        {
            _buildingSite = buildingSite;
            dataSet = new DataSet();
            #region SteelBases
            //Базы стальных колонн
            DataTable SteelBases = new DataTable("SteelBases");
            dataSet.Tables.Add(SteelBases);
            DataColumn SteelBaseId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBaseName = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn SteelBaseWidth = new DataColumn("Width", Type.GetType("System.Double"));
            DataColumn SteelBaseLength = new DataColumn("Length", Type.GetType("System.Double"));
            DataColumn SteelBaseArea = new DataColumn("Area", Type.GetType("System.Double"));
            DataColumn SteelBaseWx = new DataColumn("Wx", Type.GetType("System.Double"));
            DataColumn SteelBaseWy = new DataColumn("Wy", Type.GetType("System.Double"));

            SteelBases.Columns.Add(SteelBaseId);
            SteelBases.Columns.Add(SteelBaseName);
            SteelBases.Columns.Add(SteelBaseWidth);
            SteelBases.Columns.Add(SteelBaseLength);
            SteelBases.Columns.Add(SteelBaseArea);
            SteelBases.Columns.Add(SteelBaseWx);
            SteelBases.Columns.Add(SteelBaseWy);
            #endregion
            #region LoadCases
            DataTable LoadCases = new DataTable("LoadCases");
            dataSet.Tables.Add(LoadCases);

            DataColumn LoadCaseId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBaseIdInLoadCase = new DataColumn("SteelBaseId", Type.GetType("System.Int32"));
            DataColumn LoadCaseDescription = new DataColumn("Description", Type.GetType("System.String"));
            DataColumn LoadCasePartialSafetyFactor = new DataColumn("PartialSafetyFactor", Type.GetType("System.Double"));

            LoadCases.Columns.Add(LoadCaseId);
            LoadCases.Columns.Add(SteelBaseIdInLoadCase);
            LoadCases.Columns.Add(LoadCaseDescription);
            LoadCases.Columns.Add(LoadCasePartialSafetyFactor);
            #endregion
            #region ForceParameters
            DataTable ForceParameters = new DataTable("ForceParameters");
            dataSet.Tables.Add(ForceParameters);

            DataColumn ForceParameterId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn LoadCaseIdInForceParameter = new DataColumn("LoadCaseId", Type.GetType("System.Int32"));
            DataColumn ForceParameterDescription = new DataColumn("Description", Type.GetType("System.String"));
            DataColumn ForceParameterLabel = new DataColumn("Label", Type.GetType("System.String"));
            DataColumn ForceParameterUnit = new DataColumn("Unit", Type.GetType("System.String"));
            DataColumn ForceParameterCrcValue = new DataColumn("CrcValue", Type.GetType("System.Double"));
            DataColumn ForceParameterDesignValue = new DataColumn("DesignValue", Type.GetType("System.Double"));

            ForceParameters.Columns.Add(ForceParameterId);
            ForceParameters.Columns.Add(LoadCaseIdInForceParameter);
            ForceParameters.Columns.Add(ForceParameterDescription);
            ForceParameters.Columns.Add(ForceParameterLabel);
            ForceParameters.Columns.Add(ForceParameterUnit);
            ForceParameters.Columns.Add(ForceParameterCrcValue);
            ForceParameters.Columns.Add(ForceParameterDesignValue);
            #endregion
            #region SteelBasesParts
            DataTable SteelBasesParts = new DataTable("SteelBasesParts");
            dataSet.Tables.Add(SteelBasesParts);

            DataColumn SteelBasesPartId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBaseIdInPart = new DataColumn("SteelBaseId", Type.GetType("System.Int32"));
            DataColumn SteelBasesPartName = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn SteelBasesPartCenterX = new DataColumn("CenterX", Type.GetType("System.Double"));
            DataColumn SteelBasesPartCenterY = new DataColumn("CenterY", Type.GetType("System.Double"));
            DataColumn SteelBasesPartWidth = new DataColumn("Width", Type.GetType("System.Double"));
            DataColumn SteelBasesPartLength = new DataColumn("Length", Type.GetType("System.Double"));
            DataColumn SteelBasesPartMaxBedStrees = new DataColumn("MaxBedStrees", Type.GetType("System.Double"));
            DataColumn SteelBasesPartMaxSteelStrees = new DataColumn("MaxSteelStrees", Type.GetType("System.Double"));

            SteelBasesParts.Columns.Add(SteelBasesPartId);
            SteelBasesParts.Columns.Add(SteelBaseIdInPart);
            SteelBasesParts.Columns.Add(SteelBasesPartName);
            SteelBasesParts.Columns.Add(SteelBasesPartCenterX);
            SteelBasesParts.Columns.Add(SteelBasesPartCenterY);
            SteelBasesParts.Columns.Add(SteelBasesPartWidth);
            SteelBasesParts.Columns.Add(SteelBasesPartLength);
            SteelBasesParts.Columns.Add(SteelBasesPartMaxBedStrees);
            SteelBasesParts.Columns.Add(SteelBasesPartMaxSteelStrees);
            #endregion
        }
        #endregion
    }
}
