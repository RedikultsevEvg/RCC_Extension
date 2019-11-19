using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM.MaterialModels
{
    /// <summary>
    /// Модель линейно упругого материала, работающего только на растяжение
    /// </summary>
    public class LinearTensioned : IMaterialModel
    {
        /// <summary>
        /// Начальный модуль упругости, Па
        /// </summary>
        public double ElasticModulus { get; set; }
        /// <summary>
        /// Список констант, интерпретация констант определяется моделью
        /// </summary>
        public List<MaterialConstant> ListOfConsnstants { get; set; }
        /// <summary>
        /// Возвращает напряжения
        /// </summary>
        /// <param name="strain">Деформации, д.ед.</param>
        /// <returns>Напряжения, Па</returns>
        public double GetStress(double strain)
        {
            if (strain > 0)
            { return ElasticModulus * strain; }
            else { return ElasticModulus/1000000*strain; }
        }
        /// <summary>
        /// Возвращает секущий модуль упругости
        /// </summary>
        /// <param name="strain">Деформации, д.ед.</param>
        /// <returns>Секущий модуль упругости, Па</returns>
        public double GetSecantModulus(double strain)
        {
            if (strain > 0)
            { return ElasticModulus; }
            else { return ElasticModulus / 1000000; }
        }
    }
}
