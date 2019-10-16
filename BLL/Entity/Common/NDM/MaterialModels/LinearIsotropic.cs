using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM.MaterialModels
{
    public class LinearIsotropic :IMaterialModel
    {
        public double ElasticModulus { get; set; }

        public double GetStress(double Epsilon)
        {
            return ElasticModulus * Epsilon;
        }

        public double GetSecantModulus(double Epsilon)
        {
            return ElasticModulus;
        }
    }
}
