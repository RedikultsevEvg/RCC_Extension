using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Geometry;

namespace Test.Geometry.Mathematic
{
    [TestClass]
    public class MathTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            double d = 123.456789012;
            double expected = 123.46;
            double actual = MathOperation.Round(d, 5);
            Assert.AreEqual(expected, actual, d*0.0001);
        }
        [TestMethod]
        public void TestMethod2()
        {
            double d = 10000;
            double expected = 10000;
            double actual = MathOperation.Round(d);
            Assert.AreEqual(expected, actual, d * 0.0001);
        }
        [TestMethod]
        public void TestMethod3()
        {
            double d = 0.0000123456789;
            double expected = 0.0000123456;
            double actual = MathOperation.Round(d, 5);
            Assert.AreEqual(expected, actual, d * 0.0001);
        }
        [TestMethod]
        public void TestMethod4()
        {
            double d = -0.0000123456789;
            double expected = -0.0000123456;
            double actual = MathOperation.Round(d, 5);
            Assert.AreEqual(expected, actual, d * (-0.0001));
        }
    }
}
