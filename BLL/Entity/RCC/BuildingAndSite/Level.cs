using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Entity.RCC.WallAndColumn;
using RDBLL.Common.Service;
using System.Xml;
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;
using System.Data;
using RDBLL.Common.Interfaces;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Entity.RCC.BuildingAndSite
{
    /// <summary>
    /// Уровень
    /// </summary>
    public class Level :ICloneable, IHasParent
    {
        /// <summary>
        /// Код уровня
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на здание
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Отметка уровня
        /// </summary>
        public double Elevation { get; set; }
        /// <summary>
        /// Высота этажа
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Сдвижка сверху
        /// </summary>
        public double TopOffset { get; set; }
        /// <summary>
        /// Привязка базовой точки
        /// </summary>
        public double BasePointX { get; set; }
        /// <summary>
        /// Привязка базовой точки
        /// </summary>
        public double BasePointY { get; set; }
        /// <summary>
        /// Привязка базовой точки
        /// </summary>
        public double BasePointZ { get; set; }
        /// <summary>
        /// Привязка базовой точки
        /// </summary>
        public ObservableCollection<Wall> Walls { get; set; }
        /// <summary>
        /// Коллекция колонн
        /// </summary>
        public ObservableCollection<Column> Columns { get; set; }
        /// <summary>
        /// Коллекция стальных баз
        /// </summary>
        public ObservableCollection<SteelBase> SteelBases { get; set; }
        /// <summary>
        /// Коллекция фундаментов
        /// </summary>
        public ObservableCollection<IHasParent> Children { get; set; }

        /// <summary>
        /// Получение суммарного объема бетона
        /// </summary>
        /// <returns></returns>
        public double GetConcreteVolumeNetto()
        {
            double volume = 0;
            foreach (Wall obj in Walls)
            {
                volume += obj.GetConcreteVolumeNetto();
            }
            return volume;

        }
        #region IODataSet
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "Levels"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
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
            #region SetField
            row.SetField("Id", Id);
            row.SetField("ParentId", ParentMember.Id);
            row.SetField("Name", Name);
            row.SetField("FloorLevel", Elevation);
            row.SetField("Height", Height);
            row.SetField("TopOffset", TopOffset);
            row.SetField("BasePointX", BasePointX);
            row.SetField("BasePointY", BasePointY);
            row.SetField("BasePointZ", BasePointZ);
            #endregion
            dataTable.AcceptChanges();

            foreach (SteelBase steelBase in SteelBases)
            {
                steelBase.SaveToDataSet(dataSet, createNew);
            }
            foreach (Foundation foundation in Children)
            {
                foundation.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Открывает запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[GetTableName()];
            var level = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
            OpenFromDataSet(level);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            Elevation = dataRow.Field<double>("FloorLevel");
            Height = dataRow.Field<double>("Height");
            TopOffset = dataRow.Field<double>("TopOffset");
            BasePointX = dataRow.Field<double>("BasePointX");
            BasePointY = dataRow.Field<double>("BasePointY");
            BasePointZ = dataRow.Field<double>("BasePointZ");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (SteelBase steelBase in SteelBases)
            {
                steelBase.DeleteFromDataSet(dataSet);
            }
            foreach (Foundation foundation in Children)
            {
                foundation.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Level ()
        {
            Walls = new ObservableCollection<Wall>();
            SteelBases = new ObservableCollection<SteelBase>();
            Children = new ObservableCollection<IHasParent>();
        }

        /// <summary>
        /// Конструктор по зданию
        /// </summary>
        /// <param name="building"></param>
        public Level (Building building)
        {
            if (Id == 0) { Id = ProgrammSettings.CurrentId; }
            else {
                this.Id = Id;
            }         
            Name = "Этаж 1";
            ParentMember = building;
            Elevation = 0;
            Height = 3;
            TopOffset = -0.2;
            //Walls = new ObservableCollection<Wall>();
            SteelBases = new ObservableCollection<SteelBase>();
            Children = new ObservableCollection<IHasParent>();
        }

        /// <summary>
        /// Метод клонирования
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void RegisterParent(IDsSaveable parent)
        {
            Building building = parent as Building;
            building.Children.Add(this);
            ParentMember = parent;
        }

        public void UnRegisterParent()
        {
            Building building = ParentMember as Building;
            building.Children.Remove(this);
            ParentMember = null;
        }
    }
}
