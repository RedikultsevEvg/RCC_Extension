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
        //Properties
        #region
        public int Id { get; set; } //Код участка
        public int SteelBaseId { get; set; } //Код участка
        public SteelBase SteelBase { get; set; } //База стальной колонны к которой относится участок
        public String Name { get; set; } //Имя участка
        public double Width { get; set; } //Ширина участка
        public double Length { get; set; } //Длина участка
        public double CenterX { get; set; } //Привязка центра
        public double CenterY { get; set; } //Привязка центра
        public double LeftOffset { get; set; } //Смещение левой границы 
        public double RightOffset { get; set; } //Смещение правой границы
        public double TopOffset { get; set; } //Смещение верхней границы
        public double BottomOffset { get; set; } //Смещение нижней границы
        public bool FixLeft { get; set; } //Опора по левой границе
        public bool FixRight { get; set; } //Опора по правой границе
        public bool FixTop { get; set; } //Опора по верхней границе
        public bool FixBottom { get; set; } //Опора по нижней границе
        public bool AddSymmetricX { get; set; } //Наличие симметричного участка относительно оси X
        public bool AddSymmetricY { get; set; } //Наличие симметричного участка по оси Y

        public List<NdmConcreteArea> SubParts { get; set; }

        #endregion

        #region Constructors
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
        public SteelBasePart()
        {
        }
        public SteelBasePart(SteelBase columnBase)
        {
            SteelBaseId = columnBase.Id;
            SteelBase = columnBase;
            SetDefault();
        }
        #endregion
        #region Methods
        public void SaveToDataSet(DataSet dataSet)
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

        public void OpenFromDataSet(DataSet dataSet, int Id)
        {

        }
        #endregion
        //IClonable
        public object Clone()
        {
            SteelBasePart steelBasePart = this.MemberwiseClone() as SteelBasePart;
            steelBasePart.CenterX = this.CenterX;
            steelBasePart.CenterY = this.CenterY;
            return steelBasePart;
        }
    }
}
