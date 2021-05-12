using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.DropPanels
{
    /// <summary>
    /// Базовый класс капители
    /// </summary>
    public class DropPanelBase : IHasParent, IHasHeight, IHasConcrete
    {
        /// <summary>
        /// Код элемента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Обратная ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Высота капители
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Бетон капители
        /// </summary>
        public ConcreteUsing Concrete { get; set; }
        /// <summary>
        /// Флаг использования бетона родительского элемента
        /// </summary>
        public bool UseParentControl { get; set; }

        /// <summary>
        /// Удаление объекта из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public string GetTableName() { return "DropPanels"; }

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
