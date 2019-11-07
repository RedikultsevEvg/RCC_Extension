using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM.MaterialModels
{
    public class LinearTensioned : IMaterialModel
    {
        public double ElasticModulus { get; set; }

        public double GetStress(double Epsilon)
        {
            if (Epsilon > 0)
            { return ElasticModulus * Epsilon; }
            else { return ElasticModulus/1000000*Epsilon; }
        }

        public double GetSecantModulus(double Epsilon)
        {
            if (Epsilon > 0)
            { return ElasticModulus; }
            else { return ElasticModulus / 1000000; }
        }
    }
}
