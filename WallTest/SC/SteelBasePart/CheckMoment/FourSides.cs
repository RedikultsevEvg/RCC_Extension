using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;

namespace Test.SC.SteelBasePart.CheckMoment
{
    [TestClass]
    public class FourSides
    {
        [TestMethod]
        //Четыре стороны по контуру
        public void FourSides1()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.2;
            double length = 0.3;
            double maxStress = force * (-1D);

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = 32400;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Четыре стороны по контуру
        public void FourSides1a()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.3;
            double length = 0.2;
            double maxStress = force * (-1D);

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = 32400;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Четыре стороны по контуру
        //При соотношении сторон 1:2 момент равено 0,8 от балочного момента
        public void FourSides2()
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
            basePart.FixRight = true;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = 0.8 * ((maxStress / 1 / 1) * width * width / 8);

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        [TestMethod]
        //Четыре стороны по контуру
        //При соотношении сторон 1:3 момент равен балочному
        public void FourSides3()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.2;
            double length = 0.6;
            double maxStress = force * (-1D) / width / length;

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = (maxStress / 1 / 1) * width * width / 8;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
    }
}
