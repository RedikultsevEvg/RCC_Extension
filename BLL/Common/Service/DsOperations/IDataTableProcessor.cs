using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations
{
    internal interface IDataTableProcessor
    {
        /// <summary>
        /// Возвращает наименование таблицы, в которую следует сохранять указанный тип объекта
        /// </summary>
        /// <param name="type">тип объекта</param>
        /// <returns></returns>
        string GetTableName(Type type);
        /// <summary>
        /// Создает таблицу для указанного типа объекта, если она отсутствует в базе данных или возвращает, если таблица существует
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable CreateTable(DataSet dataSet, Type type);
        DataTable CreateTable(DataSet dataSet, Type type, IEnumerable<IColumnMapping> columnMappings);
        /// <summary>
        /// Возвращает таблицу из базы данных для указанного типа объекта
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetTable(DataSet dataSet, Type type);
    }
}
