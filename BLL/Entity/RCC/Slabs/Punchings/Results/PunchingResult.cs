using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Results
{
    public class PunchingResult
    {
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на расчет продавливания
        /// </summary>
        public Punching Punching { get; set; }
        /// <summary>
        /// Коллекция комбинаций загружений
        /// </summary>
        public List<LoadCaseResult> LoadCases { get; set; }
        public List<ContourResult> PunchingContours { get; set; }
        public List<PunchingCalcResult> ContourResults {get; set;}
    }
}
