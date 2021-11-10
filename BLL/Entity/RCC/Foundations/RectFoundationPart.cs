using RDBLL.Common.Interfaces;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Foundations.Processors;
using System.Collections.Generic;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Common.Interfaces.Shapes;

namespace RDBLL.Entity.RCC.Foundations
{
    
    /// <summary>
    /// Класс части (ступени) фундамента
    /// </summary>
    public class RectFoundationPart :FoundationPart, IDsSaveable, IDataErrorInfo, ICloneable, IRectangle
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
        public override void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region
            DsOperation.SetId(row, Id, Name, ParentMember.Id);
            row.SetField("Type", "Rect");
            row.SetField("Height", Height);
            row.SetField("CenterX", CenterX);
            row.SetField("CenterY", CenterY);
            #endregion
            row.Table.AcceptChanges();
        }
        /// <summary>
        /// Обновляет запись по строке датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public override void OpenFromDataSet(DataRow dataRow)
        {
            base.OpenFromDataSet(dataRow);
            Width = dataRow.Field<double>("Width");
            Length = dataRow.Field<double>("Length");
        }
        #endregion
        #region IDuplicate
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            RectFoundationPart rectFoundationPart = MemberwiseClone() as RectFoundationPart;
            rectFoundationPart.Id = ProgrammSettings.CurrentId;
            return rectFoundationPart;
        }

        public double GetArea()
        {
            throw new NotImplementedException();
        }

        public double GetPerimeter()
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
