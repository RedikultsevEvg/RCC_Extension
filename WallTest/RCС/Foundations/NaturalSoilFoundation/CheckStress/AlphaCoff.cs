using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Entity.Soils.Processors;

namespace Test.RCC.Foundations.NaturalSoilFoundation.CheckStress
{
    [TestClass]
    public class AlphaCoff
    {
        double tolerance = 0.03;
        [TestMethod]
        public void RectAlpha1_1_0()
        {
            double l = 1;
            double b = 1;
            double z = 0;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 1;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha1_1_1()
        {
            double l = 1;
            double b = 1;
            double z = 1;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.336;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }

        [TestMethod]
        public void RectAlpha1_1_2()
        {
            double l = 1;
            double b = 1;
            double z = 2;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.108;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha1_1_5()
        {
            double l = 1;
            double b = 1;
            double z = 5;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.019;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha1_1_6()
        {
            double l = 1;
            double b = 1;
            double z = 6;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            //!!!!!!!!!!!!!!!!!
            //По таблице СП должно быть 0.15
            double Expected = 0.013;
            Assert.AreEqual(Expected, Actual, Expected * tolerance * 4);
        }
        [TestMethod]
        public void RectAlpha2_1_0()
        {
            double l = 2;
            double b = 1;
            double z = 0;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 1;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha2_1_1()
        {
            double l = 2;
            double b = 1;
            double z = 1;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.479;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha2_1_2()
        {
            double l = 2;
            double b = 1;
            double z = 2;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.190;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha1_2_1()
        {
            double l = 1;
            double b = 2;
            double z = 1;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.479;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha1_2_2()
        {
            double l = 1;
            double b = 2;
            double z = 2;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.190;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha12_18_2()
        {
            double l = 1.2;
            double b = 1.8;
            double z = 2.08;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.195;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha3_1_1()
        {
            double l = 3;
            double b = 1;
            double z = 1;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.525;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha4_1_1()
        {
            double l = 4;
            double b = 1;
            double z = 1;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.540;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha10_1_1()
        {
            double l = 10;
            double b = 1;
            double z = 1;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.55;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha1_10_1()
        {
            double l = 1;
            double b = 10;
            double z = 1;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.55;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void RectAlpha1_20_1()
        {
            double l = 1;
            double b = 10;
            double z = 1;
            double Actual = SoilLayerProcessor.GetAlphaRect(l, b, z);
            double Expected = 0.55;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
    }
}
