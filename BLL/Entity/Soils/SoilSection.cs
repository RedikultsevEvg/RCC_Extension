using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Interfaces;
using System.ComponentModel;
using System.Data;
using DAL.Common;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс геологического разреза
    /// </summary>
    public class SoilSection : ISavableToDataSet, IDataErrorInfo, IRDObservable
    {
        #region Properies
        /// <summary>
        /// Код разреза
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код строительного объекта
        /// </summary>
        public int BuildingSiteId { get; set; }
        /// <summary>
        /// Обратная ссылка на строительный объект
        /// </summary>
        public BuildingSite BuildingSite {get;set;}
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Слои грунта
        /// </summary>
        public ObservableCollection<SoilLayer> SoilLayers { get; set; }
        /// <summary>
        /// Флаг наличия грунтовой воды
        /// </summary>
        public bool HasWater { get; set; }
        /// <summary>
        /// Уровень грунтовых вод зафиксированный
        /// </summary>
        public double NaturalWaterLevel { get; set; }
        /// <summary>
        /// Уровень грунтовых вод прогнозный
        /// </summary>
        public double WaterLevel { get; set; }
        /// <summary>
        /// Положение центра
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Положение центра
        /// </summary>
        public double CenterY { get; set; }
        /// <summary>
        /// Наименование линейных единиц измерения
        /// </summary>
        public string LinearMeasure { get { return MeasureUnitConverter.GetUnitLabelText(0); } }
        private List<IRDObserver> Observers;
        #endregion
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public SoilSection()
        {
            SoilLayers = new ObservableCollection<SoilLayer>();
            Observers = new List<IRDObserver>();
        }
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public SoilSection(BuildingSite buildingSite)
        {
            Id = ProgrammSettings.CurrentId;
            BuildingSiteId = buildingSite.Id;
            BuildingSite = buildingSite;
            Name = "Скважина-" + (buildingSite.SoilSections.Count + 1);
            HasWater = false;
            NaturalWaterLevel = 200;
            WaterLevel = 200;
            CenterX = 0;
            CenterY = 0;
            SoilLayers = new ObservableCollection<SoilLayer>();
            Observers = new List<IRDObserver>();
        }
        #region SaveToDataset
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables["SoilSections"];
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
            row.SetField("BuildingSiteId", BuildingSiteId);
            row.SetField("Name", Name);
            row.SetField("HasWater", HasWater);
            row.SetField("NaturalWaterLevel", NaturalWaterLevel);
            row.SetField("WaterLevel", WaterLevel);
            row.SetField("CenterX", CenterX);
            row.SetField("CenterY", CenterY);
            #endregion
            dataTable.AcceptChanges();
            //Удаляем из датасета все вложенные слои грунта
            DeleteLayers(dataSet);
            //Добавляем в датасет вложенные слои грунта
            foreach (SoilLayer soilLayer in SoilLayers)
            {
                soilLayer.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Обновляет запись в соответствии с датасетом
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["SoilSections"];
            var row = (from dataRow in dataTable.AsEnumerable()
                       where dataRow.Field<int>("Id") == Id
                       select dataRow).Single();
            OpenFromDataSet(row);
            SoilLayers = GetEntity.GetSoilLayers(dataSet, this);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            BuildingSiteId = dataRow.Field<int>("BuildingSiteId");
            Name = dataRow.Field<string>("Name");
            HasWater = dataRow.Field<bool>("HasWater");
            NaturalWaterLevel = dataRow.Field<double>("NaturalWaterLevel");
            WaterLevel = dataRow.Field<double>("WaterLevel");
            CenterX = dataRow.Field<double>("CenterX");
            CenterY = dataRow.Field<double>("CenterY");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DeleteLayers(dataSet);
            DsOperation.DeleteRow(dataSet, "SoilSections", Id);
        }
        private void DeleteLayers(DataSet dataSet)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables["SoilLayers"];
            DataRow[] soilRows = dataTable.Select("id="+Id);
            int count = soilRows.Length;
            for (int i = count-1; i>=0; i--)
            {
                dataTable.Rows.Remove(soilRows[i]);
            }
            dataTable.AcceptChanges();
        }
        #endregion
        #region IRDObservable
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
        #region errors
        /// <summary>
        /// Поле для ошибки
        /// </summary>
        public string Error { get { throw new NotImplementedException(); } }
        /// <summary>
        /// Поле для проверки на ошибки
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "NaturalWaterLevel":
                        {
                            if (SoilLayers.Count>0 & HasWater)
                            if (NaturalWaterLevel > SoilLayers[0].TopLevel)
                            {
                                error = "Уровень грунтовых вод должен быть ниже кровели верхнего слоя";
                            }
                        }
                        break;
                }
                return error;
            }
        }
        #endregion
    }
}
