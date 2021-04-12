using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Params
{
    /// <summary>
    /// Интерфейс для классов, имеющих лист параметров
    /// </summary>
    public interface IHasStoredParams
    {
        string Type { get; set; }
        /// <summary>
        /// Коллекция хранимых параметров
        /// </summary>
        List<StoredParam> StoredParams { get; }
    }
}
