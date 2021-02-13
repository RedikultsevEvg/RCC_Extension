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
    public interface IHasParent :ISavableToDataSet
    {
        /// <summary>
        /// Свойство для хранения ссылки на родителя
        /// </summary>
        ISavableToDataSet ParentMember { get;}
        /// <summary>
        /// Регистрирует родителя
        /// </summary>
        /// <param name="parent"></param>
        void RegisterParent(ISavableToDataSet parent);
        /// <summary>
        /// Удаляет ссылку на родителя
        /// </summary>
        void UnRegisterParent();
    }
}
