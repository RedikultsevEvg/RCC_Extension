using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using System.Collections.ObjectModel;

namespace RDBLL.Common.Interfaces
{
    /// <summary>
    /// Интерфейс для сущностей, которые включают группу нагрузок
    /// </summary>
    public interface IHaveForcesGroups
    {
        /// <summary>
        /// Коллекция групп нагрузок
        /// </summary>
        ObservableCollection<ForcesGroup> ForcesGroups { get; set; }
        /// <summary>
        /// Коллекция комбинаций
        /// </summary>
        ObservableCollection<LoadSet> LoadCases { get; set; }
        /// <summary>
        /// Флаг актуальности нагрузок
        /// </summary>
        bool IsLoadCasesActual { get; set; }
    }
}
