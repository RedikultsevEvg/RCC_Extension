using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastReport;

namespace CSL.Reports
{
    public static class ResultReport
    {
        public static void ShowReport()
        {
            using (Report report = new Report())
            {
                report.Design();
                report.Show();
            }
        }
    }
}
