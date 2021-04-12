using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Forces
{
    /// <summary>
    /// Класс величины усилия
    /// </summary>
    public class ForceParameter : IEquatable<ForceParameter>, IDsSaveable, IDuplicate
    {
        private int _KindId;
        private ForceParamKind _forceParamKind;
        private double _crcValue;
        /// <summary>
        /// Код усилия
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код комбинации загружения
        /// </summary>
        public int LoadId { get; set; }
        /// <summary>
        /// Обратная ссылка на комбинацию загружений
        /// </summary>
        public Load LoadSet { get; set; }
        /// <summary>
        /// Код вида усилия (например, продольная сила). Виды нагрузки жестко предустановлены в программе
        /// </summary>
        public int KindId
        {
            get { return _KindId; }
            set
            {
                _KindId = value;
                try
                {
                    var tmpForceParamKind = from t in ProgrammSettings.ForceParamKinds where t.Id == _KindId select t;
                    _forceParamKind = tmpForceParamKind.First();
                }
                catch { }

            }
        }
        /// <summary>
        /// Ссылка на вид усилия
        /// </summary>
        public ForceParamKind ForceParamKind
        {
            get
            {
                return _forceParamKind;
            }
            set { _forceParamKind = value; }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Длительная часть нормативного значения
        /// </summary>
        public double LongCrcValue { get; set; }
        /// <summary>
        /// Нормативное (характеристическое) значение нагрузки
        /// </summary>
        public double CrcValue
        {
            get { return _crcValue; }
            set { _crcValue = value; }
        }
        /// <summary>
        /// Нормативное (характеристическое) значение нагрузки в текущих единицах измерения
        /// </summary>
        public double CrcValueInCurUnit
        {
            get
            {
                MeasureUnitLabel measureUnitLabel = _forceParamKind.MeasureUnit.GetCurrentLabel();
                return _crcValue * measureUnitLabel.AddKoeff;
            }
            set
            {
                MeasureUnitLabel measureUnitLabel = _forceParamKind.MeasureUnit.GetCurrentLabel();
                _crcValue = value / measureUnitLabel.AddKoeff;
            }
        }
        /// <summary>
        /// Длительная часть расчетного значения
        /// </summary>
        public double LongDesignValue { get; set; }
        /// <summary>
        /// Расчетное значение нагрузки
        /// </summary>
        public double DesignValue { get; set; }
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ForceParameter(bool genId = false)
        {
            if (genId) Id = ProgrammSettings.CurrentId;
        }
        /// <summary>
        /// Конструктор по комбинации нагрузок
        /// </summary>
        /// <param name="loadSet"></param>
        public ForceParameter(Load loadSet)
        {
            Id = ProgrammSettings.CurrentId;
            LoadId = loadSet.Id;
            LoadSet = loadSet;
        }
        #endregion
        #region Methods
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "ForceParameters"; }
        /// <summary>
        /// Сохранение в указанный датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region
            row.SetField("LoadSetId", LoadId);
            row.SetField("KindId", KindId);
            row.SetField("CrcValue", CrcValue);
            #endregion
            row.AcceptChanges();
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
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        #endregion IEquatable
        /// <summary>
        /// Сравнение
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ForceParameter other)
        {
            if (this.KindId == other.KindId
                & Math.Round(this.CrcValue, 3) == Math.Round(other.CrcValue, 3)
                & Math.Round(this.DesignValue, 3) == Math.Round(other.DesignValue, 3)
                )
            {
                return true;
            }
            else { return false; }
        }
        #region IDuplicate
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            ForceParameter forceParameter = this.MemberwiseClone() as ForceParameter;
            forceParameter.Id = ProgrammSettings.CurrentId;
            return forceParameter;
        }
        #endregion
    }


}
