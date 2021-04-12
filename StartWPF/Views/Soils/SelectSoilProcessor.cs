using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RDUIL.WPF_Windows.Foundations.Soils.ControlClasses;
using RDUIL.WPF_Windows.Foundations.Soils;
using RDBLL.Entity.Soils;
using RDBLL.Common.Service;
using System.Windows;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDStartWPF.Views.Soils
{
    public static class SelectSoilProcessor
    {
        public static void SelectSoils(BuildingSite buildingSite)
        {
            string directory = Directory.GetCurrentDirectory() + "\\Images\\Patterns\\Soils\\";
            List<SoilCard> soilCards = new List<SoilCard>();

            #region GravelSoil
            SoilCard gravelCard = new SoilCard();
            gravelCard.Name = "Крупнообломочный грунт";
            gravelCard.BuildingSite = buildingSite;
            gravelCard.Description = "Задать крупнообломочный грунт (валунный, щебенистый, дресвяный)";
            gravelCard.ImageName = $"{directory}GravelSoil.png";
            gravelCard.RegisterDelegate(new SoilCard.CommandDelegate(CreateGravelSoil));
            soilCards.Add(gravelCard);
            #endregion

            #region SandSoil
            SoilCard sandCard = new SoilCard();
            sandCard.Name = "Песчаный грунт";
            sandCard.BuildingSite = buildingSite;
            sandCard.Description = "Задать песчаный грунт";
            sandCard.ImageName = $"{directory}SandSoil.png";
            sandCard.RegisterDelegate(new SoilCard.CommandDelegate(CreateSandSoil));
            soilCards.Add(sandCard);
            #endregion

            #region ClaySoil
            SoilCard clayCard = new SoilCard();
            clayCard.Name = "Глинистый грунт";
            clayCard.BuildingSite = buildingSite;
            clayCard.Description = "Задать глинистый грунт (супеси, суглинки, глины)";
            clayCard.ImageName = $"{directory}ClaySoil.png";
            clayCard.RegisterDelegate(new SoilCard.CommandDelegate(CreateClaySoil));
            soilCards.Add(clayCard);
            #endregion

            #region ClaySoil
            SoilCard rockCard = new SoilCard();
            rockCard.Name = "Скальный грунт";
            rockCard.BuildingSite = buildingSite;
            rockCard.Description = "Задать скальный грунт";
            rockCard.ImageName = $"{directory}RockSoil.png";
            rockCard.RegisterDelegate(new SoilCard.CommandDelegate(CreateRockSoil));
            soilCards.Add(rockCard);
            #endregion

            WndSoilSelect wndSoilSelect = new WndSoilSelect(soilCards);
            wndSoilSelect.ShowDialog();
        }

        public static void CreateClaySoil(BuildingSite buildingSite)
        {
            ClaySoil soil = new ClaySoil(buildingSite);
            createSoil(buildingSite, soil);
        }

        public static void CreateSandSoil(BuildingSite buildingSite)
        {
            SandSoil soil = new SandSoil(buildingSite);
            createSoil(buildingSite, soil);
        }

        public static void CreateGravelSoil(BuildingSite buildingSite)
        {
            GravelSoil soil = new GravelSoil(buildingSite);
            createSoil(buildingSite, soil);
        }

        public static void CreateRockSoil(BuildingSite buildingSite)
        {
            RockSoil rockSoil = new RockSoil(buildingSite);
            createSoil(buildingSite, rockSoil);
        }

        private static void createSoil(BuildingSite buildingSite, Soil soil)
        {
            WndClaySoil wndSoil = new WndClaySoil(soil);
            wndSoil.ShowDialog();
            if (wndSoil.DialogResult == true)
            {
                try
                {
                    soil.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    ProgrammSettings.IsDataChanged = true;
                    buildingSite.Soils.Add(soil);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
            }
        }
    }
}
