using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;

namespace RDBLL.Forces
{
    /// <summary>
    /// Группа нагрузок, приложенных в одной точке
    /// </summary>
    public class ForcesGroup
    {
        public SteelColumnBase SteelColumnBase { get; set; } //Обратная ссылка. База стальной колонны к котрой относится группа нагрузок
        public ObservableCollection<BarLoadSet> Loads { get; set; } //Коллекция набора нагрузок
        public Point2D Excentricity { get; set; } //Точка, к которой приложена группа нагрузок

        #region Constructors
        /// <summary>
        /// Конструктор создает экземпляр класса группы нагрузок
        /// </summary>
        /// <param name="steelColumnBase"></param>
        public ForcesGroup(SteelColumnBase steelColumnBase)
        {
            SteelColumnBase = steelColumnBase;
            Loads = new ObservableCollection<BarLoadSet>();
            Loads.Add(new BarLoadSet(this));
            Excentricity = new Point2D(0, 0);
        }
        #endregion
    }
}
