using RDBLL.Common.Interfaces;
using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials.SteelMaterialUsing
{
    public class BoltUsing : CircleUsingBase
    {
        public BoltUsing() : base() { }
        public BoltUsing(IDsSaveable parentMember) : base(parentMember) { SelectedId = 2; }

        /// <summary>
        /// Добавляет раскладку арматуры к использованию арматуры
        /// </summary>
        /// <param name="placement">Раскладка арматуры</param>
        public void SetPlacement(Placement placement)
        {
            base.SetPlacement(placement);
        }

        public object Clone()
        {
            BoltUsing newObject = base.Clone() as BoltUsing;
            return newObject;
        }
    }
}
