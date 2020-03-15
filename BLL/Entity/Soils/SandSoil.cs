using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс песчаного грунта
    /// </summary>
    public class SandSoil :DispersedSoil
    {
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public SandSoil(BuildingSite buildingSite) : base(buildingSite)
        {
            Description = "Песок мелкозернистый, маловлажный";
            CrcFi = 30;
            FstDesignFi = 27;
            SndDesignFi = 28;
            CrcCohesion = 0;
            FstDesignCohesion = 0;
            SndDesignCohesion = 0;
            /*BignessId
             * 0 - гравелистый
             * 1 - крупный
             * 2 - средней крупности
             * 3 - мелкий
             * 4 - пылеватый
             * */
            BignessId = 3;
            WetnessId = 2;
        }
    }
}
