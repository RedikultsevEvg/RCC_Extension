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
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс геологического разреза
    /// </summary>
    public class SoilSection : IHasParent, IDataErrorInfo
    {
        #region Properies
        /// <summary>
        /// Код разреза
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на строительный объект
        /// </summary>
        public IDsSaveable ParentMember {get; private set;}
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
        #endregion
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public SoilSection()
        {
            SoilLayers = new ObservableCollection<SoilLayer>();
        }
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public SoilSection(BuildingSite buildingSite)
        {
            Id = ProgrammSettings.CurrentId;
            RegisterParent(buildingSite);
            Name = "Скважина-" + (buildingSite.SoilSections.Count + 1);
            HasWater = false;
            NaturalWaterLevel = 200;
            WaterLevel = 200;
            CenterX = 0;
            CenterY = 0;
            SoilLayers = new ObservableCollection<SoilLayer>();
        }
        #region IODataSet
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "SoilSections"; }
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            #region setFields
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            row.SetField("HasWater", HasWater);
            row.SetField("NaturalWaterLevel", NaturalWaterLevel);
            row.SetField("WaterLevel", WaterLevel);
            row.SetField("CenterX", CenterX);
            row.SetField("CenterY", CenterY);
            #endregion
            row.AcceptChanges();
            //Удаляем из датасета все вложенные слои грунта
            DeleteLayers(dataSet);
            //Добавляем в датасет вложенные слои грунта
            foreach (SoilLayer soilLayer in SoilLayers)
            {
                //Добавляем все слои, так как удалили перед этим
                soilLayer.SaveToDataSet(dataSet, true);
            }
        }
        /// <summary>
        /// Обновляет запись в соответствии с датасетом
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[GetTableName()];
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
            EntityOperation.SetProps(dataRow, this);
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
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        private void DeleteLayers(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, "SoilLayers", "SoilSectionId", Id);
            DataTable dataTable;
            dataTable = dataSet.Tables["SoilLayers"];
            dataTable.AcceptChanges();
        }

        public void RegisterParent(IDsSaveable parent)
        {
            BuildingSite buildingSite = parent as BuildingSite;
            ParentMember = buildingSite;
            buildingSite.SoilSections.Add(this);
        }

        public void UnRegisterParent()
        {
            BuildingSite buildingSite = ParentMember as BuildingSite;
            buildingSite.SoilSections.Remove(this);
            ParentMember = null;
        }
        public bool HasChild()
        {
            bool result = false;
            //if (Observers.Count > 0) return true;
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
                                error = "Уровень грунтовых вод должен быть ниже кровли верхнего слоя";
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
