using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Builders
{
    public abstract class BuilderBase
    {
        
        public BuilderBase()
        {

        }
        public abstract void CreateGeometry();
        public abstract void AddParts();
        public abstract void AddBolts();
        public abstract void AddLoads();
        public abstract SteelBase GetSteelBase();
    }
}
