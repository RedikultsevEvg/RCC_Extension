using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column.SteelBases;
using RDBLL.Entity.SC.Column.SteelBases.Factories;

namespace Test.SC.SteelBases
{
    [TestClass]
    public class PartGroup
    {
        [TestMethod]
        public void SavePartGroup()
        {
            ProgrammSettings.InicializeNew();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            SteelBasePartGroup partGroup = GroupFactory.GetSteelBasePartGroup(GroupType.Type1);
            partGroup.SaveToDataSet(dataSet, true);
        }
        [TestMethod]
        public void CloneGroup()
        {
            ProgrammSettings.InicializeNew();
            SteelBasePartGroup partGroup = GroupFactory.GetSteelBasePartGroup(GroupType.Type1);
            SteelBasePartGroup newPartGroup = partGroup.Clone() as SteelBasePartGroup;
            Assert.AreNotSame(partGroup, newPartGroup);
        }
    }
}
