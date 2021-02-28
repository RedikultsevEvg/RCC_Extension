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

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// База стальной колонны
    /// </summary>
    public class SteelBase : ICloneable, IDsSaveable, IHasForcesGroups
    {
        #region Fields
        //private bool _isActual;
        private bool _isLoadCasesActual;
        private bool _isBoltsActual;
        private bool _isBasePartsActual;

        /// <summary>
        /// Код базы
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код уровня
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// Ссылка на уровень
        /// </summary>
        public Level Level { get; set; }
        /// <summary>
        /// Код стали
        /// </summary>
        public int SteelClassId { get; set; }
        /// <summary>
        /// Код бетона
        /// </summary>
        public int ConcreteClassId { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public String Name { get; set; } //Наименование
        public double SteelStrength { get; set; } //Расчетное сопротивление базы
        public double ConcreteStrength { get; set; } //Прочность бетона подливки
        /// <summary>
        /// Признак актуальности расчета
        /// </summary>
        public bool IsActual { get; set; }
        /// <summary>
        /// Ширина базы, м
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина базы, м
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Толщина, м
        /// </summary>
        public double Thickness { get; set; }
        /// <summary>
        /// Коэффициент условий работы
        /// </summary>
        public double WorkCondCoef { get; set; }
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
        /// <summary>
        /// Флаг актуальности нагрузок
        /// </summary>
        public bool IsLoadCasesActual
        {
            get {return _isLoadCasesActual; }
            set
            {
                if (!value) SetNotActual();
                _isLoadCasesActual = value;
            }
        }
        /// <summary>
        /// Флаг актуальности участков
        /// </summary>
        public bool IsBasePartsActual
        {
            get { return _isBasePartsActual; }
            set
            {
                if (!value) SetNotActual();
                _isBasePartsActual = value;
            }
        }
        /// <summary>
        /// Флаг актуальности болтов
        /// </summary>
        public bool IsBoltsActual
        {
            get { return _isBoltsActual; }
            set
            {
                if (!value) SetNotActual();
                _isBoltsActual = value;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Функция установки начальных параметров
        /// </summary>
        public void SetDefault()
        {
            Id = ProgrammSettings.CurrentId;
            Name = "Новая база";
            Width = 0.6;
            Length = 0.9;
            Thickness = 0.06;
            WorkCondCoef = 1.1;
            SteelClassId = 1;
            ConcreteClassId = 1;
            IsActual = false;
            SteelStrength = 240000000;
            ConcreteStrength = 10000000;
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
            basePart1.CenterX = 0.150;
            basePart1.CenterY = 0.350;
            basePart1.FixLeft = true;
            basePart1.FixRight = false;
            basePart1.FixTop = false;
            basePart1.FixBottom = true;
            basePart1.AddSymmetricX = true;
            basePart1.AddSymmetricY = true;
            this.SteelBaseParts.Add(basePart1);
            //Участок №2
            SteelBasePart basePart2 = new SteelBasePart(this);
            basePart2.Name = "2";
            basePart2.Width = 0.300;
            basePart2.Length = 0.500;
            basePart2.CenterX = 0.150;
            basePart2.CenterY = 0;
            basePart2.FixLeft = true;
            basePart2.FixRight = false;
            basePart2.FixTop = true;
            basePart2.FixBottom = true;
            basePart2.AddSymmetricX = false;
            basePart2.AddSymmetricY = true;
            this.SteelBaseParts.Add(basePart2);
            //Болты
            SteelBolt steelBolt = new SteelBolt(this);
            this.SteelBolts.Add(steelBolt);
        }
        /// <summary>
        /// Создает базу стальной колонны по указанному уровню
        /// </summary>
        /// <param name="level">Уровень, по которому создается колонна</param>
        public SteelBase(Level level)
        {
            SetDefault();
            LevelId = level.Id;
            Level = level;
        }

        /// <summary>
        /// Создает базу стальной колонны со значениями по умолчанию
        /// </summary>
        public SteelBase()
        {
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
            row.SetField("Id", Id);
            row.SetField("LevelId", LevelId);
            row.SetField("SteelClassId", SteelClassId);
            row.SetField("ConcreteClassId", ConcreteClassId);
            row.SetField("Name", Name);
            row.SetField("SteelStrength", SteelStrength);
            row.SetField("ConcreteStrength", ConcreteStrength);
            row.SetField("IsActual", IsActual);
            row.SetField("Width", Width);
            row.SetField("Length", Length);
            row.SetField("Thickness", Thickness);
            row.SetField("WorkCondCoef", WorkCondCoef);
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
            Id = dataRow.Field<int>("Id");
            LevelId = dataRow.Field<int>("LevelId");
            SteelClassId = dataRow.Field<int>("SteelClassId");
            ConcreteClassId = dataRow.Field<int>("ConcreteClassId");
            //Надо получить ссылки на сталь и бетон
            Name = dataRow.Field<string>("Name");
            SteelStrength = dataRow.Field<double>("SteelStrength");
            ConcreteStrength = dataRow.Field<double>("ConcreteStrength");
            IsActual = false;
            //dataRow.Field<bool>("IsActual"), В любом случае при загрузке данные неактуальны
            IsLoadCasesActual = false;
            IsBasePartsActual = false;
            IsBoltsActual = false;
            Width = dataRow.Field<double>("Width");
            Length = dataRow.Field<double>("Length");
            Thickness = dataRow.Field<double>("Thickness");
            WorkCondCoef = dataRow.Field<double>("WorkCondCoef");
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
            DsOperation.DeleteRow(dataSet, tableName, "SteelBaseId", Id);
        }
        #endregion

        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
