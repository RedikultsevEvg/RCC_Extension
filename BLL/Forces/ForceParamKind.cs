using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.MeasureUnits;

namespace RDBLL.Forces
{
    /// <summary>
    /// Вид усилия
    /// </summary>
    public class ForceParamKind
    {
        private string _longLabel;

        /// <summary>
        /// Код вида усилия
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Длинное наименование
        /// </summary>
        public string LongLabel { get { return _longLabel; } set { _longLabel = value; } }
        /// <summary>
        /// Длинное наименование с указанием текущих единиц измерения
        /// </summary>
        public string LongLabelInUnit
        { get
            {
                MeasureUnitLabel measureUnitLabel = MeasureUnit.GetCurrentLabel();
                return _longLabel + ", " + measureUnitLabel.UnitName;
            }
        }
        /// <summary>
        /// Короткое наименование
        /// </summary>
        public string ShortLabel { get; set; }
        /// <summary>
        /// Текущая единица измерения
        /// </summary>
        public string UnitLabelInUnit
        { get
            {
                MeasureUnitLabel measureUnitLabel = MeasureUnit.GetCurrentLabel();
                return measureUnitLabel.UnitName;
            }
        }
        /// <summary>
        /// Дополнительный текст
        /// </summary>
        public string Addition { get; set; }
        /// <summary>
        /// Ссылка на единицу измерения
        /// </summary>
        public MeasureUnit MeasureUnit { get; set; }
    }
}
