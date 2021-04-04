using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Factories
{
    public static class MatFactProc
    {
        public static void GetMatType1(SteelBase steelBase)
        {
            SteelUsing steel = new SteelUsing(steelBase);
            steel.Name = "Сталь";
            steel.Purpose = "BaseSteel";
            steel.SelectedId = ProgrammSettings.SteelKinds[0].Id;
            steelBase.Steel = steel;
            ConcreteUsing concrete = new ConcreteUsing(steelBase);
            concrete.Name = "Бетон";
            concrete.Purpose = "Filling";
            concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
            concrete.AddGammaB1();
            steelBase.Concrete = concrete;
        }
    }
}
