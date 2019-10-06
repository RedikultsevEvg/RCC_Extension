using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.SC.Column;

namespace RDBLL.Processors.SC
{
    public class SteelBoltProcessor
    {
        public static List<SteelBolt> GetSteelBoltsFromBolt (SteelBolt steelBolt)
        {
            List<SteelBolt> steelBolts = new List<SteelBolt>();
            steelBolts.Add(steelBolt);
            if (steelBolt.AddSymmetricX)
            {
                SteelBolt newSteelBolt = (SteelBolt)steelBolt.Clone();
                newSteelBolt.CoordX = (-1.0) * steelBolt.CoordX;
                steelBolts.Add(newSteelBolt);
            }
            if (steelBolt.AddSymmetricY)
            {
                SteelBolt newSteelBolt = (SteelBolt)steelBolt.Clone();
                newSteelBolt.CoordY = (-1.0) * steelBolt.CoordY;
                steelBolts.Add(newSteelBolt);
            }
            if (steelBolt.AddSymmetricX & steelBolt.AddSymmetricY)
            {
                SteelBolt newSteelBolt = (SteelBolt)steelBolt.Clone();
                newSteelBolt.CoordX = (-1.0) * steelBolt.CoordX;
                newSteelBolt.CoordY = (-1.0) * steelBolt.CoordY;
                steelBolts.Add(newSteelBolt);
            }
            return steelBolts;
        }
    }
}
