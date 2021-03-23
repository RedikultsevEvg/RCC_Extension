using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Common.Params
{
    /// <summary>
    /// Перечисление возможных типов параметра
    /// </summary>
    public enum ParamNames
    {
        i, //int
        d, //double
        s, //string
        b //bool
    }

    /// <summary>
    /// Класс универсального хранимого параметра
    /// </summary>
    public class StoredParam : IHasParent, ICloneable
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
        /// <summary>
        /// Ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }

        public StoredParam (IDsSaveable parent)
        {
            RegisterParent(parent);
        }

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
            StoredParam newObject = MemberwiseClone() as StoredParam;
            newObject.UnRegisterParent();
            newObject.Id = ProgrammSettings.CurrentId;
            return newObject;
        }

        /// <summary>
        /// Возвращает имя таблицы, в которое должно производиться сохранение
        /// </summary>
        /// <returns></returns>
        public string GetTableName() => "StoredParams";

        /// <summary>
        /// Сохраняет запись в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region setFields
            row.SetField("Type", ValueType);
            row.SetField("Value", ParameterValue);
            #endregion
            row.Table.AcceptChanges();
        }
        /// <summary>
        /// Открывает запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet) => OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
        /// <summary>
        /// Открывает строку из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            ValueType = dataRow.Field<string>("Type");
            ParameterValue = dataRow.Field<string>("Value");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet) => DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        /// <summary>
        /// Регистрирует ссылку на родителя
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent) => ParentMember = parent;
        /// <summary>
        /// Удаляет ссылку на родителя
        /// </summary>
        public void UnRegisterParent() => ParentMember = null;

    }
}
