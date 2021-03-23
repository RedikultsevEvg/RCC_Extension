using RDBLL.Common.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Patterns
{
    public abstract class PatternBase : ParametriсBase
    {
        public PatternBase(bool genId = false) : base(genId)
        {

        }
        public abstract void GetBaseParts();
    }
}
