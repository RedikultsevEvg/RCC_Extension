using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials.MatFactorys
{
    public static class MatFactory
    {
        public static List<SteelKind> GetSteelKinds()
        {
            List<SteelKind> steelKinds = new List<SteelKind>();
            SteelKind steelKind;

            steelKind = new SteelKind() { Id = 1, Name = "С235", ElasticModulus = 2.06e11, FstStrength = 2.3e8, SndStrength = 2.35e8 };
            steelKinds.Add(steelKind);

            steelKind = new SteelKind() { Id = 2, Name = "С245", ElasticModulus = 2.0e11, FstStrength = 2.4e8, SndStrength = 2.45e8 };
            steelKinds.Add(steelKind);

            steelKind = new SteelKind() { Id = 3, Name = "С255", ElasticModulus = 2.0e11, FstStrength = 2.5e8, SndStrength = 2.55e8 };
            steelKinds.Add(steelKind);

            steelKind = new SteelKind() { Id = 4, Name = "С275", ElasticModulus = 2.0e11, FstStrength = 2.7e8, SndStrength = 2.75e8 };
            steelKinds.Add(steelKind);

            steelKind = new SteelKind() { Id = 5, Name = "С345", ElasticModulus = 2.0e11, FstStrength = 3.4e8, SndStrength = 3.45e8 };
            steelKinds.Add(steelKind);

            steelKind = new SteelKind() { Id = 6, Name = "С355", ElasticModulus = 2.0e11, FstStrength = 3.5e8, SndStrength = 3.55e8 };
            steelKinds.Add(steelKind);

            return steelKinds;
        }
        public static List<ConcreteKind> GetConcreteKinds()
        {
            List<ConcreteKind> concreteKinds = new List<ConcreteKind>();
            ConcreteKind concreteKind;
            #region B10
            concreteKind = new ConcreteKind();
            concreteKind.Id = 2;
            concreteKind.Name = "B10";
            concreteKind.FstCompStrength = 6.0e6;
            concreteKind.FstTensStrength = 0.56e6;
            concreteKind.SndCompStrength = 7.5e6;
            concreteKind.SndTensStrength = 0.85e6;
            concreteKind.Epsb1Comp = 0.00150;
            concreteKind.Epsb1Tens = 0.00008;
            concreteKind.Epsb2Comp = 0.00200;
            concreteKind.Epsb2Tens = 0.00010;
            concreteKind.EpsbMaxComp = 0.00350;
            concreteKind.EpsbMaxTens = 0.00015;
            concreteKind.ElasticModulus = 1.9e10;
            concreteKind.PoissonRatio = 0.2;
            concreteKinds.Add(concreteKind);
            #endregion
            #region B15
            concreteKind = new ConcreteKind();
            concreteKind.Id = 4;
            concreteKind.Name = "B15";
            concreteKind.FstCompStrength = 8.5e6;
            concreteKind.FstTensStrength = 0.75e6;
            concreteKind.SndCompStrength = 11.0e6;
            concreteKind.SndTensStrength = 1.10e6;
            concreteKind.Epsb1Comp = 0.00150;
            concreteKind.Epsb1Tens = 0.00008;
            concreteKind.Epsb2Comp = 0.00200;
            concreteKind.Epsb2Tens = 0.00010;
            concreteKind.EpsbMaxComp = 0.00350;
            concreteKind.EpsbMaxTens = 0.00015;
            concreteKind.ElasticModulus = 2.4e10;
            concreteKind.PoissonRatio = 0.2;
            concreteKinds.Add(concreteKind);
            #endregion
            #region B20
            concreteKind = new ConcreteKind();
            concreteKind.Id = 5;
            concreteKind.Name = "B20";
            concreteKind.FstCompStrength = 11.7e6;
            concreteKind.FstTensStrength = 0.9e6;
            concreteKind.SndCompStrength = 15.0e6;
            concreteKind.SndTensStrength = 1.35e6;
            concreteKind.Epsb1Comp = 0.00150;
            concreteKind.Epsb1Tens = 0.00008;
            concreteKind.Epsb2Comp = 0.00200;
            concreteKind.Epsb2Tens = 0.00010;
            concreteKind.EpsbMaxComp = 0.00350;
            concreteKind.EpsbMaxTens = 0.00015;
            concreteKind.ElasticModulus = 2.75e10;
            concreteKind.PoissonRatio = 0.2;
            concreteKinds.Add(concreteKind);
            #endregion
            #region B25
            concreteKind = new ConcreteKind();
            concreteKind.Id = 6;
            concreteKind.Name = "B25";
            concreteKind.FstCompStrength = 14.5e6;
            concreteKind.FstTensStrength = 1.05e6;
            concreteKind.SndCompStrength = 18.5e6;
            concreteKind.SndTensStrength = 1.55e6;
            concreteKind.Epsb1Comp = 0.00150;
            concreteKind.Epsb1Tens = 0.00008;
            concreteKind.Epsb2Comp = 0.00200;
            concreteKind.Epsb2Tens = 0.00010;
            concreteKind.EpsbMaxComp = 0.00350;
            concreteKind.EpsbMaxTens = 0.00015;
            concreteKind.ElasticModulus = 3.0e10;
            concreteKind.PoissonRatio = 0.2;
            concreteKinds.Add(concreteKind);
            #endregion
            #region B30
            concreteKind = new ConcreteKind();
            concreteKind.Id = 7;
            concreteKind.Name = "B30";
            concreteKind.FstCompStrength = 17.0e6;
            concreteKind.FstTensStrength = 1.15e6;
            concreteKind.SndCompStrength = 22.0e6;
            concreteKind.SndTensStrength = 1.75e6;
            concreteKind.Epsb1Comp = 0.00150;
            concreteKind.Epsb1Tens = 0.00008;
            concreteKind.Epsb2Comp = 0.00200;
            concreteKind.Epsb2Tens = 0.00010;
            concreteKind.EpsbMaxComp = 0.00350;
            concreteKind.EpsbMaxTens = 0.00015;
            concreteKind.ElasticModulus = 3.25e10;
            concreteKind.PoissonRatio = 0.2;
            concreteKinds.Add(concreteKind);
            #endregion
            #region B35
            concreteKind = new ConcreteKind();
            concreteKind.Id = 8;
            concreteKind.Name = "B35";
            concreteKind.FstCompStrength = 19.5e6;
            concreteKind.FstTensStrength = 1.3e6;
            concreteKind.SndCompStrength = 25.5e6;
            concreteKind.SndTensStrength = 1.95e6;
            concreteKind.Epsb1Comp = 0.00150;
            concreteKind.Epsb1Tens = 0.00008;
            concreteKind.Epsb2Comp = 0.00200;
            concreteKind.Epsb2Tens = 0.00010;
            concreteKind.EpsbMaxComp = 0.00350;
            concreteKind.EpsbMaxTens = 0.00015;
            concreteKind.ElasticModulus = 3.45e10;
            concreteKind.PoissonRatio = 0.2;
            concreteKinds.Add(concreteKind);
            #endregion
            #region B40
            concreteKind = new ConcreteKind();
            concreteKind.Id = 9;
            concreteKind.Name = "B40";
            concreteKind.FstCompStrength = 22.0e6;
            concreteKind.FstTensStrength = 1.4e6;
            concreteKind.SndCompStrength = 29.0e6;
            concreteKind.SndTensStrength = 2.1e6;
            concreteKind.Epsb1Comp = 0.00150;
            concreteKind.Epsb1Tens = 0.00008;
            concreteKind.Epsb2Comp = 0.00200;
            concreteKind.Epsb2Tens = 0.00010;
            concreteKind.EpsbMaxComp = 0.00350;
            concreteKind.EpsbMaxTens = 0.00015;
            concreteKind.ElasticModulus = 3.6e10;
            concreteKind.PoissonRatio = 0.2;
            concreteKinds.Add(concreteKind);
            #endregion
            return concreteKinds;
        }
        public static List<ReinforcementKind> GetReinforcementKinds()
        {
            List<ReinforcementKind> reinforcementKinds = new List<ReinforcementKind>();
            #region А400
            ReinforcementKind reinforcementKind;
            reinforcementKind = new ReinforcementKind();
            reinforcementKind.Id = 2;
            reinforcementKind.Name = "A400";
            reinforcementKind.FstCompStrength = 350e6;
            reinforcementKind.FstTensStrength = 350e6;
            reinforcementKind.SndCompStrength = 400e6;
            reinforcementKind.SndTensStrength = 400e6;
            reinforcementKind.ElasticModulus = 2e11;
            reinforcementKinds.Add(reinforcementKind);
            #endregion
            #region А500
            reinforcementKind = new ReinforcementKind();
            reinforcementKind.Id = 3;
            reinforcementKind.Name = "A500";
            reinforcementKind.FstCompStrength = 435e6;
            reinforcementKind.FstTensStrength = 435e6;
            reinforcementKind.SndCompStrength = 500e6;
            reinforcementKind.SndTensStrength = 500e6;
            reinforcementKind.ElasticModulus = 2e11;
            reinforcementKinds.Add(reinforcementKind);
            #endregion
            return reinforcementKinds;
        }
    }
}
