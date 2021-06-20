using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Reinforcements.Ancorages;
using RDBLL.Entity.RCC.Reinforcements.Ancorages.Factories;
using RDBLL.Entity.RCC.Reinforcements.Ancorages.Repositories;

namespace Test.RCС.Reinforcement.Ancorages
{
    [TestClass]
    public class AncorageIOTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            ProgrammSettings.InicializeNew();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            IAncorage ancorage = AncorageFactory.GetAncorage(AncorageType.OneBar);
            IRepository<IAncorage> repository = new AncorageCRUD(dataSet);
            repository.Create(ancorage);
        }
    }
}
