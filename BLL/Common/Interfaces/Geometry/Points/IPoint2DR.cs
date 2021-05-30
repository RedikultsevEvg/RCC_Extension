using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Geometry.Points
{
    /// <summary>
    /// Интерфейс точки на плоскости с дополнительной координатой - поворот относительно оси Z
    /// </summary>
    public interface IPoint2DRot : IPoint2D
    {
        /// <summary>
        /// Поворот относительно оси Z, радианы
        /// </summary>
        double RotZ { get; set; }
    }
}
