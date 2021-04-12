using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CSL.Reports.Interfaces
{
    interface IReport
    {
        DataSet dataSet { get; set; }
        void ShowReport(string fileName);
    }
}
