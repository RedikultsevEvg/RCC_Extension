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
        public int Id { get; set; } //Код усилия
        public int LoadSetId { get; set; }
        public LoadSet LoadSet { get; set; }
        public int KindId //Код вида усилия (например, продольная сила). Виды нагрузки жестко предустановлены в программе
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
        public ForceParamKind ForceParamKind
        {
            get
            {
                return _forceParamKind;
            }
            set { _forceParamKind = value; }
        }
        public string Name { get; set; } //Наименование
        public double CrcValue //Величина нагрузки (численное значение)
        {
            get { return _crcValue; }
            set { _crcValue = value; }
        }
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
        public double DesignValue { get; set; } //Величина нагрузки (численное значение)
        #region Constructors
        public ForceParameter()
        {
        }
        public ForceParameter(LoadSet loadSet)
        {
            Id = ProgrammSettings.CurrentId;
            LoadSetId = loadSet.Id;
            LoadSet = loadSet;
        }
        #endregion
        #region Methods
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
