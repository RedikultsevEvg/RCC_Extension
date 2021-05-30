using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces.Geometry.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Placements
{
    /// <summary>
    /// Интерфейс для расположения элементов по определенным правилам
    /// </summary>
    public interface IPlacementEh
    {
        /// <summary>
        /// Вложенные расположения
        /// </summary>
        List<IPlacementEh> Placements { get; set; }
        /// <summary>
        /// Возвращает точки расположения с учетом вложенных расположений
        /// </summary>
        /// <returns></returns>
        List<Point2DRot> GetPoints();
        /// <summary>
        /// Возвращает точки расположения без учета вложенных расположений
        /// </summary>
        /// <returns></returns>
        List<Point2DRot> GetOwnPoints();

    }
}
