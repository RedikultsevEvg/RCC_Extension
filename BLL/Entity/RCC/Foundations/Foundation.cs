﻿using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using RDBLL.Entity.MeasureUnits;
using System.Linq;
using DAL.Common;
using RDBLL.Entity.Soils;
using RDBLL.Entity.RCC.Foundations.Processors;
using RDBLL.Entity.Common.Materials;


namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс столбчатого фундамента
    /// </summary>
    public class Foundation : IHaveForcesGroups, ISavableToDataSet, IDataErrorInfo, IRDObserver, IDuplicate, IHasSoilSection
    {
        public class FoundationResult
        {
            public List<CompressedLayerList> CompressedLayers { get; set; }
            public List<double[]> StressesWithWeigth { get; set; }
            public double MaxSettlement { get; set; }
            public double CompressHeight { get; set; }
            public double IncX { get; set; }
            public double IncY { get; set; }
            public double IncXY { get; set; }
            public double MinSndAvgStressesWithWeight { get; set; }
            public double MinSndMiddleStressesWithWeight { get; set; }
            public double MinSndCornerStressesWithWeight { get; set; }
            public double MaxSndCornerStressesWithWeight { get; set; }
            public double MaxSndTensionAreaRatioWithWeight { get; set; }
            public double SndResistance { get; set; }
            public double[] AsAct { get; set; }
            public double[] AsRec { get; set; }
            public bool GeneralResult { get; set; }
        }
        #region fields and properties
        /// <summary>
        /// Код фундамента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код уровня
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// Обратная ссылка на уровень
        /// </summary>
        public Level Level { get; set; }
        /// <summary>
        /// Код скважины
        /// </summary>
        public int? SoilSectionId { get; set; }
        /// <summary>
        /// Обратная ссылка на скважину
        /// </summary>
        public SoilSection SoilSection {get;set;}
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Относительная отметка верха фундамента
        /// </summary>
        public double RelativeTopLevel { get; set; }
        /// <summary>
        /// Относительная отметка верха грунта после планировки
        /// </summary>
        public double SoilRelativeTopLevel { get; set; }
        /// <summary>
        /// Объемный вес грунта на уступах фундамента
        /// </summary>
        public double SoilVolumeWeight { get; set; }
        /// <summary>
        /// Объемный вес бетона фундамента
        /// </summary>
        public double ConcreteVolumeWeight { get; set; }
        /// <summary>
        /// Полезная нагрузка на пол
        /// </summary>
        public double FloorLoad { get; set; }
        /// <summary>
        /// Коэффициент надежности по нагрузке для нагрузки на пол
        /// </summary>
        public double FloorLoadFactor { get; set; }
        /// <summary>
        /// Нагрузка от веса пола
        /// </summary>
        public double ConcreteFloorLoad { get; set; }
        /// <summary>
        /// Коэффициент надежности по нагрузке для нагрузки от веса пола
        /// </summary>
        public double ConcreteFloorLoadFactor { get; set; }
        /// <summary>
        /// Коллекция групп нагрузок
        /// </summary>
        public ObservableCollection<ForcesGroup> ForcesGroups { get; set; }
        /// <summary>
        /// Коллекция ступеней столбчатого фундамента
        /// </summary>
        public ObservableCollection<RectFoundationPart> Parts { get; set; }

        public MaterialUsing BottomReinforcement { get; set; }
        public MaterialUsing Concrete { get; set; }

        /// <summary>
        /// Отношение давления для ограничения глубины сжимаемой толщи
        /// </summary>
        public double CompressedLayerRatio { get; set; }
        /// <summary>
        /// Коллекция комбинаций
        /// </summary>
        public ObservableCollection<LoadSet> LoadCases { get; set; }
        /// <summary>
        /// Коллекция комбинаций с учетом веса фундамента и грунта
        /// приведенная к центру подошвы
        /// </summary>
        public ObservableCollection<LoadSet> btmLoadSetsWithWeight { get; set; }
        /// <summary>
        /// Коллекция комбинаций без учета веса фундамента и грунта
        /// приведенная к центру подошвы
        /// </summary>
        public ObservableCollection<LoadSet> btmLoadSetsWithoutWeight { get; set; }
        /// <summary>
        /// Коллекция комбинаций и кривизны с учетом веса фундамента и грунта
        /// </summary>
        public List<ForceCurvature> ForceCurvaturesWithWeight { get; set; }
        /// <summary>
        /// Коллекция комбинаций и кривизны без учета веса фундамента и грунта
        /// </summary>
        public List<ForceCurvature> ForceCurvaturesWithoutWeight { get; set; }
        /// <summary>
        /// Коллекция элементарных участков подошвы
        /// </summary>
        public List<NdmArea> NdmAreas { get; set; }    
        /// <summary>
        /// Flag of actuality of foundation
        /// </summary>
        public bool IsActual { get; set; }
        /// <summary>
        /// Признак актуальности нагрузок
        /// </summary>
        public bool IsLoadCasesActual { get; set; }
        /// <summary>
        /// Признак актуальности ступеней
        /// </summary>
        public bool IsPartsActual { get; set; }
        /// <summary>
        /// Наименование линейных единиц измерения
        /// </summary>
        public string LinearMeasure { get { return MeasureUnitConverter.GetUnitLabelText(0); } }
        /// <summary>
        /// Единица измерения объемного веса
        /// </summary>
        public string VolumeWeightMeasure { get { return MeasureUnitConverter.GetUnitLabelText(9); } }
        /// <summary>
        /// Единица измерения нагрузки, распределенной по площади
        /// </summary>
        public string DistributedLoadMeasure { get { return MeasureUnitConverter.GetUnitLabelText(13); } }
        /// <summary>
        /// Свойство для сохранения результатов
        /// </summary>
        public FoundationResult Result { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Foundation()
        {
            IsLoadCasesActual = false;
            IsPartsActual = false;
            ForcesGroups = new ObservableCollection<ForcesGroup>();
            ForcesGroups.Add(new ForcesGroup(this));
            Parts = new ObservableCollection<RectFoundationPart>();
            LoadCases = new ObservableCollection<LoadSet>();
            ForceCurvaturesWithWeight = new List<ForceCurvature>();
            ForceCurvaturesWithoutWeight = new List<ForceCurvature>();
            Result = new FoundationResult();
        }
        /// <summary>
        /// Конструктор по уровню
        /// </summary>
        /// <param name="level">Уровень</param>
        public Foundation(Level level)
        {
            Id = ProgrammSettings.CurrentId;
            LevelId = level.Id;
            Level = level;
            Name = "Новый фундамент";
            RelativeTopLevel = -0.2;
            SoilRelativeTopLevel = -0.2;
            SoilVolumeWeight = 18000;
            ConcreteVolumeWeight = 25000;
            FloorLoad = 0;
            FloorLoadFactor = 1.2;
            ConcreteFloorLoad = 0;
            ConcreteFloorLoadFactor = 1.2;
            CompressedLayerRatio = 0.2;
            ForcesGroups = new ObservableCollection<ForcesGroup>();
            ForcesGroups.Add(new ForcesGroup(this));
            Parts = new ObservableCollection<RectFoundationPart>();
            LoadCases = new ObservableCollection<LoadSet>();
            ForceCurvaturesWithWeight = new List<ForceCurvature>();
            ForceCurvaturesWithoutWeight = new List<ForceCurvature>();
            IsLoadCasesActual = true;
            IsPartsActual = true;
            Result = new FoundationResult();

            //Добавляем армирование подошвы
            #region
            BottomReinforcement = new MaterialUsing();
            BottomReinforcement.Id = ProgrammSettings.CurrentId;
            ReinforcementUsing bottomReinforcementUsing = new ReinforcementUsing();
            bottomReinforcementUsing.Id = ProgrammSettings.CurrentId;
            bottomReinforcementUsing.Name = "Армирование подошвы";
            bottomReinforcementUsing.ReinforcementKindId = 3;
            bottomReinforcementUsing.RenewKind();
            BottomReinforcement.MaterialKind = bottomReinforcementUsing;
            bottomReinforcementUsing.RegisterParent(BottomReinforcement);
            BottomReinforcement.MaterialId = BottomReinforcement.MaterialKind.Id;
            BottomReinforcement.Purpose = "BottomReinforcement";
            BottomReinforcement.RegisterParent(this);
            //Добавляем вид армирования подошвы
            //Добавляем расположение нижнего армирования вдоль оси X
            ReinforcementSpacing reinforcementSpacingsX = new ReinforcementSpacing();
            reinforcementSpacingsX.Id = ProgrammSettings.CurrentId;
            reinforcementSpacingsX.Name = "Вдоль оси X";
            reinforcementSpacingsX.SpacingType = 1;
            reinforcementSpacingsX.Spacing = 0.200;
            reinforcementSpacingsX.Diameter = 0.012;
            reinforcementSpacingsX.CoveringLayer = 0.050;
            bottomReinforcementUsing.ReinforcementSpacings.Add(reinforcementSpacingsX);
            //Добавляем расположение нижнего армирования вдоль оси Y
            ReinforcementSpacing reinforcementSpacingsY = new ReinforcementSpacing();
            reinforcementSpacingsY.Id = ProgrammSettings.CurrentId;
            reinforcementSpacingsY.Name = "Вдоль оси Y";
            reinforcementSpacingsY.SpacingType = 1;
            reinforcementSpacingsY.Spacing = 0.200;
            reinforcementSpacingsY.Diameter = 0.012;
            reinforcementSpacingsY.CoveringLayer = 0.070;
            bottomReinforcementUsing.ReinforcementSpacings.Add(reinforcementSpacingsY);
            #endregion
            //Добавляем бетон
            #region
            Concrete = new MaterialUsing();
            Concrete.Id = ProgrammSettings.CurrentId;
            Concrete.MaterialKind = ProgrammSettings.ConcreteKinds[0];
            Concrete.MaterialId = Concrete.MaterialKind.Id;
            #endregion
        }
        #endregion
        #region methods
        #region IODataset
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            RenewAll();
            DataTable dataTable = dataSet.Tables["Foundations"];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);

            #region setFields
            row.SetField("Id", Id);
            row.SetField("LevelId", LevelId);
            row.SetField("SoilSectionId", SoilSectionId);
            row.SetField("Name", Name);
            row.SetField("RelativeTopLevel", RelativeTopLevel);
            row.SetField("SoilRelativeTopLevel", SoilRelativeTopLevel);
            row.SetField("SoilVolumeWeight", SoilVolumeWeight);
            row.SetField("ConcreteVolumeWeight", ConcreteVolumeWeight);
            row.SetField("FloorLoad", FloorLoad);
            row.SetField("FloorLoadFactor", FloorLoadFactor);
            row.SetField("ConcreteFloorLoad", ConcreteFloorLoad);
            row.SetField("CompressedLayerRatio", CompressedLayerRatio);
            #endregion
            dataTable.AcceptChanges();

            foreach (RectFoundationPart foundationPart in Parts)
            {
                foundationPart.SaveToDataSet(dataSet, createNew);
            }
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                forcesGroup.SaveToDataSet(dataSet, createNew);
            }
            if (!(BottomReinforcement is null)) BottomReinforcement.SaveToDataSet(dataSet, createNew);
        }
        /// <summary>
        /// Обновляет запись в соответствии с сохраненной в датасете
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, "Foundations", Id));
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            LevelId = dataRow.Field<int>("LevelId");
            SoilSectionId = dataRow.Field<int?>("SoilSectionId");
            Name = dataRow.Field<string>("Name");
            RelativeTopLevel = dataRow.Field<double>("RelativeTopLevel");
            SoilRelativeTopLevel = dataRow.Field<double>("SoilRelativeTopLevel");
            SoilVolumeWeight = dataRow.Field<double>("SoilVolumeWeight");
            ConcreteVolumeWeight = dataRow.Field<double>("ConcreteVolumeWeight");
            FloorLoad = dataRow.Field<double>("FloorLoad");
            FloorLoadFactor = dataRow.Field<double>("FloorLoadFactor");
            ConcreteFloorLoad = dataRow.Field<double>("ConcreteFloorLoad");
            ConcreteFloorLoadFactor = dataRow.Field<double>("ConcreteFloorLoadFactor");
            CompressedLayerRatio = dataRow.Field<double>("CompressedLayerRatio");
            //Если у фундамента есть код скважины
            if (!(SoilSectionId is null)) RenewSoilSection();
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            //Удаляем вложенные части
            DeleteSubElements(dataSet, "FoundationParts");
            DeleteSubElements(dataSet, "FoundationForcesGroups");
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {

                forcesGroup.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, "Foundations", Id);
        }
        public void DeleteSubElements(DataSet dataSet, string tableName)
        {
            DsOperation.DeleteRow(dataSet, tableName, "FoundationId", Id);

        }
        #endregion
        #region IRDObserver
        public void Update()
        {
            IsActual = false;
            IsLoadCasesActual = false;
            IsPartsActual = false;
        }
        #endregion
        #region IDuplicate
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Duplicate()
        {
            Foundation foundation = new Foundation();
            foundation.Id = ProgrammSettings.CurrentId;
            #region Copy properties
            foundation.Name = Name;
            if (! (SoilSectionId is null))
            {
                foundation.SoilSectionId = SoilSectionId;
                foundation.SoilSection = SoilSection;
            }
            foundation.RelativeTopLevel = RelativeTopLevel;
            foundation.SoilRelativeTopLevel = SoilRelativeTopLevel;
            foundation.SoilVolumeWeight = SoilVolumeWeight;
            foundation.ConcreteVolumeWeight = ConcreteVolumeWeight;
            foundation.FloorLoad = FloorLoad;
            foundation.FloorLoadFactor = FloorLoadFactor;
            foundation.ConcreteFloorLoad = ConcreteFloorLoad;
            foundation.ConcreteFloorLoadFactor = ConcreteFloorLoadFactor;
            #endregion
            //Копируем нагрузки
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                ForcesGroup newForcesGroup = forcesGroup.Duplicate() as ForcesGroup;
                newForcesGroup.Foundations.Add(foundation);
                foundation.ForcesGroups.Clear();
                foundation.ForcesGroups.Add(newForcesGroup);
            }
            //Копируем ступени
            foreach (RectFoundationPart rectFoundationPart in this.Parts)
            {
                RectFoundationPart newFoundationPart = rectFoundationPart.Duplicate() as RectFoundationPart;
                newFoundationPart.FoundationId = foundation.Id;
                newFoundationPart.Foundation = foundation;
                foundation.Parts.Add(newFoundationPart);
            }
            return foundation;
        }
        #endregion
        public void RenewSoilSection()
        {
            //получаем ссылку на скважину
            foreach (SoilSection soilSection in this.Level.Building.BuildingSite.SoilSections)
            {
                if (!(SoilSection is null)) SoilSection.RemoveObserver(this);
                if (soilSection.Id == SoilSectionId)
                {
                    SoilSection = soilSection;
                    SoilSection.AddObserver(this);
                }
            }
        }
        public void RenewAll()
        {
            RenewSoilSection();
            if (! (BottomReinforcement is null)) BottomReinforcement.RenewMaterialKind();
            if (!(Concrete is null)) Concrete.RenewMaterialKind();
        }
        public void DeleteFromObservables()
        {
            if (!(SoilSection is null)) SoilSection.RemoveObserver(this);
        }
        #endregion
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
                    case "SoilVolumeWeight":
                        {
                            if (SoilVolumeWeight <= 0)
                            {
                                error = "Объемный вес грунта должен быть больше нуля";
                            }
                        }
                        break;
                    case "ConcreteVolumeWeight":
                        {
                            if (ConcreteVolumeWeight <= 0)
                            {
                                error = "Объемный вес бетона должен быть нуля";
                            }
                        }
                        break;
                }
                return error;
            }
        }
    }
}
