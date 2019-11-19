using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Geometry;
using RDBLL.Entity.SC.Column;
using RDBLL.Processors.SC;
using RDBLL.Forces;
using RDBLL.Entity.Results.SC;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RDBLL.Processors.Forces;

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

        //[TestMethod, Timeout(300)]        
        ////
        //public void CheckSteelBasePart()
        //{
        //    SteelBase steelColumnBase = new SteelBase();
        //    steelColumnBase.Width = 1;
        //    steelColumnBase.Length = 1;
        //    steelColumnBase.Thickness = 0.05;
        //    LoadSet loadSet = steelColumnBase.LoadsGroup[0].LoadSets[0];
        //    ForceParameter forceParameter = new ForceParameter();
        //    loadSet.ForceParameters.Add(forceParameter);
        //    forceParameter.KindId = 1;
        //    forceParameter.CrcValue = -100000;
        //    loadSet.PartialSafetyFactor = 1;
        //    SteelBasePart basePart = new SteelBasePart(steelColumnBase);
        //    basePart.FixLeft = true;
        //    basePart.FixRight = true;
        //    basePart.FixTop = false;
        //    basePart.FixBottom = false;
        //    basePart.Width = 1;
        //    basePart.Length = 1;
        //    SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

        //    double Actual = SteelBasePartProcessor.GetResult(basePart)[1];
        //    Assert.AreEqual(30, Actual / 1000000, 10);
        //}
    }
}
