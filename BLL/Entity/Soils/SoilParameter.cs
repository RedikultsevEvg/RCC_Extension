using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soils
{
    public class SoilParameter
    {
        public int Id;
        public SoilParameterKind SoilParameterKind;
        public double CrcValue;
        public double FirstDesValue;
        public double ScndDesValue;
    }
}
