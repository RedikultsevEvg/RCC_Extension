using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс коэффициента надежности по материалу
    /// </summary>
    public class SafetyFactor :IHasParent, IDuplicate
    {
        #region Properties
        private double[] _Coefficients;

        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        public static int CoefCount { get => 12; }
        public bool SetDiffTens { get; set; }
        public bool SetDiffLSS { get; set; }
        public bool SetDiffLong { get; set; }
        /// <summary>
        /// Коэффициент надежности для 1 группы ПС
        /// </summary>
        public double PsfFst { get => _Coefficients[0]; set { _Coefficients[0] = value; } }
        /// <summary>
        /// Коэффициент надежности для 1 группы ПС
        /// </summary>
        public double PsfFstTens { get => _Coefficients[1]; set { _Coefficients[1] = value; } }
        /// <summary>
        /// Коэффициент надежности для 2 группы ПС
        /// </summary>
        public double PsfSnd { get => _Coefficients[2]; set { _Coefficients[2] = value; } }
        /// <summary>
        /// Коэффициент надежности для 2 группы ПС
        /// </summary>
        public double PsfSndTens { get => _Coefficients[3]; set { _Coefficients[3] = value; } }
        /// <summary>
        /// Коэффициент надежности для 1 группы ПС для длительных нагрузок
        /// </summary>
        public double PsfFstLong { get => _Coefficients[4]; set { _Coefficients[4] = value; } }
        /// <summary>
        /// Коэффициент надежности для 1 группы ПС для длительных нагрузок
        /// </summary>
        public double PsfFstLongTens { get => _Coefficients[5]; set { _Coefficients[5] = value; } }
        /// <summary>
        /// Коэффициент надежности для 2 группы ПС для длительных нагрузок
        /// </summary>
        public double PsfSndLong { get => _Coefficients[6]; set { _Coefficients[6] = value; } }
        /// <summary>
        /// Коэффициент надежности для 2 группы ПС для длительных нагрузок
        /// </summary>
        public double PsfSndLongTens { get => _Coefficients[7]; set { _Coefficients[7] = value; } }
        /// <summary>
        /// Коэффициент надежности модуля упругости при сжатии для 1 группы ПС
        /// </summary>
        public double PsfEFst { get => _Coefficients[8]; set { _Coefficients[8] = value; } }
        /// <summary>
        /// Коэффициент надежности модуля упругости при растяжении для 1 группы ПС
        /// </summary>
        public double PsfEFstTens { get => _Coefficients[9]; set { _Coefficients[9] = value; } }
        /// <summary>
        /// Коэффициент надежности модуля упругости при сжатии для 2 группы ПС
        /// </summary>
        public double PsfESnd { get => _Coefficients[10]; set { _Coefficients[10] = value; } }
        /// <summary>
        /// Коэффициент надежности модуля упругости при растяжении для 2 группы ПС
        /// </summary>
        public double PsfESndTens { get => _Coefficients[11]; set { _Coefficients[11] = value; } }

        /// <summary>
        /// Ссылка на родителя
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public SafetyFactor()
        { }
        /// <summary>
        /// Конструктор с настройками по умолчанию
        /// </summary>
        /// <param name="GenId"></param>
        public SafetyFactor(bool GenId)
        {
            if (GenId) Id = ProgrammSettings.CurrentId;
            Name = "Новый коэффициент надежности по материалу";
            _Coefficients = new double[CoefCount];
            for (int i = 0; i <= CoefCount - 1; i++)
            {
                _Coefficients[i] = 1.0;
            }
        }
        #endregion
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "SafetyFactors"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region setFields
            for (int i=0; i<=CoefCount - 1; i++)
            {
                row.SetField(Convert.ToString(i), _Coefficients[i]);
            }
            #endregion
            row.AcceptChanges();

        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            _Coefficients = new double[12];
            for (int i = 0; i <= CoefCount - 1; i++)
            {
                _Coefficients[i] = dataRow.Field<double>(Convert.ToString(i));
            }
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        #region Methods
        public void RegisterParent(IDsSaveable materialUsing)
        {
            ParentMember = materialUsing;
        }
        public void UnRegisterParent()
        {
            ParentMember = null;
        }
        /// <summary>
        /// Дублирует текущий объект
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            SafetyFactor safetyFactor = MemberwiseClone() as SafetyFactor;
            safetyFactor.Id = ProgrammSettings.CurrentId;
            return safetyFactor;
        }

        public void SetCoefArray(double[] array)
        {
            _Coefficients = array;
        }
        #endregion
    }
}
