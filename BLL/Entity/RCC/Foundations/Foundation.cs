using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RDBLL.Common.Interfaces;
using System.Data;


namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс столбчатого фундамента
    /// </summary>
    public class Foundation
    {
        /// <summary>
        /// Код фундамента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код уровня
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// Обратная ссылка на уровень
        /// </summary>
        public Level Level { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Коллекция групп нагрузок
        /// </summary>
        public ObservableCollection<ForcesGroup> LoadsGroup { get; set; }
        /// <summary>
        /// Коллекция ступеней столбчатого фундамента
        /// </summary>
        public ObservableCollection<FoundationPart> Parts { get; set; }
        /// <summary>
        /// Коллекция комбинаций и кривизны
        /// </summary>
        public List<ForceCurvature> ForceCurvatures { get; set; }
        /// <summary>
        /// Признак актуальности нагрузок
        /// </summary>
        public bool IsLoadsActual { get; set; }
        /// <summary>
        /// Признак актуальности ступеней
        /// </summary>
        public bool IsPartsActual { get; set; }
    }
}
