using RDBLL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Forces.Processors
{
    public static class ForcesCloneProcessor
    {
        public static void CloneForceCollection (IHasForcesGroups source, IHasForcesGroups newObject)
        {
            //На случай если источники и приемник являются одним и тем же объектом
            List<ForcesGroup> forces = new List<ForcesGroup>();
            foreach (ForcesGroup forcesGroup in source.ForcesGroups)
            {
                forces.Add(forcesGroup.Clone() as ForcesGroup);
            }
            newObject.ForcesGroups = new ObservableCollection<ForcesGroup>();
            foreach (ForcesGroup forcesGroup in forces)
            {
                forcesGroup.Owner.Add(newObject);
                newObject.ForcesGroups.Add(forcesGroup);
            }
        }

    }
}
