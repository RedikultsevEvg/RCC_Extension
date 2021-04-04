using RDBLL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Forces.Factories
{
    public enum ForceType
    {
        N100MX200,
        N100MX200MY50,
    }
    public static class Factory
    {
        public static void ForceGroupFactory(IHasForcesGroups owner, ForceType type)
        {
            switch (type)
            {
                case ForceType.N100MX200:
                    {
                        LoadSet loadSet = new LoadSet(owner.ForcesGroups[0]);
                        owner.ForcesGroups[0].LoadSets.Add(loadSet);
                        loadSet.Name = "Постоянная";
                        loadSet.ForceParameters.Add(new ForceParameter(loadSet));
                        loadSet.ForceParameters[0].KindId = 1; //Продольная сила
                        loadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
                        loadSet.ForceParameters.Add(new ForceParameter(loadSet));
                        loadSet.ForceParameters[1].KindId = 2; //Изгибающий момент
                        loadSet.ForceParameters[1].CrcValue = 200000; //Изгибающий момент
                        loadSet.IsLiveLoad = false;
                        loadSet.BothSign = false;
                        loadSet.PartialSafetyFactor = 1.1;
                        break;
                    }
                case ForceType.N100MX200MY50:
                    {
                        LoadSet loadSet = new LoadSet(owner.ForcesGroups[0]);
                        owner.ForcesGroups[0].LoadSets.Add(loadSet);
                        loadSet.Name = "Постоянная";
                        loadSet.ForceParameters.Add(new ForceParameter(loadSet));
                        loadSet.ForceParameters[0].KindId = 1; //Продольная сила
                        loadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
                        loadSet.ForceParameters.Add(new ForceParameter(loadSet));
                        loadSet.ForceParameters[1].KindId = 2; //Изгибающий момент
                        loadSet.ForceParameters[1].CrcValue = 200000; //Изгибающий момент
                        loadSet.ForceParameters.Add(new ForceParameter(loadSet));
                        loadSet.ForceParameters[2].KindId = 3; //Изгибающий момент
                        loadSet.ForceParameters[3].CrcValue = 50000; //Изгибающий момент
                        loadSet.IsLiveLoad = false;
                        loadSet.BothSign = false;
                        loadSet.PartialSafetyFactor = 1.1;
                        break;
                    }
                default: break;
            }
        }
    }
}
