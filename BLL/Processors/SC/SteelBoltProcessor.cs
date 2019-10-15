using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.Common.NDM;

namespace RDBLL.Processors.SC
{
    public static class SteelBoltProcessor
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

        public static void GetSubParts(SteelBolt steelBolt)
        {
            steelBolt.SubPart = new NdmSteelArea();
            steelBolt.SubPart.Diametr = steelBolt.Diameter;
            steelBolt.SubPart.SteelArea.CenterX = steelBolt.CenterX;
            steelBolt.SubPart.SteelArea.CenterY = steelBolt.CenterY;
        }
    }
}
