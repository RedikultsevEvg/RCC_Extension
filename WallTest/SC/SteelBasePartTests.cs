﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.SC.Column;
using RDBLL.Processors.SC;
using RDBLL.Entity.Results.SC;
using RDBLL.Forces;

namespace WallTest.SC
{
    [TestClass]
    public class SteelBasePartTests
    {
        #region OneSides
        [TestMethod]
        //Одна сторона, закрепление слева
        public void CheckMomentPartOneSides()
        {
            double width = 0.2;
            double length = 0.4;

            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.Value = -100000;
            loadSet.PartialSafetyFactor = 1;

            SteelBasePart basePart = new SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center = new double[2] { 0, 0 };
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = false;

            ColumnBasePartResult result = SteelColumnBasePartProcessor.GetResult(basePart);
            double Actual = result.MaxMoment;
            double Expected = (100000 / 1 / 1) * width * width / 2;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }

        [TestMethod]
        //Одна сторона, закрепление снизу
        public void CheckMomentPartOneSides2()
        {
            double width = 0.2;
            double length = 0.4;

            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.Value = -100000;
            loadSet.PartialSafetyFactor = 1;

            SteelBasePart basePart = new SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center = new double[2] { 0, 0 };
            basePart.FixLeft = false;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;

            ColumnBasePartResult result = SteelColumnBasePartProcessor.GetResult(basePart);
            double Actual = result.MaxMoment;
            double Expected = (100000 / 1 / 1) * length * length / 2;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }
        #endregion
        #region TwoSides
        [TestMethod]
        //Две противоположные стороны закрепление сверху и снизу
        public void CheckMomentPartTwoOpositeSides()
        {
            double width = 0.2;
            double length = 0.4;

            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.Value = -100000;
            loadSet.PartialSafetyFactor = 1;

            SteelBasePart basePart = new SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center = new double[2] { 0, 0 };
            basePart.FixLeft = false;
            basePart.FixRight = false;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            ColumnBasePartResult result = SteelColumnBasePartProcessor.GetResult(basePart);
            double Actual = result.MaxMoment;
            double Expected = (100000/1/1) * length * length / 8;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }
        [TestMethod]
        //Две противоположные стороны закрепление справа и слева
        public void CheckMomentPartTwoOpositeSides2()
        {
            double width = 0.2;
            double length = 0.4;

            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.Value = -100000;
            loadSet.PartialSafetyFactor = 1;

            SteelBasePart basePart = new SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center = new double[2] { 0, 0 };
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = false;
            basePart.FixBottom = false;

            ColumnBasePartResult result = SteelColumnBasePartProcessor.GetResult(basePart);
            double Actual = result.MaxMoment;
            double Expected = (100000 / 1 / 1) * width * width / 8;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }
        [TestMethod]
        //Две противоположные стороны закрепление слева и снизу
        public void CheckMomentPartTwoAdjacentSides()
        {
            double width = 0.2;
            double length = 0.4;

            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.Value = -100000;
            loadSet.PartialSafetyFactor = 1;

            SteelBasePart basePart = new SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center = new double[2] { 0, 0 };
            basePart.FixLeft = true;
            basePart.FixRight = false;
            basePart.FixTop = false;
            basePart.FixBottom = true;

            ColumnBasePartResult result = SteelColumnBasePartProcessor.GetResult(basePart);
            double Actual = result.MaxMoment;
            double Expected = 1200;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }
        #endregion
        #region ThreeSides
        [TestMethod]
        //Три стороны свободная сверху
        public void CheckMomentPartThreeSides()
        {
            double width = 0.2;
            double length = 0.3;

            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.Value = -100000;
            loadSet.PartialSafetyFactor = 1;

            SteelBasePart basePart = new SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center = new double[2] { 0, 0 };
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = false;
            basePart.FixBottom = true;

            ColumnBasePartResult result = SteelColumnBasePartProcessor.GetResult(basePart);
            double Actual = result.MaxMoment;
            double Expected = 508;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }
        #endregion
        #region FourSides
        [TestMethod]
        //Четыре стороны по контуру
        public void CheckMomentPartFourSides()
        {
            double width = 0.2;
            double length = 0.3;

            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;

            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.Value = -100000;
            loadSet.PartialSafetyFactor = 1;

            SteelBasePart basePart = new SteelBasePart(steelColumnBase);
            basePart.Width = width;
            basePart.Length = length;
            basePart.Center = new double[2] { 0, 0 };
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = true;
            basePart.FixBottom = true;

            ColumnBasePartResult result = SteelColumnBasePartProcessor.GetResult(basePart);
            double Actual = result.MaxMoment;
            double Expected = 324;

            Assert.AreEqual(Actual, Expected, Expected / 1000);
        }
        #endregion

    }
}