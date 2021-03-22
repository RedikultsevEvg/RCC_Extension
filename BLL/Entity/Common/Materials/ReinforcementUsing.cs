using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Materials.Interfaces;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.RFExtenders;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс использования арматуры в конструкции
    /// </summary>
    public class ReinforcementUsing : CircleUsingBase
    {
        /// <summary>
        /// Класс преобразующий условную раскладку от поверхности в действительную по координатам
        /// </summary>
        public RFExtender Extender { get; private set; }

        #region Constructors
        public ReinforcementUsing() : base() { }
        public ReinforcementUsing(IDsSaveable parentMember) : base(parentMember) { SelectedId = 2; }
        #endregion
        #region Method
        /// <summary>
        /// Добавляет раскладку арматуры к использованию арматуры
        /// </summary>
        /// <param name="placement">Раскладка арматуры</param>
        public void SetPlacement(Placement placement)
        {
            base.SetPlacement(placement);
            if (!(Extender == null)) { Extender.SetPlacement(placement); }
        }
        public void SetExtender(RFExtender extender)
        {
            Extender = extender;
        }

        public object Clone()
        {
            ReinforcementUsing newObject = base.Clone() as ReinforcementUsing;
            Placement placement = Placement.Clone() as Placement;
            if (!(Extender is null))
            {
                newObject.SetExtender(Extender.Clone() as RFExtender);
            }
            newObject.SetPlacement(placement);
            return newObject;
        }
        #endregion
    }
}
