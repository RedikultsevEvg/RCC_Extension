using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Placements;
using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.PlacementEhs
{
    /// <summary>
    /// Родительский контейнер для расположения элементов
    /// </summary>
    public class ParentPlacement : IHasParent, IParentPlacement
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Обратная ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; }
        /// <summary>
        /// Коллекция расположений элементов
        /// </summary>
        public List<Placement> Placements { get; set; }

        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public void RegisterParent(IDsSaveable parent)
        {
            throw new NotImplementedException();
        }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }

        public void UnRegisterParent()
        {
            throw new NotImplementedException();
        }
    }
}
