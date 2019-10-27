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

        [TestMethod, Timeout(300)]
        
        ///
        public void CheckSteelBasePart()
        {
            SteelBase steelColumnBase = new SteelBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;
            LoadSet loadSet = steelColumnBase.LoadsGroup[0].LoadSets[0];
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.CrcValue = -100000;
            loadSet.PartialSafetyFactor = 1;
            SteelBasePart basePart = new SteelBasePart(steelColumnBase);
            basePart.FixLeft = true;
            basePart.FixRight = true;
            basePart.FixTop = false;
            basePart.FixBottom = false;
            basePart.Width = 1;
            basePart.Length = 1;
            SteelBaseProcessor.ActualizeLoadCases(steelColumnBase);

            double Actual = SteelBasePartProcessor.GetResult(basePart)[1];
            Assert.AreEqual(30, Actual / 1000000, 10);
        }

        [TestMethod, Timeout(300)]
        ///Проверка работоспособности инстумента создания комбинации нагрузок
        /// 
        public void CheckLoadCaseOnlyDeadLoad()
        {
            SteelBase steelColumnBase = new SteelBase();
            int i = 1;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, false);
            
            i = 2;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, false);

            List<LoadSet> ExpLoadCases = new List<LoadSet>();

            List<LoadSet> ActualList = LoadSetProcessor.GetLoadCases(steelColumnBase.LoadsGroup);
            List<LoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0] = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet.ForceParameters[0].CrcValue = -200000; //Продольная сила
            expLoadSet.ForceParameters[0].DesignValue = -220000; //Продольная сила
            expLoadSet.IsLiveLoad = false;
            expLoadSet.PartialSafetyFactor = 1.1;
            #endregion

            bool actual = CompareLoadList(ActualList, ExpectedList);
            bool expected = true;

            Assert.AreEqual(actual, expected);
        }

        [TestMethod, Timeout(300)]
        ///Проверка работоспособности инстумента создания комбинации нагрузок
        /// 
        public void CheckLoadCaseLiveLoad()
        {
            SteelBase steelColumnBase = new SteelBase();
            int i = 1;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, false);

            i = 2;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, true);

            List<LoadSet> ExpLoadCases = new List<LoadSet>();

            List<LoadSet> ActualList = LoadSetProcessor.GetLoadCases(steelColumnBase.LoadsGroup);
            List<LoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0] = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            expLoadSet.ForceParameters[0].DesignValue = -110000; //Продольная сила
            expLoadSet.IsLiveLoad = false;
            expLoadSet.PartialSafetyFactor = 1.1;


            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet1 = new LoadSet();
            ExpectedList[1] = expLoadSet1;
            expLoadSet1.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet1.ForceParameters.Add(new ForceParameter());
            expLoadSet1.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet1.ForceParameters[0].CrcValue = -200000; //Продольная сила
            expLoadSet1.ForceParameters[0].DesignValue = -220000; //Продольная сила
            expLoadSet1.IsLiveLoad = false;
            expLoadSet1.PartialSafetyFactor = 1.1;
            #endregion

            bool actual = CompareLoadList(ActualList, ExpectedList);
            bool expected = true;

            Assert.AreEqual(actual, expected);
            Assert.AreEqual(ExpectedList[1].ForceParameters[0].CrcValue, ActualList[1].ForceParameters[0].CrcValue);
        }

        [TestMethod, Timeout(300)]
        ///Проверка работоспособности инстумента создания комбинации нагрузок
        /// 
        public void CheckLoadCaseLiveChangableLoad()
        {
            SteelBase steelColumnBase = new SteelBase();
            int i = 1;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i);

            i = 2;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, true, true);

            List<LoadSet> ExpLoadCases = new List<LoadSet>();

            List<LoadSet> ActualList = LoadSetProcessor.GetLoadCases(steelColumnBase.LoadsGroup);
            List<LoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0] = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            expLoadSet.ForceParameters[0].DesignValue = -110000; //Продольная сила

            expLoadSet.IsLiveLoad = false;
            expLoadSet.PartialSafetyFactor = 1.1;
            
            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet1 = new LoadSet();
            ExpectedList[1] = expLoadSet1;
            expLoadSet1.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet1.ForceParameters.Add(new ForceParameter());
            expLoadSet1.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet1.ForceParameters[0].CrcValue = -200000; //Продольная сила
            expLoadSet1.ForceParameters[0].DesignValue = -220000; //Продольная сила

            expLoadSet1.IsLiveLoad = true;
            expLoadSet1.PartialSafetyFactor = 1.1;

            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet2 = new LoadSet();
            ExpectedList[2] = expLoadSet2;
            expLoadSet2.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(-1)";
            expLoadSet2.ForceParameters.Add(new ForceParameter());
            expLoadSet2.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet2.ForceParameters[0].CrcValue = 0; //Продольная сила
            expLoadSet2.ForceParameters[0].DesignValue = 0; //Продольная сила
            expLoadSet2.IsLiveLoad = true;
            expLoadSet2.PartialSafetyFactor = 1.1;
            #endregion

            bool actual = CompareLoadList(ActualList, ExpectedList);
            bool expected = true;

            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Добавляет нагрузку в группу нагрузок
        /// </summary>
        /// <param name="forcesGroup"></param>
        /// <param name="i">Порядковый номер нагрузки</param>
        /// <param name="isLiveLoad">Флаг временной нагрузки</param>
        /// /// <param name="bothSign">Флаг знакопеременной нагрузки</param>
        public static void AddForceParameter (ForcesGroup forcesGroup, int i, bool isLiveLoad=false, bool bothSign=false)
        {
            LoadSet loadSet = new LoadSet();
            forcesGroup.LoadSets.Add(loadSet);
            loadSet.Name = $"New_load_{i}";
            loadSet.ForceParameters.Add(new ForceParameter());
            loadSet.ForceParameters[0].Kind_id = 1; //Продольная сила
            loadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            loadSet.IsLiveLoad = isLiveLoad;
            loadSet.BothSign = bothSign;
            loadSet.PartialSafetyFactor = 1.1;
        }

        /// <summary>
        /// Сравнивает два списка набора нагрузок
        /// </summary>
        /// <param name="frstList"></param>
        /// <param name="scndtList"></param>
        /// <returns></returns>
        public static bool CompareBarLoadList(List<BarLoadSet> frstList, List<BarLoadSet> scndtList)
        {
            if (!(scndtList.Count == frstList.Count)) { return false; } //Если количество не совпадает нет смысла сравнивать
            for (int i = 0; i < frstList.Count; i++)
            {
                if (! frstList[i].Equals(scndtList[i])) { return false; }
            }
            return true;
        }

        public static bool CompareLoadList(List<LoadSet> frstList, List<LoadSet> scndtList)
        {
            if (!(scndtList.Count == frstList.Count)) { return false; } //Если количество не совпадает нет смысла сравнивать
            for (int i = 0; i < frstList.Count; i++)
            {
                if (!frstList[i].Equals(scndtList[i])) { return false; }
            }
            return true;
        }
    }
}
