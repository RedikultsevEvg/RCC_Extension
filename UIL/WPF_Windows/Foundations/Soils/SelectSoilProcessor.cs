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
            soilCard.Name = "Суглинок";
            //soilCard.SoilTypeName = "ClaySoil";
            soilCard.BuildingSite = buildingSite;
            soilCard.Description = "Задать вид грунта - Суглинок (дисперсный грунт)";
            soilCard.ImageName = $"{directory}ClaySoil.png";
            soilCard.RegisterDelegate(new SoilCard.CommandDelegate(CreateClaySoil));
            soilCards.Add(soilCard);
            #endregion

            WndSoilSelect wndSoilSelect = new WndSoilSelect(soilCards);
            wndSoilSelect.ShowDialog();
        }

        public static void CreateClaySoil(BuildingSite buildingSite)
        {
            DispersedSoil dispersedSoil = new DispersedSoil(buildingSite);

            WndClaySoil wndSoil = new WndClaySoil(dispersedSoil);
            wndSoil.ShowDialog();
            if (wndSoil.DialogResult == true)
            {
                try
                {
                    dispersedSoil.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    ProgrammSettings.IsDataChanged = true;
                    buildingSite.Soils.Add(dispersedSoil);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
            }
        }

        public static void CreateRockSoil(BuildingSite buildingSite)
        {
            RockSoil rockSoil = new RockSoil(buildingSite);

            //WndClaySoil wndSoil = new WndClaySoil(rockSoil);
            //wndSoil.ShowDialog();
            //if (wndSoil.DialogResult == true)
            //{
            //    try
            //    {
            //        rockSoil.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
            //        ProgrammSettings.IsDataChanged = true;
            //        buildingSite.Soils.Add(rockSoil);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Ошибка сохранения :" + ex);
            //    }
            //}
        }
    }
}
