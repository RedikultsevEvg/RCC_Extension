using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations
{
    internal class ColumnTemplate
    {
        public string ColumnName { get; set; }
        public Type ColumnType { get; set; }
        public object DefaultValue { get; set; }
    }
}
