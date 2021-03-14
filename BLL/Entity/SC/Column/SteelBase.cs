using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using DAL.Common;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.Common.Materials;

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// База стальной колонны
    /// </summary>
    public class SteelBase : ICloneable, IHasForcesGroups, IHasParent
    {
        #region Fields
        /// <summary>
        /// Код базы
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ссылка на уровень
        /// </summary>
        public IDsSaveable ParentMember { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public String Name { get; set; } //Наименование
        /// <summary>
        /// Признак актуальности расчета
        /// </summary>
        public bool IsActual { get; set; }
        /// <summary>
        /// Толщина, м
        /// </summary>
        public double Thickness { get; set; }
        /// <summary>
        /// Флаг расчета по упрощенному методу
        /// </summary>
        public bool UseSimpleMethod { get; set; }
        /// <summary>
        /// Коллекция групп нагрузок
        /// </summary>
        public ObservableCollection<ForcesGroup> ForcesGroups { get; set; }
        /// <summary>
        /// Коллекция участков
        /// </summary>
        public ObservableCollection<SteelBasePart> SteelBaseParts { get; set; }

        public ConcreteUsing Conrete { get; set; }
        public SteelUsing Steel { get; set; }

        /// <summary>
        /// Коллекция участков с учетом симметрии
        /// </summary>
        public List<SteelBasePart> ActualSteelBaseParts { get; set; }
        /// <summary>
        /// Коллекция болтов
        /// </summary>
        public ObservableCollection<SteelBolt> SteelBolts { get; set; }
        /// <summary>
        /// Коллекция болтов с учетом симметрии
        /// </summary>
        public List<SteelBolt> ActualSteelBolts { get; set; }
        /// <summary>
        /// Коллекция комбинаций
        /// </summary>
        public ObservableCollection<LoadSet> LoadCases { get; set; }
        /// <summary>
        /// Коллекция всех элементарных участков
        /// </summary>
        public List<NdmArea> NdmAreas { get; set; }
        /// <summary>
        /// Коллекция элементарных участков бетона
        /// </summary>
        public List<NdmArea> ConcreteNdmAreas { get; set; }
        /// <summary>
        /// Коллекция элементарных участков стали
        /// </summary>
        public List<NdmArea> SteelNdmAreas { get; set; }
        /// <summary>
        /// Коллекция комбинаций и кривизны 
        /// </summary>
        public List<ForceDoubleCurvature> ForceCurvatures { get; set; }


        //public ObservableCollection<BoltUsing> Bolts { get; set; }

        #endregion
        #region Constructors
        /// <summary>
        /// Функция установки начальных параметров
        /// </summary>
        public void SetDefault()
        {
            Id = ProgrammSettings.CurrentId;
            Name = "Новая база";
            
            Thickness = 0.06;
            IsActual = false;
            
            UseSimpleMethod = false;
            ForcesGroups = new ObservableCollection<ForcesGroup>();
            ForcesGroups.Add(new ForcesGroup(this));
            SteelBaseParts = new ObservableCollection<SteelBasePart>();
            SteelBolts = new ObservableCollection<SteelBolt>();
            ForceCurvatures = new List<ForceDoubleCurvature>();
            NdmAreas = new List<NdmArea>();
            ConcreteNdmAreas = new List<NdmArea>();
            SteelNdmAreas = new List<NdmArea>();


            // Вложенные объекты по умолчанию
            StartObjects();
        }
        /// <summary>
        /// Создание вложенных объектов по умолчанию
        /// </summary>
        public void StartObjects()
        {
            //Нагрузка
            LoadSet loadSet = new LoadSet(this.ForcesGroups[0]);
            this.ForcesGroups[0].LoadSets.Add(loadSet);
            loadSet.Name = "Постоянная";
            loadSet.ForceParameters.Add(new ForceParameter(loadSet));
            loadSet.ForceParameters[0].KindId = 1; //Продольная сила
            loadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            loadSet.ForceParameters.Add(new ForceParameter(loadSet));
            loadSet.ForceParameters[1].KindId = 2; //Изгибающий момент
            loadSet.ForceParameters[1].CrcValue = 200000; //Изгибающий момент
            loadSet.IsLiveLoad = false;
            loadSet.BothSign = false;
            loadSet.PartialSafetyFactor = 1.1;
            //Участок №1
            SteelBasePart basePart1 = new SteelBasePart(this);
            basePart1.Name = "1";
            basePart1.Width = 0.300;
            basePart1.Length = 0.200;
            basePart1.FixLeft = true;
            basePart1.FixRight = false;
            basePart1.FixTop = false;
            basePart1.FixBottom = true;
            this.SteelBaseParts.Add(basePart1);
            //Участок №2
            SteelBasePart basePart2 = new SteelBasePart(this);
            basePart2.Name = "2";
            basePart2.Width = 0.300;
            basePart2.Length = 0.500;
            basePart2.FixLeft = true;
            basePart2.FixRight = false;
            basePart2.FixTop = true;
            basePart2.FixBottom = true;
            this.SteelBaseParts.Add(basePart2);

        }
        /// <summary>
        /// Создает базу стальной колонны по указанному уровню
        /// </summary>
        /// <param name="level">Уровень, по которому создается колонна</param>
        public SteelBase(Level level)
        {
            SetDefault();
            RegisterParent(level);
        }

        /// <summary>
        /// Создает базу стальной колонны со значениями по умолчанию
        /// </summary>
        public SteelBase(bool genId = false)
        {
            if (genId) { Id = ProgrammSettings.CurrentId; }
            SetDefault();
        }

        public void SetNotActual()
        {
            IsActual = false;
        }

        #endregion
        #region Methods
        #region IODataSet
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "SteelBases"; }
        /// <summary>
        /// Сохраняет данные базы стальной колонны в указанный датасет
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
            SetEntity.SetRow(row, this);
            row.SetField("IsActual", IsActual);
            row.SetField("Thickness", Thickness);
            row.SetField("UseSimpleMethod", UseSimpleMethod);
            #endregion
            dataTable.AcceptChanges();

            foreach (SteelBasePart steelBasePart in SteelBaseParts)
            {
                steelBasePart.SaveToDataSet(dataSet, createNew);
            }
            foreach (SteelBolt steelBolt in SteelBolts)
            {
                steelBolt.SaveToDataSet(dataSet, createNew);
            }
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                forcesGroup.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Обновляет запись по датасету
        /// </summary>
        /// <param name="dataSet"></param>
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
            SetEntity.SetProps(dataRow, this);
            IsActual = false;           
            Thickness = dataRow.Field<double>("Thickness");
            UseSimpleMethod = dataRow.Field<bool>("UseSimpleMethod");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DeleteSubElements(dataSet, "SteelBaseParts");
            DeleteSubElements(dataSet, "SteelBolts");
            DeleteSubElements(dataSet, "SteelBaseForcesGroups");
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                //Нужно сделать проверку, встречается ли группа еще-где-то
                //Если не встречается, то удалять
                forcesGroup.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteSubElements(DataSet dataSet, string tableName)
        {
            DsOperation.DeleteRow(dataSet, tableName, "ParentId", Id);
        }
        #endregion

        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void RegisterParent(IDsSaveable parent)
        {
            Level level = parent as Level;
            ParentMember = level;
            level.SteelBases.Add(this);
        }

        public void UnRegisterParent()
        {
            Level level = ParentMember as Level;
            level.SteelBases.Remove(this);
        }
    }
}
