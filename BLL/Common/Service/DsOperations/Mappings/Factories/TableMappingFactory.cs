using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations.Mappings.Factories
{
    internal static class TableMappingFactory
    {
        public static List<TableMapping> GetTableMappings()
        {
            List<TableMapping> tableMappings = new List<TableMapping>();
            TableMapping tableMapping;
            tableMapping = new TableMapping() { EntityName = "RectFoundationPart", TableName = "FoundationParts" };
            tableMappings.Add(tableMapping);
            tableMapping = new TableMapping() { EntityName = "LineBySpacing", TableName = "ParametricObjects" };
            tableMappings.Add(tableMapping);
            tableMapping = new TableMapping() { EntityName = "RectArrayPlacement", TableName = "ParametricObjects" };
            tableMappings.Add(tableMapping);
            tableMapping = new TableMapping() { EntityName = "PatternType1", TableName = "ParametricObjects" };
            tableMappings.Add(tableMapping);
            tableMapping = new TableMapping() { EntityName = "PatternType2", TableName = "ParametricObjects" };
            tableMappings.Add(tableMapping);
            tableMapping = new TableMapping() { EntityName = "PatternType3", TableName = "ParametricObjects" };
            tableMappings.Add(tableMapping);
            tableMapping = new TableMapping() { EntityName = "ReinforcementUsing", TableName = "MaterialUsings" };
            tableMappings.Add(tableMapping);
            tableMapping = new TableMapping() { EntityName = "ConcreteUsing", TableName = "MaterialUsings" };
            tableMappings.Add(tableMapping);
            tableMapping = new TableMapping() { EntityName = "SteelUsing", TableName = "MaterialUsings" };
            tableMappings.Add(tableMapping);
            return tableMappings;
        }
    }
}
