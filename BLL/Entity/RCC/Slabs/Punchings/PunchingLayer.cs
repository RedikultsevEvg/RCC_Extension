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
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            PunchingLayer layer = this.MemberwiseClone() as PunchingLayer;
            layer.Id = ProgrammSettings.CurrentId;
            ConcreteUsing concrete = this.Concrete.Clone() as ConcreteUsing;
            concrete.RegisterParent(layer);
            layer.Concrete = concrete;
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
            try
            {
                OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage($"Ошибка получения элемента из базы данных. Элемент {this.GetType().Name}: " + Name, ex);
            }
        }

        public void OpenFromDataSet(DataRow dataRow)
        {
            EntityOperation.SetProps(dataRow, this);
        }

        public void RegisterParent(IDsSaveable parent)
        {
            if (ParentMember != null) { UnRegisterParent(); }
            else if (!(parent is Punching)) throw new Exception($"Parent type is not valid. Element type {GetType().Name}, Id = {Id}, Name={Name}");
            Punching punching = parent as Punching;
            punching.Children.Add(this);
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
            punching.Children.Remove(this);
            ParentMember = null;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PunchingLayer)) return false;
            PunchingLayer layer = obj as PunchingLayer;
            if (layer.Id != Id) return false;
            else if (layer.Name != Name) return false;
            else if (layer.Height != Height) return false;

            return true;
        }
    }
}
