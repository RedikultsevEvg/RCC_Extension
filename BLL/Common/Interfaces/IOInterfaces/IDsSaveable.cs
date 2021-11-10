using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Interfaces.IOInterfaces;

namespace RDBLL.Common.Interfaces
{
    /// <summary>
    /// Интерфейс объектов для которых возможно сохранение в датасет
    /// </summary>
    public interface IDsSaveable : IHasId
    {
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
        //MeasureUnitList Measures { get; }
    }
}
