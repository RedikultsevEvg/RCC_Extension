using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Абстрактный класс грунта
    /// </summary>
    public abstract class Soil
    {
        /// <summary>
        /// Код грунта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код строительного объекта
        /// </summary>
        public int BuildingSiteId { get; set; }
        /// <summary>
        /// Обратная ссылка на строительный объект
        /// </summary>
        public BuildingSite BuildingSite { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Нормативная плотность грунта
        /// </summary>
        public double CrcDensity { get; set; }
        /// <summary>
        /// Расчетная плотность грунта для 1 группы ПС
        /// </summary>
        public double FstDesignDensity { get; set; }
        /// <summary>
        /// Расчетная плотность грунта для 2 группы ПС
        /// </summary>
        public double SndDesignDensity { get; set; }
        /// <summary>
        /// Коэффициент фильтрации
        /// Необходим, так как влияет на напряжения в грунте при послойном суммировании
        /// </summary>
        public double FiltrationCoeff { get; set; }
        public string StressMeasure { get { return MeasureUnitConverter.GetUnitLabelText(3); } }
        public string DensityMeasure { get { return MeasureUnitConverter.GetUnitLabelText(8); } }
        public string FiltrationMeasure { get { return MeasureUnitConverter.GetUnitLabelText(14); } }

        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public Soil(BuildingSite buildingSite)
        {
            Id = ProgrammSettings.CurrentId;
            BuildingSiteId = buildingSite.Id;
            BuildingSite = buildingSite;
            Name = "ИГЭ-" + (buildingSite.Soils.Count + 1);
            FiltrationCoeff = 0.0001;
        }
    }
}
