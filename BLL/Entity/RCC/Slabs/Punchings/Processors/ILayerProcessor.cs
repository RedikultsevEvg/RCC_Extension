using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors
{
    public interface ILayerProcessor
    {
        /// <summary>
        /// Возвращает коллекцию контуров продавливания
        /// </summary>
        /// <param name="punching"></param>
        /// <returns></returns>
        List<PunchingContour> GetPunchingContours(Punching punching);
    }
}
