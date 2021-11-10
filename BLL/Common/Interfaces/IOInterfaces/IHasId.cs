using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.IOInterfaces
{
    /// <summary>
    /// Интерфейс для объектов, которые могут сохраняться с базу данных
    /// </summary>
    public interface IHasId
    {
        /// <summary>
        /// Код элемента
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Наименование элемента
        /// </summary>
        string Name { get; set; }
    }
}
