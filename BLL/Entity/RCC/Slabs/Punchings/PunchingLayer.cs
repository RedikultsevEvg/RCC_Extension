using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
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
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="GenId"></param>
        public PunchingLayer(bool GenId = false)
        {
            if (GenId) { Id = ProgrammSettings.CurrentId; }
        }
        public object Clone()
        {
            PunchingLayer layer = this.MemberwiseClone() as PunchingLayer;
            layer.Id = ProgrammSettings.CurrentId;
            layer.Concrete = this.Concrete.Clone() as ConcreteUsing;
            return layer;
        }

        public void DeleteFromDataSet(DataSet dataSet)
        {
            Concrete.DeleteFromDataSet(dataSet);
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }

        public string GetTableName() { return "PunchingLayers"; }

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
        //Сохранение объекта в датасет
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            try
            {
                DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
                #region setFields
                //DsOperation.SetField(row, "RelativeTopLevel", RelativeTopLevel);
                #endregion
                row.AcceptChanges();
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage($"Ошибка сохранения элемента {this.GetType().Name}: " + Name, ex);
            }
        }

        public void UnRegisterParent()
        {
            Punching punching = ParentMember as Punching;
            punching.Layers.Remove(this);
            ParentMember = null;
        }
    }
}
