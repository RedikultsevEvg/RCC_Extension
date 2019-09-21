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
        public double Coord_X { get; set; }
        public double Coord_Y { get; set; }
        public ObservableCollection<double> CompressiveStengthes { get; set; }
        public ObservableCollection<double> TensionStengthes { get; set; }
        public ObservableCollection<double> Srains { get; set; }
    }
}
