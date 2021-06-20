using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Interfaces.Materials;
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
    public interface IBarSection : IHasId, IChild, IHasReinforcement
    {
        /// <summary>
        /// Форма стержня
        /// </summary>
        ICircle Circle { get; set; }
        /// <summary>
        /// Предварительная деформация
        /// </summary>
        double Prestrain { get; set; }
    }
}
