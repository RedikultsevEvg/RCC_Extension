using RDBLL.Common.ErrorProcessing.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.BuildingAndSite.Factories
{
    public enum BuildingType
    {
        SimpleType
    }

    public static class BuildingFactory
    {
        public static Building GetBuilding(BuildingType type)
        {
            if (type == BuildingType.SimpleType)
            {
                Building building = new Building(true);
                building.RelativeLevel = 0.000;
                building.AbsoluteLevel = 260;
                building.AbsolutePlaningLevel = 259.5;
                building.MaxFoundationSettlement = 0.08;
                building.IsRigid = false;
                building.RigidRatio = 4;
                return building;
            }
            else
            {
                throw new Exception(CommonMessages.TypeIsUknown);
            }
        }
    }
}
