using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс геологического разреза
    /// </summary>
    public class SoilSection
    {
        /// <summary>
        /// Код разреза
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код строительного объекта
        /// </summary>
        public int BuildingSiteId { get; set; }
        /// <summary>
        /// Обратная ссылка на строительный объект
        /// </summary>
        public BuildingSite BuildingSite {get;set;}
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Слои грунта
        /// </summary>
        public ObservableCollection<SoilLayer> SoilLayers { get; set; }
        /// <summary>
        /// Уровень грунтовых вод зафиксированный
        /// </summary>
        public double NaturalWaterLevel { get; set; }
        /// <summary>
        /// Уровень грунтовых вод прогнозный
        /// </summary>
        public double WaterLevel { get; set; }
        /// <summary>
        /// Положение центра
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Положение центра
        /// </summary>
        public double CenterY { get; set; }
    }
}
