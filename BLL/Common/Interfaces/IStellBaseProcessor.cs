using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.Results.SC;

namespace RDBLL.Common.Interfaces
{
    interface ISteelBaseProcessor
    {
        ColumnBaseResult GetResult(SteelColumnBase steelColumnBase);
    }
}
