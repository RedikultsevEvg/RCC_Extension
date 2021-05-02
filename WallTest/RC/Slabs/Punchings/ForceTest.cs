using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings.Factories;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors;

namespace Test.RC.Slabs.Punchings
{
    [TestClass]
    public class ForceTest
    {
        [TestMethod]
        public void ForceOnlyTest1()
        {
            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, 400000, 0, 0, true);
                double expected = 1.095;
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, 400000, 0, 0, false);
                double expected = 1.216;
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
        }
        [TestMethod]
        public void ForceAndMomentTest1()
        {
            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, 400000, 20000, 20000, true);
                double expected = 1.6;
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, 400000, 20000, 20000, false);
                double expected = 1.8;
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
        }
    }
}
