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
    /// <summary>
    /// Интерфейс арматурного стержня круглого сечения
    /// </summary>
    public interface IBarSection : IHasParent
    {
        /// <summary>
        /// Форма стержня
        /// </summary>
        ICircle Circle { get; set; }
        /// <summary>
        /// Использование материала арматуры
        /// </summary>
        ReinforcementUsing Reinforcement { get; set; }
    }
}
