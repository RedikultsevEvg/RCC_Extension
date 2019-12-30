using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM
{
    public abstract class NdmArea
    {
        public double Area { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public IMaterialModel MaterialModel { get; set; }

        public double GetSecantModulus(double epsilon)
        {
            double secantModulus = MaterialModel.GetSecantModulus(epsilon);
            return secantModulus;
        }
    }
}
