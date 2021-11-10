using RDBLL.Common.Service.DsOperations.Mappings;
using RDBLL.Common.Service.DsOperations.Mappings.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations
{
    internal class DataTableProcessor : IDataTableProcessor
    {
        /// <summary>
        /// Создает таблицу для указанного типа объекта, если она отсутствует в базе данных или возвращает, если таблица существует
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable CreateTable(DataSet dataSet, Type type)
        {
            string tableName = GetTableName(type);
            DataTable dataTable;
            if (dataSet.Tables.Contains(tableName))
            {
                dataTable = GetTable(dataSet, type);
            }
            else
            {
                dataTable = dataSet.Tables.Add(tableName);
            }
            return dataTable;
        }

        public DataTable CreateTable(DataSet dataSet, Type type, IEnumerable<IColumnMapping> columnMappings)
        {
            DataTable dataTable = CreateTable(dataSet, type);

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                string propertyName = propertyInfo.Name;
                foreach (ColumnMapping columnMapping in columnMappings)
                {
                    string targetPropertyName = columnMapping.PropertyName;
                    if (propertyName == targetPropertyName)
                    {
                        DsOperation.AddColumnsFromProperties(dataTable, type);
                    }
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Возвращает таблицу из базы данных для указанного типа объекта
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetTable(DataSet dataSet, Type type)
        {
            string tableName = GetTableName(type);
            DataTable dataTable = dataSet.Tables[tableName];
            return dataTable;
        }
        /// <summary>
        /// Возвращает наименование таблицы, в которую следует сохранять указанный тип объекта
        /// </summary>
        /// <param name="type">тип объекта</param>
        /// <returns></returns>
        public string GetTableName(Type type)
        {
            IEnumerable<TableMapping> tableMappings = TableMappingFactory.GetTableMappings();
            string tableName = type.Name + "s";
            foreach (TableMapping mapping in tableMappings)
            {
                if (mapping.EntityName == type.Name)
                {
                    tableName = mapping.TableName;
                    break;
                }
            }
            return tableName;
        }
    }
}
