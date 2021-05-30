using RDBLL.Common.Interfaces.Geometry.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Geometry
{
    /// <summary>
    /// Класс точки на плоскости с дополнительной координатой - поворотом
    /// </summary>
    public class Point2DRot : Point2D, IPoint2DRot
    {
        /// <summary>
        /// Поворот относительно оси Z, radians
        /// </summary>
        public double RotZ { get; set; }
        /// <summary>
        /// Конструктор точки с координатами 0,0,0
        /// </summary>
        public Point2DRot() : base()
        {
            RotZ = 0;
        }
        /// <summary>
        /// Конструктор точки с указанными координатами
        /// </summary>
        /// <param name="x">Координата по оси X, metres</param>
        /// <param name="y">Координата по оси Y, metres</param>
        /// <param name="rotZ">Поворот относительно оси Z, rad</param>
        public Point2DRot(double x, double y, double rotZ) : base(x, y)
        {
            RotZ = rotZ;
        }
        /// <summary>
        /// Конструктор по точке и углу поворота
        /// </summary>
        /// <param name="point2D">Точка на плоскости</param>
        /// <param name="rotZ">Поворот относительно оси Z, rad</param>
        public Point2DRot(Point2D point2D, double rotZ) : base(point2D.X, point2D.Y)
        {
            RotZ = rotZ;
        }
    }
}
