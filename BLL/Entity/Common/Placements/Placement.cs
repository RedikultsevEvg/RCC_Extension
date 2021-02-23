using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service;
using RDBLL.Common.Params;
using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using DAL.Common;

namespace RDBLL.Entity.Common.Placements
{
    /// <summary>
    /// Base class for spacing
    /// </summary>
    public abstract class Placement : IHasParent, ICloneable, IHasStoredParams
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
        /// Ссылка на родительский элемент
        /// </summary>
        public ISavableToDataSet ParentMember { get; private set; }
        /// <summary>
        /// Коллекция хранимых параметров
        /// </summary>
        public List<StoredParam> StoredParams { get; set; }
        #region Constructors
        /// <summary>
        /// Default constructor 
        /// </summary>
        public Placement()
        {
            Id = ProgrammSettings.CurrentId;
            StoredParams = new List<StoredParam>();
            StoredParams.Add(new StoredParam() { Id = ProgrammSettings.CurrentId, Name = "ItemAngle" });
            StoredParams[0].SetDoubleValue(0.00);
        }
        /// <summary>
        /// Конструктор по строке параметров
        /// </summary>
        /// <param name="s"></param>
        public Placement(string s)
        {
            StoredParams = StoredParamProc.GetFromString(s);
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        /// <summary>
        /// Возвращает имя таблицы
        /// </summary>
        /// <returns></returns>
        public string GetTableName() {return "Placements";}
        /// <summary>
        /// Обновление записи в датасете
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
        }
        /// <summary>
        /// Обновление строки датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            StoredParams = StoredParamProc.GetFromString(dataRow.Field<string>("ValuesString"));
        }
        /// <summary>
        /// Добавление ссылки на родителя
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(ISavableToDataSet parent) {ParentMember = parent;}
        /// <summary>
        /// Сохранение записи в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables[GetTableName()];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            row.SetField("Name", Name);
            row.SetField("ParentId", ParentMember.Id);
            if (this is LineBySpacing) { row.SetField("Type", "LineBySpacing"); }
            else { throw new Exception("Type of Placement is unknown"); }
            row.SetField("ValuesString", StoredParamProc.GetValueString(this));
            #endregion
            dataTable.AcceptChanges();
        }
        /// <summary>
        /// Удаление ссылки на родителя
        /// </summary>
        public void UnRegisterParent() { ParentMember = null; }
        #endregion
        /// <summary>
        /// Клонирование элемента
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Placement placement;
            if (this is LineBySpacing) { placement = new LineBySpacing(); }
            else { throw new Exception("Type of Placement is unknown"); }
            placement.Id = ProgrammSettings.CurrentId;
            placement.Name = Name;
            placement.StoredParams = StoredParamProc.GetFromString(StoredParamProc.GetValueString(this));
            return placement;
        }
        public abstract List<Point2D> GetElementPoints();
    }
}
