using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Placements
{
    /// <summary>
    /// Интерфейс для элементов, имеющих расположение
    /// </summary>
    public interface IHasPlacements
    {
        /// <summary>
        /// Ссылка на родительский контейнер расположения элементов
        /// </summary>
        IParentPlacement Placement { get; set; }
    }
}
