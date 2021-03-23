using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Factorys
{
    internal static class BoltFactProc
    {
        public static void GetBoltsType1(SteelBase steelBase, double width, double length, int quantityX, int quantityY)
        {
            steelBase.SteelBolts = new ObservableCollection<SteelBolt>();
            SteelBolt steelBolt = SteelBoltFactory.BoltFactory(BoltType.Array2x2);
            RectArrayPlacement placement = steelBolt.Placement as RectArrayPlacement;
            placement.SizeX = width;
            placement.SizeY = length;
            placement.QuantityX = quantityX;
            placement.QuantityY = quantityY;
            steelBolt.RegisterParent(steelBase);
        }
    }
}
