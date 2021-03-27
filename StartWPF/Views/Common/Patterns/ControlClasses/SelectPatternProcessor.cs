using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.SC.Column.SteelBases.Builders;
using RDBLL.Entity.SC.Column.SteelBases.Patterns;
using RDStartWPF.Views.SC.Columns.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RDStartWPF.Views.Common.Patterns.ControlClasses
{
    public static class SelectPatternProcessor
    {
        public static List<PatternCard> SelectSteelBasePattern(IDsSaveable parent)
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
            patternCard.ImageName = $"{directory}Default.png";
            patternCard.RegisterDelegate(new PatternCard.CommandDelegate(CreateSteelBasePattern2));
            patternCards.Add(patternCard);
            #endregion
            return patternCards;
        }

        public static void CreateSteelBaseDefault(IDsSaveable parent)
        {
            BuilderTempate1 builder = new BuilderTempate1();
            SteelBase steelBase = BaseMaker.MakeSteelBase(builder);
            steelBase.RegisterParent(parent);
            CreateSteelBase(steelBase);
        }
        public static void CreateSteelBasePattern1(IDsSaveable parent)
        {
            SteelBase steelBase = new SteelBase(parent);
            PatternBase Pattern = new PatternType1(true);
            Pattern.RegisterParent(steelBase);
            steelBase.Pattern = Pattern;
            CreateSteelBase(steelBase);
        }
        public static void CreateSteelBasePattern2(IDsSaveable parent)
        {
            SteelBase steelBase = new SteelBase(parent);
            PatternBase Pattern = new PatternType2(true);
            Pattern.RegisterParent(steelBase);
            steelBase.Pattern = Pattern;
            CreateSteelBase(steelBase);
        }
        private static void CreateSteelBase(SteelBase steelBase)
        {
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
