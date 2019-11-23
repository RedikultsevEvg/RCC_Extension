using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;

namespace Test.SC.SteelBasePart.CheckMoment
{
    [TestClass]
    public class TwoSides
    {
        [TestMethod]
        //Две противоположные стороны закрепление сверху и снизу
        public void OpositeSides()
        {
            double width = 0.2;
            double length = 0.4;
            double maxStress = 10000000;

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = -100000;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.CenterX = 0;
            basePart.CenterY = 0;
            basePart.FixLeft = false;
            basePart.FixRight = false;
            basePart.FixTop = true;
            basePart.FixBottom = true;
            SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

            double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
            double Expected = (maxStress / 1 / 1) * length * length / 8;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Две противоположные стороны закрепление справа и слева
        public void OpositeSides2()
        {
            double width = 0.2;
            double length = 0.4;
            double maxStress = 10000000;

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = -100000;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.CenterX = 0;
            basePart.CenterY = 0;
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = false;
            basePart.FixBottom = false;
            SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

            double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
            double Expected = (maxStress / 1 / 1) * width * width / 8;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Две противоположные стороны закрепление слева и снизу
        public void AdjacentSidesLeftBottom()
        {
            double force = -10000000;
            double baseWidth = 1;
            double baseLength = 1;
            double width = 0.2;
            double length = 0.4;
            double maxStress = force * (-1D);

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = force;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.CenterX = 0;
            basePart.CenterY = 0;
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

            double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
            double Expected = 120000;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Две противоположные стороны закрепление слева и снизу
        public void AdjacentSidesLeftBottom2()
        {
            double force = -10000000;
            double baseWidth = 1;
            double baseLength = 1;
            double width = 0.2;
            double length = 0.6;
            double maxStress = force * (-1D);

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = force;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.CenterX = 0;
            basePart.CenterY = 0;
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

            double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
            double Expected = 240000;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Две противоположные стороны закрепление слева и снизу
        public void AdjacentSidesLeftBottom3()
        {
            double force = -10000000;
            double baseWidth = 1;
            double baseLength = 1;
            double width = 0.2;
            double length = 0.8;
            double maxStress = force * (-1D);

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = force;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.CenterX = 0;
            basePart.CenterY = 0;
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

            double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
            double Expected = 408000;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Две противоположные стороны закрепление справа и сверху
        public void AdjacentSidesRightTop()
        {
            double force = -10000000;
            double baseWidth = 1;
            double baseLength = 1;
            double width = 0.4;
            double length = 0.2;
            double maxStress = force * (-1D);

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = force;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.CenterX = 0;
            basePart.CenterY = 0;
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

            double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
            double Expected = 120000;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Две противоположные стороны закрепление справа и сверху
        public void AdjacentSidesRightTop2()
        {
            double force = -10000000;
            double baseWidth = 1;
            double baseLength = 1;
            double width = 0.6;
            double length = 0.2;
            double maxStress = force * (-1D);

            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = baseWidth;
            steelColumnBase.Length = baseLength;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.KindId = 1;
            forceParameter.CrcValue = force;
            loadSet.PartialSafetyFactor = 1;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.CenterX = 0;
            basePart.CenterY = 0;
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;
            SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

            double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
            double Expected = 240000;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
    }
}
