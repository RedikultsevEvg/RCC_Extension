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
                newSteelBolt.CenterX = (-1.0) * steelBolt.CenterX;
                steelBolts.Add(newSteelBolt);
            }
            if (steelBolt.AddSymmetricY)
            {
                SteelBolt newSteelBolt = (SteelBolt)steelBolt.Clone();
                newSteelBolt.CenterY = (-1.0) * steelBolt.CenterY;
                steelBolts.Add(newSteelBolt);
            }
            if (steelBolt.AddSymmetricX & steelBolt.AddSymmetricY)
            {
                SteelBolt newSteelBolt = (SteelBolt)steelBolt.Clone();
                newSteelBolt.CenterX = (-1.0) * steelBolt.CenterX;
                newSteelBolt.CenterY = (-1.0) * steelBolt.CenterY;
                steelBolts.Add(newSteelBolt);
            }
            return steelBolts;
        }
    }
}
