using RDBLL.Common.Interfaces;
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
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.RFExtenders;

namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс столбчатого фундамента
    /// </summary>
    public class Foundation : IHasForcesGroups, IHasParent, IDataErrorInfo, IRDObserver, ICloneable, IHasSoilSection
    {
        /// <summary>
        /// Класс для хранения результатов расчета фундамента
        /// </summary>
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
        /// Обратная ссылка на уровень
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Обратная ссылка на скважину
        /// </summary>
        public SoilSectionUsing SoilSectionUsing { get; private set; }
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

        public MaterialContainer BottomReinforcement { get; set; }
        public MaterialContainer VerticalReinforcement { get; set; }
        public ConcreteUsing Concrete { get; set; }

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
            addMaterial(this);
        }
        /// <summary>
        /// Конструктор по уровню
        /// </summary>
        /// <param name="level">Уровень</param>
        public Foundation(Level level)
        {
            Id = ProgrammSettings.CurrentId;
            RegisterParent(level);
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
            //Использование скважины грунта
            SoilSectionUsing soilSectionUsing = new SoilSectionUsing(true);
            soilSectionUsing.RegisterParent(this);
            IsLoadCasesActual = true;
            IsPartsActual = true;
            Result = new FoundationResult();
            addMaterial(this);
        }
        #endregion
        #region methods
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "Foundations"; }
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            try
            {
                DataTable dataTable = dataSet.Tables[GetTableName()];
                DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
                #region setFields
                DsOperation.SetId(row, Id, Name, ParentMember.Id);
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
                BottomReinforcement.SaveToDataSet(dataSet, createNew);
                VerticalReinforcement.SaveToDataSet(dataSet, createNew);
                Concrete.SaveToDataSet(dataSet, createNew);
                SoilSectionUsing.SaveToDataSet(dataSet, createNew);
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage("Ошибка сохранения элемента: " + Name, ex);
            }
        }
        /// <summary>
        /// Обновляет запись в соответствии с сохраненной в датасете
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            try
            {
                OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage("Ошибка получения элемента из базы данных. Элемент: " + Name, ex);
            }        
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
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
            List<MaterialContainer> materialContainers = GetEntity.GetContainers(dataRow.Table.DataSet, this);
            foreach (MaterialContainer materialContainer in materialContainers)
            {
                if (string.Compare(materialContainer.Purpose, "BtmRF") == 0) { BottomReinforcement = materialContainer; }
                else if (string.Compare(materialContainer.Purpose, "UndColumn") == 0) { VerticalReinforcement = materialContainer; }
                else throw new Exception("Container type is not valid");
                materialContainer.RegisterParent(this);
            }

            foreach (MaterialUsing  materialUsing in BottomReinforcement.MaterialUsings)
            {
                ReinforcementUsing reinforcement = materialUsing as ReinforcementUsing;
                reinforcement.SetExtender(new LineToSurfBySpacing());
                reinforcement.Extender.SetPlacement(reinforcement.Placement);
            }

            foreach (MaterialUsing materialUsing in VerticalReinforcement.MaterialUsings)
            {
                ReinforcementUsing reinforcement = materialUsing as ReinforcementUsing;
                reinforcement.SetExtender(ExtenderFactory.GetCoveredArray(ExtenderType.CoveredArray));
                reinforcement.Extender.SetPlacement(reinforcement.Placement);
            }

            List<MaterialUsing> materialUsings = GetEntity.GetMaterialUsings(dataRow.Table.DataSet, this);
            foreach (MaterialUsing materialUsing in materialUsings)
            {
                if (materialUsing is ConcreteUsing)
                {
                    ConcreteUsing concrete = materialUsing as ConcreteUsing;
                    if (string.Compare(materialUsing.Purpose, "MainConcrete") == 0) { Concrete = concrete; }
                    else throw new Exception("Concrete type is not valid");
                }
                else throw new Exception("Material type is not valid");
                materialUsing.RegisterParent(this);
            }
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            //Удаляем вложенные части
            DeleteSubElements(dataSet, "FoundationParts");
            DeleteSubElements(dataSet, "ParentForcesGroups");
            DeleteSubElements(dataSet, "SoilSectionsUsings");
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                forcesGroup.DeleteFromDataSet(dataSet);
            }
            BottomReinforcement.DeleteFromDataSet(dataSet);
            VerticalReinforcement.DeleteFromDataSet(dataSet);
            Concrete.DeleteFromDataSet(dataSet);
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        public void DeleteSubElements(DataSet dataSet, string tableName)
        {
            DsOperation.DeleteRow(dataSet, tableName, "ParentId", Id);

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
        public object Clone()
        {
            Foundation foundation = MemberwiseClone() as Foundation;
            foundation.Id = ProgrammSettings.CurrentId;
            FoundCloneProcessor.FoundationClone(this, foundation);
            return foundation;
        }
        #endregion
        public void DeleteFromObservables()
        {
            
        }

        private void addMaterial(Foundation foundation)
        {
            ReinforcementUsing GetRF(MaterialContainer container, string rusName, string engName)
            {
                ReinforcementUsing rf = new ReinforcementUsing(container);
                rf.Name = rusName;
                rf.Purpose = engName;
                rf.Diameter = 0.012;
                rf.SelectedId = ProgrammSettings.ReinforcementKinds[0].Id;
                return rf;
            }
            ReinforcementUsing GetBottomReinforcement(MaterialContainer container, double coveringLayer, string rusName, string engName)
            {
                ReinforcementUsing rf = GetRF(container, rusName, engName);
                LineBySpacing placement = new LineBySpacing();
                placement.RegisterParent(rf);
                LineToSurfBySpacing extender = ExtenderFactory.GetCoveredArray(ExtenderType.CoveredLine) as LineToSurfBySpacing;
                rf.SetExtender(extender);
                rf.SetPlacement(placement);
                extender.CoveringLayer = coveringLayer;
                return rf;
            }
            ReinforcementUsing GetUndColumnRF(MaterialContainer container, double coveringLayer, string rusName, string engName)
            {
                ReinforcementUsing rf = GetRF(container, rusName, engName);
                RectArrayPlacement placement = new RectArrayPlacement();
                placement.OffSet = 0.05;
                placement.RegisterParent(rf);
                rf.SetExtender(ExtenderFactory.GetCoveredArray(ExtenderType.CoveredArray));
                rf.SetPlacement(placement);
                return rf;
            }

            #region Армирование подошвы
            MaterialContainer materialContainer = new MaterialContainer(this);
            materialContainer.Name = "Армирование подошвы";
            materialContainer.Purpose = "BtmRF";
            ReinforcementUsing rfX = GetBottomReinforcement(materialContainer, 0.07, "Вдоль оси X", "Along X-axes");
            ReinforcementUsing rfY = GetBottomReinforcement(materialContainer, 0.07, "Вдоль оси Y", "Along Y-axes");
            materialContainer.MaterialUsings.Add(rfX);
            materialContainer.MaterialUsings.Add(rfY);
            foundation.BottomReinforcement = materialContainer;
            #endregion
            #region Армирование подколонника
            MaterialContainer verticalContainer = new MaterialContainer(this);
            verticalContainer.Name = "Вертикальное армирование";
            verticalContainer.Purpose = "UndColumn";
            foundation.VerticalReinforcement = verticalContainer;
            ReinforcementUsing rfVert = GetUndColumnRF(verticalContainer, 0.07, "Подколонник", "UndColumn");
            verticalContainer.MaterialUsings.Add(rfVert);
            foundation.VerticalReinforcement = verticalContainer;
            #endregion
            #region Добавляем бетон
            foundation.Concrete = new ConcreteUsing();
            Concrete.RegisterParent(this);
            foundation.Concrete.Id = ProgrammSettings.CurrentId;
            foundation.Concrete.Name = "Бетон";
            foundation.Concrete.Purpose = "MainConcrete";
            foundation.Concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
            foundation.Concrete.SelectedId = Concrete.MaterialKind.Id;
            foundation.Concrete.AddGammaB1();
            #endregion
        }

        public void RegisterParent(IDsSaveable parent)
        {
            Level level = parent as Level;
            ParentMember = level;
            level.Children.Add(this);
        }
        public void UnRegisterParent()
        {
            Level level = ParentMember as Level;
            level.Children.Remove(this);
        }

        public void RegSSUsing(SoilSectionUsing soilSectionUsing)
        {
            SoilSectionUsing = soilSectionUsing;
        }

        public void UnRegSSUsing(SoilSectionUsing soilSectionUsing)
        {
            SoilSectionUsing = null;
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
