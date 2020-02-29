using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Forces;
using RDBLL.Processors.Forces;

namespace Test.Forces
{
    [TestClass]
    public class LoadSetTest
    {
        private double tolerance = 0.01; 

        [TestMethod] //Продольная сила
        public void LoadSetTransfromNzTest()
        {
            LoadSet expectedLoadSet = ExpectedLoadSet();
            double[] delta = new double[3] { 0, 0, -1.6 };

            LoadSet actualLoadSet = LoadSetProcessor.GetLoadSetTransform(PrepareLoadSet(), delta);
            double expectedValue = expectedLoadSet.ForceParameters[0].CrcValue;
            double actualValue = actualLoadSet.ForceParameters[0].CrcValue;

            Assert.AreEqual(expectedValue, actualValue, (-1D) * expectedValue * tolerance);
        }

        [TestMethod] //Момент Mx
        public void LoadSetTransfromMxTest()
        {
            LoadSet expectedLoadSet = ExpectedLoadSet();
            double[] delta = new double[3] { 0, 0, -1.6 };

            LoadSet actualLoadSet = LoadSetProcessor.GetLoadSetTransform(PrepareLoadSet(), delta);
            double expectedValue = expectedLoadSet.ForceParameters[1].CrcValue;
            double actualValue = actualLoadSet.ForceParameters[1].CrcValue;

            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        [TestMethod] //Момент My
        public void LoadSetTransfromMyTest()
        {
            LoadSet expectedLoadSet = ExpectedLoadSet();
            double[] delta = new double[3] { 0, 0, -1.6 };

            LoadSet actualLoadSet = LoadSetProcessor.GetLoadSetTransform(PrepareLoadSet(), delta);
            double expectedValue = expectedLoadSet.ForceParameters[2].CrcValue;
            double actualValue = actualLoadSet.ForceParameters[2].CrcValue;

            Assert.AreEqual(expectedValue, actualValue, expectedValue * tolerance);
        }

        private LoadSet PrepareLoadSet()
        {
            LoadSet loadSet = new LoadSet();
            ForceParameter forceParameter;
            forceParameter = new ForceParameter();
            forceParameter.KindId = 1; //Продольная сила
            forceParameter.CrcValue = -260000; //Продольная сила
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
            return loadSet;
        }
        private LoadSet ExpectedLoadSet()
        {
            LoadSet loadSet = new LoadSet();
            ForceParameter forceParameter;
            forceParameter = new ForceParameter();
            forceParameter.KindId = 1; //Продольная сила
            forceParameter.CrcValue = -260000; //Продольная сила
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 2; //Момент Mx
            forceParameter.CrcValue = 28000; //Момент Mx
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 3; //Момент My
            forceParameter.CrcValue = 241000; //Момент My
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 4; //Поперечная сила Qx
            forceParameter.CrcValue = 90000; //Поперечная сила Qx
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 5; //Поперечная сила Qy
            forceParameter.CrcValue = -5000; //Поперечная сила Qy
            loadSet.ForceParameters.Add(forceParameter);
            return loadSet;
        }
    }
}
