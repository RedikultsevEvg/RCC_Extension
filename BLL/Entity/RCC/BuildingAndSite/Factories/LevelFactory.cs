using RDBLL.Common.ErrorProcessing.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.BuildingAndSite.Factories
{
    public enum LevelType
    {
        SimpleType
    }
    public static class LevelFactory
    {
        public static Level GetLevel(LevelType type)
        {
            if (type == LevelType.SimpleType)
            {
                return GetSimpleType();
            }
            else
            {
                throw new Exception(CommonMessages.TypeIsUknown);
            }
        }

        private static Level GetSimpleType()
        {
            Level level = new Level(true);
            level.Name = "Этаж 1";
            level.Elevation = 0;
            level.Height = 3;
            level.TopOffset = -0.2;
            return level;
        }
    }
}
