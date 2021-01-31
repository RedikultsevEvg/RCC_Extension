using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс параметра расположения армирования
    /// </summary>
    public class RFSpacingParameter : ISavableToDataSet
    {
        private string _ValueType;

        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Значение параметра
        /// </summary>
        public string ValueType
        { get { return _ValueType; }
          private set
            {
                if (!(value=="int" || value == "double" || value == "string")) { throw new Exception("Type of value is not valid!"); }
                _ValueType = value;
            }
        }
        public string ParameterValue {get; set;}
        #region ISavableToDataSet
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
        }
        #endregion
        #region Constructors
        public RFSpacingParameter(RFSpacingBase rfSpacingBase)
        {
            Id = ProgrammSettings.CurrentId;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Устанавливает значение параметра как целое число
        /// </summary>
        /// <param name="value"></param>
        public void SetIntValue(int value)
        {
            ParameterValue = Convert.ToString(value);
        }
        /// <summary>
        /// Устанавливает значение параметра как число двойной точности
        /// </summary>
        /// <param name="value"></param>
        public void SetDoubleValue(double value)
        {
            ParameterValue = Convert.ToString(value);
        }
        /// <summary>
        /// Устанавливает значение параметра как целое число
        /// </summary>
        /// <param name="valuetype"></param>
        /// <param name="value"></param>
        public void SetParameterValue(string valuetype, int value)
        {
            ValueType = valuetype;
            ParameterValue = Convert.ToString(value);
        }
        /// <summary>
        /// Устанавливает значение параметра как число двойной точности
        /// </summary>
        /// <param name="valuetype"></param>
        /// <param name="value"></param>
        public void SetParameterValue(string valuetype, double value)
        {
            ValueType = valuetype;
            ParameterValue = Convert.ToString(value);
        }
        /// <summary>
        /// Устанавливает значение параметра как строку
        /// </summary>
        /// <param name="value"></param>
        public void SetParameterValue(string value)
        {
            ValueType = "string";
            ParameterValue = value;
        }
        /// <summary>
        /// Возвращает значение параметра в виде целого числа
        /// </summary>
        /// <returns></returns>
        public int GetIntValue()
        {
            if (!(ValueType == "int")) { throw new Exception("Type of value is not valid!"); }
            return Convert.ToInt32(ParameterValue);
        }
        /// <summary>
        /// Возвращает значение параметра в виде числе двойной точности
        /// </summary>
        /// <returns></returns>
        public double GetDoubleValue()
        {
            if (!(ValueType=="int" || ValueType == "double")) { throw new Exception("Type of value is not valid!"); }
            return Convert.ToDouble(ParameterValue);
        }
        /// <summary>
        /// Возвращает значение параметра в виде строки
        /// </summary>
        /// <returns></returns>
        public string GetStringValue()
        {
            return ParameterValue;
        }
        #endregion
    }
}
