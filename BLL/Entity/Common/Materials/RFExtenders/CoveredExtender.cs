using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials.RFExtenders
{
    public abstract class CoveredExtender :RFExtender
    {
        /// <summary>
        /// Величина защитного слоя арматуры, м
        /// </summary>
        public abstract double CoveringLayer { get; set; }
    }
}
