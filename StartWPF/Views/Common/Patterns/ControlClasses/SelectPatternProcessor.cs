using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.SC.Column.SteelBases.Builders;
using RDBLL.Entity.SC.Column.SteelBases.Patterns;
using RDStartWPF.Views.Common.Patterns.ControlClasses.Factories;
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
        //Карточки паттернов стальной базы
        public static List<PatternCard> SelectSteelBasePattern(IDsSaveable parent) { return SteelBaseCardFactory.GetCards(parent);}
    }
}
