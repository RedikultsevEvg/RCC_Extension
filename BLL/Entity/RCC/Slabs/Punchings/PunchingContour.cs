using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings
{
    /// <summary>
    /// Класс контура продавливания для расчета
    /// </summary>
    public class PunchingContour
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int OrderNum { get; set; }
        /// <summary>
        /// Коллекция субконтуров
        /// </summary>
        public List<PunchingSubContour> SubContours { get; set; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="genId">флаг необходимости генерации кода</param>
        public PunchingContour(bool genId = false)
        {
            if (genId) Id = ProgrammSettings.CurrentId;
            SubContours = new List<PunchingSubContour>();
        }
    }
}
