using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Entity.RCC.Foundations.Processors;

namespace Test.RC.Foundations.NaturalSoilFoundation.CheckStress
{
    [TestClass]
    public class MCofficients
    {
        double tolerance = 0.03;
        [TestMethod]
        public void MGamma_10()
        {
            double fi = 10;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[0];
            double Expected = 0.18;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void MQ_10()
        {
            double fi = 10;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[1];
            double Expected = 1.73;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void MC_10()
        {
            double fi = 10;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[2];
            double Expected = 4.17;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        #region fi = 20
        [TestMethod]
        public void MGamma_20()
        {
            double fi = 20;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[0];
            double Expected = 0.51;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void MQ_20()
        {
            double fi = 20;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[1];
            double Expected = 3.06;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void MC_20()
        {
            double fi = 20;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[2];
            double Expected = 5.66;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        #endregion
        #region fi = 45
        [TestMethod]
        public void MGamma_45()
        {
            double fi = 45;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[0];
            double Expected = 3.66;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void MQ_45()
        {
            double fi = 45;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[1];
            double Expected = 15.64;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        [TestMethod]
        public void MC_45()
        {
            double fi = 45;
            double Actual = FoundationProcessor.SndResistanceCoff(fi)[2];
            double Expected = 14.64;
            Assert.AreEqual(Expected, Actual, Expected * tolerance);
        }
        #endregion
    }
}
