using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Geometry.Points
{
    /// <summary>
    /// Интерфейс для точки на плоскости
    /// </summary>
    public interface IPoint2D
    {
        /// <summary>
        /// Координата X, метры
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// Координата Y, метры
        /// </summary>
        double Y { get; set; }
    }
}
