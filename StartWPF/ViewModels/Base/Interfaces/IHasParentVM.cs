using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.ViewModels.Base.Interfaces
{
    internal interface IHasParentVM
    {
        ViewModelBase ParentVM { get; set; }
    }
}
