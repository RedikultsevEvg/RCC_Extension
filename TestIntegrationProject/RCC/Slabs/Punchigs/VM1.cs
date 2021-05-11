using System;
using System.Data;
using CSL.Reports;
using CSL.Reports.Interfaces;
using CSL.Reports.RCC.Slabs.Punchings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings.Factories;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors;

namespace TestIntegrationProject.RCC.Slabs.Punchigs
{
    [TestClass]
    public class VM1
    {
        private Punching _Punching; 
        [TestMethod] //Тестирование сохранения в датасет
        public void SaveTest()
        {
            ProgrammSettings.InicializeNew();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            SolvePunching();
            BuildingSite buildingSite = BuildingProcessor.GetBuildingSite(_Punching);
            buildingSite.DeleteFromDataSet(dataSet);
            buildingSite.SaveToDataSet(dataSet, true);
        }

        [TestMethod] //Тестирование сохранения в датасет
        public void SaveToFileTest()
        {
            ProgrammSettings.InicializeNew();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            SolvePunching();
            string filePath = "E:\\1.xml";
            ProgrammSettings.FilePath = filePath;
            //Сохраняем проект в файл
            ProgrammSettings.SaveProjectToFile(false);
            //Открываем проект из файла
            ProgrammSettings.OpenExistDataset(filePath);
        }

        [TestMethod]
        public void ReportPunchigTest()
        {
            //Инициализируем окружение программы
            ProgrammSettings.InicializeNew();
            //создаем необходимые объекты
            SolvePunching();
            //Ссылка на строительный объект
            BuildingSite buildingSite = ProgrammSettings.BuildingSite;
            IReport report = new PunchingReport(buildingSite);
            report.PrepareReport();
            //Проверяем, что отчет не пустой
            Assert.IsNotNull(report);
            DataSet dataSet = report.dataSet;
            //Проверяем, что датасет не пустой
            Assert.IsNotNull(dataSet);
            //Проверяем количество таблиц в датасете
            int tableCount = dataSet.Tables.Count;
            Assert.AreEqual(8, tableCount);
        }

        private Punching PreparePunching()
        {
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            #region Building
            BuildingSite buildingSite = ProgrammSettings.BuildingSite;
            Building building = buildingSite.Children[0];
            building.RelativeLevel = 0.000;
            building.AbsoluteLevel = 260;
            building.IsRigid = false;
            Level level = new Level(building);
            level.SaveToDataSet(dataSet, true);
            #endregion
            Punching newObj = TestCaseFactory.GetPunching(PunchingType.TestType1_400х400х200);
            newObj.RegisterParent(level);
            return newObj;
        }

        private void SolvePunching()
        {
            _Punching = PreparePunching();
            IBearingProcessor processor = new BearingProcessor();
            processor.CalcResult(_Punching);
        }
    }
}
