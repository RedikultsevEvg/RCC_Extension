using DAL.Common;
using RDBLL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service
{
    /// <summary>
    /// Класс для операций с объектам в датасете
    /// </summary>
    public static class SetEntity
    {
        /// <summary>
        /// Заполняет строку в соответствии с общими свойствами объекта
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        public static void SetRow(DataRow row, IDsSaveable entity)
        {
            if (entity is IHasParent)
            {
                IHasParent child = entity as IHasParent;
                DsOperation.SetId(row, child.Id, child.Name, child.ParentMember.Id);
            }
            else
            {
                DsOperation.SetId(row, entity.Id, entity.Name, null);
            }
        }
        public static void SetProps(DataRow row, IDsSaveable entity)
        {
            entity.Id = row.Field<int>("Id");
            entity.Name = row.Field<string>("Name");
        }
    }
}
