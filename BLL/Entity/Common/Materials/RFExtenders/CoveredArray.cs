using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Params;
using RDBLL.Common.Service;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Geometry;
using RDBLL.Entity.Common.Placements;

namespace RDBLL.Entity.Common.Materials.RFExtenders
{
    /// <summary>
    /// Абстрактный класс экстендера для массива элементов
    /// </summary>
    public class CoveredArray : RFExtender
    {
        public bool VisibleCover { get; set; }
        public bool VisibleSizes { get; set; }
        public bool VisibleCenter { get; set; }
        public bool VisibleFillArray { get; set; }
    }
}
