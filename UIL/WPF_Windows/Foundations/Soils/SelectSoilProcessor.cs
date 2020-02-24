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

namespace RDUIL.WPF_Windows.Foundations.Soils
{
    public static class SelectSoilProcessor
    {
        public static void SelectSoils(BuildingSite buildingSite)
        {
            string directory = Directory.GetCurrentDirectory() + "\\Images\\Soils\\";
            List<SoilCard> soilCards = new List<SoilCard>();
            #region ClaySoil
            SoilCard soilCard = new SoilCard();
            soilCard.Name = "Глинистый грунт";
            soilCard.BuildingSite = buildingSite;
            soilCard.Description = "Задать глинистый грунт";
            soilCard.ImageName = $"{directory}ClaySoil.png";
            soilCard.RegisterDelegate(new SoilCard.CommandDelegate(CreateClaySoil));
            soilCards.Add(soilCard);
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
            DispersedSoil dispersedSoil = new DispersedSoil(buildingSite);
            createSoil(buildingSite, dispersedSoil);
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
