using RDBLL.Common.Interfaces;
using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials
{
    public abstract class CircleUsingBase : PlacementUsingBase
    {
        /// <summary>
        /// Диаметр арматурного стержня
        /// </summary>
        public double Diameter { get; set; }
        /// <summary>
        /// Предварительная деформация, д.ед.
        /// </summary>
        public double Prestrain { get; set; }
        /// <summary>
        /// Площадь арматурного стержня
        /// </summary>
        public double BarArea { get { return Diameter * Diameter * Math.PI / 4; } }
        /// <summary>
        /// Суммарная площадь арматурных стержней
        /// </summary>
        public double TotalBarsArea { get { return BarArea * Placement.GetElementPoints().Count(); } }

        public CircleUsingBase() : base() { }
        public CircleUsingBase(IDsSaveable parentMember) : base(parentMember) { }

        /// <summary>
        /// Добавляет расположение к использованию
        /// </summary>
        /// <param name="placement">Раскладка арматуры</param>
        public virtual void SetPlacement(Placement placement)
        {
            base.SetPlacement(placement);
        }
    }
}
