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
    public class MaterialContainer : ISavableToDataSet
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
        /// Коллекция использования материалов
        /// </summary>
        public List<MaterialUsing> MaterialUsings {get;set;}
        /// <summary>
        /// Ссылка на родительский элемент
        /// </summary>
        public ISavableToDataSet ParentMember { get; set; }

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
            dataTable = dataSet.Tables["Materialcontainers"];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            row.SetField("Name", Name);
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
            throw new NotImplementedException();
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
