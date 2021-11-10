using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.IOInterfaces
{
    /// <summary>
    /// Интерфейс сохранения элемента в хранилище
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>
        /// Создание элемента в базе данных
        /// </summary>
        /// <param name="obj"></param>
        void Create(T obj);
        /// <summary>
        /// Удаление элемента из базы данных
        /// </summary>
        void Delete(T obj);
        /// <summary>
        /// Возвращает объект из хранилища
        /// </summary>
        /// <returns></returns>
        T GetEntityById(int id);
        /// <summary>
        /// Обновление объекта в базе данных
        /// </summary>
        /// <param name="obj"></param>
        void Update(T obj);
        /// <summary>
        /// Обновляет объект в соответствии со строкой датасета
        /// </summary>
        /// <param name="obj"></param>
        void UpdateObject(T obj);
        /// <summary>
        /// Подтверждает внесенные изменения
        /// </summary>
        void Save();

    }
}
