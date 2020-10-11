using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.ComponentModel;
using System.Data;
using DAL.Common;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс слоя грунта
    /// </summary>
    public class SoilLayer : ISavableToDataSet, IRDObservable, IRDObserver

    {
        /// <summary>
        /// Код слоя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код модели грунта
        /// </summary>
        public int SoilId { get; set; }
        /// <summary>
        /// Ссылка на модель грунта
        /// </summary>
        public Soil Soil { get; set; }
        /// <summary>
        /// Код геологического разреза
        /// </summary>
        public int SoilSectionId { get; set; }
        /// <summary>
        /// Обратная ссылка на геологический разрез
        /// </summary>
        public SoilSection SoilSection { get; set; }
        /// <summary>
        /// Отметка верха слоя
        /// </summary>
        public double TopLevel { get; set; }
        private List<IRDObserver> Observers;
        #region SaveToDataset
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables["SoilLayers"];
            if (createNew)
            {
                row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            else
            {
                var soil = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
                row = soil;
            }
            #region setFields
            row.SetField("Id", Id);
            row.SetField("SoilId", SoilId);
            row.SetField("SoilSectionId", SoilSectionId);
            row.SetField("TopLevel", TopLevel);
            #endregion
            dataTable.AcceptChanges();
        }
        /// <summary>
        /// Обновляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["SoilLayers"];
            var row = (from dataRow in dataTable.AsEnumerable()
                       where dataRow.Field<int>("Id") == Id
                       select dataRow).Single();
            OpenFromDataSet(row);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            SoilId = dataRow.Field<int>("SoilId");
            SoilSectionId = dataRow.Field<int>("SoilSectionId");
            TopLevel = dataRow.Field<double>("TopLevel");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, "SoilLayers", Id);
        }
        #endregion
        #region IObservable
        /// <summary>
        /// Добавляет объект в коллекцию наблюдателей
        /// </summary>
        /// <param name="obj"></param>
        public void AddObserver(IRDObserver obj)
        {
            Observers.Add(obj);
        }
        /// <summary>
        /// Удаляет объект из коллекции наблюдателей
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObserver(IRDObserver obj)
        {
            Observers.Remove(obj);
        }
        /// <summary>
        /// Уведомляет наблюдателей об изменении
        /// </summary>
        public void NotifyObservers()
        {
            foreach (IRDObserver observer in Observers)
            {
                observer.Update();
            }
        }
        /// <summary>
        /// Возвращат наличие объектов, где применяется данный объект
        /// </summary>
        /// <returns></returns>
        public bool HasChild()
        {
            bool result = false;
            if (Observers.Count > 0) return true;
            return result;
        }
        #endregion
        #region IObserver
        public void Update()
        {
            NotifyObservers();
        }
        #endregion
    }
}
