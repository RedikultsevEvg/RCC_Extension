using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;

namespace Test.SC.CheckSteelBase.CheckPressure
{
    [TestClass]
    public class Pressure
    {
        [TestMethod]
        //Расчет давления
        public void CheckMaxPressure1()
        {
            double N = -216 * 1e3;
            double M = 74.38 * 1e3;
            double baseWidth = 0.4;
            double baseLength = 0.5;

            #region steelbase
            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].LoadSets[0];
            loadSet.ForceParameters.Clear();
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = N;
            ForceParameter forceParameter1 = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter1);
            forceParameter1.KindId = 2;
            forceParameter1.CrcValue = M;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = 0.2;
            basePart.Length = 0.08;
            basePart.CenterX = 0.1;
            basePart.CenterY = 0.21;
            basePart.FixLeft = false;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            basePart.AddSymmetricX = true;
            basePart.AddSymmetricY = true;

            RDBLL.Entity.SC.Column.SteelBasePart basePart1 = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart1.Width = 0.2;
            basePart1.Length = 0.34;
            basePart1.CenterX = 0.1;
            basePart1.CenterY = 0;
            basePart1.FixLeft = true;
            basePart1.FixRight = false;
            basePart1.FixTop = true;
            basePart1.FixBottom = true;
            basePart1.AddSymmetricX = false;
            basePart1.AddSymmetricY = true;

            steelColumnBase.SteelBaseParts.Clear();
            steelColumnBase.SteelBaseParts.Add(basePart);
            steelColumnBase.SteelBaseParts.Add(basePart1);

            SteelBolt steelBolt = new SteelBolt(steelColumnBase);
            steelBolt.Diameter = 0.03;
            steelBolt.CenterX = 0.075;
            steelBolt.CenterY = 0.22;
            steelBolt.AddSymmetricX = true;
            steelBolt.AddSymmetricY = true;
            steelColumnBase.SteelBolts.Clear();
            steelColumnBase.SteelBolts.Add(steelBolt);

            SteelBaseProcessor.SolveSteelColumnBase(steelColumnBase);
            #endregion

            double maxStress = N / (baseWidth * baseLength) - M / (baseWidth * baseLength * baseLength / 6);
            double minStress = N / (baseWidth * baseLength) + M / (baseWidth * baseLength * baseLength / 6);
            double x = baseLength * maxStress / (maxStress - minStress);
            double zs = baseLength / 2 - x / 3 + 0.22;
            double zn = baseLength / 2 - x / 3;
            double nb = (N * zn + M) / zs;
            double Actual = SteelBasePartProcessor.GetGlobalMinStressNonLinear(steelColumnBase.ActualSteelBaseParts[1]);
            double Expected = maxStress;
            Assert.AreEqual(Expected, Actual, (-1.0) * Expected / 100);
        }
        [TestMethod]
        //Расчет давления
        public void CheckMaxPressure2()
        {
            double N = -95000;
            double M = 83140;
            double baseWidth = 0.4;
            double baseLength = 0.5;

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].LoadSets[0];
            loadSet.ForceParameters.Clear();
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = N;
            ForceParameter forceParameter1 = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter1);
            forceParameter1.KindId = 2;
            forceParameter1.CrcValue = M;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = 0.2;
            basePart.Length = 0.08;
            basePart.CenterX = 0.1;
            basePart.CenterY = 0.21;
            basePart.FixLeft = false;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            basePart.AddSymmetricX = true;
            basePart.AddSymmetricY = true;

            RDBLL.Entity.SC.Column.SteelBasePart basePart1 = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart1.Width = 0.2;
            basePart1.Length = 0.34;
            basePart1.CenterX = 0.1;
            basePart1.CenterY = 0;
            basePart1.FixLeft = true;
            basePart1.FixRight = false;
            basePart1.FixTop = true;
            basePart1.FixBottom = true;
            basePart1.AddSymmetricX = false;
            basePart1.AddSymmetricY = true;

            steelColumnBase.SteelBaseParts.Clear();
            steelColumnBase.SteelBaseParts.Add(basePart);
            steelColumnBase.SteelBaseParts.Add(basePart1);

            SteelBolt steelBolt = new SteelBolt(steelColumnBase);
            steelBolt.Diameter = 0.03;
            steelBolt.CenterX = 0.075;
            steelBolt.CenterY = 0.22;
            steelBolt.AddSymmetricX = true;
            steelBolt.AddSymmetricY = true;
            steelColumnBase.SteelBolts.Clear();
            steelColumnBase.SteelBolts.Add(steelBolt);

            SteelBaseProcessor.SolveSteelColumnBase(steelColumnBase);


            double maxStress = N / (baseWidth * baseLength) - M / (baseWidth * baseLength * baseLength / 6);
            double minStress = N / (baseWidth * baseLength) + M / (baseWidth * baseLength * baseLength / 6);
            double x = baseLength * maxStress / (maxStress - minStress);
            double zs = baseLength / 2 - x / 3 + 0.22;
            double zn = baseLength / 2 - x / 3;
            double nb = (N * zn + M) / zs;
            double Actual = SteelBasePartProcessor.GetGlobalMinStressNonLinear(steelColumnBase.ActualSteelBaseParts[1]);
            double Expected = maxStress;
            Assert.AreEqual(Expected, Actual, (-1.0) * Expected / 100);
        }
        [TestMethod]
        //Расчет усилий в болтах
        public void CheckBoltForce1()
        {
            double N = -216 * 1e3;
            double M = 74.38 * 1e3;
            double baseWidth = 0.4;
            double baseLength = 0.5;

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].LoadSets[0];
            loadSet.ForceParameters.Clear();
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = N;
            ForceParameter forceParameter1 = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter1);
            forceParameter1.KindId = 2;
            forceParameter1.CrcValue = M;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = 0.2;
            basePart.Length = 0.08;
            basePart.CenterX = 0.1;
            basePart.CenterY = 0.21;
            basePart.FixLeft = false;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            basePart.AddSymmetricX = true;
            basePart.AddSymmetricY = true;

            RDBLL.Entity.SC.Column.SteelBasePart basePart1 = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart1.Width = 0.2;
            basePart1.Length = 0.34;
            basePart1.CenterX = 0.1;
            basePart1.CenterY = 0;
            basePart1.FixLeft = true;
            basePart1.FixRight = false;
            basePart1.FixTop = true;
            basePart1.FixBottom = true;
            basePart1.AddSymmetricX = false;
            basePart1.AddSymmetricY = true;

            steelColumnBase.SteelBaseParts.Clear();
            steelColumnBase.SteelBaseParts.Add(basePart);
            steelColumnBase.SteelBaseParts.Add(basePart1);

            SteelBolt steelBolt = new SteelBolt(steelColumnBase);
            steelBolt.Diameter = 0.03;
            steelBolt.CenterX = 0.075;
            steelBolt.CenterY = 0.22;
            steelBolt.AddSymmetricX = true;
            steelBolt.AddSymmetricY = true;
            steelColumnBase.SteelBolts.Clear();
            steelColumnBase.SteelBolts.Add(steelBolt);

            SteelBaseProcessor.SolveSteelColumnBase(steelColumnBase);


            double maxStress = N / (baseWidth * baseLength) - M / (baseWidth * baseLength * baseLength / 6);
            double minStress = N / (baseWidth * baseLength) + M / (baseWidth * baseLength * baseLength / 6);
            double x = baseLength * maxStress / (maxStress - minStress);
            double zs = baseLength / 2 - x / 3 + 0.22;
            double zn = baseLength / 2 - x / 3;
            double nb = (N * zn + M) / zs;
            double Actual = SteelBoltProcessor.GetMaxStressNonLinear(steelColumnBase.ActualSteelBolts[0]) * 0.03 * 0.03 * 0.785;
            double Expected = nb / 2;
            Assert.AreEqual(Expected, Actual, Expected / 50);
        }
        [TestMethod]
        //Расчет усилий в болтах
        public void CheckBoltForce2()
        {
            double N = -95000;
            double M = 83140;
            double baseWidth = 0.4;
            double baseLength = 0.5;

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].LoadSets[0];
            loadSet.ForceParameters.Clear();
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = N;
            ForceParameter forceParameter1 = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter1);
            forceParameter1.KindId = 2;
            forceParameter1.CrcValue = M;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = 0.2;
            basePart.Length = 0.08;
            basePart.CenterX = 0.1;
            basePart.CenterY = 0.21;
            basePart.FixLeft = false;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            basePart.AddSymmetricX = true;
            basePart.AddSymmetricY = true;

            RDBLL.Entity.SC.Column.SteelBasePart basePart1 = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart1.Width = 0.2;
            basePart1.Length = 0.34;
            basePart1.CenterX = 0.1;
            basePart1.CenterY = 0;
            basePart1.FixLeft = true;
            basePart1.FixRight = false;
            basePart1.FixTop = true;
            basePart1.FixBottom = true;
            basePart1.AddSymmetricX = false;
            basePart1.AddSymmetricY = true;

            steelColumnBase.SteelBaseParts.Clear();
            steelColumnBase.SteelBaseParts.Add(basePart);
            steelColumnBase.SteelBaseParts.Add(basePart1);

            SteelBolt steelBolt = new SteelBolt(steelColumnBase);
            steelBolt.Diameter = 0.03;
            steelBolt.CenterX = 0.075;
            steelBolt.CenterY = 0.22;
            steelBolt.AddSymmetricX = true;
            steelBolt.AddSymmetricY = true;
            steelColumnBase.SteelBolts.Clear();
            steelColumnBase.SteelBolts.Add(steelBolt);

            SteelBaseProcessor.SolveSteelColumnBase(steelColumnBase);


            double maxStress = N / (baseWidth * baseLength) - M / (baseWidth * baseLength * baseLength / 6);
            double minStress = N / (baseWidth * baseLength) + M / (baseWidth * baseLength * baseLength / 6);
            double x = baseLength * maxStress / (maxStress - minStress);
            double zs = baseLength / 2 - x / 3 + 0.22;
            double zn = baseLength / 2 - x / 3;
            double nb = (N * zn + M) / zs;
            double Actual = SteelBoltProcessor.GetMaxStressNonLinear(steelColumnBase.ActualSteelBolts[0]) * 0.03 * 0.03 * 0.785;
            double Expected = nb / 2;
            Assert.AreEqual(Expected, Actual, Expected / 50);
        }
    }
}
