using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Reinforcements.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Ancorages
{
    /// <summary>
    /// Интерфейс расчета длин анкеровки
    /// </summary>
    public interface IAncorage : IHasId, IChild, IHasConcrete
    {
        /// <summary>
        /// Коллекция сечений арматурных стержней
        /// </summary>
        List<IBarSection> BarSections { get; set; }
        /// <summary>
        /// Доля длительности нагрузки (отношение длительных нагрузок к полным)
        /// </summary>
        double LongLoadRate { get; set; }
        /// <summary>
        /// Логика расчета длин анкеровки
        /// </summary>
        IAncorageLogic AncorageLogic { get; }
    }
}
