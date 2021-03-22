using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.MeasureUnits
{
    public class MeasureUnitList
    {
        /// <summary>
        /// Наименование линейных единиц измерения
        /// </summary>
        public string Linear { get => MeasureUnitConverter.GetUnitLabelText(0); }
    }
}
