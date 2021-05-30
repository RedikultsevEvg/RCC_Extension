using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Placements
{
    /// <summary>
    /// Родительский контейнер для расположений элементов
    /// </summary>
    public interface IParentPlacement
    {
        /// <summary>
        /// Расположения элементов
        /// </summary>
        List<Placement> Placements { get; set; }
    }
}
