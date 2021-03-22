using RDBLL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials.SteelMaterialUsing
{
    /// <summary>
    /// Использование стали по классу
    /// </summary>
    public class SteelUsing : MaterialUsing
    {
        public SteelUsing(bool genId = false) : base(genId)
        {

        }
        public SteelUsing(IDsSaveable parentMember) : base(parentMember)
        {

        }
    }
}
