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
    /// Класс ненесущего грунта
    /// </summary>
    public class NonBearingSoil :Soil
    {
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public NonBearingSoil(BuildingSite buildingSite) : base(buildingSite)
        {

        }
    }
}
