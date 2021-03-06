﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;

namespace Test.SC.SteelBasePart.CheckMoment
{

    [TestClass]
    public class ThreeSides
    {
        #region ThreeSides1
        [TestMethod]
        //Три стороны свободная сверху
        public void CheckMomentPartThreeSides1()
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
            basePart.FixTop = false;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = 50800;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }
        #endregion
        #region ThreeSides1a
        [TestMethod]
        //Три стороны свободная справа
        public void CheckMomentPartThreeSides1a()
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
            basePart.FixRight = false;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = 50800;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }
        #endregion
        #region ThreeSides2
        [TestMethod]
        //Три стороны свободная сверху с большим соотношением сторон,
        //таким образом, чтобы считалось по шарнирной схеме
        public void CheckMomentPartThreeSides2()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.2;
            double length = 0.5;
            double maxStress = force * (-1D);

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = false;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = (maxStress / 1 / 1) * width * width / 8;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        #endregion
        #region ThreeSides2a
        [TestMethod]
        //Три стороны свободная справа с большим соотношением сторон,
        //таким образом, чтобы считалось по шарнирной схеме
        public void CheckMomentPartThreeSides2a()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.5;
            double length = 0.2;
            double maxStress = force * (-1D);

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = (maxStress / 1 / 1) * length * length / 8;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        #endregion
        #region ThreeSides3
        [TestMethod]
        //Три стороны свободная сверху с большим соотношением сторон,
        //таким образом, чтобы считалось по консольной схеме
        public void CheckMomentPartThreeSides3()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.5;
            double length = 0.2;
            double maxStress = force * (-1D);

            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = false;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = (maxStress / 1 / 1) * length * length / 2;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        #endregion
        #region ThreeSides3a
        [TestMethod]
        //Три стороны свободная справа с большим соотношением сторон,
        //таким образом, чтобы считалось по консольной схеме
        public void CheckMomentPartThreeSides3a()
        {
            ProgrammSettings.InicializeNew();
            double force = -10000000;
            double width = 0.2;
            double length = 0.5;
            double maxStress = force * (-1D);
            
            RDBLL.Entity.SC.Column.SteelBasePart basePart = new RDBLL.Entity.SC.Column.SteelBasePart();
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center.X = 0;
            basePart.Center.Y = 0;
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            double Actual = SteelBasePartProcessor.GetMoment(basePart, maxStress);
            double Expected = (maxStress / 1 / 1) * width * width / 2;

            Assert.AreEqual(Expected, Actual, Expected / 1000);
        }
        #endregion
    }
}
