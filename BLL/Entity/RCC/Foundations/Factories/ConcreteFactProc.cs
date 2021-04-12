using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Factories
{
    public static class ConcreteFactProc
    {
        public static void AddConcrete(Foundation foundation)
        {
            foundation.Concrete = new ConcreteUsing(true);
            foundation.Concrete.RegisterParent(foundation);
            foundation.Concrete.Name = "Бетон";
            foundation.Concrete.Purpose = "MainConcrete";
            foundation.Concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
            foundation.Concrete.AddGammaB1();
        }
    }
}
