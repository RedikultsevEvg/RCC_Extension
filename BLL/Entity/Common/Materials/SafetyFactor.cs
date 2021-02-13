using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using DAL.Common;
using RDBLL.Common.Service;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс коэффициента надежности по материалу
    /// </summary>
    public class SafetyFactor :IHasParent, IDuplicate
    {
        #region Properties
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Коэффициент надежности для 1 группы ПС
        /// </summary>
        public double PsfFst { get; set; }
        /// <summary>
        /// Коэффициент надежности для 2 группы ПС
        /// </summary>
        public double PsfSnd { get; set; }
        /// <summary>
        /// Коэффициент надежности для 1 группы ПС для длительных нагрузок
        /// </summary>
        public double PsfFstLong { get; set; }
        /// <summary>
        /// Коэффициент надежности для 2 группы ПС для длительных нагрузок
        /// </summary>
        public double PsfSndLong { get; set; }
        /// <summary>
        /// Ссылка на родителя
        /// </summary>
        public ISavableToDataSet ParentMember { get; private set; }
        private static string TableName { get { return "SafetyFactors"; } }
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
            PsfFst = 1.0;
            PsfSnd = 1.0;
            PsfFstLong = 1.0;
            PsfSndLong = 1.0;
        }
        #endregion
        #region IODataset
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables[TableName];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            row.SetField("Name", Name);
            row.SetField("ParentId", ParentMember.Id);
            row.SetField("PsfFst", PsfFst);
            row.SetField("PsfSnd", PsfSnd);
            row.SetField("PsfFstLong", PsfFstLong);
            row.SetField("PsfSndLong", PsfSndLong);
            #endregion
            dataTable.AcceptChanges();

        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, TableName, Id));
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            PsfFst = dataRow.Field<double>("PsfFst");
            PsfSnd = dataRow.Field<double>("PsfSnd");
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, TableName, Id);
        }
        #endregion
        #region Methods
        public void RegisterParent(ISavableToDataSet materialUsing)
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
        public object Duplicate()
        {
            SafetyFactor safetyFactor = new SafetyFactor(true);
            safetyFactor.Name = Name;
            safetyFactor.PsfFst = PsfFst;
            safetyFactor.PsfSnd = PsfSnd;
            safetyFactor.PsfFstLong = PsfFstLong;
            safetyFactor.PsfSndLong = PsfSndLong;
            return safetyFactor;
        }
        #endregion
    }
}
