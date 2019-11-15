using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;

namespace RDBLL.Entity.Results.NDM
{
    /// <summary>
    /// Набор комбинации нагрузок и прямоугольников со значениями,
    /// соответствующими комбинации
    /// </summary>
    public class LoadCaseRectangleValue
    {
        /// <summary>
        /// Комбинация нагрузок
        /// </summary>
        public LoadSet LoadCase { get; set; } 
        /// <summary>
        /// Коллекция прямоугольников со значениями
        /// </summary>
        public List<RectangleValue> RectangleValues { get; set; }
        /// <summary>
        /// Конструктор без аргументов
        /// </summary>
        public LoadCaseRectangleValue()
        {
            RectangleValues = new List<RectangleValue>();
        }
    }
}
