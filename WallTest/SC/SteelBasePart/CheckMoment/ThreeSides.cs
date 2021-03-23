using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;

namespace Test.SC.SteelBasePart.CheckMoment
{

    //[TestClass]
    //public class ThreeSides
    //{
    //    #region ThreeSides1
    //    [TestMethod]
    //    //Три стороны свободная сверху
    //    public void CheckMomentPartThreeSides1()
    //    {
    //        double force = -10000000;
    //        double baseWidth = 1;
    //        double baseLength = 1;
    //        double width = 0.2;
    //        double length = 0.3;
    //        double maxStress = force * (-1D);

    //        SteelBase steelColumnBase = new SteelBase();
    //        steelColumnBase.Width = baseWidth;
    //        steelColumnBase.Length = baseLength;
    //        steelColumnBase.Thickness = 0.05;

    //        LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
    //        ForceParameter forceParameter = new ForceParameter();
    //        loadSet.ForceParameters.Add(forceParameter);
    //        forceParameter.KindId = 1;
    //        forceParameter.CrcValue = force;
    //        loadSet.PartialSafetyFactor = 1;

    //        RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
    //        basePart.Width = width;
    //        basePart.Length = length;
    //        basePart.CenterX = 0;
    //        basePart.CenterY = 0;
    //        basePart.FixLeft = true;
    //        basePart.FixRight = true;
    //        basePart.FixTop = false;
    //        basePart.FixBottom = true;
    //        SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

    //        double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
    //        double Expected = 50800;

    //        Assert.AreEqual(Actual, Expected, Expected / 1000);
    //    }
    //    #endregion
    //    #region ThreeSides1a
    //    [TestMethod]
    //    //Три стороны свободная справа
    //    public void CheckMomentPartThreeSides1a()
    //    {
    //        double force = -10000000;
    //        double baseWidth = 1;
    //        double baseLength = 1;
    //        double width = 0.3;
    //        double length = 0.2;
    //        double maxStress = force * (-1D);

    //        SteelBase steelColumnBase = new SteelBase();
    //        steelColumnBase.Width = baseWidth;
    //        steelColumnBase.Length = baseLength;
    //        steelColumnBase.Thickness = 0.05;

    //        LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
    //        ForceParameter forceParameter = new ForceParameter();
    //        loadSet.ForceParameters.Add(forceParameter);
    //        forceParameter.KindId = 1;
    //        forceParameter.CrcValue = force;
    //        loadSet.PartialSafetyFactor = 1;

    //        RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
    //        basePart.Width = width;
    //        basePart.Length = length;
    //        basePart.CenterX = 0;
    //        basePart.CenterY = 0;
    //        basePart.FixLeft = true;
    //        basePart.FixRight = false;
    //        basePart.FixTop = true;
    //        basePart.FixBottom = true;
    //        SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

    //        double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
    //        double Expected = 50800;

    //        Assert.AreEqual(Actual, Expected, Expected / 1000);
    //    }
    //    #endregion
    //    #region ThreeSides2
    //    [TestMethod]
    //    //Три стороны свободная сверху с большим соотношением сторон,
    //    //таким образом, чтобы считалось по шарнирной схеме
    //    public void CheckMomentPartThreeSides2()
    //    {
    //        double force = -10000000;
    //        double baseWidth = 1;
    //        double baseLength = 1;
    //        double width = 0.2;
    //        double length = 0.5;
    //        double maxStress = force * (-1D);

    //        SteelBase steelColumnBase = new SteelBase();
    //        steelColumnBase.Width = baseWidth;
    //        steelColumnBase.Length = baseLength;
    //        steelColumnBase.Thickness = 0.05;

    //        LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
    //        ForceParameter forceParameter = new ForceParameter();
    //        loadSet.ForceParameters.Add(forceParameter);
    //        forceParameter.KindId = 1;
    //        forceParameter.CrcValue = force;
    //        loadSet.PartialSafetyFactor = 1;

    //        RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
    //        basePart.Width = width;
    //        basePart.Length = length;
    //        basePart.CenterX = 0;
    //        basePart.CenterY = 0;
    //        basePart.FixLeft = true;
    //        basePart.FixRight = true;
    //        basePart.FixTop = false;
    //        basePart.FixBottom = true;
    //        SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

    //        double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
    //        double Expected = (maxStress / 1 / 1) * width * width / 8;

    //        Assert.AreEqual(Expected, Actual, Expected / 1000);
    //    }
    //    #endregion
    //    #region ThreeSides2a
    //    [TestMethod]
    //    //Три стороны свободная справа с большим соотношением сторон,
    //    //таким образом, чтобы считалось по шарнирной схеме
    //    public void CheckMomentPartThreeSides2a()
    //    {
    //        double force = -10000000;
    //        double baseWidth = 1;
    //        double baseLength = 1;
    //        double width = 0.5;
    //        double length = 0.2;
    //        double maxStress = force * (-1D);

    //        SteelBase steelColumnBase = new SteelBase();
    //        steelColumnBase.Width = baseWidth;
    //        steelColumnBase.Length = baseLength;
    //        steelColumnBase.Thickness = 0.05;

    //        LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
    //        ForceParameter forceParameter = new ForceParameter();
    //        loadSet.ForceParameters.Add(forceParameter);
    //        forceParameter.KindId = 1;
    //        forceParameter.CrcValue = force;
    //        loadSet.PartialSafetyFactor = 1;

    //        RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
    //        basePart.Width = width;
    //        basePart.Length = length;
    //        basePart.CenterX = 0;
    //        basePart.CenterY = 0;
    //        basePart.FixLeft = true;
    //        basePart.FixRight = false;
    //        basePart.FixTop = true;
    //        basePart.FixBottom = true;
    //        SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

    //        double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
    //        double Expected = (maxStress / 1 / 1) * length * length / 8;

    //        Assert.AreEqual(Expected, Actual, Expected / 1000);
    //    }
    //    #endregion
    //    #region ThreeSides3
    //    [TestMethod]
    //    //Три стороны свободная сверху с большим соотношением сторон,
    //    //таким образом, чтобы считалось по консольной схеме
    //    public void CheckMomentPartThreeSides3()
    //    {
    //        double force = -10000000;
    //        double baseWidth = 1;
    //        double baseLength = 1;
    //        double width = 0.5;
    //        double length = 0.2;
    //        double maxStress = force * (-1D);

    //        SteelBase steelColumnBase = new SteelBase();
    //        steelColumnBase.Width = baseWidth;
    //        steelColumnBase.Length = baseLength;
    //        steelColumnBase.Thickness = 0.05;

    //        LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
    //        ForceParameter forceParameter = new ForceParameter();
    //        loadSet.ForceParameters.Add(forceParameter);
    //        forceParameter.KindId = 1;
    //        forceParameter.CrcValue = force;
    //        loadSet.PartialSafetyFactor = 1;

    //        RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
    //        basePart.Width = width;
    //        basePart.Length = length;
    //        basePart.CenterX = 0;
    //        basePart.CenterY = 0;
    //        basePart.FixLeft = true;
    //        basePart.FixRight = true;
    //        basePart.FixTop = false;
    //        basePart.FixBottom = true;
    //        SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

    //        double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
    //        double Expected = (maxStress / 1 / 1) * length * length / 2;

    //        Assert.AreEqual(Expected, Actual, Expected / 1000);
    //    }
    //    #endregion
    //    #region ThreeSides3a
    //    [TestMethod]
    //    //Три стороны свободная справа с большим соотношением сторон,
    //    //таким образом, чтобы считалось по консольной схеме
    //    public void CheckMomentPartThreeSides3a()
    //    {
    //        double force = -10000000;
    //        double baseWidth = 1;
    //        double baseLength = 1;
    //        double width = 0.2;
    //        double length = 0.5;
    //        double maxStress = force * (-1D);

    //        SteelBase steelColumnBase = new SteelBase();
    //        steelColumnBase.Width = baseWidth;
    //        steelColumnBase.Length = baseLength;
    //        steelColumnBase.Thickness = 0.05;

    //        LoadSet loadSet = steelColumnBase.ForcesGroups[0].LoadSets[0];
    //        ForceParameter forceParameter = new ForceParameter();
    //        loadSet.ForceParameters.Add(forceParameter);
    //        forceParameter.KindId = 1;
    //        forceParameter.CrcValue = force;
    //        loadSet.PartialSafetyFactor = 1;

    //        RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart(steelColumnBase);
    //        basePart.Width = width;
    //        basePart.Length = length;
    //        basePart.CenterX = 0;
    //        basePart.CenterY = 0;
    //        basePart.FixLeft = true;
    //        basePart.FixRight = false;
    //        basePart.FixTop = true;
    //        basePart.FixBottom = true;
    //        SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

    //        double Actual = SteelBasePartProcessor.GetResult(basePart, maxStress)[0];
    //        double Expected = (maxStress / 1 / 1) * width * width / 2;

    //        Assert.AreEqual(Expected, Actual, Expected / 1000);
    //    }
    //    #endregion
    //}
}
