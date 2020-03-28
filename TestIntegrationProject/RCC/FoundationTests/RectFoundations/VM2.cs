using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Forces;
using RDBLL.Processors.Forces;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.Soils;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Foundations.Processors;
using RDBLL.Common.Service;

namespace TestIntegrationProject.RCC.FoundationTests.RectFoundations
{
    [TestClass]
    public class VM2
    {
        private double tolerance = 0.01;
        private Foundation Foundation;
        [TestMethod] //Тестирование максимальной осадки
        public void SettlementTest1()
        {
            double expectedValue = 0.00763;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = (-1D) * Foundation.Result.MaxSettlement;
            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        [TestMethod] //Тестирование глубины сжимаемой толщи
        public void CompressionHeightTest()
        {
            double expectedValue = 2.00;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.CompressHeight;
            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        [TestMethod] //Тестирование среднего давления под подошвой
        public void SndAvgStressTest()
        {
            double expectedValue = -101170;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.MinSndAvgStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue, (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование краевого давления под подошвой
        public void SndMiddleStressTest()
        {
            double expectedValue = -250000;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.MinSndMiddleStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue, (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование углового давления под подошвой
        public void SndCornerStressTest()
        {
            double expectedValue = -277500;
            ProgrammSettings.InicializeNew();
            SolveFoundation();
            double actualValue = Foundation.Result.MinSndCornerStressesWithWeight;
            Assert.AreEqual(expectedValue, actualValue, (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Тестирование определения расчетного сопротивления
        public void SndResistanceTest1()
        {
            double expectedValue = 527700;
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
            building.IsRigid = false;
            buildingSite.Buildings.Add(building);
            Level level = new Level(building);
            building.Levels.Add(level);
            #endregion
            #region Soil
            DispersedSoil soil = new ClaySoil(buildingSite);
            soil.Name = "ИГЭ-1";
            soil.Description = "Суглинок песчанистый, тугопластичный";
            soil.CrcDensity = 1950;
            soil.FstDesignDensity = 1800;
            soil.SndDesignDensity = 1900;
            soil.CrcParticularDensity = 2700;
            soil.FstParticularDensity = 2700;
            soil.SndParticularDensity = 2700;
            soil.PorousityCoef = 0.7;
            soil.ElasticModulus = 15e6;
            soil.SndElasticModulus = 15e6;
            soil.PoissonRatio = 0.3;
            soil.CrcFi = 20;
            soil.FstDesignFi = 18;
            soil.SndDesignFi = 19;
            soil.CrcCohesion = 50000;
            soil.FstDesignCohesion = 47000;
            soil.SndDesignCohesion = 49000;
            soil.IsDefinedFromTest = true;
            #endregion
            #region SoilSection
            SoilSection soilSection = new SoilSection(buildingSite);
            SoilLayer soilLayer = new SoilLayer();
            soilSection.Id = 3;
            soilLayer.Soil = soil;
            soilLayer.TopLevel = 259.5;
            soilSection.SoilLayers.Add(soilLayer);
            #endregion
            #region Foundation
            Foundation foundation = new Foundation(level);
            foundation.RelativeTopLevel = -0.400;
            foundation.SoilRelativeTopLevel = 0.000;
            foundation.SoilSectionId = soilSection.Id;
            foundation.SoilSection = soilSection;
            foundation.SoilVolumeWeight = 20000;
            #endregion
            #region Parts
            RectFoundationPart foundationPart;
            foundationPart = new RectFoundationPart(foundation);
            foundationPart.Width = 0.6;
            foundationPart.Length = 0.6;
            foundationPart.Height = 1.3;
            foundation.Parts.Add(foundationPart);
            foundationPart = new RectFoundationPart(foundation);
            foundationPart.Width = 2.4;
            foundationPart.Length = 1.8;
            foundationPart.Height = 0.3;
            foundation.Parts.Add(foundationPart);
            #endregion
            #region Forces
            ForcesGroup forcesGroup = foundation.ForcesGroups[0];
            //Постоянная нагрузка
            LoadSet loadSet = new LoadSet(forcesGroup);
            loadSet.Name = "Постоянная";
            loadSet.PartialSafetyFactor = 1.1;
            ForceParameter forceParameter;
            forceParameter = new ForceParameter();
            forceParameter.KindId = 1; //Продольная сила
            forceParameter.CrcValue = -290000; //Продольная сила
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 2; //Момент Mx
            forceParameter.CrcValue = 20000; //Момент Mx
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 3; //Момент My
            forceParameter.CrcValue = 97000; //Момент My
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 4; //Поперечная сила Qx
            forceParameter.CrcValue = 90000; //Поперечная сила Qx
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 5; //Поперечная сила Qy
            forceParameter.CrcValue = -5000; //Поперечная сила Qy
            loadSet.ForceParameters.Add(forceParameter);
            forcesGroup.LoadSets.Add(loadSet);
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
