using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;
using RDBLL.Common.Service;

namespace RDBLL.Forces
{
    /// <summary>
    /// Группа нагрузок, приложенных в одной точке
    /// </summary>
    public class ForcesGroup
    {
        public int Id { get; set; }
        public List<SteelBase> SteelBases { get; set; } //Обратная ссылка. База стальной колонны к котрой относится группа нагрузок
        public ObservableCollection<LoadSet> LoadSets { get; set; } //Коллекция набора нагрузок
        public double CenterX { get; set; } //Точка, к которой приложена группа нагрузок
        public double CenterY { get; set; } //Точка, к которой приложена группа нагрузок

        #region Constructors
        /// <summary>
        /// Конструктор создает экземпляр класса группы нагрузок
        /// </summary>
        /// <param name="steelColumnBase"></param>
        public ForcesGroup(SteelBase steelColumnBase)
        {
            Id = ProgrammSettings.CurrentId;
            SteelBases = new List<SteelBase>();
            SteelBases.Add(steelColumnBase);
            LoadSets = new ObservableCollection<LoadSet>();
            CenterX = 0;
            CenterY = 0;
        }
        #endregion
    }
}
