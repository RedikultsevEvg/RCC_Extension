using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column
{
    public class SteelBaseSubPart
    {
        public int Id { get; set; }
        public double Area { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Strain { get; set; }
        public double Stress { get; set; }
        public double Force { get; set; }

        public void SetStress()
        {
            double elasticModulus = 3e+10;
            if (Strain>0) {Stress = Strain * elasticModulus; }
            else { Stress = 0; }
        }

        public void SetForce()
        {
            SetStress();
            Force = Stress * Area;
        }
    }
}
