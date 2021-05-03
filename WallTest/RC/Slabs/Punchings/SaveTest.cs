using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings.Factories;

namespace Test.RC.Slabs.Punchings
{
    [TestClass]
    public class SaveTest
    {
        [TestMethod]
        public void SavePunchingTest()
        {
            ProgrammSettings.InicializeNew();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            Punching newObj = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            newObj.SaveToDataSet(dataSet, true);
            newObj.SaveToDataSet(dataSet, false);
            newObj.OpenFromDataSet(dataSet);
            newObj.DeleteFromDataSet(dataSet);
        }
        [TestMethod]
        public void ClonePunchingTest()
        {
            ProgrammSettings.InicializeNew();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            Punching oldObj = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            Punching newObj = oldObj.Clone() as Punching;
            Assert.IsNotNull(newObj);
            Assert.AreNotSame(oldObj, newObj);
            Assert.AreEqual(oldObj, newObj);
        }
    }
}
