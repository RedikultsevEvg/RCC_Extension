using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.NDM
{
    public class NdmConcreteArea
    {
        private double _Width;
        private double _Length;
        private NdmArea _NdmArea { get; set; }
        public double Width
        { get { return _Width; }
            set
            {
                _Width = value;
                _NdmArea.Area = _Width * _Length;
            }
        }
        public double Length
        {
            get { return _Length; }
            set
            {
                _Length = value;
                _NdmArea.Area = _Width * _Length;
            }
        }
        public NdmArea ConcreteArea { get { return _NdmArea; } set {_NdmArea = value; } }
        public NdmConcreteArea()
        {
            ConcreteArea = new NdmArea();
            ConcreteArea.ElasticModulus = 3e+10;
        }
    }
}
