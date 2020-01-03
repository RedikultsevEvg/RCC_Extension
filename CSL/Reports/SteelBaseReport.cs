﻿using CSL.Common;
using CSL.DataSets.SC;
using CSL.Reports.Interfaces;
using FastReport;
using RDBLL.Common.Service;
using RDBLL.DrawUtils.SteelBase;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;
using System;
using System.Data;
using System.Linq;
using System.Windows.Controls;

namespace CSL.Reports
{
    public class SteelBaseReport :IReport
    {
        private BuildingSite _buildingSite;
        private double linearSizeCoefficient;
        private double forceCoefficient;
        private double momentCoefficient;
        private double stressCoefficient;
        private double geometryAreaCoefficient;
        private double geometrySecMomentCoefficient { get; }
        private double geometryMomentCoefficient;
        private double MassCoefficient;
        public DataSet dataSet { get; set; }

        public void ShowReport(string fileName)
        {
            PrepareReport();
            using (Report report = new Report())
            {
                report.Load(fileName);
                CommonServices.PrepareMeasureUnit(report);
                report.RegisterData(dataSet);
                //report.Design();
                report.Show();
            }
        }
        private void PrepareReport()
        {
            foreach (Building building in _buildingSite.Buildings)
            {
                foreach (Level level in building.Levels)
                {
                    DataTable SteelBases = dataSet.Tables["SteelBases"];
                    foreach (SteelBase steelBase in level.SteelBases)
                    {
                        if (!steelBase.IsActual)
                        {
                            SteelBaseProcessor.SolveSteelColumnBase(steelBase);
                        }

                        DataRow newSteelBase = SteelBases.NewRow();
                        double A = steelBase.Width * steelBase.Length;
                        double Wx = steelBase.Width * steelBase.Length * steelBase.Length / 6;
                        double Wy = steelBase.Length * steelBase.Width * steelBase.Width / 6;
                        #region Picture
                        Canvas canvas = new Canvas();
                        canvas.Width = 600;
                        canvas.Height = 600;
                        DrawSteelBase.DrawBase(steelBase, canvas);
                        byte[] b = CommonServices.ExportToByte(canvas);
                        #endregion
                        newSteelBase.ItemArray = new object[]
                        { steelBase.Id, b, steelBase.Name,
                            steelBase.SteelStrength * stressCoefficient,
                            steelBase.ConcreteStrength * stressCoefficient,
                            steelBase.Width * linearSizeCoefficient,
                            steelBase.Length * linearSizeCoefficient,
                            steelBase.Thickness * linearSizeCoefficient,
                            A * geometryAreaCoefficient,
                            Wx * geometrySecMomentCoefficient,
                            Wy * geometrySecMomentCoefficient};
                        
                        SteelBases.Rows.Add(newSteelBase);

                        ProcessLoadSets(steelBase);
                        //ProcessLoadCases(steelBase);
                        ProcessPart(steelBase);
                        ProcessBolt(steelBase);
                    }
                }
            }
        }

        

        #region Constructors
        public SteelBaseReport(BuildingSite buildingSite)
        {
            _buildingSite = buildingSite;
            dataSet = SteelBaseDataSet.GetDataSet();

            linearSizeCoefficient = MeasureUnitConverter.GetCoefficient(0);
            forceCoefficient = MeasureUnitConverter.GetCoefficient(1);
            momentCoefficient = MeasureUnitConverter.GetCoefficient(2);
            stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
            geometryAreaCoefficient = MeasureUnitConverter.GetCoefficient(4);
            geometrySecMomentCoefficient = MeasureUnitConverter.GetCoefficient(5);
            geometryMomentCoefficient = MeasureUnitConverter.GetCoefficient(6);
            MassCoefficient = MeasureUnitConverter.GetCoefficient(7);
        }
        #endregion
        /// <summary>
        /// Добавление в датасет данных по нагрузкам
        /// </summary>
        /// <param name="steelBase"></param>
        private void ProcessLoadSets(SteelBase steelBase)
        {
            CommonServices.AddLoadsToDataset(dataSet, "LoadSets", "SteelBases", "SteelBaseId", steelBase.ForcesGroups[0].LoadSets, steelBase.Id, steelBase.Name);
            CommonServices.AddLoadsToDataset(dataSet, "LoadCases", "SteelBases", "SteelBaseId", steelBase.LoadCases, steelBase.Id, steelBase.Name);
        }
        /// <summary>
        /// Оставлено 2020_01_03 на всякий случай. Добавление в датасет данных сочетаниям нагрузок
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        private void ProcessLoadCases(SteelBase steelBase)
        {
            //DataTable dataTable = dataSet.Tables["LoadCases"];
            //foreach (LoadSet loadSet in steelBase.LoadCases)
            //{
            //    string forceDescription = "";
            //    DataRow newLoadCase = dataTable.NewRow();
            //    DataTable ForceParameters = dataSet.Tables["LoadCasesForceParameters"];
            //    foreach (ForceParameter forceParameter in loadSet.ForceParameters)
            //    {
            //        DataRow newForceParameter = ForceParameters.NewRow();
            //        var tmpForceParamLabels = from t in ProgrammSettings.ForceParamKinds where t.Id == forceParameter.KindId select t;
            //        MeasureUnitLabel measureUnitLabel = tmpForceParamLabels.First().MeasureUnit.GetCurrentLabel();
            //        newForceParameter.ItemArray = new object[] { tmpForceParamLabels.First().Id,
            //            loadSet.Id,
            //            tmpForceParamLabels.First().LongLabel,
            //            tmpForceParamLabels.First().ShortLabel,
            //            measureUnitLabel.UnitName,
            //            Math.Round(forceParameter.CrcValueInCurUnit, 3),
            //            Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3)
            //        };
            //        ForceParameters.Rows.Add(newForceParameter);
            //        forceDescription += tmpForceParamLabels.First().ShortLabel +"="+ Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3) + measureUnitLabel.UnitName + "; ";
            //    }
            //    newLoadCase.ItemArray = new object[]
            //        { loadSet.Id, steelBase.Id,
            //        loadSet.Name, loadSet.PartialSafetyFactor,
            //        forceDescription
            //        };
            //    dataTable.Rows.Add(newLoadCase);
            //}
        }
        /// <summary>
        /// Добавление в датасет данных по участку стальной базы
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        private void ProcessPart(SteelBase steelBase)
        {
            DataTable dataTable = dataSet.Tables["SteelBasesParts"];
            foreach (SteelBasePart steelBasePart in steelBase.ActualSteelBaseParts)
            {
                DataRow newSteelBasePart = dataTable.NewRow();
                //double maxBedStress = SteelColumnBasePartProcessor.GetGlobalMinStressLinear(steelBasePart) * (-1D);
                double maxBedStress = SteelBasePartProcessor.GetGlobalMinStressNonLinear(steelBasePart) * (-1D);
                double maxStress = SteelBasePartProcessor.GetResult(steelBasePart, maxBedStress)[1];
                double actualThickness = steelBasePart.SteelBase.Thickness;
                double steelStrength = steelBasePart.SteelBase.SteelStrength;
                double recomendedThickness = 0;
                if (maxStress>0) { recomendedThickness = actualThickness * Math.Pow((maxStress / steelStrength), 0.5); }
                #region Picture
                Canvas canvasPart = new Canvas();
                canvasPart.Width = 300;
                canvasPart.Height = 300;
                double zoom_factor_X = canvasPart.Width / steelBasePart.Width / 1.2;
                double zoom_factor_Y = canvasPart.Height / steelBasePart.Length / 1.2;
                double scale_factor;
                if (zoom_factor_X < zoom_factor_Y) { scale_factor = zoom_factor_X; } else { scale_factor = zoom_factor_Y; }
                canvasPart.Width = steelBasePart.Width * 1.2 * scale_factor;
                canvasPart.Height = steelBasePart.Length * 1.2 * scale_factor;
                double[] columnBaseCenter = new double[2] { canvasPart.Width / 2 - steelBasePart.CenterX * scale_factor, canvasPart.Height / 2 + steelBasePart.CenterY * scale_factor };
                DrawSteelBase.DrawBasePart(steelBasePart, canvasPart, columnBaseCenter, scale_factor, 1, 1, 1, false);
                byte[] bPart = CommonServices.ExportToByte(canvasPart);
                #endregion
                newSteelBasePart.ItemArray = new object[]
                { steelBasePart.Id, steelBase.Id,
                                bPart, steelBasePart.Name,
                                steelBasePart.CenterX * linearSizeCoefficient,
                                steelBasePart.CenterY * linearSizeCoefficient,
                                steelBasePart.Width * linearSizeCoefficient,
                                steelBasePart.Length * linearSizeCoefficient,
                                Math.Round(stressCoefficient * maxBedStress, 3),
                                Math.Round(stressCoefficient * maxStress, 3),
                                Math.Round(linearSizeCoefficient * recomendedThickness, 3)
            };
                dataTable.Rows.Add(newSteelBasePart);
            }

        }
        /// <summary>
        /// Добавление в датасет данных по болту стальной базы
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        private void ProcessBolt(SteelBase steelBase)
        {
            DataTable dataTable = dataSet.Tables["SteelBasesBolts"];
            foreach (SteelBolt steelBolt in steelBase.ActualSteelBolts)
            {
                DataRow newSteelBaseBolt = dataTable.NewRow();
                double area = steelBolt.Diameter * steelBolt.Diameter * 0.785;
                double maxStress = SteelBoltProcessor.GetMaxStressNonLinear(steelBolt);
                double force = maxStress * area;
                newSteelBaseBolt.ItemArray = new object[]
                {steelBolt.Id, steelBase.Id, steelBolt.Name,
                                steelBolt.Diameter * linearSizeCoefficient,
                                steelBolt.CenterX * linearSizeCoefficient,
                                steelBolt.CenterY * linearSizeCoefficient,
                                Math.Round(maxStress * stressCoefficient, 3),
                                Math.Round(force * forceCoefficient, 3) };
                dataTable.Rows.Add(newSteelBaseBolt);
            }
        }

    }
}