using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RDBLL.Common.Interfaces
{
    /// <summary>
    /// Интерфейс объектов для которых возможно сохранение в датасет
    /// </summary>
    public interface IDsSaveable
    {
        /// <summary>
        /// Код элемента
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Возвращает наименование таблицы для сохранения
        /// </summary>
        /// <returns></returns>
        string GetTableName();
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="createNew">Флаг создания нового элемента</param>
        void SaveToDataSet(DataSet dataSet, bool createNew);
        /// <summary>
        /// Открыть запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        void OpenFromDataSet(DataSet dataSet);
        /// <summary>
        /// Открыть строку из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        void OpenFromDataSet(DataRow dataRow);
        /// <summary>
        /// Удалить запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        void DeleteFromDataSet(DataSet dataSet);
    }
}
