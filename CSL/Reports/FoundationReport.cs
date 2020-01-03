﻿using CSL.Common;
using CSL.DataSets.SC;
using CSL.Reports.Interfaces;
using FastReport;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.RCC.BuildingAndSite;
using System.Data;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.RCC.Foundations.Processors;
using System.Windows.Controls;
using RDBLL.DrawUtils.SteelBase;
using CSL.DataSets.RCC.Foundations;
using RDBLL.Forces;
using System.Collections.ObjectModel;
using RDBLL.Processors.Forces;

namespace CSL.Reports
{
    public class FoundationReport : IReport
    {
        private BuildingSite _buildingSite;
        public DataSet dataSet { get; set; }

        public void ShowReport(string fileName)
        {
            PrepareReport();
            using (Report report = new Report())
            {
                report.Load(fileName);
                CommonServices.PrepareMeasureUnit(report);
                report.RegisterData(dataSet);
                report.Design();
                //report.Show();
            }
        }
        private void PrepareReport()
        {
            foreach (Building building in _buildingSite.Buildings)
            {
                foreach (Level level in building.Levels)
                {
                    DataTable Foundations = dataSet.Tables["Foundations"];
                    foreach (Foundation foundation in level.Foundations)
                    {
                        if (!(foundation.IsLoadCasesActual & foundation.IsPartsActual))
                        {
                            FoundationProcessor.SolveFoundation(foundation);
                        }

                        double foundationWidth = FoundationProcessor.GetContourSize(foundation)[0];
                        double foundationLength = FoundationProcessor.GetContourSize(foundation)[1];
                        double foundationHeight = FoundationProcessor.GetContourSize(foundation)[2];
                        double foundationSoilVolume = FoundationProcessor.GetSoilVolume(foundation);
                        double foundationConcreteVolume = FoundationProcessor.GetConcreteVolume(foundation);
                        

                        DataRow newSteelBase = Foundations.NewRow();
                        double A = foundationWidth * foundationLength;
                        double Wx = foundationWidth * foundationLength * foundationLength / 6;
                        double Wy = foundationWidth * foundationWidth * foundationLength / 6;
                        #region Picture
                        Canvas canvas = new Canvas();
                        canvas.Width = 600;
                        canvas.Height = 600;
                        DrawFoundation.DrawTopScatch(foundation, canvas);
                        byte[] b = CommonServices.ExportToByte(canvas);
                        #endregion
                        newSteelBase.ItemArray = new object[]
                        { foundation.Id, foundation.Name, b,
                            foundationWidth * MeasureUnitConverter.GetCoefficient(0),
                            foundationLength * MeasureUnitConverter.GetCoefficient(0),
                            foundationHeight * MeasureUnitConverter.GetCoefficient(0),
                            foundation.SoilVolumeWeight * MeasureUnitConverter.GetCoefficient(9),
                            foundation.ConcreteVolumeWeight * MeasureUnitConverter.GetCoefficient(9),
                            foundationSoilVolume * MeasureUnitConverter.GetCoefficient(11),
                            foundationConcreteVolume * MeasureUnitConverter.GetCoefficient(11),
                            A * MeasureUnitConverter.GetCoefficient(4),
                            Wx * MeasureUnitConverter.GetCoefficient(5),
                            Wy * MeasureUnitConverter.GetCoefficient(5)};

                        Foundations.Rows.Add(newSteelBase);

                        ProcessLoadSets(foundation);
                    }
                }
            }
        }

        public FoundationReport(BuildingSite buildingSite)
        {
            _buildingSite = buildingSite;
            dataSet = FoundationDataSet.GetDataSet();
        }

        /// <summary>
        /// Добавление в датасет данных по нагрузкам
        /// </summary>
        /// <param name="foundation"></param>
        private void ProcessLoadSets(Foundation foundation)
        {
            CommonServices.AddLoadsToDataset(dataSet, "LoadSets", "Foundations", "FoundationId", foundation.ForcesGroups[0].LoadSets, foundation.Id, foundation.Name);
            CommonServices.AddLoadsToDataset(dataSet, "LoadCases", "Foundations", "FoundationId", foundation.LoadCases, foundation.Id, foundation.Name);

            ObservableCollection<LoadSet> btmLoadSetsWithWeight = FoundationProcessor.GetBottomLoadCasesWithWeight(foundation);
            //btmLoadSetsWithWeight = LoadSetProcessor.GetLoadSetsTransform(btmLoadSetsWithWeight, FoundationProcessor.GetDeltaDistance(foundation));
            CommonServices.AddLoadsToDataset(dataSet, "BottomLoadCasesWithWeight", "Foundations", "FoundationId", btmLoadSetsWithWeight, foundation.Id, foundation.Name);

            ObservableCollection<LoadSet> btmLoadSets = foundation.LoadCases;
            //btmLoadSets = LoadSetProcessor.GetLoadSetsTransform(btmLoadSetsWithWeight, FoundationProcessor.GetDeltaDistance(foundation));
            CommonServices.AddLoadsToDataset(dataSet, "BottomLoadCases", "Foundations", "FoundationId", btmLoadSets, foundation.Id, foundation.Name);
        }
    }
}