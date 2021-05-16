using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Entity.RCC.Common.Processors;

namespace Test.RCC.Common
{
    [TestClass]
    public class RectCrossSection
    {
        double tolerance = 0.01;
        [TestMethod]
        public void LimitMoment1()
        {
            double b = 0.3;
            double h0 = 0.57;
            double As = 6.28e-4;
            double Rs = 3.5e8;
            double Rc = 1.7e7;
            double actual = RectSectionProcessor.GetUltMoment(b, h0, As, Rs, Rc);
            double expected = 120500;
            Assert.AreEqual(expected, actual, expected * tolerance);
        }
        [TestMethod]
        public void LimitMoment2()
        {
            double b = 0.3;
            double h0 = 0.57;
            double As = 6.28e-4 * 8;
            double Rs = 3.5e8;
            double Rc = 1.7e7;
            double actual = RectSectionProcessor.GetUltMoment(b, h0, As, Rs, Rc);
            double expected = 648000;
            Assert.AreEqual(expected, actual, expected * tolerance);
        }
        [TestMethod]
        public void Area1()
        {
            double M = 120500;
            double b = 0.3;
            double h0 = 0.57;
            double Rs = 3.5e8;
            double Rc = 1.7e7;
            double actual = RectSectionProcessor.GetReinforcementArea(M, b, h0, Rs, Rc);
            double expected = 6.24e-4;
            Assert.AreEqual(expected, actual, expected * tolerance);
        }
    }
}
