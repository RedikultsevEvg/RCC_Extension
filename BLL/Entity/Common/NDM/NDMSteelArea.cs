using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.MaterialModels;

namespace RDBLL.Entity.Common.NDM
{
    public class NdmSteelArea
    {
        private double _Diametr;
        private NdmArea _NdmArea { get; set; }
        public double Diametr
        {
            get { return _Diametr; }
            set
            {
                _Diametr = value;
                _NdmArea.Area = _Diametr * _Diametr * 0.785;
            }
        }

        public NdmArea SteelArea { get { return _NdmArea; } set { _NdmArea = value; } }
        public NdmSteelArea()
        {
            SteelArea = new NdmArea(new LinearTensioned());
            //SteelArea = new NdmArea(new LinearIsotropic());
            SteelArea.ElasticModulus = 2e+11;
        }
    }
}
