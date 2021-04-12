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
using RDBLL.Entity.Soils.Factories;

namespace TestIntegrationProject.RCC.FoundationTests.RectFoundations
{
    [TestClass]
    public class VM1
    {
        private double tolerance = 0.01;
        private Foundation Foundation;
        [TestMethod] //Тестирование максимальной осадки
        public void SettlementTest1()
        {
            double expectedValue = 0.01687;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = (-1D) * Foundation.Result.MaxSettlement;
            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        [TestMethod] //Тестирование глубины сжимаемой толщи
        public void CompressionHeightTest()
        {
            double expectedValue = 2.10;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.CompressHeight;
            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        [TestMethod] //Тестирование среднего давления под подошвой
        public void SndAvgStressTest()
        {
            double expectedValue = -268000;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.MinSndAvgStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue, (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование краевого давления под подошвой
        public void SndMiddleStressTest()
        {
            double expectedValue = -380000;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.MinSndMiddleStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue,  (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование углового давления под подошвой
        public void SndCornerStressTest()
        {
            double expectedValue = -387000;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.MinSndCornerStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue,  (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование определения расчетного сопротивления
        public void SndResistanceTest1()
        {
            double expectedValue = 921000;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.SndResistance;
            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        private Foundation PrepareFoundation()
        {           
            #region Building
            BuildingSite buildingSite = new BuildingSite();
            Building building = new Building(buildingSite);
            building.RelativeLevel = 0.000;
            building.AbsoluteLevel = 260;
            building.AbsolutePlaningLevel = 259.5;
            building.IsRigid = false;
            buildingSite.Children.Add(building);
            Level level = new Level(building);
            building.Children.Add(level);
            #endregion
            #region Soil
            Soil soil = SoilFactory.GetSoil(buildingSite, FactorySoilType.FoundationVM1);
            #endregion
            #region SoilSection
            SoilSection soilSection = new SoilSection(buildingSite);
            SoilLayer soilLayer = new SoilLayer();
            soilSection.Id = ProgrammSettings.CurrentId;
            soilLayer.Soil = soil;
            soilLayer.TopLevel = 260;
            soilSection.SoilLayers.Add(soilLayer);
            #endregion
            #region Foundation
            BuilderBase builder = new BuilderVM1();
            Foundation foundation = FoundMaker.MakeFoundation(builder);
            foundation.RegisterParent(level);
            foundation.RelativeTopLevel = -0.200;
            foundation.SoilRelativeTopLevel = -0.200;
            foundation.SoilSectionUsing.SelectedId = soilSection.Id;
            foundation.SoilVolumeWeight = 19000;
            #endregion
            return foundation;
        }

        private void SolveFoundation()
        {
            Foundation = PrepareFoundation();
            FoundationProcessor.SolveFoundation(Foundation);
        }
    }
}
