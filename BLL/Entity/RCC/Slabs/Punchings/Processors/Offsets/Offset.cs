using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors.Offsets
{
    /// <summary>
    /// Класс размера отступа от  расчетного контура
    /// </summary>
    public class Offset
    {
        /// <summary>
        /// Размер отступа
        /// </summary>
        public double Size { get; set; }
        /// <summary>
        /// Флаг, означающий что линия является несущей
        /// </summary>
        public bool IsBearing { get; set; }
    }
}
