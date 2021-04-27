using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;

namespace Test.SC.SteelBasePart.CheckMoment
{
    [TestClass]
    public class OneSide
    {
        [TestMethod]
        //Одна сторона, закрепление слева
        public void OneSides1()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.2;
            double length = 0.4;
            double maxStress = force * (-1D);

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = false;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = (maxStress / 1 / 1) * width * width / 2;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Одна сторона, закрепление снизу
        public void CheckMomentPartOneSides2()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.2;
            double length = 0.4;
            double maxStress = force * (-1D);

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = false;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = (maxStress / 1 / 1) * length * length / 2;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
    }
}
