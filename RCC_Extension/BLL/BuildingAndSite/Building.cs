using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.WallAndColumn;

namespace RCC_Extension.BLL.BuildingAndSite
{
    public class Building : ICloneable
    {
        public string Name { get; set; }
        public List<Level> LevelList { get; set; }
        public List<WallType> WallTypeList { get; set; }

        public Building(BuildingSite buildingSite)
        {         
            Name = "Мое здание";
            LevelList = new List<Level>();
            WallTypeList = new List<WallType>();
            buildingSite.BuildingList.Add(this);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
