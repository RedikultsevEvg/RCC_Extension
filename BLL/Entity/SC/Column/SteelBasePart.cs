using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;
using System.Collections.Generic;
using RDBLL.Common.Interfaces;
using System.Data;
using System.Linq;
using DAL.Common;
using RDBLL.Entity.Common.Placements;

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// Класс участков баз стальных колонн
    /// </summary>
    public class SteelBasePart : ICloneable, IHasParent
    {
        #region Properties
        /// <summary>
        /// Код участка
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
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

        Placement Placement { get; set; }

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
            FixLeft = true;
            FixRight = true;
            FixTop = true;
            FixBottom = true;
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
        /// <param name="parent"></param>
        public SteelBasePart(SteelBase parent)
        {
            RegisterParent(parent);
            SetDefault();
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "SteelBaseParts"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables[GetTableName()];
            if (createNew)
            {
                row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            else
            {
                var tmpRow = (from dataRow in dataTable.AsEnumerable()
                              where dataRow.Field<int>("Id") == Id
                              select dataRow).Single();
                row = tmpRow;
            }
            #region
            DsOperation.SetId(row, Id, Name, ParentMember.Id);            
            row.SetField("Width", Width);
            row.SetField("Length", Length);
            row.SetField("LeftOffset", LeftOffset);
            row.SetField("RightOffset", RightOffset);
            row.SetField("TopOffset", TopOffset);
            row.SetField("BottomOffset", BottomOffset);
            row.SetField("FixLeft", FixLeft);
            row.SetField("FixRight", FixRight);
            row.SetField("FixTop", FixTop);
            row.SetField("FixBottom", FixBottom);
            #endregion
            dataTable.AcceptChanges();
        }
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[GetTableName()];
            var row = (from dataRow in dataTable.AsEnumerable()
                       where dataRow.Field<int>("Id") == Id
                       select dataRow).Single();
            OpenFromDataSet(row);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            Width = dataRow.Field<double>("Width");
            Length = dataRow.Field<double>("Length");
            LeftOffset = dataRow.Field<double>("LeftOffset");
            RightOffset = dataRow.Field<double>("RightOffset");
            TopOffset = dataRow.Field<double>("TopOffset");
            BottomOffset = dataRow.Field<double>("BottomOffset");
            FixLeft = dataRow.Field<bool>("FixLeft");
            FixRight = dataRow.Field<bool>("FixRight");
            FixTop = dataRow.Field<bool>("FixTop");
            FixBottom = dataRow.Field<bool>("FixBottom");
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
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            SteelBasePart steelBasePart = this.MemberwiseClone() as SteelBasePart;
            steelBasePart.Placement = Placement.Clone() as Placement;
            return steelBasePart;
        }
        /// <summary>
        /// Регистрация на родителе
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            SteelBase steelBase = parent as SteelBase;
            ParentMember = steelBase;
            steelBase.SteelBaseParts.Add(this);
        }
        //Удаление регистрации родителя
        public void UnRegisterParent()
        {
            SteelBase steelBase = ParentMember as SteelBase;
            steelBase.SteelBaseParts.Remove(this);
            ParentMember = null;
        }
    }
}
