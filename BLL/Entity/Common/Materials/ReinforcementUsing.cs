using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Materials.Interfaces;
using RDBLL.Common.Interfaces;
using System.Data;
using DAL.Common;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.RFExtenders;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс использования арматуры в конструкции
    /// </summary>
    public class ReinforcementUsing : MaterialUsing, IHasPlacement
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
        public double TotalBarsArea { get { return BarArea*Placement.GetElementPoints().Count(); } }
        /// <summary>
        /// Класс расположения арматуры
        /// </summary>
        public Placement Placement { get; private set; }
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
            Placement = placement;
            Placement.RegisterParent(this);
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
