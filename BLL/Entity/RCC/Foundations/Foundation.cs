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


namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс столбчатого фундамента
    /// </summary>
    public class Foundation : IHaveForcesGroups, ISavableToDataSet, IDataErrorInfo
    {
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
        /// Код стали
        /// </summary>
        public int ReinfSteelClassId { get; set; }
        /// <summary>
        /// Код бетона
        /// </summary>
        public int ConcreteClassId { get; set; }
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
        /// <summary>
        /// Защитный слой арматуры подошвы вдоль оси X
        /// </summary>
        public double CoveringLayerX { get; set; }
        /// <summary>
        /// Защитный слой арматуры подошвы вдоль оси Y
        /// </summary>
        public double CoveringLayerY { get; set; }
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
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Foundation()
        {

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
            ReinfSteelClassId = 1;
            ConcreteClassId = 1;
            Name = "Новый фундамент";
            RelativeTopLevel = -0.2;
            SoilRelativeTopLevel = -0.2;
            SoilVolumeWeight = 18000;
            ConcreteVolumeWeight = 25000;
            FloorLoad = 0;
            FloorLoadFactor = 1.2;
            ConcreteFloorLoad = 0;
            ConcreteFloorLoadFactor = 1.2;
            CoveringLayerX = 0.09;
            CoveringLayerY = 0.07;
            CompressedLayerRatio = 0.2;
            ForcesGroups = new ObservableCollection<ForcesGroup>();
            ForcesGroups.Add(new ForcesGroup(this));
            Parts = new ObservableCollection<RectFoundationPart>();
            LoadCases = new ObservableCollection<LoadSet>();
            ForceCurvaturesWithWeight = new List<ForceCurvature>();
            ForceCurvaturesWithoutWeight = new List<ForceCurvature>();
            IsLoadCasesActual = true;
            IsPartsActual = true;
        }
        #endregion
        #region methods
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["Foundations"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
                { Id, LevelId,ReinfSteelClassId, ConcreteClassId,
                Name, RelativeTopLevel, SoilRelativeTopLevel, SoilVolumeWeight, ConcreteVolumeWeight,
                FloorLoad, FloorLoadFactor, ConcreteFloorLoad,
                ConcreteFloorLoadFactor, CoveringLayerX, CoveringLayerY,
                CompressedLayerRatio
                };
            dataTable.Rows.Add(dataRow);
            foreach (RectFoundationPart foundationPart in Parts)
            {
                foundationPart.SaveToDataSet(dataSet);
            }
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                forcesGroup.SaveToDataSet(dataSet);
            }
        }
        /// <summary>
        /// Обновляет датасет в соответствии с записью
        /// </summary>
        /// <param name="dataSet"></param>
        public void Save(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["Foundations"];
            var foundation = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
            foundation.SetField("ReinfSteelClassId", ReinfSteelClassId);
            foundation.SetField("ConcreteClassId", ConcreteClassId);
            foundation.SetField("Name", Name);
            foundation.SetField("RelativeTopLevel", RelativeTopLevel);
            foundation.SetField("SoilRelativeTopLevel", SoilRelativeTopLevel);
            foundation.SetField("SoilVolumeWeight", SoilVolumeWeight);
            foundation.SetField("ConcreteVolumeWeight", ConcreteVolumeWeight);
            foundation.SetField("FloorLoad", FloorLoad);
            foundation.SetField("FloorLoadFactor", FloorLoadFactor);
            foundation.SetField("ConcreteFloorLoad", ConcreteFloorLoad);
            foundation.SetField("CoveringLayerX", CoveringLayerX);
            foundation.SetField("CoveringLayerY", CoveringLayerY);
            foundation.SetField("CompressedLayerRatio", CompressedLayerRatio);
            dataTable.AcceptChanges();
        }
        /// <summary>
        /// Обновляет запись в соответствии с сохраненной в датасете
        /// </summary>
        /// <param name="dataSet"></param>
        public void Revert(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["Foundations"];
            var foundation = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
            ReinfSteelClassId = foundation.Field<int>("ReinfSteelClassId");
            ConcreteClassId = foundation.Field <int>("ConcreteClassId");
            Name = foundation.Field<string>("Name");
            RelativeTopLevel = foundation.Field<double>("RelativeTopLevel");
            SoilRelativeTopLevel = foundation.Field<double>("SoilRelativeTopLevel");
            SoilVolumeWeight = foundation.Field<double>("SoilVolumeWeight");
            ConcreteVolumeWeight = foundation.Field<double>("ConcreteVolumeWeight");
            FloorLoad = foundation.Field<double>("FloorLoad");
            FloorLoadFactor = foundation.Field<double>("FloorLoadFactor");
            ConcreteFloorLoad = foundation.Field<double>("ConcreteFloorLoad");
            ConcreteFloorLoadFactor = foundation.Field<double>("ConcreteFloorLoadFactor");
            CoveringLayerX = foundation.Field<double>("CoveringLayerX");
            CoveringLayerY = foundation.Field<double>("CoveringLayerY");
            CompressedLayerRatio = foundation.Field<double>("CompressedLayerRatio");
        }
        public void OpenFromDataSet(DataSet dataSet, int id)
        {
            throw new NotImplementedException();
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
