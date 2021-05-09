using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Results.Factories
{
    public interface IPunchingResultBuilder
    {
        PunchingResult GetPunchingResult();
    }
}
