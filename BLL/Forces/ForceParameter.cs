using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Forces
{
    /// <summary>
    /// Класс величины усилия
    /// </summary>
    public class ForceParameter : IEquatable<ForceParameter>, ISavableToDataSet
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
        public int LoadSetId { get; set; }
        /// <summary>
        /// Обратная ссылка на комбинацию загружений
        /// </summary>
        public LoadSet LoadSet { get; set; }
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
        /// Расчетное значение нагрузки
        /// </summary>
        public double DesignValue { get; set; }
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ForceParameter()
        {
        }
        /// <summary>
        /// Конструктор по комбинации нагрузок
        /// </summary>
        /// <param name="loadSet"></param>
        public ForceParameter(LoadSet loadSet)
        {
            Id = ProgrammSettings.CurrentId;
            LoadSetId = loadSet.Id;
            LoadSet = loadSet;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Сохранение в указанный датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["ForceParameters"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
            { Id, LoadSetId, KindId, Name, CrcValue
            };
            dataTable.Rows.Add(dataRow);
        }

        public void OpenFromDataSet(DataSet dataSet, int Id)
        {

        }
        #endregion
        //IEquatable
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
    }


}
