using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM.MaterialModels
{
    /// <summary>
    /// Модель линейно упругого материала
    /// </summary>
    public class LinearIsotropic :IMaterialModel
    {
        /// <summary>
        /// Начальный модуль упругости, Па
        /// </summary>
        public double ElasticModulus { get; set; }
        /// <summary>
        /// Список констант, интерпретация констант определяется моделью
        /// </summary>
        public List<MaterialConstant> ListOfConsnstants { get; set; }
        public LinearIsotropic(double elasticModulus, double copmressionCoef, double tensionCoef)
        {
            ElasticModulus = elasticModulus;
            ListOfConsnstants = new List<MaterialConstant>();
            MaterialConstant NewConstant;
            NewConstant = new MaterialConstant { Name = "CompressionCoefficient", ConstantValue = copmressionCoef };
            ListOfConsnstants.Add(NewConstant);
            NewConstant = new MaterialConstant { Name = "TensionCoefficient", ConstantValue = tensionCoef };
            ListOfConsnstants.Add(NewConstant);
        }
        /// <summary>
        /// Возвращает напряжения
        /// </summary>
        /// <param name="strain">Деформации, д.ед.</param>
        /// <returns>Напряжения, Па</returns>
        public double GetStress(double strain)
        {
            double elasticModulus = ElasticModulus;
            if (strain < 0) { elasticModulus *= ListOfConsnstants[0].ConstantValue; }
            else { elasticModulus *= ListOfConsnstants[1].ConstantValue; }
            return elasticModulus * strain;
        }
        /// <summary>
        /// Возвращает секущий модуль упругости
        /// </summary>
        /// <param name="strain">Деформации, д.ед.</param>
        /// <returns>Секущий модуль упругости, Па</returns>
        public double GetSecantModulus(double strain)
        {
            double elasticModulus = ElasticModulus;
            if (strain < 0) { elasticModulus *= ListOfConsnstants[0].ConstantValue; }
            else if (strain > 0) { elasticModulus *= ListOfConsnstants[1].ConstantValue; }
            return elasticModulus;
        }
    }
}
