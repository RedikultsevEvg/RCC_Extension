using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Service;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс грунта с заданными расчетными сопротивлениями
    /// </summary>
    public class NominalSoil :BearingSoil
    {
        /// <summary>
        /// Расчетное сопротивление для 1 группы ПС
        /// </summary>
        public double FstDesignResistance { get; set; }
        /// <summary>
        /// Расчетное сопротивление для 2 группы ПС
        /// </summary>
        public double SndDesignResistance { get; set; }

        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite">Строительный объект</param>
        public NominalSoil(BuildingSite buildingSite) : base(buildingSite)
        {

        }
    }
}
