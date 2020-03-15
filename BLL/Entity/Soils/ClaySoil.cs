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
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public ClaySoil(BuildingSite buildingSite) : base(buildingSite)
        {
            Description = "Суглинок песчанистый, тугопластичный";
            CrcFi = 20;
            FstDesignFi = 17;
            SndDesignFi = 18;
            CrcCohesion = 20000;
            FstDesignCohesion = 17000;
            SndDesignCohesion = 18000;
            IL = 0.25;
        }
    }
}
