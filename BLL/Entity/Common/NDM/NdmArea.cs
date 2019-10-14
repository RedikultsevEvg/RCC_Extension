using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace RDBLL.Entity.Common.NDM
{
    public class NdmArea
    {
        public double Area { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double ElasticModulus { get; set; }
        //public ObservableCollection<double> CompressiveStengthes { get; set; }
        //public ObservableCollection<double> TensionStengthes { get; set; }
        //public ObservableCollection<double> Srains { get; set; }
        public NdmArea()
        {

        }
    }
}
