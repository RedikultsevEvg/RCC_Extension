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
            SteelColumnBase steelColumnBase = new SteelColumnBase();
            steelColumnBase.Width = 1;
            steelColumnBase.Length = 1;
            steelColumnBase.Thickness = 0.05;
            //LoadSet loadSet = new LoadSet();
            LoadSet loadSet = steelColumnBase.LoadsGroup[0].Loads[0].LoadSet;
            ForceParameter forceParameter = new ForceParameter();
            loadSet.ForceParameters.Add(forceParameter);
            forceParameter.Kind_id = 1;
            forceParameter.CrcValue = -100000;
            loadSet.PartialSafetyFactor = 1;
            SteelBasePart steelBasePart = new SteelBasePart(steelColumnBase);
            steelBasePart.FixLeft = true;
            steelBasePart.FixRight = true;
            steelBasePart.FixTop = false;
            steelBasePart.FixBottom = false;
            steelBasePart.Width = 1;
            steelBasePart.Length = 1;
            ColumnBaseResult columnResult = SteelColumnBaseProcessor.GetResult(steelColumnBase);
            ColumnBasePartResult baseResult = SteelColumnBasePartProcessor.GetResult(steelBasePart);
            Assert.AreEqual(30, baseResult.MaxStress / 1000000, 10);
        }

        [TestMethod, Timeout(300)]
        ///Проверка работоспособности инстумента создания комбинации нагрузок
        /// 
        public void CheckLoadCaseOnlyDeadLoad()
        {
            SteelColumnBase steelColumnBase = new SteelColumnBase();
            int i = 1;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, true);
            
            i = 2;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, true);

            List<BarLoadSet> ExpLoadCases = new List<BarLoadSet>();

            List<BarLoadSet> ActualList = BarLoadSetProcessor.GetLoadCases(steelColumnBase.LoadsGroup);
            List<BarLoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new BarLoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0].LoadSet = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet.ForceParameters[0].CrcValue = -200000; //Продольная сила
            expLoadSet.ForceParameters[0].DesignValue = -220000; //Продольная сила
            expLoadSet.IsDeadLoad = false;
            expLoadSet.PartialSafetyFactor = 1.1;
            #endregion

            bool actual = CompareBarLoadList(ActualList, ExpectedList);
            bool expected = true;

            Assert.AreEqual(actual, expected);
        }

        [TestMethod, Timeout(300)]
        ///Проверка работоспособности инстумента создания комбинации нагрузок
        /// 
        public void CheckLoadCaseLiveLoad()
        {
            SteelColumnBase steelColumnBase = new SteelColumnBase();
            int i = 1;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, true);

            i = 2;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, false);

            List<BarLoadSet> ExpLoadCases = new List<BarLoadSet>();

            List<BarLoadSet> ActualList = BarLoadSetProcessor.GetLoadCases(steelColumnBase.LoadsGroup);
            List<BarLoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new BarLoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0].LoadSet = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            expLoadSet.ForceParameters[0].DesignValue = -110000; //Продольная сила
            expLoadSet.IsDeadLoad = false;
            expLoadSet.PartialSafetyFactor = 1.1;
            

            ExpectedList.Add(new BarLoadSet());
            LoadSet expLoadSet1 = new LoadSet();
            ExpectedList[1].LoadSet = expLoadSet1;
            expLoadSet1.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet1.ForceParameters.Add(new ForceParameter());
            expLoadSet1.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet1.ForceParameters[0].CrcValue = -200000; //Продольная сила
            expLoadSet1.ForceParameters[0].DesignValue = -220000; //Продольная сила
            expLoadSet1.IsDeadLoad = false;
            expLoadSet1.PartialSafetyFactor = 1.1;
            #endregion

            bool actual = CompareBarLoadList(ActualList, ExpectedList);
            bool expected = true;

            Assert.AreEqual(actual, expected);
        }

        [TestMethod, Timeout(300)]
        ///Проверка работоспособности инстумента создания комбинации нагрузок
        /// 
        public void CheckLoadCaseLiveChangableLoad()
        {
            SteelColumnBase steelColumnBase = new SteelColumnBase();
            int i = 1;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i);

            i = 2;
            AddForceParameter(steelColumnBase.LoadsGroup[0], i, false, true);

            List<BarLoadSet> ExpLoadCases = new List<BarLoadSet>();

            List<BarLoadSet> ActualList = BarLoadSetProcessor.GetLoadCases(steelColumnBase.LoadsGroup);
            List<BarLoadSet> ExpectedList = ExpLoadCases;
            #region //Свойства ожидаемой комбинации нагрузок
            ExpectedList.Add(new BarLoadSet());
            LoadSet expLoadSet = new LoadSet();
            ExpectedList[0].LoadSet = expLoadSet;
            expLoadSet.Name = "Новая нагрузка*(1) + New_load_1*(1)";
            expLoadSet.ForceParameters.Add(new ForceParameter());
            expLoadSet.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            expLoadSet.ForceParameters[0].DesignValue = -110000; //Продольная сила

            expLoadSet.IsDeadLoad = false;
            expLoadSet.PartialSafetyFactor = 1.1;
            
            ExpectedList.Add(new BarLoadSet());
            LoadSet expLoadSet1 = new LoadSet();
            ExpectedList[1].LoadSet = expLoadSet1;
            expLoadSet1.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(1)";
            expLoadSet1.ForceParameters.Add(new ForceParameter());
            expLoadSet1.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet1.ForceParameters[0].CrcValue = -200000; //Продольная сила
            expLoadSet1.ForceParameters[0].DesignValue = -220000; //Продольная сила

            expLoadSet1.IsDeadLoad = false;
            expLoadSet1.PartialSafetyFactor = 1.1;

            ExpectedList.Add(new BarLoadSet());
            LoadSet expLoadSet2 = new LoadSet();
            ExpectedList[2].LoadSet = expLoadSet2;
            expLoadSet2.Name = "Новая нагрузка*(1) + New_load_1*(1) + New_load_2*(-1)";
            expLoadSet2.ForceParameters.Add(new ForceParameter());
            expLoadSet2.ForceParameters[0].Kind_id = 1; //Продольная сила
            expLoadSet2.ForceParameters[0].CrcValue = 0; //Продольная сила
            expLoadSet2.ForceParameters[0].DesignValue = 0; //Продольная сила
            expLoadSet2.IsDeadLoad = false;
            expLoadSet2.PartialSafetyFactor = 1.1;
            #endregion

            bool actual = CompareBarLoadList(ActualList, ExpectedList);
            bool expected = true;

            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Добавляет нагрузку в группу нагрузок
        /// </summary>
        /// <param name="forcesGroup"></param>
        /// <param name="i"></param>
        /// <param name="isDeadLoad"></param>
        public static void AddForceParameter (ForcesGroup forcesGroup, int i, bool isDeadLoad=true, bool bothSign=false)
        {
            BarLoadSet barLoadSet = new BarLoadSet();
            LoadSet loadSet = new LoadSet();
            barLoadSet.LoadSet = loadSet;
            forcesGroup.Loads.Add(barLoadSet);
            loadSet.Name = $"New_load_{i}";
            loadSet.ForceParameters.Add(new ForceParameter());
            loadSet.ForceParameters[0].Kind_id = 1; //Продольная сила
            loadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            loadSet.IsDeadLoad = isDeadLoad;
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
    }
}
