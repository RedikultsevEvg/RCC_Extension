using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.BuildingAndSite.Factories;
using RDBLL.Entity.RCC.Reinforcements.Ancorages;
using RDBLL.Entity.RCC.Reinforcements.Ancorages.Factories;
using TestIntegrationProject.Infrastracture.Factories;

namespace TestIntegrationProject.Examples.RCC.Reinforcement.Ancorages
{
    [TestClass]
    public class AncorageExample1
    {
        [TestMethod]
        public void SaveAncorageOneBar()
        {
            ProgrammSettings.InicializeNew();
            string filePath = FilePathFactory.GetFilePath(PathType.Ancorage);
            filePath += "\\Анкеровка_1.xml";
            ProgrammSettings.FilePath = filePath;
            IAncorage ancorage = PrepareAncorageOneBar();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            BuildingSite buildingSite = BuildingProcessor.GetBuildingSite(ancorage.ParentMember as Level);
            ProgrammSettings.BuildingSite = buildingSite;
            buildingSite.DeleteFromDataSet(dataSet);
            buildingSite.SaveToDataSet(dataSet, true);
            //Сохраняем проект в файл
            ProgrammSettings.SaveProjectToFile(false);
            //Открываем проект из файла
            ProgrammSettings.OpenExistDataset(filePath);
        }

        [TestMethod]
        public void SaveAncorageMultyBar()
        {
            ProgrammSettings.InicializeNew();
            string filePath = FilePathFactory.GetFilePath(PathType.Ancorage);
            filePath += "\\Анкеровка_2.xml";
            ProgrammSettings.FilePath = filePath;
            IAncorage ancorage = PrepareAncorageMultyBar();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            BuildingSite buildingSite = BuildingProcessor.GetBuildingSite(ancorage.ParentMember as Level);
            ProgrammSettings.BuildingSite = buildingSite;
            buildingSite.DeleteFromDataSet(dataSet);
            buildingSite.SaveToDataSet(dataSet, true);
            //Сохраняем проект в файл
            ProgrammSettings.SaveProjectToFile(false);
            //Открываем проект из файла
            ProgrammSettings.OpenExistDataset(filePath);
        }

        private IAncorage PrepareAncorageOneBar()
        {
            #region Building
            BuildingSite buildingSite = new BuildingSite(true);
            Building building = BuildingFactory.GetBuilding(BuildingType.SimpleType);
            building.RegisterParent(buildingSite);
            Level level = LevelFactory.GetLevel(LevelType.SimpleType);
            level.RegisterParent(building);
            #endregion
            IAncorage ancorage;
            ancorage = AncorageFactory.GetAncorage(AncorageType.OneBar);
            ancorage.RegisterParent(level);
            return ancorage;
        }

        private IAncorage PrepareAncorageMultyBar()
        {
            #region Building
            BuildingSite buildingSite = new BuildingSite(true);
            Building building = BuildingFactory.GetBuilding(BuildingType.SimpleType);
            building.RegisterParent(buildingSite);
            Level level = LevelFactory.GetLevel(LevelType.SimpleType);
            level.RegisterParent(building);
            #endregion
            IAncorage ancorage;
            ancorage = AncorageFactory.GetAncorage(AncorageType.MultyBar);
            ancorage.RegisterParent(level);
            return ancorage;
        }
    }
}
