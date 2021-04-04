using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Placements.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Factories
{
    public enum BoltType
    {
        Array2x2,
    }
    public static class SteelBoltFactory
    {
        public static SteelBolt BoltFactory (BoltType type)
        {
            switch (type)
            {
                case BoltType.Array2x2:
                    {
                        SteelBolt steelBolt = new SteelBolt(true);
                        steelBolt.Name = "Болт №_";
                        SteelUsing steel = new SteelUsing(steelBolt);
                        steel.Name = "Сталь";
                        steel.Purpose = "BoltSteel";
                        steel.SelectedId = ProgrammSettings.SteelKinds[0].Id;
                        steelBolt.Steel = steel;
                        steelBolt.Diameter = 0.03;
                        RectArrayPlacement placement = PlacementFactory.GetPlacement(PcmType.Rect2x2x0) as RectArrayPlacement;
                        placement.RegisterParent(steelBolt);
                        steelBolt.SetPlacement(placement);
                        return steelBolt;
                    }
                default: return null;
            }
        }
    }
}
