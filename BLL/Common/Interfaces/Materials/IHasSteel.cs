using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Materials
{
    public interface IHasSteel
    {
        SteelUsing Steel { get; set; }
    }
}
