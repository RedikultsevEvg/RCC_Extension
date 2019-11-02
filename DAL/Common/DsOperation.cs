using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DAL.Common
{
    public static class DsOperation
    {
        public static void AddIdColumn(DataTable dataTable)
        {
            DataColumn IdColumn;
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            IdColumn.Unique = true;
            dataTable.Columns.Add(IdColumn);
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["Id"] };
        }
        public static void AddFkIdColumn(string parentDataTableName, string parentColumnName, DataTable childDataTable)
        {
            DataSet dataSet = childDataTable.DataSet;
            DataTable parentDataTable = dataSet.Tables[parentDataTableName];
            string columnName = parentColumnName;
            DataColumn FkIdColumn;
            FkIdColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            childDataTable.Columns.Add(FkIdColumn);
            ForeignKeyConstraint foreignKey;
            foreignKey = new ForeignKeyConstraint(parentDataTable.Columns["Id"], childDataTable.Columns[columnName])
            {
                ConstraintName = parentDataTableName + childDataTable.TableName + "ForeignKey",
                DeleteRule = Rule.SetNull,
                UpdateRule = Rule.Cascade
            };
            childDataTable.Constraints.Add(foreignKey);
            dataSet.EnforceConstraints = true;
            dataSet.Relations.Add(parentDataTable.TableName + childDataTable.TableName, parentDataTable.Columns["Id"], childDataTable.Columns[columnName]);
        }
        public static void AddNameColumn(DataTable dataTable)
        {
            DataColumn NameColumn;
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            dataTable.Columns.Add(NameColumn);
        }
        public static void AddIntColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            dataTable.Columns.Add(NewColumn);
        }
        public static void AddDoubleColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
        }
        public static void AddBoolColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Boolean"));
            dataTable.Columns.Add(NewColumn);
        }
    }
}
