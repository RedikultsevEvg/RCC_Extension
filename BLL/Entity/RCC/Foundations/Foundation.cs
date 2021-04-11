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
using RDBLL.Entity.Soils;
using RDBLL.Entity.RCC.Foundations.Processors;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.RFExtenders;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Entity.RCC.Foundations.Results;
using RDBLL.Entity.RCC.Foundations.Factories;

namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс столбчатого фундамента
    /// </summary>
    public class Foundation : IHasForcesGroups, IHasParent, IDataErrorInfo, ICloneable, IHasSoilSection, IHasConcrete
    {
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
        /// Наименование единиц измерения
        /// </summary>
        public MeasureUnitList Measures { get => new MeasureUnitList(); }
        /// <summary>
        /// Свойство для сохранения результатов
        /// </summary>
        public FoundationResult Result { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Foundation(bool genId = false)
        {
            if (genId) Id = ProgrammSettings.CurrentId;
            ForcesGroups = new ObservableCollection<ForcesGroup>();
            ForcesGroups.Add(new ForcesGroup(this));
            Parts = new ObservableCollection<RectFoundationPart>();
            LoadCases = new ObservableCollection<LoadSet>();
            ForceCurvaturesWithWeight = new List<ForceCurvature>();
            ForceCurvaturesWithoutWeight = new List<ForceCurvature>();
            IsActual = false;
            Result = new FoundationResult();
            ConcreteFactProc.AddConcrete(this);
            ReinforcementFactProc.GetReinforcement(this);
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
                DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
                #region setFields
                DsOperation.SetField(row, "RelativeTopLevel", RelativeTopLevel);
                DsOperation.SetField(row, "SoilRelativeTopLevel", SoilRelativeTopLevel);
                DsOperation.SetField(row, "SoilVolumeWeight", SoilVolumeWeight);
                DsOperation.SetField(row, "ConcreteVolumeWeight", ConcreteVolumeWeight);
                DsOperation.SetField(row, "FloorLoad", FloorLoad);
                DsOperation.SetField(row, "FloorLoadFactor", FloorLoadFactor);
                DsOperation.SetField(row, "ConcreteFloorLoad", ConcreteFloorLoad);
                DsOperation.SetField(row, "CompressedLayerRatio", CompressedLayerRatio);
                #endregion
                row.AcceptChanges();

                foreach (RectFoundationPart foundationPart in Parts)
                {
                    foundationPart.SaveToDataSet(dataSet, createNew);
                }
                BottomReinforcement.SaveToDataSet(dataSet, createNew);
                VerticalReinforcement.SaveToDataSet(dataSet, createNew);
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
            EntityOperation.SetProps(dataRow, this);
            double d = 0;
            DsOperation.Field(dataRow, ref d, "RelativeTopLevel", -0.2);
            RelativeTopLevel = d;
            DsOperation.Field(dataRow, ref d, "SoilRelativeTopLevel", -0.2);
            SoilRelativeTopLevel = d;
            DsOperation.Field(dataRow, ref d, "SoilVolumeWeight", 18000);
            SoilVolumeWeight = d;
            DsOperation.Field(dataRow, ref d, "ConcreteVolumeWeight", 25000);
            ConcreteVolumeWeight = d;
            DsOperation.Field(dataRow, ref d, "FloorLoad", 0);
            FloorLoad = d;
            DsOperation.Field(dataRow, ref d, "FloorLoadFactor", 1.2);
            FloorLoadFactor = d;
            DsOperation.Field(dataRow, ref d, "ConcreteFloorLoad", 0);
            ConcreteFloorLoad = d;
            DsOperation.Field(dataRow, ref d, "ConcreteFloorLoadFactor", 1.2);
            ConcreteFloorLoadFactor = d;
            DsOperation.Field(dataRow, ref d, "CompressedLayerRatio", 0.2);
            CompressedLayerRatio = d;
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
            DeleteSubElements(dataSet, "SoilSectionUsings");
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
        }
        #endregion
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
        public void DeleteFromObservables()
        {
            
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
            ParentMember = null;
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
