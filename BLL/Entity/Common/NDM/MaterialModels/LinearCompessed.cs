using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM.MaterialModels
{
    /// <summary>
    /// Интерфейс модели материала
    /// </summary>
    public class LinearCompressed :IMaterialModel
    {
        /// <summary>
        /// Начальный модуль упругости, Па
        /// </summary>
        public double ElasticModulus { get; set; }
        /// <summary>
        /// Возвращает напряжения по известной деформации
        /// </summary>
        /// <param name="Epsilon"></param>
        /// <returns></returns>
        public double GetStress(double Epsilon)
        {
            if (Epsilon < 0) { return ElasticModulus * Epsilon; } else { return ElasticModulus/1000000 * Epsilon; }
        }
        /// <summary>
        /// Возвращает секущий модуль упругости по известному напряжению
        /// </summary>
        /// <param name="Epsilon"></param>
        /// <returns></returns>
        public double GetSecantModulus(double Epsilon)
        {
            if (Epsilon < 0) { return ElasticModulus; } else { return ElasticModulus / 1000000; }
        }
    }
}
