using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM
{
    public class NdmArea
    {
        private double _ElasticModulus;

        public double Area { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double ElasticModulus
        { get {return _ElasticModulus; }
            set
            {
                _ElasticModulus = value;
                MaterialModel.ElasticModulus = _ElasticModulus;
            }
        }
        public IMaterialModel MaterialModel { get; set; }

        //public ObservableCollection<double> CompressiveStengthes { get; set; }
        //public ObservableCollection<double> TensionStengthes { get; set; }
        //public ObservableCollection<double> Srains { get; set; }
        public NdmArea(IMaterialModel materialModel)
        {
            MaterialModel = materialModel;
        }
        public double GetSecantModulus(double epsilon)
        {
            double secantModulus = MaterialModel.GetSecantModulus(epsilon);
            return secantModulus;
        }
    }
}
