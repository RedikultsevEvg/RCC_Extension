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
using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Entity.RCC.Reinforcements.Bars;
using RDBLL.Entity.RCC.Reinforcements.Bars.Storages;
using RDBLL.Common.ErrorProcessing.Messages;
using RDBLL.Entity.RCC.Reinforcements.Ancorages;
using RDBLL.Entity.RCC.Reinforcements.Ancorages.Repositories;

namespace RDBLL.Entity.RCC.BuildingAndSite
{
    /// <summary>
    /// Уровень
    /// </summary>
    public class Level :ICloneable, IHasParent, IHasChildren
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
        /// Коллекция дочерних элементов
        /// </summary>
        public ObservableCollection<IHasId> Children { get; private set; }

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
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            foreach (IHasId child in Children)
                {
                    if (child is IDsSaveable)
                    {
                        
                    }
                    else
                    {
                        string childTableName = DsOperation.GetTableName(child);
                        DataTable dataTable = DsOperation.GetDataTable(dataSet, childTableName);
                        IRepository<IAncorage> repository;
                        if (child is IAncorage)
                        {
                            repository = new AncorageCRUD(dataSet);
                        }
                        else
                        {
                            throw new Exception(CommonMessages.TypeIsUknown);
                        }
                    if (createNew)
                    {
                        repository.Create(child as IAncorage);
                    }
                        else
                    {
                        repository.Update(child as IAncorage);
                    }
                    }
                }
            #region SetField
            row.SetField("FloorLevel", Elevation);
            row.SetField("Height", Height);
            row.SetField("TopOffset", TopOffset);
            row.SetField("BasePointX", BasePointX);
            row.SetField("BasePointY", BasePointY);
            row.SetField("BasePointZ", BasePointZ);
            #endregion
            row.AcceptChanges();
        }
        /// <summary>
        /// Открывает запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            string tableName = DsOperation.GetTableName(this);
            DataTable dataTable = DsOperation.GetDataTable(dataSet, tableName);
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
            foreach (IHasId child in Children)
            {
                if (child is IDsSaveable)
                {
                    IDsSaveable dsChild = child as IDsSaveable;
                    dsChild.DeleteFromDataSet(dataSet);
                }
                else
                {
                    string childTableName = DsOperation.GetTableName(child);
                    DataTable dataTable = DsOperation.GetDataTable(dataSet, childTableName);
                    IRepository<IAncorage> repository;
                    if (child is IAncorage)
                    {
                        repository = new AncorageCRUD(dataSet);
                    }
                    else
                    {
                        throw new Exception(CommonMessages.TypeIsUknown);
                    }
                    try
                    {
                        repository.Delete(child as IAncorage);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            string tableName = DsOperation.GetTableName(this);
            DsOperation.DeleteRow(dataSet, tableName, Id);
        }
        #endregion
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Level (bool genId = false)
        {
            if (genId)
            {
                Id = ProgrammSettings.CurrentId;
            }
            //Walls = new ObservableCollection<Wall>();
            Children = new ObservableCollection<IHasId>();
        }

        /// <summary>
        /// Конструктор по зданию
        /// </summary>
        /// <param name="building"></param>
        public Level (Building building)
        {
            Id = ProgrammSettings.CurrentId;
            Name = "Этаж 1";
            RegisterParent(building);
            Elevation = 0;
            Height = 3;
            TopOffset = -0.2;
            //Walls = new ObservableCollection<Wall>();
            Children = new ObservableCollection<IHasId>();
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
            if (ParentMember != null)
            {
                Building building = ParentMember as Building;
                building.Children.Remove(this);
                ParentMember = null;
            }
        }
    }
}
