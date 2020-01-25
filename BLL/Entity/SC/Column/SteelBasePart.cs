using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;
using System.Collections.Generic;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// Класс участков баз стальных колонн
    /// </summary>
    public class SteelBasePart : ICloneable, ISavableToDataSet
    {
        #region Properties
        /// <summary>
        /// Код участка
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код стальной базы
        /// </summary>
        public int SteelBaseId { get; set; }
        /// <summary>
        /// Обратная ссылка на базу стальной колонны
        /// </summary>
        public SteelBase SteelBase { get; set; } //База стальной колонны к которой относится участок
        /// <summary>
        /// Наименование
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Ширина участка
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина участка
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Привязка центра
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Привязка центра
        /// </summary>
        public double CenterY { get; set; }
        /// <summary>
        /// Смещение левой границы 
        /// </summary>
        public double LeftOffset { get; set; }
        /// <summary>
        /// Смещение правой границы
        /// </summary>
        public double RightOffset { get; set; }
        /// <summary>
        /// Смещение верхней границы
        /// </summary>
        public double TopOffset { get; set; }
        /// <summary>
        /// Смещение нижней границы
        /// </summary>
        public double BottomOffset { get; set; }
        /// <summary>
        /// Опора по левой границе
        /// </summary>
        public bool FixLeft { get; set; }
        /// <summary>
        /// Опора по правой границе
        /// </summary>
        public bool FixRight { get; set; }
        /// <summary>
        /// Опора по верхней границе
        /// </summary>
        public bool FixTop { get; set; }
        /// <summary>
        /// Опора по нижней границе
        /// </summary>
        public bool FixBottom { get; set; }
        /// <summary>
        /// Наличие симметричного участка относительно оси X
        /// </summary>
        public bool AddSymmetricX { get; set; }
        /// <summary>
        /// Наличие симметричного участка по оси Y
        /// </summary>
        public bool AddSymmetricY { get; set; }

        /// <summary>
        /// Коллекция элементарных участков
        /// </summary>
        public List<NdmRectangleArea> SubParts { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Метод настройки параметров по умолчания
        /// </summary>
        public void SetDefault()
        {
            Id = ProgrammSettings.CurrentId;
            Name = "Новый участок";
            Width = 0.2;
            Length = 0.2;
            CenterX = 0;
            CenterY = 0;
            FixLeft = true;
            FixRight = true;
            FixTop = true;
            FixBottom = true;
            AddSymmetricX = true;
            AddSymmetricY = true;
        }
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public SteelBasePart()
        {
        }
        /// <summary>
        /// Конструктор по стальной базе
        /// </summary>
        /// <param name="columnBase"></param>
        public SteelBasePart(SteelBase columnBase)
        {
            SteelBaseId = columnBase.Id;
            SteelBase = columnBase;
            SetDefault();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["SteelBaseParts"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
            { Id, SteelBaseId, Name,
                Width, Length,
                CenterX, CenterY,
                LeftOffset, RightOffset, TopOffset, TopOffset,
                FixLeft, FixRight, FixTop, FixBottom,
                AddSymmetricX, AddSymmetricY
            };
            dataTable.Rows.Add(dataRow);
        }

        public void OpenFromDataSet(DataSet dataSet)
        {

        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        #endregion
        //IClonable
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            SteelBasePart steelBasePart = this.MemberwiseClone() as SteelBasePart;
            steelBasePart.CenterX = this.CenterX;
            steelBasePart.CenterY = this.CenterY;
            return steelBasePart;
        }
    }
}
