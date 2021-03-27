using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.ViewModels.Base;
using RDStartWPF.Views.Common.Patterns.ControlClasses;
using RDStartWPF.Views.SC.Columns.Bases.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.Views.Common.Patterns.UserControls
{
    internal class PatternCardControlVM : ViewModelBase
    {
        public PatternCard PatternCard { get; private set; }
        private CommandBase _SelectCommand;
        public CommandBase SelectCommand
        {
            get
            {
                return _SelectCommand ??
                    (_SelectCommand = new CommandBase
                    (newObject =>
                    {
                        PatternCard.RunCommand();
                    }
                    )
                    );
            }
        }

        public PatternCardControlVM(PatternCard patternCard)
        {
            PatternCard = patternCard;
        }
    }
}
