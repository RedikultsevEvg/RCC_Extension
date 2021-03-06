﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces
{
    interface IHasChildren : IDsSaveable
    {
        ObservableCollection<IHasParent> Children { get; }
    }
}
