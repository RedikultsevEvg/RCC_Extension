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

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс геологического разреза
    /// </summary>
    public class SoilSection : ISavableToDataSet, IDataErrorInfo
    {
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
            BuildingSiteId = buildingSite.Id;
            BuildingSite = buildingSite;
            Name = "Скважина-" + (buildingSite.SoilSections.Count + 1);
            HasWater = false;
            NaturalWaterLevel = 200;
            CenterX = 0;
            CenterY = 0;
            SoilLayers = new ObservableCollection<SoilLayer>();
        }
        #region SaveToDataset
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["SoilSections"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
                { Id, BuildingSiteId, Name, HasWater, NaturalWaterLevel, WaterLevel, CenterX, CenterY };
            dataTable.Rows.Add(dataRow);
            foreach (SoilLayer soilLayer in SoilLayers)
            {
                //soilLayer.SaveToDataSet(dataSet);
            }
        }
        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
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
