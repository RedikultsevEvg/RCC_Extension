using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Results
{
    /// <summary>
    /// Класс для хранения результата расчета контура продавливания на сочетание нагрузок
    /// </summary>
    public class PunchingCalcResult
    {
        /// <summary>
        /// Код результата
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на панчинг (расчет на продавливание)
        /// </summary>
        public Punching Punching { get; set; }
        /// <summary>
        /// Обратная ссылка на сочетание нагрузок
        /// </summary>
        public LoadSet LoadSet { get; set; }
        /// <summary>
        /// Сочетание нагрузок, пребразованное к центру тяжести контура
        /// </summary>
        public LoadSet TransformedLoadSet { get; set; }
        /// <summary>
        /// Обратная ссылка на расчетный контур
        /// </summary>
        public PunchingContour PunchingContour {get;set;}
        /// <summary>
        /// Коэффициент использования несущей способности (если несущая способность обеспечена, то коээфициент меньше 1,0)
        /// </summary>
        public double BearingCoef { get; set; }
    }
}
