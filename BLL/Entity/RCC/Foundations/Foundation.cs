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
using System.Collections.ObjectModel;
using RDBLL.Common.Interfaces;
using System.Data;



namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс столбчатого фундамента
    /// </summary>
    public class Foundation : IHaveForcesGroups
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
        public ObservableCollection<ForcesGroup> ForcesGroups { get; set; }
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
        public bool IsLoadCasesActual { get; set; }
        /// <summary>
        /// Признак актуальности ступеней
        /// </summary>
        public bool IsPartsActual { get; set; }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Foundation()
        {

        }
        /// <summary>
        /// Конструктор по уровню
        /// </summary>
        /// <param name="level">Уровень</param>
        public Foundation(Level level)
        {
            Id = ProgrammSettings.CurrentId;
            LevelId = level.Id;
            Level = level;
            Name = "Новый фундамент";
            ForcesGroups = new ObservableCollection<ForcesGroup>();
            ForcesGroups.Add(new ForcesGroup(this));
            Parts = new ObservableCollection<FoundationPart>();
            ForceCurvatures = new List<ForceCurvature>();
            IsLoadCasesActual = true;
            IsPartsActual = true;
        }
    }
}
