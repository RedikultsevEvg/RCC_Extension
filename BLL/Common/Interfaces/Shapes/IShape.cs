using RDBLL.Common.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Shapes
{
    public interface IShape
    {
        /// <summary>
        /// Точка центра тяжести
        /// </summary>
        Point2D Center { get; set; }
        /// <summary>
        /// Возвращает площадь фигуры
        /// </summary>
        /// <returns></returns>
        double GetArea();
    }
}
