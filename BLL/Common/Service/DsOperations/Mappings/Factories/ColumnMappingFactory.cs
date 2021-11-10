using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations.Mappings.Factories
{
    internal static class ColumnMappingFactory
    {
        public static IEnumerable<ColumnMapping> GetColumnMappings()
        {
            List<ColumnMapping> columnMappings = new List<ColumnMapping>();
            ColumnMapping columnMapping;
            columnMapping = new ColumnMapping() { PropertyName = "Circle", ColumnName = "ICircle" };
            columnMappings.Add(columnMapping);
            return columnMappings;
        }
    }
}
