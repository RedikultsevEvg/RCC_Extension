using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.SC.Column.SteelBases.Builders;
using RDBLL.Entity.SC.Column.SteelBases.Factories;

namespace Test.SC.SteelBases
{
    [TestClass]
    public class SteelBaseFactory
    {
        [TestMethod]
        public void Patternt1()
        {
            ProgrammSettings.InicializeNew();
            BuilderBase builder = new BuilderPattern1();
            SteelBase steelBase = BaseMaker.MakeSteelBase(builder);
            steelBase.Pattern.GetBaseParts();
            Assert.AreEqual(6, steelBase.SteelBaseParts.Count, 0);
        }
        [TestMethod]
        public void Patternt2()
        {
            ProgrammSettings.InicializeNew();
            BuilderBase builder = new BuilderPattern2();
            SteelBase steelBase = BaseMaker.MakeSteelBase(builder);
            steelBase.Pattern.GetBaseParts();
            Assert.AreEqual (10, steelBase.SteelBaseParts.Count, 0);
        }
        [TestMethod]
        public void Patternt3()
        {
            ProgrammSettings.InicializeNew();
            BuilderBase builder = new BuilderPattern3();
            SteelBase steelBase = BaseMaker.MakeSteelBase(builder);
            steelBase.Pattern.GetBaseParts();
            Assert.AreEqual(9, steelBase.SteelBaseParts.Count, 0);
        }
    }
}
