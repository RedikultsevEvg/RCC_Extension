using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Placements;

namespace RDBLL.Entity.Common.Materials.RFExtenders
{
    public class LineToSurfBySpacing : LineToSurf
    {
        /// <summary>
        /// Максимальное расстояние между стержнями арматуры
        /// </summary>
        public double Spacing
        {
            get
            {
                LineBySpacing lineBySpacing = Placement as LineBySpacing;
                return lineBySpacing.Spacing;
            }
            set
            {
                LineBySpacing lineBySpacing = Placement as LineBySpacing;
                lineBySpacing.Spacing = value;
            }
        }
        
    }
}
