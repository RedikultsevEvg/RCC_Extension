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
        public IDsSaveable ParentMember { get; private set; }
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
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "ItemAngle" });
            StoredParams[0].SetDoubleValue(0.00);
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (StoredParam param in StoredParams) { param.DeleteFromDataSet(dataSet); }
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        /// <summary>
        /// Возвращает имя таблицы
        /// </summary>
        /// <returns></returns>
        public string GetTableName() => "Placements";
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
        }
        /// <summary>
        /// Добавление ссылки на родителя
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent) {ParentMember = parent;}
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
            else if (this is RectArrayPlacement) { row.SetField("Type", "RectArrayPlacement"); }
            else { throw new Exception("Type of Placement is unknown"); }
            dataTable.AcceptChanges();
            #endregion
            foreach (StoredParam param in StoredParams) { param.SaveToDataSet(dataSet, createNew);}
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
            Placement placement = MemberwiseClone() as Placement;
            placement.Id = ProgrammSettings.CurrentId;
            placement.StoredParams = new List<StoredParam>();
            foreach (StoredParam param in StoredParams)
            {
                StoredParam newParam = param.Clone() as StoredParam;
                newParam.Id = ProgrammSettings.CurrentId;
                newParam.RegisterParent(placement);
                placement.StoredParams.Add(newParam);
            }
            return placement;
        }
        public abstract List<Point2D> GetElementPoints();
    }
}
