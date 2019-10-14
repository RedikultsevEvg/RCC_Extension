using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.NDM
{
    public class StiffnessCoefficient
    {
        public double[,] Coefficients { get; set; } 
        public StiffnessCoefficient()
        {
            Coefficients = new double[3, 3];
        }
    }
}
