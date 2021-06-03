using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Reinforcements.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Ancorages
{
    /// <summary>
    /// Интерфейс логики расчета длин анкеровки
    /// </summary>
    public interface IAncorageLogic
    {
        /// <summary>
        /// Возвращает длину анкеровки арматуры в бетоне
        /// </summary>
        /// <param name="concrete">Применяемый бетон</param>
        /// <param name="barSection">Сечение арматурного стержня</param>
        /// <param name="loadRate">Доля длительности нагрузки</param>
        /// <returns></returns>
        double GetAncorageLenth(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression, double areaCofficient = 1);
        /// <summary>
        /// Возвращает длину нахлеста арматуры в бетоне для нахлеста с разбежкой
        /// </summary>
        /// <param name="concrete">Применяемый бетон</param>
        /// <param name="barSection">Сечение арматурного стержня</param>
        /// <param name="loadRate">Доля длительности нагрузки</param>
        /// <returns></returns>
        double GetSimpleLappingLenth(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression, double areaCofficient = 1);
        /// <summary>
        /// Возвращает длину разбежки арматуры в бетоне для нахлеста с разбежкой
        /// </summary>
        /// <param name="concrete">Применяемый бетон</param>
        /// <param name="barSection">Сечение арматурного стержня</param>
        /// <param name="loadRate">Доля длительности нагрузки</param>
        /// <returns></returns>
        double GetSimpleLappingGapLenth(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression, double areaCofficient = 1);
        /// <summary>
        /// Возвращает длину нахлеста арматуры в бетоне для нахлеста без разбежки
        /// </summary>
        /// <param name="concrete">Применяемый бетон</param>
        /// <param name="barSection">Сечение арматурного стержня</param>
        /// <param name="loadRate">Доля длительности нагрузки</param>
        /// <returns></returns>
        double GetDoubleLappingLenth(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression, double areaCofficient = 1);
    }
}
