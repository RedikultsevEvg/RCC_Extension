using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings
{
    public class PunchingSubContour : IHasConcrete, IHasHeight
    {
        /// <summary>
        /// Бетон элементарного контура
        /// </summary>
        public ConcreteUsing Concrete { get; set; }
        /// <summary>
        /// Высота элементарного контура
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Коллекция линий контура
        /// </summary>
        public List<PunchingLine> Lines { get; set; }

        public PunchingSubContour()
        {
            Lines = new List<PunchingLine>();
        }
    }
}
