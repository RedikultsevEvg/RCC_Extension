﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using DAL.Common;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс коэффициента надежности по материалу
    /// </summary>
    public class SafetyFactor :ISavableToDataSet
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
        #endregion
        #region Constructors
        public SafetyFactor()
        {
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
            dataTable = dataSet.Tables["Safetyfactors"];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            row.SetField("Name", Name);
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
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, "Safetyfactors", Id));
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
            DsOperation.DeleteRow(dataSet, "Safetyfactors", Id);
            throw new NotImplementedException();
        }
        #endregion
    }
}
