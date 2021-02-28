using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Placements;

namespace RDBLL.Entity.Common.Materials.RFExtenders
{
    /// <summary>
    /// Абстрактный класс адаптера раскладки по координатам к другим типам раскладок
    /// </summary>
    public abstract class RFExtender : ICloneable
    {
        /// <summary>
        /// Ссылка на расположение арматуры
        /// </summary>
        public Placement Placement { get; private set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void SetPlacement(Placement placement)
        {
            Placement = placement;
        }
    }
}
