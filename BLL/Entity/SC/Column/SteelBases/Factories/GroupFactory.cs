using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Factories
{
    public enum GroupType
    {
        Type1,
    }
    public static class GroupFactory
    {
        public static SteelBasePartGroup GetSteelBasePartGroup(GroupType type)
        {
            switch (type)
            {
                case GroupType.Type1:
                    {
                        SteelBasePartGroup partGroup = new SteelBasePartGroup(true);
                        partGroup.Name = "Новая группа участков";
                        partGroup.Pressure = 1e7;
                        partGroup.Height = 0.06;
                        //Добавляем материалы
                        MatFactProc.GetMatType(partGroup, MatType.SteelBasePartGroup);
                        SteelBasePart part = new SteelBasePart(partGroup);
                        part.Name = "Новый участок";
                        part.RightOffset = 0.008;
                        part.LeftOffset = 0.008;
                        part.TopOffset = 0.008;
                        part.BottomOffset = 0.008;
                        part.FixRight = true;
                        part.FixLeft = true;
                        part.FixTop = true;
                        part.FixBottom = true;
                        return partGroup;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
