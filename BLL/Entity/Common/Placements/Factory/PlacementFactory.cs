using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Placements.Factory
{
    public enum PcmType
    {
        Rect2x2x0,
        Rect4x4x0,
        Rect4x4x1,
    }

    public static class PlacementFactory
    {
        public static Placement GetPlacement(PcmType type)
        {
            switch (type)
            {
                case PcmType.Rect2x2x0 :
                    {
                        RectArrayPlacement placement = new RectArrayPlacement(true);
                        placement.Name = "Новый массив";
                        placement.OffSet = 0;
                        placement.SetCenter(0, 0);
                        placement.QuantityX = 2;
                        placement.QuantityY = 2;
                        placement.FillArray = false;
                        return placement;
                    }
                case PcmType.Rect4x4x0:
                    {
                        RectArrayPlacement placement = new RectArrayPlacement(true);
                        placement.Name = "Новый массив";
                        placement.OffSet = 0;
                        placement.SetCenter(0, 0);
                        placement.QuantityX = 4;
                        placement.QuantityY = 4;
                        placement.FillArray = false;
                        return placement;
                    }
                case PcmType.Rect4x4x1:
                    {
                        RectArrayPlacement placement = new RectArrayPlacement(true);
                        placement.Name = "Новый массив";
                        placement.OffSet = 0;
                        placement.SetCenter(0, 0);
                        placement.QuantityX = 4;
                        placement.QuantityY = 4;
                        placement.FillArray = true;
                        return placement;
                    }
                default: return null;
            }

        }
    }
}
