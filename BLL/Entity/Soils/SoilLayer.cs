using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.ComponentModel;
using System.Data;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс слоя грунта
    /// </summary>
    public class SoilLayer : IDsSaveable, IRDObservable, IRDObserver

    {
        /// <summary>
        /// Код слоя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
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
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SoilLayer() {; }
        /// <summary>
        /// Constructor from soil and soil section (borehole)
        /// </summary>
        /// <param name="soil">Soil</param>
        /// <param name="soilSection">SoilSection</param>
        /// <param name="building">Building of construction</param>
        /// <param name="prevThick">Thickness of previous layer of soil</param>
        public SoilLayer(Soil soil, SoilSection soilSection, Building building, double prevThick = 2)
        {
            Id = ProgrammSettings.CurrentId;
            Soil = soil;
            SoilId = soil.Id;
            SoilSection = soilSection;
            SoilSectionId = soilSection.Id;
            //Количество слоев грунта, существующих в скважине до создания данного грунта
            int count = soilSection.SoilLayers.Count;
            double topLevel;
            //Если до добавления слоя грунта слои уже были, то уровень кровли слоя назначаем на 2м ниже кровли предыдущего слоя
            if (count > 0) { topLevel = soilSection.SoilLayers[count - 1].TopLevel - prevThick; }
            else
            {
                //Если здание не передано
                if (building == null)
                {
                    //Если в объекте нет зданий, то выдаем ошибку
                    if ((soilSection.ParentMember as BuildingSite).Childs.Count == 0) { throw new Exception("Building site not contain any buildings"); }
                    //Иначе присваиваем первое здание
                    else building = (soilSection.ParentMember as BuildingSite).Childs[0];
                }
                //Назначаем уровень верха по отметке нуля для здания
                topLevel = building.AbsoluteLevel - building.RelativeLevel;
            }
            TopLevel = topLevel;
            soilSection.SoilLayers.Add(this);
        }
        #endregion
        #region IODataset
        public string GetTableName() { return "SoilLayers"; }
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables[GetTableName()];
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
            DataTable dataTable = dataSet.Tables[GetTableName()];
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
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
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
