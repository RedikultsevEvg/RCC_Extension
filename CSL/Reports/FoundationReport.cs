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
using System.Linq;
using RDBLL.Entity.Common.NDM;



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
#if DEBUG
                {
                    report.Design();
                }
#else
                {
                    report.Show();
                }
#endif
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
                        //if (!(foundation.IsLoadCasesActual & foundation.IsPartsActual))
                        //{
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
                        //}
                        //else //Если актуальны, то сразу подготавливаем отчет
                        //{
                            //Заносим результаты расчета в таблицы датасета
                            //ProcessSubElements(Foundations, foundation);
                        //}
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
        private DataRow ProcessFoundation(DataTable dataTable, Foundation foundation)
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
            return newFoundation;
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
            double[] foundSizes = FoundationProcessor.GetContourSize(foundation);
            double width = foundSizes[0];
            double length = foundSizes[1];

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
                List<FoundationBodyProcessor.PartMomentAreas> partMoments = foundation.Result.partMomentAreas;
                List<double>[] moments;
                double momentCoff = MeasureUnitConverter.GetCoefficient(2);
                foreach (FoundationBodyProcessor.PartMomentAreas partMomentAreas in partMoments)
                {
                    if (partMomentAreas.FoundationPartId == foundationPart.Id)
                    {
                        moments = new List<double>[4];
                        moments[0] = new List<double>();
                        moments[1] = new List<double>();
                        moments[2] = new List<double>();
                        moments[3] = new List<double>();
                        foreach (LoadCombination comb in partMomentAreas.loadCombinationsX)
                        {
                            moments[1].Add(comb.ForceParameters[0].CrcValue);
                            moments[3].Add(comb.ForceParameters[0].DesignValue);
                        }
                        //Получаем моменты на ширину ступени
                        newFoundationPart.SetField("CrcMomentXMin", moments[1].Min() * momentCoff);
                        newFoundationPart.SetField("DesignMomentXMin", moments[3].Min() * momentCoff);
                        newFoundationPart.SetField("CrcMomentXMax", moments[1].Max() * momentCoff);
                        newFoundationPart.SetField("DesignMomentXMax", moments[3].Max() * momentCoff);
                        //Получаем моменты на единицу ширины ступени
                        newFoundationPart.SetField("CrcMomentXMinDistr", moments[1].Min() * momentCoff / width);
                        newFoundationPart.SetField("DesignMomentXMinDistr", moments[3].Min() * momentCoff / width);
                        newFoundationPart.SetField("CrcMomentXMaxDistr", moments[1].Max() * momentCoff / width);
                        newFoundationPart.SetField("DesignMomentXMaxDistr", moments[3].Max() * momentCoff / width);

                        moments = new List<double>[4];
                        moments[0] = new List<double>();
                        moments[1] = new List<double>();
                        moments[2] = new List<double>();
                        moments[3] = new List<double>();
                        foreach (LoadCombination comb in partMomentAreas.loadCombinationsY)
                        {
                            moments[1].Add(comb.ForceParameters[0].CrcValue);
                            moments[3].Add(comb.ForceParameters[0].DesignValue);
                        }
                        //Получаем моменты на ширину ступени
                        newFoundationPart.SetField("CrcMomentYMin", moments[1].Min() * momentCoff);
                        newFoundationPart.SetField("DesignMomentYMin", moments[3].Min() * momentCoff);
                        newFoundationPart.SetField("CrcMomentYMax", moments[1].Max() * momentCoff);
                        newFoundationPart.SetField("DesignMomentYMax", moments[3].Max() * momentCoff);
                        //Получаем моменты на единицу ширины ступени
                        newFoundationPart.SetField("CrcMomentYMinDistr", moments[1].Min() * momentCoff / length);
                        newFoundationPart.SetField("DesignMomentYMinDistr", moments[3].Min() * momentCoff / length);
                        newFoundationPart.SetField("CrcMomentYMaxDistr", moments[1].Max() * momentCoff / length);
                        newFoundationPart.SetField("DesignMomentYMaxDistr", moments[3].Max() * momentCoff / length);
                    }
                }

                FoundationParts.Rows.Add(newFoundationPart);
            }
        }
        /// <summary>
        /// Добавление в датасет данных по давлениям под подошвой
        /// </summary>
        /// <param name="foundation"></param>
        private void ProcessBtmStresses(Foundation foundation, DataRow newFoundation)
        {
            double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
            DataTable FoundationStresses = dataSet.Tables["FoundationStressesWithWeight"];
            foreach (double[] stresses in foundation.Result.StressesWithWeigth)
            {
                DataRow newTableItem = FoundationStresses.NewRow();
                
                newTableItem.ItemArray = new object[]
                        { ProgrammSettings.CurrentTmpId,
                        foundation.Id,
                        stresses[0] * stressCoefficient,
                        stresses[1] * stressCoefficient,
                        stresses[2] * stressCoefficient,
                        stresses[3] * stressCoefficient,
                        stresses[4] * stressCoefficient,
                        stresses[5] * stressCoefficient,
                        stresses[6] * stressCoefficient,
                        stresses[7] * stressCoefficient,
                        stresses[8] * stressCoefficient,
                        stresses[9] * stressCoefficient,
                        stresses[10]/(stresses[10]+stresses[11]),
                        stresses[12]/(stresses[12]+stresses[13])
                        };
                FoundationStresses.Rows.Add(newTableItem);
            }

            newFoundation.SetField("MinSndAvgStressesWithWeight", foundation.Result.MinSndAvgStressesWithWeight * stressCoefficient);
            newFoundation.SetField("MinSndMiddleStressesWithWeight", foundation.Result.MinSndMiddleStressesWithWeight * stressCoefficient);
            newFoundation.SetField("MinSndCornerStressesWithWeight", foundation.Result.MinSndCornerStressesWithWeight * stressCoefficient);
            newFoundation.SetField("MaxSndCornerStressesWithWeight", foundation.Result.MaxSndCornerStressesWithWeight * stressCoefficient);
            newFoundation.SetField("MaxSndTensionAreaRatioWithWeight", foundation.Result.MaxSndTensionAreaRatioWithWeight);
            newFoundation.SetField("sndResistance", foundation.Result.SndResistance * stressCoefficient);

            List<double[]> stressesWithoutWeigth = FoundationProcessor.GetStresses(foundation, foundation.ForceCurvaturesWithoutWeight);
            FoundationStresses = dataSet.Tables["FoundationStressesWithoutWeight"];
            foreach (double[] stresses in stressesWithoutWeigth)
            {
                DataRow newTableItem = FoundationStresses.NewRow();
                newTableItem.ItemArray = new object[]
                        { ProgrammSettings.CurrentTmpId,
                        foundation.Id,
                        stresses[0] * stressCoefficient,
                        stresses[1] * stressCoefficient,
                        stresses[2] * stressCoefficient,
                        stresses[3] * stressCoefficient,
                        stresses[4] * stressCoefficient,
                        stresses[5] * stressCoefficient,
                        stresses[6] * stressCoefficient,
                        stresses[7] * stressCoefficient,
                        stresses[8] * stressCoefficient,
                        stresses[9] * stressCoefficient,
                        stresses[10]/(stresses[10]+stresses[11]),
                        stresses[12]/(stresses[12]+stresses[13])
                        };
                FoundationStresses.Rows.Add(newTableItem);

                Compare compare = new Compare(3);
                #region AvgStresCheck
                compare.Name = "Проверка величины среднего давления";
                compare.Val1Name = "Sigma";
                compare.Val2Name = "R";
                compare.Val1 = foundation.Result.MinSndAvgStressesWithWeight * (-1D);
                compare.Val2 = foundation.Result.SndResistance;
                if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
                newFoundation.SetField("AvgStressConclusion", compare.CompareResult());
                #endregion
                #region MiddleStresCheck
                compare.Name = "Проверка величины краевого давления";
                compare.Val1Name = "Sigma";
                compare.Val2Name = "1.2R";
                compare.Val1 = foundation.Result.MinSndMiddleStressesWithWeight * (-1D);
                compare.Val2 = foundation.Result.SndResistance * 1.2;
                if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
                newFoundation.SetField("MiddleStressConclusion", compare.CompareResult());
                #endregion
                #region CornerStresCheck
                compare.Name = "Проверка величины углового давления";
                compare.Val1Name = "Sigma";
                compare.Val2Name = "1.5R";
                compare.Val1 = foundation.Result.MinSndCornerStressesWithWeight * (-1D);
                compare.Val2 = foundation.Result.SndResistance * 1.5;
                if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
                newFoundation.SetField("CornerStressConclusion", compare.CompareResult());
                #endregion
            }
        }
        /// <summary>
        /// Обработчик данных по осадкам
        /// </summary>
        /// <param name="foundation"></param>
        /// <param name="newFoundation"></param>
        private void ProcessSettlement(Foundation foundation, DataRow newFoundation)
        {
            List<CompressedLayerList> mainCompressedLayers = foundation.Result.CompressedLayers;
            double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
            DataTable SettlementSets = dataSet.Tables["SettlementSets"];
            DataTable ComressedLayers = dataSet.Tables["ComressedLayers"];
            int i = 0;
            foreach (CompressedLayerList compressedLayersList in mainCompressedLayers)
            {
                int setId = ProgrammSettings.CurrentTmpId;
                DataRow newTableItem = SettlementSets.NewRow();
                double sumSettlement = compressedLayersList.CompressedLayers[0].SumSettlement;
                double roundSumSettlement = sumSettlement * MeasureUnitConverter.GetCoefficient(0);
                double compressionHeight = SoilLayerProcessor.ComressedHeight(compressedLayersList.CompressedLayers);
                newTableItem.ItemArray = new object[]
                        { setId,
                        foundation.Id,
                        foundation.LoadCases[i].Name,
                        roundSumSettlement,
                        compressionHeight
                        };

                SumForces sumForces = new SumForces(foundation.LoadCases[i], false);
                double Nz = sumForces.ForceMatrix[2, 0];
                double Mx = sumForces.ForceMatrix[0, 0];
                double My = sumForces.ForceMatrix[1, 0];
                double[] rotates = SoilLayerProcessor.Inclination(Mx, My, compressedLayersList.CompressedLayers, foundation);
                double MxInc = rotates[0];
                double MyInc = rotates[1];
                double MxyInc = Math.Sqrt(MxInc * MxInc + MyInc * MyInc);

                string NzStiffnessString = "---";
                string MxStiffnessString = "---";
                string MyStiffnessString = "---";

                if (sumSettlement != 0)
                {
                    double NzStiffness = Nz / sumSettlement;
                    double NzStiffnessRound = Math.Round(NzStiffness * MeasureUnitConverter.GetCoefficient(1) / MeasureUnitConverter.GetCoefficient(0), 3);
                    NzStiffnessString = Convert.ToString(NzStiffnessRound);
                    if (MxInc != 0)
                    {
                        double MxStiffness = Mx / MxInc;
                        double MxStiffnessRound = Math.Round(MxStiffness * MeasureUnitConverter.GetCoefficient(2), 3);
                        MxStiffnessString = Convert.ToString(MxStiffnessRound);
                    }
                    if (MyInc != 0)
                    {
                        double MyStiffness = My / MyInc;
                        double MyStiffnessRound = Math.Round(MyStiffness * MeasureUnitConverter.GetCoefficient(2), 3);
                        MxStiffnessString = Convert.ToString(MyStiffnessRound);
                    }
                }

                newTableItem.SetField("SumRotateX", MxInc);
                newTableItem.SetField("SumRotateY", MyInc);
                newTableItem.SetField("SumRotateXY", Math.Sqrt(MxInc * MxInc + MyInc * MyInc));
                newTableItem.SetField("NzStiffness", NzStiffnessString);
                newTableItem.SetField("MxStiffness", MxStiffnessString);
                newTableItem.SetField("MyStiffness", MyStiffnessString);

                SettlementSets.Rows.Add(newTableItem);
                i++;
                foreach (CompressedLayer compressedLayer in compressedLayersList.CompressedLayers)
                {
                    DataRow newSettleItem = ComressedLayers.NewRow();
                    newSettleItem.ItemArray = new object[]
                        { ProgrammSettings.CurrentTmpId,
                        setId,
                        compressedLayer.Zlevel,
                        compressedLayer.SoilElementaryLayer.TopLevel,
                        compressedLayer.SoilElementaryLayer.BottomLevel,
                        compressedLayer.Alpha,
                        compressedLayer.SigmZg * stressCoefficient,
                        compressedLayer.SigmZgamma * stressCoefficient,
                        compressedLayer.SigmZp * stressCoefficient,
                        compressedLayer.LocalSettlement * MeasureUnitConverter.GetCoefficient(0),
                        compressedLayer.SumSettlement * MeasureUnitConverter.GetCoefficient(0)
                        };
                    ComressedLayers.Rows.Add(newSettleItem);
                }
            }
            FoundationProcessor.SettleMentResult SettleMentResult = FoundationProcessor.GetSettleMentResult(foundation);
            newFoundation.SetField("SettlementMin", SettleMentResult.Settlement * MeasureUnitConverter.GetCoefficient(0));
            newFoundation.SetField("CompressionHeightMax", SettleMentResult.CompressionHeight);
            newFoundation.SetField("IncXMax", SettleMentResult.IncX);
            newFoundation.SetField("IncYMax", SettleMentResult.IncY);
            newFoundation.SetField("IncXYMax", SettleMentResult.IncXY);
            newFoundation.SetField("NzStiffnessStringMin", SettleMentResult.NzStiffnessStringMin);
            newFoundation.SetField("MxStiffnessStringMin", SettleMentResult.MxStiffnessStringMin);
            newFoundation.SetField("MyStiffnessStringMin", SettleMentResult.MyStiffnessStringMin);
            newFoundation.SetField("NzStiffnessStringMax", SettleMentResult.NzStiffnessStringMax);
            newFoundation.SetField("MxStiffnessStringMax", SettleMentResult.MxStiffnessStringMax);
            newFoundation.SetField("MyStiffnessStringMax", SettleMentResult.MyStiffnessStringMax);

            #region SettlementCheck
            Compare compare = new Compare(0);
            compare.Name = "Проверка величины осадки";
            compare.Val1Name = "S";
            compare.Val2Name = "Smax";
            compare.Val1 = foundation.Result.MaxSettlement * (-1D);
            compare.Val2 = foundation.Level.Building.MaxFoundationSettlement;
            if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
            newFoundation.SetField("SettlementConclusion", compare.CompareResult());
            #endregion
        }
        private void ProcessSubElements(DataTable Foundations, Foundation foundation)
        {
            foundation.Result.GeneralResult = true;
            DataRow newFoundation = ProcessFoundation(Foundations, foundation);
            ProcessFoundationParts(foundation);
            ProcessLoadSets(foundation);
            ProcessBtmStresses(foundation, newFoundation);
            ProcessSettlement(foundation, newFoundation);
            string result = $"В соответствии с выполненным расчетом, несущая способность, надежность и долговечность фундамента {foundation.Name} ";
            if (! foundation.Result.GeneralResult) result += "не ";
            result += "соответствуют требованиям действуйющих норм.";
            newFoundation.SetField("GeneralConclusion", result);
        }
    }
}
