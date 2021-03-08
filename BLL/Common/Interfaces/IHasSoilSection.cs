using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Soils;

namespace RDBLL.Common.Interfaces
{
    public interface IHasSoilSection : IDsSaveable
    {
        /// <summary>
        /// Использование скважины
        /// </summary>
        SoilSectionUsing SoilSectionUsing { get; }
        void RegSSUsing(SoilSectionUsing soilSectionUsing);
        void UnRegSSUsing(SoilSectionUsing soilSectionUsing);
    }
}
