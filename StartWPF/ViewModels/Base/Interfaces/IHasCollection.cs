using RDStartWPF.InfraStructure.Comands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.ViewModels.Base.Interfaces
{
    interface IHasCollection
    {
        CommandBase AddCommand { get;}
        CommandBase RemoveCommand { get;}
    }
}
