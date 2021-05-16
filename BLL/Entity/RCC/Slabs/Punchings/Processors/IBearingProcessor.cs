using RDBLL.Entity.RCC.Slabs.Punchings.Results;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors
{
    /// <summary>
    /// Интерфейс для выполнения расчета на продавливание
    /// </summary>
    public interface IBearingProcessor
    {
        /// <summary>
        /// Возвращает коэффициент несущей способности по заданным усилиям для заданного контура
        /// </summary>
        /// <param name="contour">Контур продавливания</param>
        /// <param name="loadSet">Коллекция нагрузок</param>
        /// <returns>Коэффициент использования несущей способности</returns>
        double GetBearingCapacityCoefficient(PunchingContour contour, LoadSet loadSet);
        /// <summary>
        /// Возвращает коэффициент несущей способности по заданным усилиям для заданного контура
        /// </summary>
        /// <param name="contour">Контур продавливания</param>
        /// <param name="force">Продольная сила</param>
        /// <param name="Mx">Момент относительно оси X</param>
        /// <param name="My">Момент относительно оси Y</param>
        /// <param name="fullLoad">Флаг полной/длительной нагрузки</param>
        /// <returns>Коэффициент несущей способности</returns>
        double GetBearingCapacityCoefficient(PunchingContour contour, double force, double Mx, double My, bool fullLoad = true);
        /// <summary>
        /// Вычисление результата для продавливания
        /// </summary>
        /// <param name="punching">Расчет на продавливание</param>
        void CalcResult(Punching punching);
    }
}
