using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;

namespace RDBLL.Entity.Results.SC
{
    public class ColumnBaseResult
    {
        public List<BarLoadSet> LoadCases { get; set; }
        public double MinStress { get; set; }
        public double MaxStress { get; set; }
        public double BoltForce { get; set; }
    }
}
