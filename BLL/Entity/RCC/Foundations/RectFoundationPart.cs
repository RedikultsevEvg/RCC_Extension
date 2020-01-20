﻿using RDBLL.Common.Interfaces;
using System;
using System.ComponentModel;
using System.Data;

namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс части (ступени) фундамента
    /// </summary>
    public class RectFoundationPart :FoundationPart, ISavableToDataSet, IDataErrorInfo
    {
        #region fields and properties
        /// <summary>
        /// Ширина (размер вдоль оси X)
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина (размер вдоль оси Y)
        /// </summary>
        public double Length { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров 
        /// </summary>
        public RectFoundationPart() :base()
        {

        }
        /// <summary>
        /// Конструктор ступени по фундаменту
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        public RectFoundationPart(Foundation foundation) :base(foundation)
        {
            if (foundation.Parts.Count == 0)
            {
                Width = 1.2;
                Length = 1.2;
            }
            else
            {
                Width = foundation.Parts[(foundation.Parts.Count - 1)].Width + 0.6;
                Length = foundation.Parts[(foundation.Parts.Count - 1)].Length + 0.6;
            }
        }
        #endregion
        #region methods
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["FoundationParts"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
                { Id, FoundationId, Name, Width, Length, Height, CenterX, CenterY};
            dataTable.Rows.Add(dataRow);
        }
        public void OpenFromDataSet(DataSet dataSet, int i)
        {
            throw new NotImplementedException();
        }
        #endregion
        public string Error { get { throw new NotImplementedException(); } }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Width":
                        {
                            if (Width <= 0)
                            {
                                error = "Ширина ступени не может быть меньше нуля";
                            }
                        }
                        break;
                    case "Length":
                        {
                            if (Length <= 0)
                            {
                                error = "Длина ступени не может быть меньше нуля";
                            }
                        }
                        break;
                    case "Height":
                        {
                            if (Height <= 0)
                            {
                                error = "Высота ступени не может быть меньше нуля";
                            }
                        }
                        break;
                }
                return error;
            }
        }
    }
}
