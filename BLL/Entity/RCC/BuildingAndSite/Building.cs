using DAL.Common;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.RCC.WallAndColumn;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace RDBLL.Entity.RCC.BuildingAndSite
{
    /// <summary>
    /// Класс здания
    /// </summary>
    public class Building : ICloneable, IHasParent, IDataErrorInfo
    {
        /// <summary>
        /// Код здания
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на строительный объект
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Относительная отметка
        /// </summary>
        public double RelativeLevel { get; set; }
        /// <summary>
        /// Абсолютная отметка, соответствующая указанной относительной отметке
        /// </summary>
        public double AbsoluteLevel { get; set; }
        /// <summary>
        /// Абсолютная отметка планировки
        /// </summary>
        public double AbsolutePlaningLevel { get; set; }
        /// <summary>
        /// Предельное значение осадки фундамента
        /// </summary>
        public double MaxFoundationSettlement { get; set; }
        /// <summary>
        /// Флаг отнесения здания к жесткой системе
        /// </summary>
        public bool IsRigid { get; set; }
        /// <summary>
        /// Отношение, характеризующее жесткость здания
        /// </summary>
        public double RigidRatio { get; set; }
        /// <summary>
        /// Коллекция уровней
        /// </summary>
        public ObservableCollection<Level> Children { get; set; }
        ///// <summary>
        ///// Коллекция типов стен
        ///// </summary>
        //public ObservableCollection<WallType> WallTypeList { get; set; }
        ///// <summary>
        ///// Коллекция отверстий
        ///// </summary>
        //public ObservableCollection<OpeningType> OpeningTypeList { get; set; }
        /// <summary>
        /// Наименование линейных единиц измерения
        /// </summary>
        public string LinearMeasure { get { return MeasureUnitConverter.GetUnitLabelText(0); } }
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Building()
        {
            Children = new ObservableCollection<Level>();
            //WallTypeList = new ObservableCollection<WallType>();
            //OpeningTypeList = new ObservableCollection<OpeningType>();
        }
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public Building(BuildingSite buildingSite)
        {
            Id = ProgrammSettings.CurrentId;
            Name = "Мое здание";
            RelativeLevel = 0;
            AbsoluteLevel = 260;
            AbsolutePlaningLevel = 260;
            RegisterParent(buildingSite);
            MaxFoundationSettlement = 0.08;
            IsRigid = false;
            RigidRatio = 4;
            Children = new ObservableCollection<Level>();
            //WallTypeList = new ObservableCollection<WallType>();
            //OpeningTypeList = new ObservableCollection<OpeningType>();
        }
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "Buildings"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="createNew">Флаг инсерт/апдейт</param>
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
            DsOperation.SetId(row, Id, Name, ParentMember.Id);
            row.SetField("RelativeLevel", RelativeLevel);
            row.SetField("AbsoluteLevel", AbsoluteLevel);
            row.SetField("AbsolutePlaningLevel", AbsolutePlaningLevel);
            row.SetField("MaxFoundationSettlement", MaxFoundationSettlement);
            row.SetField("IsRigid", IsRigid);
            row.SetField("RigidRatio", RigidRatio);
            dataTable.AcceptChanges();
            foreach (Level level in Children)
            {
                level.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Открытие из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[GetTableName()];
            var building = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
            OpenFromDataSet(building);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            RelativeLevel = dataRow.Field<double>("RelativeLevel");
            AbsoluteLevel = dataRow.Field<double>("AbsoluteLevel");
            AbsolutePlaningLevel = dataRow.Field<double>("AbsolutePlaningLevel");
            MaxFoundationSettlement = dataRow.Field<double>("MaxFoundationSettlement");
            IsRigid = dataRow.Field<bool>("IsRigid");
            RigidRatio = dataRow.Field<double>("RigidRatio");
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
            return this.MemberwiseClone();
        }
        /// <summary>
        /// Регистрирует родительский элемент
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            BuildingSite buildingSite;
            try
            {
                buildingSite = parent as BuildingSite;
            }
            catch
            {
                throw new Exception("Parent type is not valid");
            }
            buildingSite.Childs.Add(this);
            ParentMember = buildingSite;
        }

        public void UnRegisterParent()
        {
            BuildingSite buildingSite = ParentMember as BuildingSite;
            buildingSite.Childs.Remove(this);
            ParentMember = null;
        }

        /// <summary>
        /// Поле для ошибки
        /// </summary>
        public string Error { get { throw new NotImplementedException(); } }
        /// <summary>
        /// Поле для проверки на ошибки
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "RelativeLevel":
                        {
                            if (RelativeLevel < -5)
                            {
                                error = "Ошибка относительной отметки";
                            }
                        }
                        break;
                    case "AbsoluteLevel":
                        {
                            if (AbsoluteLevel <= 0)
                            {
                                error = "Абсолютная отметка не может быть меньше нуля";
                            }
                            if (AbsoluteLevel >= 1000)
                            {
                                error = "Абсолютная отметка не может быть больше 1000м";
                            }
                        }
                        break;
                    case "AbsolutePlaningLevel":
                        {
                            if (AbsolutePlaningLevel <= 0)
                            {
                                error = "Абсолютная отметка планировки не может быть меньше нуля";
                            }
                            if (AbsolutePlaningLevel < AbsoluteLevel - 20)
                            {
                                error = "Абсолютная отметка планировки назначена неверно";
                            }
                        }
                        break;
                    case "MaxFoundationSettlement":
                        {
                            if (MaxFoundationSettlement < 0)
                            {
                                error = "Введите корректное значение осадки";
                            }
                        }
                        break;
                    case "RigidRatio":
                        {
                            if (RigidRatio < 0)
                            {
                                error = "Отношение длины к высоте не может быть меньше нуля";
                            }
                        }
                        break;
                }
                return error;
            }
        }
    }
}
