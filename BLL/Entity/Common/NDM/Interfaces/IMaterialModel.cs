using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.NDM.Interfaces
{
    /// <summary>
    /// Интерфейс модели материалов
    /// </summary>
    public interface IMaterialModel
    {
        /// <summary>
        /// Начальный модуль упругости, Па
        /// </summary>
        double ElasticModulus {get;set;}
        /// <summary>
        /// Список констант, интерпретация констант определяется моделью
        /// </summary>
        List<MaterialConstant> ListOfConsnstants { get; set; }
        /// <summary>
        /// Возвращает напряжения
        /// </summary>
        /// <param name="strain">Деформации, д.ед.</param>
        /// <returns>Напряжения, Па</returns>
        double GetStress(double strain);
        /// <summary>
        /// Возвращает секущий модуль упругости
        /// </summary>
        /// <param name="strain">Деформации, д.ед.</param>
        /// <returns>Секущий модуль упругости, Па</returns>
        double GetSecantModulus(double strain);
    }
}
