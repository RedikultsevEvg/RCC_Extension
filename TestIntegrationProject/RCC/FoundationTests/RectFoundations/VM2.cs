using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Forces;
using RDBLL.Processors.Forces;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.Soils;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Foundations.Processors;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Foundations.Builders;
using System.Data;
using RDBLL.Common.Service.DsOperations.Factory;
using RDBLL.Entity.Soils.Factories;
using CSL.Reports.Interfaces;
using CSL.Reports.RCC.Slabs.Punchings;
using CSL.Reports;

namespace TestIntegrationProject.RCC.FoundationTests.RectFoundations
{
    [TestClass]
    public class VM2
    {
        private double tolerance = 0.01;
        private Foundation _Foundation;
        [TestMethod]
        public void SettlementTest1()
        {
            //Тестирование максимальной осадки
            double expectedValue = 0.00763;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = (-1D) * _Foundation.Result.MaxSettlement;
            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        [TestMethod]
        public void CompressionHight()
        {
            //Тестирование глубины сжимаемой толщи
            double expectedValue = 2.00;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = _Foundation.Result.CompressHeight;
            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        [TestMethod] //Тестирование сохранения в датасет
        public void SaveTest()
        {
            ProgrammSettings.InicializeNew();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            SolveFoundation();
            BuildingSite buildingSite = BuildingProcessor.GetBuildingSite(_Foundation);
            buildingSite.DeleteFromDataSet(dataSet);
            buildingSite.SaveToDataSet(dataSet, true);
            buildingSite.OpenFromDataSet(dataSet);
        }

        [TestMethod] //Тестирование клонирования фундамента
        public void CloneTest()
        {
            ProgrammSettings.InicializeNew();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            SolveFoundation();
            Foundation foundation = _Foundation.Clone() as Foundation;
        }

        //Тестирование подготовки отчета
        [TestMethod]
        public void ReportFoundationTest()
        {
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            //Ссылка на строительный объект
            BuildingSite buildingSite = ProgrammSettings.BuildingSite;
            IReport report = new FoundationReport(buildingSite);
            report.PrepareReport();
            //Проверяем, что отчет не пустой
            Assert.IsNotNull(report);
            DataSet dataSet = report.dataSet;
            //Проверяем, что датасет не пустой
            Assert.IsNotNull(dataSet);
            //Проверяем количество таблиц в датасете
            int tableCount = dataSet.Tables.Count;
            Assert.AreEqual(15, tableCount);
        }

        [TestMethod] //Тестирование среднего давления под подошвой
        public void SndAvgStressTest()
        {
            double expectedValue = -101170;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = _Foundation.Result.MinSndAvgStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue, (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование краевого давления под подошвой
        public void SndMiddleStressTest()
        {
            double expectedValue = -250000;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = _Foundation.Result.MinSndMiddleStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue, (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование углового давления под подошвой
        public void SndCornerStressTest()
        {
            double expectedValue = -277500;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = _Foundation.Result.MinSndCornerStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue, (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование определения расчетного сопротивления
        public void SndResistanceTest1()
        {
            double expectedValue = 841000;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = _Foundation.Result.SndResistance;
            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        private Foundation PrepareFoundation()
        {
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            #region Building
            BuildingSite buildingSite = ProgrammSettings.BuildingSite;
            Building building = buildingSite.Children[0];
            building.RelativeLevel = 0.000;
            building.AbsoluteLevel = 260;
            building.IsRigid = false;
            Level level = new Level(building);
            level.SaveToDataSet(dataSet, true);
            #endregion
            #region Soil
            Soil soil = SoilFactory.GetSoil(buildingSite, FactorySoilType.FoundationVM2);
            soil.SaveToDataSet(dataSet, true);
            #endregion
            #region SoilSection
            SoilSection soilSection = new SoilSection(buildingSite);
            SoilLayer soilLayer = new SoilLayer(soil, soilSection, building);
            soilSection.Id = ProgrammSettings.CurrentId;
            soilLayer.TopLevel = 259.5;
            soilSection.SaveToDataSet(dataSet, true);
            #endregion
            #region Foundation
            BuilderBase builder = new BuilderVM2();
            Foundation foundation = FoundMaker.MakeFoundation(builder);
            foundation.RegisterParent(level);
            foundation.RelativeTopLevel = -0.400;
            foundation.SoilRelativeTopLevel = 0.000;
            foundation.SoilSectionUsing.SelectedId = soilSection.Id;
            foundation.SoilVolumeWeight = 20000;
            foundation.SaveToDataSet(dataSet, true);
            #endregion
            return foundation;
        }

        private void SolveFoundation()
        {
            _Foundation = PrepareFoundation();
            FoundationProcessor.SolveFoundation(_Foundation);
        }
    }
}
