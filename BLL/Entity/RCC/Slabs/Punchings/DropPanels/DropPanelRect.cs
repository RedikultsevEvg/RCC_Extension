using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.DropPanels
{
    /// <summary>
    /// Клас прямоугольной капители
    /// </summary>
    public class DropPanelRect : DropPanelBase, IRectangle
    {
        /// <summary>
        /// Ширина капители (размер вдоль X)
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина капители (размер вдоль Y)
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Точка центра капители
        /// </summary>
        public Point2D Center { get; set; }

        public double GetArea()
        {
            throw new NotImplementedException();
        }

        public double GetPerimeter()
        {
            throw new NotImplementedException();
        }
    }
}
