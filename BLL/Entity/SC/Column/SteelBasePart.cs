using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;
using System.Collections.Generic;
using RDBLL.Common.Interfaces;
using System.Data;
using System.Linq;
using RDBLL.Entity.Common.Placements;
using RDBLL.Common.Geometry;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.SC.Column.SteelBases;

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// Класс участков баз стальных колонн
    /// </summary>
    public class SteelBasePart : ICloneable, IHasParent, IRectangle
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
        /// Центр участка
        /// </summary>
        public Point2D Center { get; set; }
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
        /// Коллекция элементарных участков
        /// </summary>
        public List<NdmRectangleArea> SubParts { get; set; }
        public MeasureUnitList Measures { get => new MeasureUnitList(); }

        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public SteelBasePart(bool genId = false)
        {
            if (genId) Id = ProgrammSettings.CurrentId;
            Center = new Point2D();
        }
        /// <summary>
        /// Конструктор по стальной базе
        /// </summary>
        /// <param name="parent"></param>
        public SteelBasePart(IDsSaveable parent)
        {
            Id = ProgrammSettings.CurrentId;
            RegisterParent(parent);
            Name = "Новый участок";
            Width = 0.2;
            Length = 0.2;
            FixLeft = false;
            FixRight = false;
            FixTop = false;
            FixBottom = false;
            Center = new Point2D();
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
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region        
            row.SetField("LeftOffset", LeftOffset);
            row.SetField("RightOffset", RightOffset);
            row.SetField("TopOffset", TopOffset);
            row.SetField("BottomOffset", BottomOffset);
            row.SetField("FixLeft", FixLeft);
            row.SetField("FixRight", FixRight);
            row.SetField("FixTop", FixTop);
            row.SetField("FixBottom", FixBottom);
            #endregion
            row.AcceptChanges();
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
            EntityOperation.SetProps(dataRow, this);
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
            steelBasePart.Id = ProgrammSettings.CurrentId;
            return steelBasePart;
        }
        /// <summary>
        /// Регистрация на родителе
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            ParentMember = parent;
            if (parent is SteelBase)
            {
                SteelBase steelBase = parent as SteelBase;
                steelBase.SteelBaseParts.Add(this);
            }
            if (parent is SteelBasePartGroup)
            {
                SteelBasePartGroup partGroup = parent as SteelBasePartGroup;
                partGroup.SteelBaseParts.Add(this);
            }
        }
        //Удаление регистрации родителя
        public void UnRegisterParent()
        {
            if (ParentMember is SteelBase)
            {
                SteelBase steelBase = ParentMember as SteelBase;
                steelBase.SteelBaseParts.Remove(this);
            }
            if (ParentMember is SteelBasePartGroup)
            {
                SteelBasePartGroup partGroup = ParentMember as SteelBasePartGroup;
                partGroup.SteelBaseParts.Remove(this);
            }
            ParentMember = null;
        }

        public double GetArea()
        {
            return Width * Length;
        }

        public double GetPerimeter()
        {
            throw new NotImplementedException();
        }
    }
}
