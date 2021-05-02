using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings
{
    /// <summary>
    /// Класс слоя продавливания
    /// </summary>
    public class PunchingLayer : IHasParent, ICloneable, IHasConcrete, IHasHeight
    {
        /// <summary>
        /// Код слоя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя слоя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Обратная ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Бетон слоя
        /// </summary>
        public ConcreteUsing Concrete { get; set; }
        /// <summary>
        /// Высота слоя
        /// </summary>
        public double Height { get; set; }

        public PunchingLayer(bool GenId = false)
        {
            if (GenId) { Id = ProgrammSettings.CurrentId; }
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public string GetTableName()
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
            if (ParentMember != null) { UnRegisterParent(); }
            Punching punching = parent as Punching;
            punching.Layers.Add(this);
            ParentMember = parent;
        }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }

        public void UnRegisterParent()
        {
            Punching punching = ParentMember as Punching;
            punching.Layers.Remove(this);
            ParentMember = null;
        }
    }
}
