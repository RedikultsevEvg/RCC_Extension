using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс дисперсного грунта
    /// </summary>
    public class DispersedSoil :BearingSoil
    {
        /// <summary>
        /// Нормативное значение угла внутреннего трения
        /// </summary>
        public double CrcFi { get; set; }
        /// <summary>
        /// Расчетное значение угла внутреннего трения для 1-й группы ПС
        /// </summary>
        public double FstDesignFi { get; set; }
        /// <summary>
        /// Расчетное значение угла внутреннего трения для 2-й группы ПС
        /// </summary>
        public double SndDesignFi { get; set; }
        /// <summary>
        /// Нормативное значение сцепления, Па
        /// </summary>
        public double CrcCohesion { get; set; }
        /// <summary>
        /// Расчетное значение сцепления для 1-й группы ПС
        /// </summary>
        public double FstDesignCohesion { get; set; }
        /// <summary>
        /// Расчетное значение сцепления для 2-й группы ПС
        /// </summary>
        public double SndDesignCohesion { get; set; }

        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public DispersedSoil(BuildingSite buildingSite) :base(buildingSite)
        {
            ElasticModulus = 2e7;
            SndElasticModulus = 1e8;
            CrcDensity = 1950;
            FstDesignDensity = 1800;
            SndDesignDensity = 1900;
            CrcFi = 20;
            FstDesignFi = 18;
            SndDesignFi = 17;
            CrcCohesion = 20000;
            FstDesignCohesion = 17000;
            SndDesignCohesion = 18000;
            PoissonRatio = 0.3;
        }
    }
}
