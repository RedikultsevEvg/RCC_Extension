using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.NDM.Interfaces
{
    public interface IMaterialModel
    {
        double ElasticModulus {get;set;}

        double GetStress(double Epsilon);
        double GetSecantModulus(double Epsilon);
    }
}
