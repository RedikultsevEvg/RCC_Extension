using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors.Offsets
{
    /// <summary>
    /// Класс прямоугольной группы отсупов
    /// </summary>
    public class RectOffsetGroup
    {
        /// <summary>
        /// Отступ слева
        /// </summary>
        public Offset LeftOffset { get; set; }
        /// <summary>
        /// Отступ справа
        /// </summary>
        public Offset RightOffset { get; set; }
        /// <summary>
        /// Отступ сверху
        /// </summary>
        public Offset TopOffset { get; set; }
        /// <summary>
        /// Отступ снизу
        /// </summary>
        public Offset BottomOffset { get; set; }

        public RectOffsetGroup()
        {
            LeftOffset = new Offset();
            RightOffset = new Offset();
            TopOffset = new Offset();
            BottomOffset = new Offset();
        }
        public RectOffsetGroup(Offset left, Offset right, Offset top, Offset bottom)
        {
            LeftOffset = left;
            RightOffset = right;
            TopOffset = new Offset();
            BottomOffset = new Offset();
        }
    }
}
