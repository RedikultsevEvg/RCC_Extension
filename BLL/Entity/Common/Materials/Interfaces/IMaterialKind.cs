using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;

namespace RDBLL.Entity.Common.Materials.Interfaces
{
    /// <summary>
    /// Интерфейс для материалов, применяемых для расчетов
    /// </summary>
    public interface IMaterialKind :IDsSaveable
    {
        /// <summary>
        /// модуль упругости
        /// </summary>
        double ElasticModulus { get; set; }
        /// <summary>
        /// Коэффициент Пуассона
        /// </summary>
        double PoissonRatio { get; }
    }
}
