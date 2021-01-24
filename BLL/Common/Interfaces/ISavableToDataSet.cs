﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RDBLL.Common.Interfaces
{
    public interface ISavableToDataSet
    {
        int Id { get; set; }
        void SaveToDataSet(DataSet dataSet, bool createNew);
        void OpenFromDataSet(DataSet dataSet);
        void OpenFromDataSet(DataRow dataRow);
        void DeleteFromDataSet(DataSet dataSet);

        /*
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
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
            #endregion
            dataTable.AcceptChanges();    
            throw new NotImplementedException();
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, "MyTable", Id);    
            throw new NotImplementedException();
        }
        */
    }
}
