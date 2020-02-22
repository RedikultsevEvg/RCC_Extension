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
        public SandSoil(BuildingSite buildingSite) : base(buildingSite)
        {
            
        }
    }
}
