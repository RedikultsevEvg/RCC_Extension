using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Builders
{
    public static class BaseMaker
    {
        public static SteelBase MakeSteelBase(BuilderBase builder)
        {
            return builder.GetSteelBase();
        }
    }
}
