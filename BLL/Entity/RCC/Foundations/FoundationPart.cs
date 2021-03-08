﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Service;
using RDBLL.Common.Interfaces;
using System.Data;
using DAL.Common;
using RDBLL.Entity.RCC.Foundations.Processors;

namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс ступени фундамента
    /// </summary>
    public abstract class FoundationPart : IDsSaveable
    {
        /// <summary>
        /// Класс результатов вычислений ступени фундамента
        /// </summary>
        public class PartResult
        {
            /// <summary>
            /// Координата верха ступени, считая от подошвы фундамента
            /// </summary>
            public double ZMax { get; set; }
            /// <summary>
            /// Свойство для хранения комбинаций усилий в подошве фундамента
            /// </summary>
            public FoundationBodyProcessor.PartMomentAreas partMomentAreas { get; set; }
            /// <summary>
            /// Момент обрызования трещин в двух направлениях
            /// </summary>
            public double[] Mcrc { get; set; }
            /// <summary>
            /// Ширина раскрытия трещин в двух направлениях
            /// </summary>
            public double[] CrcWidth { get; set; }
            /// <summary>
            /// Предельный момент для ступени в двух направлениях
            /// </summary>
            public double[,] Mult { get; set; }
            /// <summary>
            /// Рекомендуемая площадь арматуры в двух направлениях
            /// </summary>
            public double[] AsRec { get; set; }
        }
        /// <summary>
        /// Код ступени фундамента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код фундамента
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// Обратная ссылка на фундамент
        /// </summary>
        public Foundation Foundation { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
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
        /// Свойство результатов
        /// </summary>
        public PartResult Result { get; set; }
        /// <summary>
        /// Наименование линейных единиц измерения
        /// </summary>
        public string LinearMeasure { get { return MeasureUnitConverter.GetUnitLabelText(0); } }

        #region events
        public class PartEventHandler : EventArgs
        {

        }

        public void Change()
        {
            Foundation.IsActual = false;
            AfterChanged?.Invoke(this, new PartEventHandler());
        }
        public void Delete()
        {
            BeforeDelete?.Invoke(this, new PartEventHandler());
        }
        public event EventHandler<PartEventHandler> AfterChanged;
        public event EventHandler<PartEventHandler> BeforeDelete;
        #endregion


        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public FoundationPart()
        {

        }
        /// <summary>
        /// Конструктор по фундаменту
        /// </summary>
        /// <param name="foundation"></param>
        public FoundationPart(Foundation foundation)
        {
            Id = ProgrammSettings.CurrentId;
            ParentId = foundation.Id;
            Foundation = foundation;
            if (foundation.Parts.Count == 0)
            {
                Name = "Подколонник";
                Height = 1;
            }
            else
            {
                Name = "Ступень " + foundation.Parts.Count;
                Height = 0.3;
            }
            CenterX = 0;
            CenterY = 0;
        }
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "FoundationParts"; }
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public virtual void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Обновляет запись в соответствии с датасетом
        /// </summary>
        /// <param name="dataSet"></param>
        public virtual void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public virtual void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            ParentId = dataRow.Field<int>("ParentId");
            Name = dataRow.Field<string>("Name");
            Height = dataRow.Field<double>("Height");
            CenterX = dataRow.Field<double>("CenterX");
            CenterY = dataRow.Field<double>("CenterY");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
    }
}
