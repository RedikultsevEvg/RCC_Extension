using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.SC.Column.SteelBases.Builders;
using RDStartWPF.Views.SC.Columns.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RDStartWPF.Views.Common.Patterns.ControlClasses.Factories
{
    /// <summary>
    /// Класс для создания карточек паттернов стальной базы
    /// </summary>
    public static class SteelBaseCardFactory
    {
        public static List<PatternCard> GetCards(IDsSaveable parent)
        {
            string directory = Directory.GetCurrentDirectory() + "\\Images\\Patterns\\SC\\Bases\\";
            List<PatternCard> patternCards = new List<PatternCard>();
            PatternCard patternCard = new PatternCard();
            #region Default
            patternCard.Name = "Без паттерна";
            patternCard.RegisterParent(parent);
            patternCard.Description = "Создать базу по умолчанию";
            patternCard.ImageName = $"{directory}Default.png";
            patternCard.RegisterDelegate(new PatternCard.CommandDelegate(CreateSteelBaseDefault));
            patternCards.Add(patternCard);
            #endregion
            #region Pattern1
            patternCard = new PatternCard();
            patternCard.Name = "Паттерн база двутавра";
            patternCard.RegisterParent(parent);
            patternCard.Description = "Создать базу двутавровой колонны с одним подкрепляющим ребром";
            patternCard.ImageName = $"{directory}Pattern1.png";
            patternCard.RegisterDelegate(new PatternCard.CommandDelegate(CreateSteelBasePattern1));
            patternCards.Add(patternCard);
            #endregion
            #region Pattern2
            patternCard = new PatternCard();
            patternCard.Name = "Паттерн база двутавра";
            patternCard.RegisterParent(parent);
            patternCard.Description = "Создать базу двутавровой колонны с опциональным расположением ребер";
            patternCard.ImageName = $"{directory}Pattern2.png";
            patternCard.RegisterDelegate(new PatternCard.CommandDelegate(CreateSteelBasePattern2));
            patternCards.Add(patternCard);
            #endregion
            #region Pattern3
            patternCard = new PatternCard();
            patternCard.Name = "Паттерн база двухветвевой колонны";
            patternCard.RegisterParent(parent);
            patternCard.Description = "Создать базу двухветвевой колонны с опциональным расположением ребер";
            patternCard.ImageName = $"{directory}Pattern3.png";
            patternCard.RegisterDelegate(new PatternCard.CommandDelegate(CreateSteelBasePattern3));
            patternCards.Add(patternCard);
            #endregion
            return patternCards;
        }

        public static void CreateSteelBaseDefault(IDsSaveable parent)
        {
            BuilderBase builder = new BuilderTempate1();
            CreateSteelBase(builder, parent);
        }
        public static void CreateSteelBasePattern1(IDsSaveable parent)
        {
            BuilderBase builder = new BuilderPattern1();
            CreateSteelBase(builder, parent);
        }
        public static void CreateSteelBasePattern2(IDsSaveable parent)
        {
            BuilderBase builder = new BuilderPattern2();
            CreateSteelBase(builder, parent);
        }
        public static void CreateSteelBasePattern3(IDsSaveable parent)
        {
            BuilderBase builder = new BuilderPattern3();
            CreateSteelBase(builder, parent);
        }
        private static void CreateSteelBase(BuilderBase builder, IDsSaveable parent)
        {
            SteelBase steelBase = BaseMaker.MakeSteelBase(builder);
            steelBase.RegisterParent(parent);
            steelBase.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
            WndSteelColumnBase wndSteelColumnBase = new WndSteelColumnBase(steelBase);
            wndSteelColumnBase.ShowDialog();
            if (wndSteelColumnBase.DialogResult == true)
            {
                try
                {
                    steelBase.SaveToDataSet(ProgrammSettings.CurrentDataSet, false);
                    steelBase.IsActual = false;
                    if (steelBase.Pattern != null) steelBase.Pattern.GetBaseParts();
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения :" + ex);
                }
            }
            else
            {
                steelBase.UnRegisterParent();
                steelBase.DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
            }
        }
    }
}
