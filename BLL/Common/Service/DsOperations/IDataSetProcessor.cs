using RDBLL.Common.Service.DsOperations.Mappings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations
{
    /// <summary>
    /// Интерфейс процессора для сохранения свойств сущности в датасет
    /// </summary>
    internal interface IDataSetProcessor
    {
        /// <summary>
        /// Сохранение свойств сущности в датасет
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="entity">Объект сущности, которая сохраняется</param>
        /// <param name="columnMappings">Коллекция маппингов столбцов</param>
        void SavePropertiesToDataSet(DataRow dataRow, object entity, IEnumerable<ColumnMapping> columnMappings);
        /// <summary>
        /// Назначение свойств сущности из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="entity">Объект сущности, свойства которой заполняются</param>
        /// <param name="columnMappings">Коллекция маппингов столбцов</param>
        void LoadPropertiesFromDataSet(DataRow dataRow, object entity, IEnumerable<ColumnMapping> columnMappings);
    }
}
