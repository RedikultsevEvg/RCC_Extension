using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Geometry;
using RDBLL.Entity.SC.Column;
using RDBLL.Processors.SC;
using RDBLL.Forces;
using RDBLL.Entity.Results.SC;


namespace WallTest
{
    [TestClass]
    public class wallTest
    {
        [TestMethod]
        public void CheckAreaWithoutOpenings()
        {
            double inter2number;
            inter2number = MathOperation.InterpolateNumber(0.9, 0.107, 1, 0.112, 1);
            Assert.AreEqual(0.112, inter2number);
        }

        [TestMethod, Timeout(300)]
        public void CheckSteelBasePart()
        {
            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;
            //LoadSet loadSet = new LoadSet();
            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.Value = -100000;
            loadSet.PartialSafetyFactor = 1;
            SteelBasePart steelBasePart = new SteelBasePart(steelColumnBase);
            steelBasePart.FixLeft = true;
            steelBasePart.FixRight = true;
            steelBasePart.FixTop = false;
            steelBasePart.FixBottom = false;
            steelBasePart.Width = 1;
            steelBasePart.Length = 1;
            SteelColumnBaseProcessor columBaseProcessor = new SteelColumnBaseProcessor();
            ColumnBaseResult columnResult = columBaseProcessor.GetResult(steelColumnBase);
            ColumnBasePartResult baseResult = SteelColumnBasePartProcessor.GetResult(steelBasePart);
            Assert.AreEqual(300, baseResult.MaxStress / 1000000, 10);
        }
    }
}
