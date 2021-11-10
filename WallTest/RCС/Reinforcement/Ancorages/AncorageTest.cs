using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Geometry.Sections;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Reinforcements.Ancorages;
using RDBLL.Entity.RCC.Reinforcements.Bars;
using System;

namespace Test.RCС.Reinforcement.Ancorages
{
    [TestClass]
    public class AncorageTest
    {
        [TestMethod]
        public void B10_A400_10()
        {
            double ds = 0.010;
            ProgrammSettings.InicializeNew();
            ICircle circle = new CircleSection() { Center = null, Diameter = ds};
            ConcreteUsing concrete = new ConcreteUsing();
            concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
            concrete.AddGammaB1();
            ReinforcementUsing reinforcement = new ReinforcementUsing();
            reinforcement.SelectedId = ProgrammSettings.ReinforcementKinds[0].Id;
            IBarSection barSection = new CircleBarSection(circle) { Reinforcement = reinforcement};
            IAncorageLogic ancorageLogic = new AncorageLogic();
            //Проверка длины анкеровки растянутой арматуры
            double expected = 0.675;
            double actual = ancorageLogic.GetAncorageLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины анкеровки сжатой арматуры
            expected = 0.506;
            actual = ancorageLogic.GetAncorageLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста растянутой арматуры с разбежкой
            expected = 0.810;
            actual = ancorageLogic.GetSimpleLappingLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста сжатой арматуры с разбежкой
            expected = 0.607;
            actual = ancorageLogic.GetSimpleLappingLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста растянутой арматуры в одном сечении
            expected = 1.349;
            actual = ancorageLogic.GetDoubleLappingLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста сжатой арматуры в одном сечении
            expected = 0.810;
            actual = ancorageLogic.GetDoubleLappingLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
        }
        [TestMethod]
        public void B10_A400_20()
        {
            double ds = 0.020;
            ProgrammSettings.InicializeNew();
            ICircle circle = new CircleSection() { Center = null, Diameter = ds };
            ConcreteUsing concrete = new ConcreteUsing();
            concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
            concrete.AddGammaB1();
            ReinforcementUsing reinforcement = new ReinforcementUsing();
            reinforcement.SelectedId = ProgrammSettings.ReinforcementKinds[0].Id;
            IBarSection barSection = new CircleBarSection(circle) {Reinforcement = reinforcement };
            IAncorageLogic ancorageLogic = new AncorageLogic();
            //Проверка длины анкеровки растянутой арматуры
            double expected = 1.349;
            double actual = ancorageLogic.GetAncorageLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины анкеровки сжатой арматуры
            expected = 1.012;
            actual = ancorageLogic.GetAncorageLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста растянутой арматуры с разбежкой
            expected = 1.619;
            actual = ancorageLogic.GetSimpleLappingLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста сжатой арматуры с разбежкой
            expected = 1.214;
            actual = ancorageLogic.GetSimpleLappingLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста растянутой арматуры в одном сечении
            expected = 2.698;
            actual = ancorageLogic.GetDoubleLappingLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста сжатой арматуры в одном сечении
            expected = 1.619;
            actual = ancorageLogic.GetDoubleLappingLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
        }
        [TestMethod]
        public void B10_A400_40()
        {
            double ds = 0.040;
            ProgrammSettings.InicializeNew();
            ICircle circle = new CircleSection() { Center = null, Diameter = ds };
            ConcreteUsing concrete = new ConcreteUsing();
            concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
            concrete.AddGammaB1();
            ReinforcementUsing reinforcement = new ReinforcementUsing();
            reinforcement.SelectedId = ProgrammSettings.ReinforcementKinds[0].Id;
            IBarSection barSection = new CircleBarSection(circle) {Reinforcement = reinforcement };
            IAncorageLogic ancorageLogic = new AncorageLogic();
            //Проверка длины анкеровки растянутой арматуры
            double expected = 2.998;
            double actual = ancorageLogic.GetAncorageLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины анкеровки сжатой арматуры
            expected = 2.249;
            actual = ancorageLogic.GetAncorageLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста растянутой арматуры с разбежкой
            expected = 3.598;
            actual = ancorageLogic.GetSimpleLappingLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста сжатой арматуры с разбежкой
            expected = 2.698;
            actual = ancorageLogic.GetSimpleLappingLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста растянутой арматуры в одном сечении
            expected = 5.996;
            actual = ancorageLogic.GetDoubleLappingLenth(concrete, barSection, 1.0, false, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
            //Проверка длины нахлеста сжатой арматуры в одном сечении
            expected = 3.598;
            actual = ancorageLogic.GetDoubleLappingLenth(concrete, barSection, 1.0, true, 1);
            Assert.AreEqual(expected, actual, expected * 0.01);
        }
    }
}
