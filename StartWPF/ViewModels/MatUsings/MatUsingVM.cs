using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDStartWPF.Views.MatUsings;
using System.Collections.ObjectModel;
using RDBLL.Entity.Common.Materials;

namespace RDStartWPF.ViewModels.MatUsings
{
    internal class MatUsingVM : ViewModelBase
    {
        private static CommandBase _OpenPSF;

        public override string this[string columnName] => throw new NotImplementedException();

        public static CommandBase OpenPSF
        {
            get
            {
                return _OpenPSF ??
                    (
                     _OpenPSF = new CommandBase(obj =>
                     {
                         MaterialUsing parentMember = obj as MaterialUsing;
                         WndSafetyFactors wndSafetyFactors = new WndSafetyFactors(parentMember);
                         wndSafetyFactors.ShowDialog();
                     }));
            }
        }
    }
}
