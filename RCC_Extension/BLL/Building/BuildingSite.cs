using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCC_Extension.BLL.Building
{
    public class BuildingSite :ICloneable
    {
        public string Name { get; set; }
        public List<Building> BuildingList { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
