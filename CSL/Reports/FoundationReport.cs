using CSL.Common;
using CSL.DataSets.RCC.Foundations;
using CSL.Reports.Interfaces;
using FastReport;
using RDBLL.DrawUtils.SteelBase;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.RCC.Foundations.Processors;
using RDBLL.Forces;
using RDBLL.Processors.Forces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;
using System.Windows;
using System;
using RDBLL.Common.Service;
using RDBLL.Entity.Soils.Processors;
using RDBLL.Entity.Soils;

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
                        //Если данные по фундаменту неактуальны выполняем расчет
                        if (!(foundation.IsLoadCasesActual & foundation.IsPartsActual))
                        {
                            //Если расчет выполнен успешно
                            if (FoundationProcessor.SolveFoundation(foundation))
                            {
                                //Заносим результаты расчета в таблицы датасета
                                ProcessSubElements(Foundations, foundation);
                            }
                            else
                            {
                                //Иначе показываем пользователю, что произошла ошибка расчета
                                MessageBox.Show($" фундамент: {foundation.Name} Ошибка расчета");
                            }
                        }
                        else //Если актуальны, то сразу подготавливаем отчет
                        {
                            //Заносим результаты расчета в таблицы датасета
                            ProcessSubElements(Foundations, foundation);
                        }
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
        /// Добавление в датасет данных по фундаменту
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="foundation"></param>
        private void ProcessFoundation(DataTable dataTable, Foundation foundation)
        {
            double foundationWidth = FoundationProcessor.GetContourSize(foundation)[0];
            double foundationLength = FoundationProcessor.GetContourSize(foundation)[1];
            double foundationHeight = FoundationProcessor.GetContourSize(foundation)[2];
            double foundationSoilVolume = FoundationProcessor.GetSoilVolume(foundation);
            double foundationConcreteVolume = FoundationProcessor.GetConcreteVolume(foundation);

            DataRow newFoundation = dataTable.NewRow();
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
            newFoundation.ItemArray = new object[]
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

            dataTable.Rows.Add(newFoundation);
        }
        /// <summary>
        /// Добавление в датасет данных по нагрузкам
        /// </summary>
        /// <param name="foundation"></param>
        private void ProcessLoadSets(Foundation foundation)
        {
            CommonServices.AddLoadsToDataset(dataSet, "LoadSets", "Foundations", "FoundationId", foundation.ForcesGroups[0].LoadSets, foundation.Id, foundation.Name);
            CommonServices.AddLoadsToDataset(dataSet, "LoadCases", "Foundations", "FoundationId", foundation.LoadCases, foundation.Id, foundation.Name);

            ObservableCollection<LoadSet> loadSetsWith = foundation.btmLoadSetsWithWeight;
            CommonServices.AddLoadsToDataset(dataSet, "BottomLoadCasesWithWeight", "Foundations", "FoundationId", loadSetsWith, foundation.Id, foundation.Name);

            ObservableCollection<LoadSet> loadSetsWithout = foundation.btmLoadSetsWithoutWeight;
            CommonServices.AddLoadsToDataset(dataSet, "BottomLoadCases", "Foundations", "FoundationId", loadSetsWithout, foundation.Id, foundation.Name);
        }
        /// <summary>
        /// Добавление в датасет данных по ступеням фундаментов
        /// </summary>
        /// <param name="foundation"></param>
        private void ProcessFoundationParts(Foundation foundation)
        {
            DataTable FoundationParts = dataSet.Tables["FoundationParts"];
            foreach (RectFoundationPart foundationPart in foundation.Parts)
            {
                DataRow newFoundationPart = FoundationParts.NewRow();
                newFoundationPart.ItemArray = new object[]
                        {
                            foundationPart.Id,
                            foundationPart.Foundation.Id,
                            foundationPart.Name,
                            foundationPart.Width * MeasureUnitConverter.GetCoefficient(0),
                            foundationPart.Length * MeasureUnitConverter.GetCoefficient(0),
                            foundationPart.Height * MeasureUnitConverter.GetCoefficient(0),
                            foundationPart.Width * foundationPart.Length * foundationPart.Height * MeasureUnitConverter.GetCoefficient(11),
                            foundationPart.CenterX * MeasureUnitConverter.GetCoefficient(0),
                            foundationPart.CenterY * MeasureUnitConverter.GetCoefficient(0)
                        };

                FoundationParts.Rows.Add(newFoundationPart);
            }
        }
        private void ProcessBtmStresses(Foundation foundation)
        {
            List<double[]> stressesWithWeigth = FoundationProcessor.GetStresses(foundation, foundation.ForceCurvaturesWithWeight);
            DataTable FoundationStresses = dataSet.Tables["FoundationStressesWithWeight"];
            foreach (double[] stresses in stressesWithWeigth)
            {
                DataRow newTableItem = FoundationStresses.NewRow();
                double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
                newTableItem.ItemArray = new object[]
                        { ProgrammSettings.CurrentTmpId,
                        foundation.Id,
                        Math.Round(stresses[0] * stressCoefficient, 3),
                        Math.Round(stresses[1] * stressCoefficient, 3),
                        Math.Round(stresses[2] * stressCoefficient, 3),
                        Math.Round(stresses[3] * stressCoefficient, 3),
                        Math.Round(stresses[4] * stressCoefficient, 3),
                        Math.Round(stresses[5] * stressCoefficient, 3),
                        Math.Round(stresses[6] * stressCoefficient, 3),
                        Math.Round(stresses[7] * stressCoefficient, 3),
                        Math.Round(stresses[8] * stressCoefficient, 3),
                        Math.Round(stresses[9] * stressCoefficient, 3),
                        Math.Round(stresses[10]/(stresses[10]+stresses[11]), 3)*100,
                        Math.Round(stresses[12]/(stresses[12]+stresses[13]), 3)*100
                        };
                FoundationStresses.Rows.Add(newTableItem);
            }
            List<double[]> stressesWithoutWeigth = FoundationProcessor.GetStresses(foundation, foundation.ForceCurvaturesWithoutWeight);
            FoundationStresses = dataSet.Tables["FoundationStressesWithoutWeight"];
            foreach (double[] stresses in stressesWithoutWeigth)
            {
                DataRow newTableItem = FoundationStresses.NewRow();
                double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
                newTableItem.ItemArray = new object[]
                        { ProgrammSettings.CurrentTmpId,
                        foundation.Id,
                        Math.Round(stresses[0] * stressCoefficient, 3),
                        Math.Round(stresses[1] * stressCoefficient, 3),
                        Math.Round(stresses[2] * stressCoefficient, 3),
                        Math.Round(stresses[3] * stressCoefficient, 3),
                        Math.Round(stresses[4] * stressCoefficient, 3),
                        Math.Round(stresses[5] * stressCoefficient, 3),
                        Math.Round(stresses[6] * stressCoefficient, 3),
                        Math.Round(stresses[7] * stressCoefficient, 3),
                        Math.Round(stresses[8] * stressCoefficient, 3),
                        Math.Round(stresses[9] * stressCoefficient, 3),
                        Math.Round(stresses[10]/(stresses[10]+stresses[11]), 3)*100,
                        Math.Round(stresses[12]/(stresses[12]+stresses[13]), 3)*100
                        };
                FoundationStresses.Rows.Add(newTableItem);
            }
        }
        private void ProcessSettlement(Foundation foundation)
        {
            List<CompressedLayerList> mainCompressedLayers = foundation.CompressedLayers;
            double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
            DataTable SettlementSets = dataSet.Tables["SettlementSets"];
            DataTable ComressedLayers = dataSet.Tables["ComressedLayers"];
            int i = 0;
            foreach (CompressedLayerList compressedLayersList in mainCompressedLayers)
            {
                int setId = ProgrammSettings.CurrentTmpId;
                DataRow newTableItem = SettlementSets.NewRow();
                newTableItem.ItemArray = new object[]
                        { setId,
                        foundation.Id,
                        foundation.LoadCases[i].Name,
                        Math.Round(compressedLayersList.CompressedLayers[0].SumSettlement * MeasureUnitConverter.GetCoefficient(0), 3),
                        Math.Round(SoilLayerProcessor.ComressedHeight(compressedLayersList.CompressedLayers), 3)
                        };
                SettlementSets.Rows.Add(newTableItem);
                i++;
                foreach (CompressedLayer compressedLayer in compressedLayersList.CompressedLayers)
                {
                    DataRow newSettleItem = ComressedLayers.NewRow();
                    newSettleItem.ItemArray = new object[]
                        { ProgrammSettings.CurrentTmpId,
                        setId,
                        0,
                        Math.Round(compressedLayer.SoilElementaryLayer.TopLevel, 3),
                        Math.Round(compressedLayer.SoilElementaryLayer.BottomLevel, 3),
                        Math.Round(compressedLayer.SigmZg * stressCoefficient, 3),
                        Math.Round(compressedLayer.SigmZgamma * stressCoefficient, 3),
                        Math.Round(compressedLayer.SigmZp * stressCoefficient, 3),
                        Math.Round(compressedLayer.LocalSettlement * MeasureUnitConverter.GetCoefficient(0), 3),
                        Math.Round(compressedLayer.SumSettlement * MeasureUnitConverter.GetCoefficient(0), 3)
                        };
                    ComressedLayers.Rows.Add(newSettleItem);
                }
            }
        }
        private void ProcessSubElements(DataTable Foundations, Foundation foundation)
        {
            ProcessFoundation(Foundations, foundation);
            ProcessFoundationParts(foundation);
            ProcessLoadSets(foundation);
            ProcessBtmStresses(foundation);
            ProcessSettlement(foundation);
        }
    }
}
