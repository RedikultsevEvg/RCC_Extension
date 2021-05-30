using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Placements;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Bars
{
    public interface IBarSection : IHasParent
    {
        IShape Shape { get; set; }
        ReinforcementUsing Reinforcement { get; set; }
    }
}
