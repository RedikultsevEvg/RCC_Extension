using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces
{
    /// <summary>
    /// Интерфейс для объектов, имеющих ссылку на родителя
    /// </summary>
    public interface IHasParent :IDsSaveable
    {
        /// <summary>
        /// Свойство для хранения ссылки на родителя
        /// </summary>
        IDsSaveable ParentMember { get;}
        /// <summary>
        /// Регистрирует родителя
        /// </summary>
        /// <param name="parent"></param>
        void RegisterParent(IDsSaveable parent);
        /// <summary>
        /// Удаляет ссылку на родителя
        /// </summary>
        void UnRegisterParent();
    }
}
