using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Common.Params
{
    public class StoredParam : ICloneable
    {
        /// <summary>
        /// Поле для типа параметра
        /// </summary>
        private string _ValueType;
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование параметра
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Тип параметра
        /// </summary>
        public string ValueType
        {
            get { return _ValueType; }
            private set
            {
                if (!(value == "int" || value == "double" || value == "string" || value == "bool")) { throw new Exception("Type of value is not valid!"); }
                _ValueType = value;
            }
        }
        /// <summary>
        /// Значение параметра
        /// </summary>
        public string ParameterValue { get; set; }
        public void SetValueType(string s)
        {
            _ValueType = s;
        }


        /// <summary>
        /// Устанавливает значение параметра как целое число
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public void SetIntValue(int value)
        {
            SetValueType("int");
            ParameterValue = Convert.ToString(value);
        }
        /// <summary>
        /// Устанавливает значение параметра как число двоичной точности
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public void SetDoubleValue(double value)
        {
            SetValueType("double");
            ParameterValue = Convert.ToString(value);
        }
        /// <summary>
        /// Устанавливает значение параметра как строку
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public void SetStringValue(string value)
        {
            SetValueType("string");
            ParameterValue = value;
        }
        /// <summary>
        /// Устанавливает значение параметра как булево значение
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public void SetBoolValue(bool value)
        {
            SetValueType("bool");
            ParameterValue = Convert.ToString(value);
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
            if (!(ValueType == "int" || ValueType == "double")) { throw new Exception("Type of value is not valid!"); }
            return CommonOperation.ConvertToDouble(ParameterValue);
        }
        /// <summary>
        /// Возвращает значение параметра в виде строки
        /// </summary>
        /// <returns></returns>
        public string GetStringValue()
        {
            return ParameterValue;
        }
        /// <summary>
        /// Возвращает значение параметра в виде иудува значения
        /// </summary>
        /// <returns></returns>
        public bool GetBoolValue()
        {
            if (!(ValueType == "bool")) { throw new Exception("Type of value is not valid!"); }
            return Convert.ToBoolean(ParameterValue);
        }


        public object Clone()
        {
            StoredParam newObject = new StoredParam();
            newObject.Id = ProgrammSettings.CurrentId;
            newObject.ValueType = ValueType;
            newObject.ParameterValue = ParameterValue;
            return newObject;
        }
    }
}
