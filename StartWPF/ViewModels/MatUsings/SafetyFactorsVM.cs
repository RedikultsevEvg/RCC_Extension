using RDStartWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Materials;
using System.Collections.ObjectModel;
using RDStartWPF.InfraStructure.Comands.Base;
using RDBLL.Common.Interfaces;
using RDStartWPF.ViewModels.Base.Interfaces;

namespace RDStartWPF.ViewModels.MatUsings
{
    internal class SafetyFactorsVM : ViewModelBase, IHasCollection
    {
        private SafetyFactor _SelectedPSF;
        private MaterialUsing _ParentMember;

        public ObservableCollection<SafetyFactor> SafetyFactors { get; set; }
        public SafetyFactor SelectedPSF
        {
            get
            {
                return _SelectedPSF;
            }
            set
            {
                _SelectedPSF = value;
                OnPropertyChanged("SelectedPSF");
            }
        }

        private CommandBase _AddCommand;
        public CommandBase AddCommand
        {
            get
            {
                return _AddCommand ??
                    (_AddCommand = new CommandBase(newObject =>
                    {
                        SafetyFactor factor = new SafetyFactor(true);
                        factor.RegisterParent(_ParentMember);
                        SelectedPSF = factor;
                        SafetyFactors.Add(factor);
                    }));
            }
        }

        private CommandBase _RemoveCommand;
        public CommandBase RemoveCommand
        {
            get
            {
                return _RemoveCommand ??
                    (_RemoveCommand = new CommandBase(oldObject =>
                    {
                        SafetyFactor factor = oldObject as SafetyFactor;
                        if (factor != null) { SafetyFactors.Remove(factor); }
                    },
                    oldObject => SafetyFactors.Count > 0));
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        public SafetyFactorsVM(MaterialUsing parentMember)
        {
            SafetyFactors = parentMember.SafetyFactors;
            if (SafetyFactors.Count > 0) { _SelectedPSF = SafetyFactors[0]; }
            _ParentMember = parentMember;
        }

    }
}
