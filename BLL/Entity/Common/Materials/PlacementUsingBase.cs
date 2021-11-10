using RDBLL.Common.Interfaces;
using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Базовый класс использования материалов, которые имеют расположение
    /// </summary>
    public abstract class PlacementUsingBase : MaterialUsing, IHasPlacement, ICloneable
    {
        /// <summary>
        /// Класс расположения
        /// </summary>
        public Placement Placement { get; private set; }

        public PlacementUsingBase(bool genId = false) : base(genId) { }
        public PlacementUsingBase(IDsSaveable parentMember) : base(parentMember) { }

        /// <summary>
        /// Добавляет расположение к использованию
        /// </summary>
        /// <param name="placement">Раскладка арматуры</param>
        public virtual void SetPlacement(Placement placement)
        {
            Placement = placement;
            Placement.RegisterParent(this);
        }
    }
}
