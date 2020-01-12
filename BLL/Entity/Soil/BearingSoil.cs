using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soil
{
    /// <summary>
    /// Абстрактный класс несущего грунта
    /// </summary>
    public abstract class BearingSoil :SoilBase
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
    }
}
