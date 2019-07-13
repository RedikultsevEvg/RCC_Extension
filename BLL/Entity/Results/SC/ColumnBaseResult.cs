using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using RDBLL.Entity.Results.Forces;

namespace RDBLL.Entity.Results.SC
{
    public class ColumnBaseResult
    {
        public List<BarLoadSet> LoadCases { get; set; }
        public MinMaxStressInRect Stresses { get; set; }
        public double BoltForce { get; set; }
        
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ColumnBaseResult()
        {
            Stresses = new MinMaxStressInRect();
        }
    }
}
