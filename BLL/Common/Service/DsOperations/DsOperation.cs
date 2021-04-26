using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using RDBLL.Common.Interfaces;

namespace RDBLL.Common.Service.DsOperations
{
    public enum TableNames
    {
        Fondations,
        FoundationParts,
    }
    public enum FieldsNames
    {
        Id,
        Name,
        ParentId,
    }
    /// <summary>
    /// Базовые операции с датасетами
    /// </summary>
    public static class DsOperation
    {
        /// <summary>
        /// Добавляет столбец с кодом в таблицу
        /// </summary>
        /// <param name="dataTable">Таблица датасета</param>
        /// <param name="addNameColumn">Флаг необходимости добавления столбца с наименованием</param>
        public static List<DataColumn> AddIdColumn(DataTable dataTable, bool addNameColumn = false)
        {
            List<DataColumn> dataColumns = new List<DataColumn>();
            DataColumn IdColumn;
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            IdColumn.ColumnMapping = MappingType.Attribute;
            //IdColumn.Unique = true;
            dataTable.Columns.Add(IdColumn);
            dataColumns.Add(IdColumn);
            //dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["Id"] };
            if (addNameColumn) { dataColumns.Add(AddNameColumn(dataTable)); }
            return dataColumns;
        }
        /// <summary>
        /// Добавление внешного ключа
        /// </summary>
        /// <param name="parentDataTableName">Таблица данных</param>
        /// <param name="parentColumnName">Наименование столбца в дочерней таблице</param>
        /// <param name="childDataTable">Наименование дочерней таблицы</param>
        /// <param name="allowNull">Флаг допустимости Null</param>
        public static DataColumn AddFkIdColumn(string parentDataTableName, string parentColumnName, DataTable childDataTable, bool allowNull = false)
        {
            DataSet dataSet = childDataTable.DataSet;
            DataTable parentDataTable = dataSet.Tables[parentDataTableName];
            string columnName = parentColumnName;
            DataColumn FkIdColumn;
            FkIdColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            FkIdColumn.ColumnMapping = MappingType.Attribute;
            childDataTable.Columns.Add(FkIdColumn);
            ForeignKeyConstraint foreignKey;
            //Если допускается нулевое значение, то при удалении устанавливаем в нулл
            if (allowNull)
            {
                foreignKey = new ForeignKeyConstraint(parentDataTable.Columns["Id"], childDataTable.Columns[columnName])
                {
                    ConstraintName = parentDataTableName + childDataTable.TableName + "ForeignKey",
                    DeleteRule = Rule.SetNull,
                    UpdateRule = Rule.Cascade
                };
            }
            //Иначе при удалении удаляем каскадно
            else
            {
                foreignKey = new ForeignKeyConstraint(parentDataTable.Columns["Id"], childDataTable.Columns[columnName])
                {
                    ConstraintName = parentDataTableName + childDataTable.TableName + "ForeignKey",
                    DeleteRule = Rule.Cascade,
                    UpdateRule = Rule.Cascade
                };
            }
            childDataTable.Constraints.Add(foreignKey);
            dataSet.EnforceConstraints = true;
            dataSet.Relations.Add(parentDataTable.TableName + childDataTable.TableName, parentDataTable.Columns["Id"], childDataTable.Columns[columnName]);

            return FkIdColumn;
        }
        public static DataColumn AddFkIdColumn(string parentColumnName, DataTable childDataTable, bool allowNull = false)
        {
            DataSet dataSet = childDataTable.DataSet;
            string columnName = parentColumnName;
            DataColumn FkIdColumn;
            FkIdColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            FkIdColumn.AllowDBNull = allowNull;
            FkIdColumn.ColumnMapping = MappingType.Attribute;
            childDataTable.Columns.Add(FkIdColumn);
            return FkIdColumn;
        }
        private static DataColumn AddNameColumn(DataTable dataTable)
        {
            DataColumn NameColumn;
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            NameColumn.ColumnMapping = MappingType.Attribute;
            dataTable.Columns.Add(NameColumn);
            return NameColumn;
        }
        public static List<DataColumn> AddIdNameParentIdColumn(DataTable dataTable, string parentTable = null)
        {
            List<DataColumn> dataColumns = new List<DataColumn>();
            dataColumns.AddRange(AddIdColumn(dataTable, true));
            if (string.IsNullOrEmpty(parentTable))
            {
                DataColumn ParentIdColumn;
                ParentIdColumn = new DataColumn("ParentId", Type.GetType("System.Int32"));
                ParentIdColumn.ColumnMapping = MappingType.Attribute;
                dataTable.Columns.Add(ParentIdColumn);
                dataColumns.Add(ParentIdColumn);
            }
            else dataColumns.Add(AddFkIdColumn(parentTable, "ParentId", dataTable));
            return dataColumns;
        }
        public static void AddIntColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            dataTable.Columns.Add(NewColumn);
        }
        public static DataColumn AddIntColumn(DataTable dataTable, string columnName, int defaultValue)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            NewColumn.AllowDBNull = false;
            NewColumn.DefaultValue = defaultValue;
            dataTable.Columns.Add(NewColumn);
            return NewColumn;
        }
        public static DataColumn AddDoubleColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            return NewColumn;
        }
        public static DataColumn AddDoubleColumn(DataTable dataTable, string columnName, double defaultValue)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Double"));
            NewColumn.AllowDBNull = false;
            NewColumn.DefaultValue = defaultValue;
            dataTable.Columns.Add(NewColumn);
            return NewColumn;
        }
        public static void AddBoolColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Boolean"));
            dataTable.Columns.Add(NewColumn);
        }
        public static void AddBoolColumn(DataTable dataTable, string columnName, bool defaultValue)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Boolean"));
            NewColumn.AllowDBNull = false;
            NewColumn.DefaultValue = defaultValue;
            dataTable.Columns.Add(NewColumn);
        }
        /// <summary>
        /// Добавляет столбец типа String в таблицу датасета
        /// </summary>
        /// <param name="dataTable">Таблица датасета</param>
        /// <param name="columnName">Имя создаваемого столбца</param>
        public static void AddStringColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.String"));
            dataTable.Columns.Add(NewColumn);
        }
        public static void AddByteColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Byte[]"));
            dataTable.Columns.Add(NewColumn);
        }
        public static void DeleteRow(DataSet dataSet, string dataTableName, int Id)
        {
            DeleteRow(dataSet, dataTableName, "Id", Id);
        }
        public static void DeleteRow(DataSet dataSet, string dataTableName, string keyFieldName, int Id)
        {
            DataTable dataTable = dataSet.Tables[dataTableName];
            DataRow[] rows = dataTable.Select($"{keyFieldName}={Id}");
            int count = rows.Length;
            for (int j = count - 1; j >= 0; j--)
            {
                dataTable.Rows.Remove(rows[j]);
            }
        }
        /// <summary>
        /// Возвращает строку из датасета с указанным Id
        /// </summary>
        /// <param name="Id">Код элемента</param>
        /// <param name="createNew">Флаг необходимости создания нового элемента</param>
        /// <param name="dataTable">Ссылка на таблицу в которой ищется запись</param>
        /// <param name=""></param>
        /// <returns></returns>
        public static DataRow CreateNewRow (int Id, bool createNew, DataTable dataTable)
        {
            DataRow row;
            //Если флаг создания новой записи установлен
            if (createNew)
            {
                row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            //Иначе
            else
            {
                try
                {
                    //Находим нужную запись

                    var oldRow = (from dataRow in dataTable.AsEnumerable()
                                  where dataRow.Field<int>("Id") == Id
                                  select dataRow).Single();
                    row = oldRow;
                }
                catch //Если нужная запись не находится
                {
                    //Добавляем ее
                    row = dataTable.NewRow();
                    dataTable.Rows.Add(row);
                }
            }
            return row;
        }
        /// <summary>
        /// Возвращает строку из датасета по Id
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// /// <param name="dataTableName">Имя таблицы</param>
        /// <param name="Id">Код элемента</param>
        /// <returns></returns>
        public static DataRow OpenFromDataSetById (DataSet dataSet, string dataTableName, int Id)
        {
            DataTable dataTable = dataSet.Tables[dataTableName];
            var row = (from dataRow in dataTable.AsEnumerable()
                       where dataRow.Field<int>("Id") == Id
                       select dataRow).Single();
            return row;
        }
        /// <summary>
        /// Возвращает строку из датасета по элементу (необходимо для отката изменений)
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static DataRow OpenFromDataSetById(DataSet dataSet, IDsSaveable item)
        {
            return OpenFromDataSetById(dataSet, item.GetTableName(), item.Id);
        }
        public static void SetId(DataRow dataRow, int id, string name = null, int? parentId = null)
        {
            dataRow.SetField("Id", id);
            if (name != null) dataRow.SetField("Name", name);
            if (parentId != null) dataRow.SetField("ParentId", parentId);
        }
        public static object GetCoolection(DataSet dataSet, string tableName, IDsSaveable parent)
        {
            DataTable dataTable = dataSet.Tables[tableName];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("ParentId") == parent.Id
                        select dataRow;
            return query;
        }
        public static void SetField<T>(DataRow row, string columnName, T value)
        {
            DataTable table = row.Table;
            if (! table.Columns.Contains(columnName))
            {
                DataColumn column = new DataColumn(columnName, typeof(T));
                table.Columns.Add(column);
            }
            row.SetField<T>(columnName, value);
        }
        public static void Field<T>(DataRow row, ref T target, string columnName, T defaultValue)
        {
            DataTable table = row.Table;
            if (!table.Columns.Contains(columnName))
            {
                DataColumn column = new DataColumn(columnName, typeof(T));
                column.DefaultValue = defaultValue;
                table.Columns.Add(column);
            }
            target = row.Field<T>(columnName);
        }

        //public static ValueType GetField<T>(DataRow row, string columnName, T defaultValue)
        //{
        //    DataTable table = row.Table;
        //    if (!table.Columns.Contains(columnName))
        //    {
        //        DataColumn column = new DataColumn(columnName, typeof(T));
        //        column.DefaultValue = defaultValue;
        //        table.Columns.Add(column);
        //    }
        //    return row.Field<T>(columnName);
        //}
        /// <summary>
        /// Возвращает таблицу если она имеется в датасете или создает новую
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(DataSet dataSet, string tableName)
        {
            DataTable dataTable;
            //Если датасет содержит нужную таблицу, то получаем ее
            if (dataSet.Tables.Contains(tableName)) { dataTable = dataSet.Tables[tableName]; }
            //Иначе создаем нужную таблицу
            else
            {
                dataTable = new DataTable(tableName);
                dataSet.Tables.Add(dataTable);
                AddIdNameParentIdColumn(dataTable);
            }
            return dataTable;
        }
    }
}
