using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Soils;

namespace RDBLL.Common.Interfaces
{
    interface IHasSoilSection
    {
        /// <summary>
        /// Код скважины
        /// </summary>
        int? SoilSectionId { get; set; }
        /// <summary>
        /// Обратная ссылка на скважину
        /// </summary>
        SoilSection SoilSection { get; set; }
    }
}
