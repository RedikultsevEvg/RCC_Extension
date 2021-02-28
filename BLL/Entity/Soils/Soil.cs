using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Interfaces;
using System.Data;
using DAL.Common;
using System.Collections.ObjectModel;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Абстрактный класс грунта
    /// </summary>
    public abstract class Soil : IDsSaveable, IRDObservable
    {
        #region Properties
        /// <summary>
        /// Код грунта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код строительного объекта
        /// </summary>
        public int BuildingSiteId { get; set; }
        /// <summary>
        /// Обратная ссылка на строительный объект
        /// </summary>
        public BuildingSite BuildingSite { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Расширенное наименование
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Нормативная плотность грунта
        /// </summary>
        public double CrcDensity { get; set; }
        /// <summary>
        /// Расчетная плотность грунта для 1 группы ПС
        /// </summary>
        public double FstDesignDensity { get; set; }
        /// <summary>
        /// Расчетная плотность грунта для 2 группы ПС
        /// </summary>
        public double SndDesignDensity { get; set; }
        /// <summary>
        /// Нормативный вес частиц
        /// </summary>
        public double CrcParticularDensity { get; set; }
        /// <summary>
        /// Вес частиц для 1 группы ПС
        /// </summary>
        public double FstParticularDensity { get; set; }
        /// <summary>
        /// Вес частиц для 2 группы ПС
        /// </summary>
        public double SndParticularDensity { get; set; }
        /// <summary>
        /// Коэффициент пористости
        /// </summary>
        public double PorousityCoef { get; set; }
        /// <summary>
        /// Коэффициент фильтрации
        /// Необходим, так как влияет на напряжения в грунте при послойном суммировании
        /// </summary>
        public double FiltrationCoeff { get; set; }
        /// <summary>
        /// Единицы измерения напряжений, только для чтения
        /// </summary>
        public string StressMeasure { get { return MeasureUnitConverter.GetUnitLabelText(3); } }
        /// <summary>
        /// Единицы измерения плотности
        /// </summary>
        public string DensityMeasure { get { return MeasureUnitConverter.GetUnitLabelText(8); } }
        /// <summary>
        /// Единицы измерения коэффициента фильтрации
        /// </summary>
        public string FiltrationMeasure { get { return MeasureUnitConverter.GetUnitLabelText(14); } }
        public List<IRDObserver> Observers;
        #endregion
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public Soil(BuildingSite buildingSite)
        {
            Id = ProgrammSettings.CurrentId;
            BuildingSiteId = buildingSite.Id;
            BuildingSite = buildingSite;
            Name = $"ИГЭ-{buildingSite.Soils.Count + 1}";
            FiltrationCoeff = 0.0001;
            Observers = new List<IRDObserver>();
        }
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "Soils"; }
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="createNew"></param>
        public virtual void SaveToDataSet(DataSet dataSet, bool createNew)
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
            SaveToDataSet(row);
            dataTable.AcceptChanges();
        }
        /// <summary>
        /// Сохраняет запись в строку датасета
        /// </summary>
        /// <param name="dataRow">Строка датасета</param>
        public virtual void SaveToDataSet(DataRow dataRow)
        {
            //Не удалять, так как этот участок необходим с учетом наследования
            dataRow["Id"] = Id;
            dataRow["BuildingSiteId"] = BuildingSiteId;
            dataRow["Name"] = Name;
            dataRow["Description"] = Description;
            dataRow["CrcDensity"] = CrcDensity;
            dataRow["FstDesignDensity"] = FstDesignDensity;
            dataRow["SndDesignDensity"] = SndDesignDensity;
            dataRow["CrcParticularDensity"] = CrcParticularDensity;
            dataRow["FstParticularDensity"] = FstParticularDensity;
            dataRow["SndParticularDensity"] = SndParticularDensity;
            dataRow["PorousityCoef"] = PorousityCoef;
            dataRow["FiltrationCoeff"] = FiltrationCoeff;
        }
        /// <summary>
        /// Получает свойства класса из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public virtual void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[GetTableName()];
            var soilRow = (from dataRow in dataTable.AsEnumerable()
                           where dataRow.Field<int>("Id") == Id
                           select dataRow).Single();
            OpenFromDataSet(soilRow);
        }
        /// <summary>
        /// Получает свойства класса из строки датасета
        /// </summary>
        /// <param name="dataRow">Строка датасета</param>
        public virtual void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            BuildingSiteId = dataRow.Field<int>("BuildingSiteId");
            Name = dataRow.Field<string>("Name");
            Description = dataRow.Field<string>("Description");
            CrcDensity = dataRow.Field<double>("CrcDensity");
            FstDesignDensity = dataRow.Field<double>("FstDesignDensity");
            SndDesignDensity = dataRow.Field<double>("SndDesignDensity");
            CrcParticularDensity = dataRow.Field<double>("CrcParticularDensity");
            FstParticularDensity = dataRow.Field<double>("FstParticularDensity");
            SndParticularDensity = dataRow.Field<double>("SndParticularDensity");
            PorousityCoef = dataRow.Field<double>("PorousityCoef");
            FiltrationCoeff = dataRow.Field<double>("FiltrationCoeff");
        }
        /// <summary>
        /// Удаляет запись из строки датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            //Определяем участвует ли грунт в каких-либо слоях скважин
            dataTable = dataSet.Tables["SoilLayers"];
            var soilLayers = from dataRow in dataTable.AsEnumerable()
                             where dataRow.Field<int>("SoilId") == Id
                             select dataRow;
            int count = 0;
            foreach (DataRow dataRow in soilLayers)
            {
                count++;
            }
            if (count > 0) { throw new Exception("Нельзя удалить грунт, который участвует в скважинах"); }
            else { DsOperation.DeleteRow(dataSet, GetTableName(), Id); }
        }
        #endregion
        #region IObservable
        public void AddObserver(IRDObserver obj)
        {
            Observers.Add(obj);
        }
        public void RemoveObserver(IRDObserver obj)
        {
            Observers.Remove(obj);
        }
        public void NotifyObservers()
        {
            foreach (IRDObserver observer in Observers)
            {
                observer.Update();
            }
        }
        #endregion
        public bool HasChild()
        {
            bool result = false;
            foreach (SoilSection soilSection in BuildingSite.SoilSections)
            {
                foreach (SoilLayer soilLayer in soilSection.SoilLayers)
                {
                    if (soilLayer.Soil.Id == Id) return true;
                }
            }
            return result;
        }
        public string Error { get { throw new NotImplementedException(); } }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "CrcDensity":
                        {
                            if (CrcDensity <= 0)
                            {
                                error = "Нормативная плотность грунта должна быть больше нуля";
                            }
                        }
                        break;
                }
                return error;
            }
        }

    }
}
