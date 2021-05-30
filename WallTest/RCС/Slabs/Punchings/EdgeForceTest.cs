using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings.Factories;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors;

namespace Test.RCС.Slabs.Punchings
{
    [TestClass]
    public class EdgeForceTest
    {
        [TestMethod]
        public void ForceOnlyTestEdge1()
        {
            double Nz = 400000;
            double Mx = 0;
            double My = 0;

            ProgrammSettings.InicializeNew();
            Punching punching = TestCaseFactory.GetPunching(PunchingType.Edge1);
            ILayerProcessor layerProcessor = new MultiLayersProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double actual = bearingProcessor.GetBearingCapacityCoefficient(contours[0], Nz, Mx, My, true);
            double expected = 1.509;
            //Проверка количества расчетных контуров
            Assert.AreEqual(1, contours.Count);
            //Проверка коэффициента несущей способности
            Assert.AreEqual(expected, actual, expected * 0.01);
        }
        [TestMethod]
        public void ForceOnlyTestEdge1Offset250()
        {
            double Nz = 400000;
            double Mx = 0;
            double My = 0;

            ProgrammSettings.InicializeNew();
            Punching punching = TestCaseFactory.GetPunching(PunchingType.Edge1_offset250);
            ILayerProcessor layerProcessor = new MultiLayersProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();

            double actual = bearingProcessor.GetBearingCapacityCoefficient(contours[0], Nz, Mx, My, true);
            double expected = 1.1373;
            //Проверка количества расчетных контуров
            Assert.AreEqual(2, contours.Count);
            //Проверка коэффициента несущей способности
            Assert.AreEqual(expected, actual, expected * 0.01);
            actual = bearingProcessor.GetBearingCapacityCoefficient(contours[1], Nz, Mx, My, true);
            expected = 1.0216;
            //Проверка коэффициента несущей способности
            Assert.AreEqual(expected, actual, expected * 0.01);
        }
        [TestMethod]
        public void ForceMomentTestEdge1Offset250()
        {
            double Nz = 400000;
            double Mx = 20000;
            double My = 0;

            ProgrammSettings.InicializeNew();
            Punching punching = TestCaseFactory.GetPunching(PunchingType.Edge1_offset250);
            ILayerProcessor layerProcessor = new MultiLayersProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();

            double actual = bearingProcessor.GetBearingCapacityCoefficient(contours[0], Nz, Mx, My, true);
            double expected = 1.645;
            //Проверка количества расчетных контуров
            Assert.AreEqual(2, contours.Count);
            //Проверка коэффициента несущей способности
            Assert.AreEqual(expected, actual, expected * 0.01);
            actual = bearingProcessor.GetBearingCapacityCoefficient(contours[1], Nz, Mx, My, true);
            expected = 1.5323;
            //Проверка коэффициента несущей способности
            Assert.AreEqual(expected, actual, expected * 0.01);
        }
        [TestMethod]
        public void ForceMomentTestEdge2Offset250()
        {
            double Nz = 400000;
            double Mx = -20000;
            double My = 0;

            ProgrammSettings.InicializeNew();
            Punching punching = TestCaseFactory.GetPunching(PunchingType.Edge1_offset250);
            ILayerProcessor layerProcessor = new MultiLayersProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();

            double actual = bearingProcessor.GetBearingCapacityCoefficient(contours[0], Nz, Mx, My, true);
            double expected = 1.706;
            //Проверка количества расчетных контуров
            Assert.AreEqual(2, contours.Count);
            //Проверка коэффициента несущей способности
            Assert.AreEqual(expected, actual, expected * 0.01);
            actual = bearingProcessor.GetBearingCapacityCoefficient(contours[1], Nz, Mx, My, true);
            expected = 1.5323;
            //Проверка коэффициента несущей способности
            Assert.AreEqual(expected, actual, expected * 0.01);
        }
    }
}
