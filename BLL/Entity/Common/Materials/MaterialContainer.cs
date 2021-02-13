using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using System.Collections.ObjectModel;
using RDBLL.Common.Service;
using DAL.Common;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс контейнера арматурных элементов
    /// </summary>
    public class MaterialContainer : IHasParent, IDuplicate
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
        public ISavableToDataSet ParentMember { get; private set; }
        private static string TableName { get { return "MaterialContainers"; } }
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
        public MaterialContainer(ISavableToDataSet parentMember)
        {
            Id = ProgrammSettings.CurrentId;
            ParentMember = parentMember;
            MaterialUsings = new List<MaterialUsing>();
        }
        #endregion
        #region ISavableToDataSet
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables[TableName];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            row.SetField("Name", Name);
            row.SetField("Purpose", Purpose);
            row.SetField("ParentId", ParentMember.Id);
            #endregion
            dataTable.AcceptChanges();
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
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, TableName, Id));
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
            DsOperation.DeleteRow(dataSet, TableName, Id);
        }

        public object Duplicate()
        {
            MaterialContainer newObject = new MaterialContainer();
            newObject.Id = ProgrammSettings.CurrentId;
            newObject.Name = Name;
            newObject.Purpose = Purpose;
            foreach (MaterialUsing materialUsing in MaterialUsings)
            {
                MaterialUsing newMaterialUsing = materialUsing.Duplicate() as MaterialUsing;
                newMaterialUsing.RegisterParent(newMaterialUsing);
                newObject.MaterialUsings.Add(newMaterialUsing);
            }
            return newObject;
        }

        public void RegisterParent(ISavableToDataSet parent)
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
