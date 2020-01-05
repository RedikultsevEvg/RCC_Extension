using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using System;
using System.ComponentModel;
using System.Data;

namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс части (ступени) фундамента
    /// </summary>
    public class FoundationPart :ISavableToDataSet, IDataErrorInfo
    {
        #region fields and properties
        /// <summary>
        /// Код ступени фундамента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код фундамента
        /// </summary>
        public int FoundationId { get; set; }
        /// <summary>
        /// Обратная ссылка на фундамент
        /// </summary>
        public Foundation Foundation { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ширина (размер вдоль оси X)
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина (размер вдоль оси Y)
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Высота
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Положение центра ступени по X
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Положение центра ступени по Y
        /// </summary>
        public double CenterY { get; set; }
        /// <summary>
        /// Наименование линейных единиц измерения
        /// </summary>
        public string LinearMeasure { get { return MeasureUnitConverter.GetUnitLabelText(0); } }
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров 
        /// </summary>
        public FoundationPart()
        {

        }
        /// <summary>
        /// Конструктор ступени по фундаменту
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        public FoundationPart(Foundation foundation)
        {
            Id = ProgrammSettings.CurrentId;
            FoundationId = foundation.Id;
            Foundation = foundation;
            if (foundation.Parts.Count == 0)
            {
                Name = "Подколонник";
                Width = 1.2;
                Length = 1.2;
                Height = 1;
            }
            else
            {
                Name = "Ступень " + foundation.Parts.Count;
                Width = foundation.Parts[(foundation.Parts.Count - 1)].Width + 0.6;
                Length = foundation.Parts[(foundation.Parts.Count - 1)].Length + 0.6;
                Height = 0.3;
            }
            CenterX = 0;
            CenterY = 0;
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
