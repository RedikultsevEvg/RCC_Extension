using CSL.Common;
using CSL.DataSets.RCC.Foundations;
using CSL.Reports.Interfaces;
using FastReport;
using RDBLL.DrawUtils.SteelBases;
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
using RDBLL.Common.Interfaces;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Common.Geometry;

namespace CSL.Reports
{
    public class FoundationReport : IReport
    {
        private BuildingSite _buildingSite;
        public DataSet dataSet { get; set; }

        public void ShowReport(string fileName)
        {
            PrepareReport();
            Report report = new Report();
            try
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
                    //Если отчет подготовлен, выводим его
                    if (report.Prepare()) report.Show();
                    else MessageBox.Show("Ошибка подготовки отчета");
                }
#endif
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage($"Ошибка вывода отчета", ex);
            }
            finally
            {
                report.Dispose();
            }
        }
        private void PrepareReport()
        {
            foreach (Building building in _buildingSite.Children)
            {
                foreach (Level level in building.Children)
                {
                    DataTable Foundations = dataSet.Tables["Foundations"];
                    foreach (IHasParent child in level.Children)
                    {
                        if (child is Foundation)
                        {
                            Foundation foundation = child as Foundation;
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

            DataRow newFound = dataTable.NewRow();
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
            DsOperation.SetId(newFound, foundation.Id, foundation.Name, foundation.ParentMember.Id);
            newFound.SetField("Picture", b);
            newFound.SetField("Width", foundationWidth * MeasureUnitConverter.GetCoefficient(0));
            newFound.SetField("Length", foundationLength * MeasureUnitConverter.GetCoefficient(0));
            newFound.SetField("Height", foundationHeight * MeasureUnitConverter.GetCoefficient(0));
            newFound.SetField("SoilVolumeWeight", foundation.SoilVolumeWeight * MeasureUnitConverter.GetCoefficient(9));
            newFound.SetField("ConcreteVolumeWeight", foundation.ConcreteVolumeWeight * MeasureUnitConverter.GetCoefficient(9));
            newFound.SetField("SoilVolume", foundationSoilVolume * MeasureUnitConverter.GetCoefficient(11));
            newFound.SetField("ConcreteVolume", foundationConcreteVolume * MeasureUnitConverter.GetCoefficient(11));
            newFound.SetField("Area", A * MeasureUnitConverter.GetCoefficient(4));
            newFound.SetField("Wx", Wx * MeasureUnitConverter.GetCoefficient(5));
            newFound.SetField("Wy", Wy * MeasureUnitConverter.GetCoefficient(5));

            //Actual area of reinforcement in cm^2
            newFound.SetField("AsBottomXAct", foundation.Result.AsAct[0] * 1e4);
            newFound.SetField("AsBottomYAct", foundation.Result.AsAct[1] * 1e4);
            //Recuared area of reinforcement in cm^2
            newFound.SetField("AsBottomXMax", foundation.Result.AsRec[0] * 1e4);
            newFound.SetField("AsBottomYMax", foundation.Result.AsRec[1] * 1e4);
            #region AsXCheck
            Compare compare = new Compare();
            compare.Name = "Проверка площади армирования вдоль оси X";
            compare.ValUnitName = "кв.см";
            compare.Val1Name = "As,x(треб.)";
            compare.Val2Name = "As,x(факт.)";
            compare.Val1 = foundation.Result.AsRec[0] * 1e4;
            compare.Val2 = foundation.Result.AsAct[0] * 1e4;
            if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
            newFound.SetField("AsFstConclusionX", compare.CompareResult());
            #endregion
            #region AsYCheck
            compare = new Compare();
            compare.Name = "Проверка площади армирования вдоль оси Y";
            compare.ValUnitName = "кв.см";
            compare.Val1Name = "As,y(треб.)";
            compare.Val2Name = "As,y(факт.)";
            compare.Val1 = foundation.Result.AsRec[1] * 1e4;
            compare.Val2 = foundation.Result.AsAct[1] * 1e4;
            if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
            newFound.SetField("AsFstConclusionY", compare.CompareResult());
            #endregion
            dataTable.Rows.Add(newFound);
            return newFound;
        }
        /// <summary>
        /// Добавление в датасет данных по нагрузкам
        /// </summary>
        /// <param name="foundation"></param>
        private void ProcessLoadSets(Foundation foundation)
        {
            CommonServices.AddLoadsToDataset(dataSet, "LoadSets", "Foundations", foundation.ForcesGroups[0].LoadSets, foundation.Id, foundation.Name);
            CommonServices.AddLoadsToDataset(dataSet, "LoadCases", "Foundations", foundation.LoadCases, foundation.Id, foundation.Name);

            ObservableCollection<LoadSet> loadSetsWith = foundation.btmLoadSetsWithWeight;
            CommonServices.AddLoadsToDataset(dataSet, "BottomLoadCasesWithWeight", "Foundations", loadSetsWith, foundation.Id, foundation.Name);

            ObservableCollection<LoadSet> loadSetsWithout = foundation.btmLoadSetsWithoutWeight;
            CommonServices.AddLoadsToDataset(dataSet, "BottomLoadCases", "Foundations", loadSetsWithout, foundation.Id, foundation.Name);
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
            foreach (RectFoundationPart part in foundation.Parts)
            {
                DataRow newPart = FoundationParts.NewRow();
                DsOperation.SetId(newPart, part.Id, part.Name, part.ParentMember.Id);
                newPart.SetField("Width", part.Width * MeasureUnitConverter.GetCoefficient(0));
                newPart.SetField("Length", part.Length * MeasureUnitConverter.GetCoefficient(0));
                newPart.SetField("Heigth", part.Height * MeasureUnitConverter.GetCoefficient(0));
                newPart.SetField("Volume", part.Width * part.Length * part.Height * MeasureUnitConverter.GetCoefficient(11));
                newPart.SetField("CenterX", part.CenterX * MeasureUnitConverter.GetCoefficient(0));
                newPart.SetField("CenterY", part.CenterY * MeasureUnitConverter.GetCoefficient(0));

                #region Moments
                List<double>[] moments;
                double momentCoff = MeasureUnitConverter.GetCoefficient(2);

                moments = new List<double>[4];
                moments[0] = new List<double>();
                moments[1] = new List<double>();
                moments[2] = new List<double>();
                moments[3] = new List<double>();
                foreach (LoadCombination comb in part.Result.partMomentAreas.LoadCombinationsY)
                {
                    foreach (ForceParameter forceParameter in comb.ForceParameters)
                    {
                        if (forceParameter.KindId == 2)
                        {
                            moments[1].Add(forceParameter.CrcValue);
                            moments[3].Add(forceParameter.DesignValue);
                        }
                    }
                }
                #region MomentsX
                //Получаем моменты на ширину ступени
                newPart.SetField("CrcMomentXMin", moments[1].Min() * momentCoff);
                newPart.SetField("DesignMomentXMin", moments[3].Min() * momentCoff);
                newPart.SetField("CrcMomentXMax", moments[1].Max() * momentCoff);
                newPart.SetField("DesignMomentXMax", moments[3].Max() * momentCoff);
                //Получаем моменты на единицу ширины ступени
                newPart.SetField("CrcMomentXMinDistr", moments[1].Min() * momentCoff / length);
                newPart.SetField("DesignMomentXMinDistr", moments[3].Min() * momentCoff / length);
                newPart.SetField("CrcMomentXMaxDistr", moments[1].Max() * momentCoff / length);
                newPart.SetField("DesignMomentXMaxDistr", moments[3].Max() * momentCoff / length);
                #endregion
                moments = new List<double>[4];
                moments[0] = new List<double>();
                moments[1] = new List<double>();
                moments[2] = new List<double>();
                moments[3] = new List<double>();
                foreach (LoadCombination comb in part.Result.partMomentAreas.LoadCombinationsX)
                {
                    foreach (ForceParameter forceParameter in comb.ForceParameters)
                    {
                        if (forceParameter.KindId == 2)
                        {
                            moments[1].Add(forceParameter.CrcValue);
                            moments[3].Add(forceParameter.DesignValue);
                        }
                    }
                }
                #region MomentsY
                //Получаем моменты на ширину ступени
                newPart.SetField("CrcMomentYMin", moments[1].Min() * momentCoff);
                newPart.SetField("DesignMomentYMin", moments[3].Min() * momentCoff);
                newPart.SetField("CrcMomentYMax", moments[1].Max() * momentCoff);
                newPart.SetField("DesignMomentYMax", moments[3].Max() * momentCoff);
                //Получаем моменты на единицу ширины ступени
                newPart.SetField("CrcMomentYMinDistr", moments[1].Min() * momentCoff / width);
                newPart.SetField("DesignMomentYMinDistr", moments[3].Min() * momentCoff / width);
                newPart.SetField("CrcMomentYMaxDistr", moments[1].Max() * momentCoff / width);
                newPart.SetField("DesignMomentYMaxDistr", moments[3].Max() * momentCoff / width);
                #endregion
                #endregion
                #region Mcrc
                newPart.SetField("CrcMomentX", part.Result.Mcrc[1] * momentCoff);
                newPart.SetField("CrcMomentY", part.Result.Mcrc[0] * momentCoff);
                #endregion
                #region CrackWidth
                #endregion
                FoundationParts.Rows.Add(newPart);
            }
        }
        /// <summary>
        /// Добавление в датасет данных по давлениям под подошвой
        /// </summary>
        /// <param name="foundation"></param>
        private void ProcessBtmStresses(Foundation foundation, DataRow foundRow)
        {
            double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
            DataTable FoundationStresses = dataSet.Tables["FoundationStressesWithWeight"];
            foreach (double[] stresses in foundation.Result.StressesWithWeigth)
            {
                DataRow stress = FoundationStresses.NewRow();
                DsOperation.SetId(stress, ProgrammSettings.CurrentTmpId, null, foundation.Id);
                stress.SetField("crcAvgStress", stresses[0] * stressCoefficient);
                stress.SetField("crcCenterStress", stresses[1] * stressCoefficient);
                stress.SetField("crcMiddleSresses", stresses[2] * stressCoefficient);
                stress.SetField("crcCornerSressesMin", stresses[3] * stressCoefficient);
                stress.SetField("crcCornerSressesMax", stresses[4] * stressCoefficient);
                stress.SetField("designAvgStress", stresses[5] * stressCoefficient);
                stress.SetField("designCenterStress", stresses[6] * stressCoefficient);
                stress.SetField("designMiddleSresses", stresses[7] * stressCoefficient);
                stress.SetField("designCornerSressesMin", stresses[8] * stressCoefficient);
                stress.SetField("designCornerSressesMax", stresses[9] * stressCoefficient);
                stress.SetField("CrcTensionAreaRatio", stresses[10] / (stresses[10] + stresses[11]));
                stress.SetField("DesignTensionAreaRatio", stresses[12] / (stresses[12] + stresses[13]));

                FoundationStresses.Rows.Add(stress);
            }



            foundRow.SetField("MinSndAvgStressesWithWeight", foundation.Result.MinSndAvgStressesWithWeight * stressCoefficient);
            foundRow.SetField("MinSndMiddleStressesWithWeight", foundation.Result.MinSndMiddleStressesWithWeight * stressCoefficient);
            foundRow.SetField("MinSndCornerStressesWithWeight", foundation.Result.MinSndCornerStressesWithWeight * stressCoefficient);
            foundRow.SetField("MaxSndCornerStressesWithWeight", foundation.Result.MaxSndCornerStressesWithWeight * stressCoefficient);
            foundRow.SetField("MaxSndTensionAreaRatioWithWeight", foundation.Result.MaxSndTensionAreaRatioWithWeight);
            foundRow.SetField("sndResistance", foundation.Result.SndResistance * stressCoefficient);

            List<double[]> stressesWithoutWeigth = FoundationProcessor.GetStresses(foundation, foundation.ForceCurvaturesWithoutWeight);
            FoundationStresses = dataSet.Tables["FoundationStressesWithoutWeight"];
            foreach (double[] stresses in MathOperation.Round(stressesWithoutWeigth))
            {
                
                DataRow newTableItem = FoundationStresses.NewRow();
                DsOperation.SetId(newTableItem, ProgrammSettings.CurrentTmpId, null, foundation.Id);
                newTableItem.SetField("crcAvgStress", stresses[0] * stressCoefficient);
                newTableItem.SetField("crcCenterStress", stresses[1] * stressCoefficient);
                newTableItem.SetField("crcMiddleSresses", stresses[2] * stressCoefficient);
                newTableItem.SetField("crcCornerSressesMin", stresses[3] * stressCoefficient);
                newTableItem.SetField("crcCornerSressesMax", stresses[4] * stressCoefficient);
                newTableItem.SetField("designAvgStress", stresses[5] * stressCoefficient);
                newTableItem.SetField("designCenterStress", stresses[6] * stressCoefficient);
                newTableItem.SetField("designMiddleSresses", stresses[7] * stressCoefficient);
                newTableItem.SetField("designCornerSressesMin", stresses[8] * stressCoefficient);
                newTableItem.SetField("designCornerSressesMax", stresses[9] * stressCoefficient);
                newTableItem.SetField("CrcTensionAreaRatio", stresses[10] / (stresses[10] + stresses[11]));
                newTableItem.SetField("DesignTensionAreaRatio", stresses[12] / (stresses[12] + stresses[13]));

                FoundationStresses.Rows.Add(newTableItem);

                Compare compare = new Compare(3);
                #region AvgStresCheck
                compare.Name = "Проверка величины среднего давления";
                compare.Val1Name = "Sigma";
                compare.Val2Name = "R";
                compare.Val1 = foundation.Result.MinSndAvgStressesWithWeight * (-1D);
                compare.Val2 = foundation.Result.SndResistance;
                if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
                foundRow.SetField("AvgStressConclusion", compare.CompareResult());
                #endregion
                #region MiddleStresCheck
                compare.Name = "Проверка величины краевого давления";
                compare.Val1Name = "Sigma";
                compare.Val2Name = "1.2R";
                compare.Val1 = foundation.Result.MinSndMiddleStressesWithWeight * (-1D);
                compare.Val2 = foundation.Result.SndResistance * 1.2;
                if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
                foundRow.SetField("MiddleStressConclusion", compare.CompareResult());
                #endregion
                #region CornerStresCheck
                compare.Name = "Проверка величины углового давления";
                compare.Val1Name = "Sigma";
                compare.Val2Name = "1.5R";
                compare.Val1 = foundation.Result.MinSndCornerStressesWithWeight * (-1D);
                compare.Val2 = foundation.Result.SndResistance * 1.5;
                if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
                foundRow.SetField("CornerStressConclusion", compare.CompareResult());
                #endregion
            }
        }
        /// <summary>
        /// Обработчик данных по осадкам
        /// </summary>
        /// <param name="foundation"></param>
        /// <param name="newFound"></param>
        private void ProcessSettlement(Foundation foundation, DataRow newFound)
        {
            List<CompressedLayerList> mainCompressedLayers = foundation.Result.CompressedLayers;
            double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
            DataTable SettlementSets = dataSet.Tables["SettlementSets"];
            DataTable ComressedLayers = dataSet.Tables["ComressedLayers"];
            int i = 0;
            foreach (CompressedLayerList compressedLayersList in mainCompressedLayers)
            {
                int setId = ProgrammSettings.CurrentTmpId;
                DataRow SettleSet = SettlementSets.NewRow();
                SettleSet.SetField("Id", setId);
                SettleSet.SetField("Name", foundation.LoadCases[i].Name);
                SettleSet.SetField("ParentId", foundation.Id);

                double sumSettlement = compressedLayersList.CompressedLayers[0].SumSettlement;
                double roundSumSettlement = sumSettlement * MeasureUnitConverter.GetCoefficient(0);
                double compressionHeight = SoilLayerProcessor.ComressedHeight(compressedLayersList.CompressedLayers);
                SettleSet.SetField("MinSettlement", roundSumSettlement);
                SettleSet.SetField("CompressedHeight", compressionHeight);

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

                SettleSet.SetField("SumRotateX", MxInc);
                SettleSet.SetField("SumRotateY", MyInc);
                SettleSet.SetField("SumRotateXY", Math.Sqrt(MxInc * MxInc + MyInc * MyInc));
                SettleSet.SetField("NzStiffness", NzStiffnessString);
                SettleSet.SetField("MxStiffness", MxStiffnessString);
                SettleSet.SetField("MyStiffness", MyStiffnessString);

                SettlementSets.Rows.Add(SettleSet);
                i++;
                foreach (CompressedLayer compLayer in compressedLayersList.CompressedLayers)
                {
                    DataRow compLayerRow = ComressedLayers.NewRow();
                    DsOperation.SetId(compLayerRow, ProgrammSettings.CurrentTmpId, null, setId);
                    compLayerRow.SetField("ZLevel", compLayer.Zlevel);
                    compLayerRow.SetField("TopLevel", compLayer.SoilElementaryLayer.TopLevel);
                    compLayerRow.SetField("BtmLevel", compLayer.SoilElementaryLayer.BottomLevel);
                    compLayerRow.SetField("Alpha", compLayer.Alpha);
                    compLayerRow.SetField("SigmZg", compLayer.SigmZg * stressCoefficient);
                    compLayerRow.SetField("SigmZgamma", compLayer.SigmZgamma * stressCoefficient);
                    compLayerRow.SetField("SigmZp", compLayer.SigmZp * stressCoefficient);
                    compLayerRow.SetField("LocalSettlement", compLayer.LocalSettlement * MeasureUnitConverter.GetCoefficient(0));
                    compLayerRow.SetField("SumSettlement", compLayer.SumSettlement * MeasureUnitConverter.GetCoefficient(0));
                    ComressedLayers.Rows.Add(compLayerRow);
                }
            }
            FoundationProcessor.SettleMentResult SettleMentResult = FoundationProcessor.GetSettleMentResult(foundation);
            newFound.SetField("SettlementMin", SettleMentResult.Settlement * MeasureUnitConverter.GetCoefficient(0));
            newFound.SetField("CompressionHeightMax", SettleMentResult.CompressionHeight);
            newFound.SetField("IncXMax", SettleMentResult.IncX);
            newFound.SetField("IncYMax", SettleMentResult.IncY);
            newFound.SetField("IncXYMax", SettleMentResult.IncXY);
            newFound.SetField("NzStiffnessStringMin", SettleMentResult.NzStiffnessStringMin);
            newFound.SetField("MxStiffnessStringMin", SettleMentResult.MxStiffnessStringMin);
            newFound.SetField("MyStiffnessStringMin", SettleMentResult.MyStiffnessStringMin);
            newFound.SetField("NzStiffnessStringMax", SettleMentResult.NzStiffnessStringMax);
            newFound.SetField("MxStiffnessStringMax", SettleMentResult.MxStiffnessStringMax);
            newFound.SetField("MyStiffnessStringMax", SettleMentResult.MyStiffnessStringMax);

            #region SettlementCheck
            Compare compare = new Compare(0);
            compare.Name = "Проверка величины осадки";
            compare.Val1Name = "S";
            compare.Val2Name = "Smax";
            compare.Val1 = foundation.Result.MaxSettlement * (-1D);
            compare.Val2 = ((foundation.ParentMember as IHasParent).ParentMember as Building).MaxFoundationSettlement;
            if (!compare.BoolResult()) foundation.Result.GeneralResult = false;
            newFound.SetField("SettlementConclusion", compare.CompareResult());
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
            result += "соответствуют требованиям действующих норм.";
            newFoundation.SetField("GeneralConclusion", result);
        }
    }
}
