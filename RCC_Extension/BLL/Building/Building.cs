using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCC_Extension.BLL.Building
{
    class Building : ICloneable
    {
        public string Name { get; set; }
        public List<Level> LevelList { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
