using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using System.Collections.ObjectModel;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс контейнера арматурных элементов
    /// </summary>
    public class MaterialContainer : IHasParent, ICloneable
    {
        #region fields
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование контейнера
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Назначение контейнера в родительском элементе
        /// </summary>
        public string Purpose { get; set; }
        /// <summary>
        /// Коллекция использования материалов
        /// </summary>
        public List<MaterialUsing> MaterialUsings {get;set;}
        /// <summary>
        /// Ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public MaterialContainer()
        {
            MaterialUsings = new List<MaterialUsing>();
        }
        /// <summary>
        /// Конструктор по родительскому элементу
        /// </summary>
        /// <param name="parentMember"></param>
        public MaterialContainer(IDsSaveable parentMember)
        {
            Id = ProgrammSettings.CurrentId;
            ParentMember = parentMember;
            MaterialUsings = new List<MaterialUsing>();
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "MaterialContainers"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region setFields
            row.SetField("Purpose", Purpose);
            #endregion
            row.AcceptChanges();
            foreach (MaterialUsing materialUsing in MaterialUsings)
            {
                materialUsing.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            Purpose = dataRow.Field<string>("Purpose");
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (MaterialUsing materialUsing in MaterialUsings)
            {
                materialUsing.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }

        public object Clone()
        {
            MaterialContainer newObject = new MaterialContainer();
            newObject.Id = ProgrammSettings.CurrentId;
            newObject.Name = Name;
            newObject.Purpose = Purpose;
            foreach (MaterialUsing materialUsing in MaterialUsings)
            {
                MaterialUsing newMaterialUsing;
                if (materialUsing is ConcreteUsing) { newMaterialUsing = (materialUsing as ConcreteUsing).Clone() as ConcreteUsing; }
                else if (materialUsing is ReinforcementUsing) { newMaterialUsing = (materialUsing as ReinforcementUsing).Clone() as ReinforcementUsing; }
                else throw new Exception("Type of using is not valid");
                newMaterialUsing.RegisterParent(newObject);
                newObject.MaterialUsings.Add(newMaterialUsing);
            }
            return newObject;
        }

        public void RegisterParent(IDsSaveable parent)
        {
            ParentMember = parent;
        }

        public void UnRegisterParent()
        {
            ParentMember = null;
        }
        #endregion
    }
}
