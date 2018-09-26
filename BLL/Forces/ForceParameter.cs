using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Forces
{
    public class ForceParameter
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public ForceParamKind ForceParamKind { get; set; }
        public List<ForceParamKind> ForceParamKinds { get; set; }

        public ForceParameter()
        {
            ForceParamKinds = ProgrammSettings.ForceParamKinds;
        }
    }


}
