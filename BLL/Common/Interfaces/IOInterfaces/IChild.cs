using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.IOInterfaces
{
    /// <summary>
    /// Интерфейс для объектов, имеющих ссылку на родителя
    /// </summary>
    public interface IChild
    {
        /// <summary>
        /// Свойство для хранения ссылки на родителя
        /// </summary>
        IHasId ParentMember { get; }
        /// <summary>
        /// Регистрирует родителя
        /// </summary>
        /// <param name="parent"></param>
        void RegisterParent(IHasId parent);
        /// <summary>
        /// Удаляет ссылку на родителя
        /// </summary>
        void UnRegisterParent();
    }
}
