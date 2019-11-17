using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// База стальной колонны
    /// </summary>
    public class SteelBase : ICloneable, ISavableToDataSet
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
        public int ConcreteClassId { get; set; } //Код бетона
        public String Name { get; set; } //Наименование
        public double SteelStrength { get; set; } //Расчетное сопротивление базы
        public double ConcreteStrength { get; set; } //Прочность бетона подливки
        public bool IsActual { get; set; } //Признак актуальности расчета
        public double Width { get; set; } //Ширина базы, м
        public double Length { get; set; } //Длина базы, м
        public double Thickness { get; set; } //Толщина, м
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
        public ObservableCollection<ForcesGroup> LoadsGroup { get; set; }
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
        public List<LoadSet> LoadCases { get; set; }
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
        public List<ForceCurvature> ForceCurvatures { get; set; }

        public bool IsLoadCasesActual
        {
            get {return _isLoadCasesActual; }
            set
            {
                if (!value) SetNotActual();
                _isLoadCasesActual = value;
            }
        }
        public bool IsBasePartsActual
        {
            get { return _isBasePartsActual; }
            set
            {
                if (!value) SetNotActual();
                _isBasePartsActual = value;
            }
        }
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
            UseSimpleMethod = true;
            LoadsGroup = new ObservableCollection<ForcesGroup>();
            LoadsGroup.Add(new ForcesGroup(this));
            SteelBaseParts = new ObservableCollection<SteelBasePart>();
            SteelBolts = new ObservableCollection<SteelBolt>();
            ForceCurvatures = new List<ForceCurvature>();
            NdmAreas = new List<NdmArea>();
            ConcreteNdmAreas = new List<NdmArea>();
            SteelNdmAreas = new List<NdmArea>();

            /// Вложенные объекты по умолчанию
            StartObjects();
        }
        public void StartObjects()
        {
            //Нагрузка
            LoadSet loadSet = new LoadSet(this.LoadsGroup[0]);
            this.LoadsGroup[0].LoadSets.Add(loadSet);
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
            level.SteelBases.Add(this);
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
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
             dataTable = dataSet.Tables["SteelBases"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
                { Id, LevelId, SteelClassId, ConcreteClassId,
                    Name, SteelStrength, ConcreteStrength, IsActual,
                    Width, Length, Thickness, WorkCondCoef,
                    UseSimpleMethod };
            dataTable.Rows.Add(dataRow);
            foreach (SteelBasePart steelBasePart in SteelBaseParts)
            {
                steelBasePart.SaveToDataSet(dataSet);
            }
            foreach (SteelBolt steelBolt in SteelBolts)
            {
                steelBolt.SaveToDataSet(dataSet);
            }
            foreach (ForcesGroup forcesGroup in LoadsGroup)
            {
                forcesGroup.SaveToDataSet(dataSet);
            }
        }
        public void OpenFromDataSet(DataSet dataSet, int Id)
        {

        }
        #endregion

        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
