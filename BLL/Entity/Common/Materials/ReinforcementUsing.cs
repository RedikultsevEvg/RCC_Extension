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
using RDBLL.Entity.Common.Materials.RFPlacementAdapters;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс использования арматуры в конструкции
    /// </summary>
    public class ReinforcementUsing : MaterialUsing, IHavePlacement
    {
        /// <summary>
        /// Диаметр арматурного стержня
        /// </summary>
        public double Diameter { get; set; }
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
        public RFPlacementAdapter Adapter { get; private set; }

        #region Constructors
        public ReinforcementUsing() : base() { }
        public ReinforcementUsing(ISavableToDataSet parentMember) : base(parentMember) { SelectedId = 2; }
        #endregion
        #region Method
        /// <summary>
        /// Добавляет раскладку арматуры к использованию арматуры
        /// </summary>
        /// <param name="placement">Раскладка арматуры</param>
        public void SetPlacement(Placement placement)
        {
            Placement = placement;
            if (!(Adapter == null)) { Adapter.SetPlacement(placement); }
        }
        public void SetAdapter(RFPlacementAdapter adapter)
        {
            Adapter = adapter;
        }
        #endregion
    }
}
