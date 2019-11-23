using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;
using RDBLL.Forces;
using RDBLL.Processors.Forces;

namespace Test.Forces
{
    [TestClass]
    public class LoadCase
    {
        [TestMethod, Timeout(300)]
        ///Проверка работоспособности инстумента создания комбинации нагрузок
        /// 
        public void CheckLoadCaseOnlyDeadLoad()
        {
            SteelBase steelColumnBase = new SteelBase();
            int i = 1;
            AddForceParameter(steelColumnBase.ForcesGroups[0], i, false);

            i = 2;
            AddForceParameter(steelColumnBase.ForcesGroups[0], i, false);

            List<LoadSet> ExpLoadCases = new List<LoadSet>();

            List<LoadSet> ActualList = LoadSetProcessor.GetLoadCases(steelColumnBase.ForcesGroups);
            List<LoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0] = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].KindId = 1; //Продольная сила
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
            AddForceParameter(steelColumnBase.ForcesGroups[0], i, false);

            i = 2;
            AddForceParameter(steelColumnBase.ForcesGroups[0], i, true);

            List<LoadSet> ExpLoadCases = new List<LoadSet>();

            List<LoadSet> ActualList = LoadSetProcessor.GetLoadCases(steelColumnBase.ForcesGroups);
            List<LoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0] = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].KindId = 1; //Продольная сила
            expLoadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            expLoadSet.ForceParameters[0].DesignValue = -110000; //Продольная сила
            expLoadSet.IsLiveLoad = false;
            expLoadSet.PartialSafetyFactor = 1.1;


            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet1 = new LoadSet();
            ExpectedList[1] = expLoadSet1;
            expLoadSet1.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet1.ForceParameters.Add(new ForceParameter());
            expLoadSet1.ForceParameters[0].KindId = 1; //Продольная сила
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
            AddForceParameter(steelColumnBase.ForcesGroups[0], i);

            i = 2;
            AddForceParameter(steelColumnBase.ForcesGroups[0], i, true, true);

            List<LoadSet> ExpLoadCases = new List<LoadSet>();

            List<LoadSet> ActualList = LoadSetProcessor.GetLoadCases(steelColumnBase.ForcesGroups);
            List<LoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0] = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].KindId = 1; //Продольная сила
            expLoadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            expLoadSet.ForceParameters[0].DesignValue = -110000; //Продольная сила

            expLoadSet.IsLiveLoad = false;
            expLoadSet.PartialSafetyFactor = 1.1;

            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet1 = new LoadSet();
            ExpectedList[1] = expLoadSet1;
            expLoadSet1.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet1.ForceParameters.Add(new ForceParameter());
            expLoadSet1.ForceParameters[0].KindId = 1; //Продольная сила
            expLoadSet1.ForceParameters[0].CrcValue = -200000; //Продольная сила
            expLoadSet1.ForceParameters[0].DesignValue = -220000; //Продольная сила

            expLoadSet1.IsLiveLoad = true;
            expLoadSet1.PartialSafetyFactor = 1.1;

            ExpectedList.Add(new LoadSet());
            LoadSet expLoadSet2 = new LoadSet();
            ExpectedList[2] = expLoadSet2;
            expLoadSet2.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(-1)";
            expLoadSet2.ForceParameters.Add(new ForceParameter());
            expLoadSet2.ForceParameters[0].KindId = 1; //Продольная сила
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
        public static void AddForceParameter(ForcesGroup forcesGroup, int i, bool isLiveLoad = false, bool bothSign = false)
        {
            LoadSet loadSet = new LoadSet();
            forcesGroup.LoadSets.Add(loadSet);
            loadSet.Name = $"New_load_{i}";
            loadSet.ForceParameters.Add(new ForceParameter());
            loadSet.ForceParameters[0].KindId = 1; //Продольная сила
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
                if (!frstList[i].Equals(scndtList[i])) { return false; }
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
