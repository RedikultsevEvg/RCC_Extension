using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс глинистого грунта
    /// </summary>
    public class ClaySoil :DispersedSoil
    {
        /// <summary>
        /// Флаг отнесения к суглинкам
        /// </summary>
        public bool IsSemiClay { get; set; }
        /// <summary>
        /// Показатель текучести
        /// </summary>
        public double IL { get; set; }
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public ClaySoil(BuildingSite buildingSite) : base(buildingSite)
        {
            IsSemiClay = true;
            IL = 0.25;
        }
    }
}
