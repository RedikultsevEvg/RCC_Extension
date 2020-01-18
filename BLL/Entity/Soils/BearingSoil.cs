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
    /// Абстрактный класс несущего грунта
    /// </summary>
    public abstract class BearingSoil :Soil
    {
        /// <summary>
        /// Модуль деформации
        /// </summary>
        public double ElasticModulus { get; set; }
        /// <summary>
        /// Модуль деформации по вторичной ветви нагружения
        /// </summary>
        public double SndElasticModulus { get; set; }
        /// <summary>
        /// Коэффициент Пуассона
        /// </summary>
        public double PoissonRatio { get; set; }

        public BearingSoil(BuildingSite buildingSite) : base(buildingSite)
        {
        }
    }
}
