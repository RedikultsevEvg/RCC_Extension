using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Forces
{
    public class ForceParameter
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public ForceParamKind ForceParamKind { get; set; }
    }
}
