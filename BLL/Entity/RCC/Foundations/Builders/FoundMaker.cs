using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Builders
{
    public static class FoundMaker
    {
        public static Foundation MakeFoundation(BuilderBase builder)
        {
            return builder.GetFoundation();
        }
    }
}
