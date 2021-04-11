using RDBLL.Entity.RCC.BuildingAndSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soils.Factories
{
    /// <summary>
    /// Перечисление типов грунта для фабрики грунтов
    /// </summary>
    public enum FactorySoilType
    {
        /// <summary>
        /// Грунт для тестового примера Foundation Verification Manual 1
        /// </summary>
        FoundationVM1,
        /// <summary>
        /// Грунт для тестового примера Foundation Verification Manual 1
        /// </summary>
        FoundationVM2,
    }
    /// <summary>
    /// Фабрика грунтов
    /// </summary>
    public static class SoilFactory
    {
        public static Soil GetSoil(BuildingSite site, FactorySoilType type)
        {
            switch (type)
            {
                case FactorySoilType.FoundationVM1:
                    {
                        DispersedSoil soil = new ClaySoil(site);
                        soil.Name = "ИГЭ-1";
                        soil.Description = "Суглинок песчанистый, тугопластичный";
                        soil.CrcDensity = 1950;
                        soil.FstDesignDensity = 1800;
                        soil.SndDesignDensity = 1900;
                        soil.CrcParticularDensity = 2700;
                        soil.FstParticularDensity = 2700;
                        soil.SndParticularDensity = 2700;
                        soil.PorousityCoef = 0.7;
                        soil.ElasticModulus = 15e6;
                        soil.SndElasticModulus = 15e6;
                        soil.PoissonRatio = 0.3;
                        soil.CrcFi = 20;
                        soil.FstDesignFi = 18;
                        soil.SndDesignFi = 19;
                        soil.CrcCohesion = 50000;
                        soil.FstDesignCohesion = 47000;
                        soil.SndDesignCohesion = 49000;
                        soil.IsDefinedFromTest = true;
                        return soil;
                    }
                case FactorySoilType.FoundationVM2:
                    {
                        DispersedSoil soil = new ClaySoil(site);
                        soil.Name = "ИГЭ-1";
                        soil.Description = "Суглинок песчанистый, тугопластичный";
                        soil.CrcDensity = 1950;
                        soil.FstDesignDensity = 1800;
                        soil.SndDesignDensity = 1900;
                        soil.CrcParticularDensity = 2700;
                        soil.FstParticularDensity = 2700;
                        soil.SndParticularDensity = 2700;
                        soil.PorousityCoef = 0.7;
                        soil.ElasticModulus = 15e6;
                        soil.SndElasticModulus = 15e6;
                        soil.PoissonRatio = 0.3;
                        soil.CrcFi = 20;
                        soil.FstDesignFi = 18;
                        soil.SndDesignFi = 19;
                        soil.CrcCohesion = 50000;
                        soil.FstDesignCohesion = 47000;
                        soil.SndDesignCohesion = 49000;
                        soil.IsDefinedFromTest = true;
                        return soil;
                    }
                default: return null;
            }

        }
    }
}
