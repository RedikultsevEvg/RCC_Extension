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
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.Common.Materials;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.SC.Column.SteelBases.Patterns;

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// База стальной колонны
    /// </summary>
    public class SteelBase : ICloneable, IHasForcesGroups, IHasParent, IHasConcrete, IHasSteel, IHasHeight
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
        public double Height { get; set; }
        /// <summary>
        /// Флаг расчета по упрощенному методу
        /// </summary>
        public bool UseSimpleMethod { get; set; }
        public PatternBase Pattern { get; set; }
        /// <summary>
        /// Коллекция групп нагрузок
        /// </summary>
        public ObservableCollection<ForcesGroup> ForcesGroups { get; set; }
        /// <summary>
        /// Коллекция участков
        /// </summary>
        public ObservableCollection<SteelBasePart> SteelBaseParts { get; set; }

        public ConcreteUsing Concrete { get; set; }
        public SteelUsing Steel { get; set; }

        /// <summary>
        /// Коллекция болтов
        /// </summary>
        public ObservableCollection<SteelBolt> SteelBolts { get; set; }
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
            
            Height = 0.06;
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
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region
            row.SetField("IsActual", IsActual);
            row.SetField("UseSimpleMethod", UseSimpleMethod);
            #endregion
            row.AcceptChanges();
            foreach (SteelBasePart steelBasePart in SteelBaseParts)
            {
                steelBasePart.SaveToDataSet(dataSet, createNew);
            }
            foreach (SteelBolt steelBolt in SteelBolts)
            {
                steelBolt.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Обновляет запись по датасету
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            EntityOperation.SetProps(dataRow, this);
            IsActual = false;           
            UseSimpleMethod = dataRow.Field<bool>("UseSimpleMethod");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, "SteelBaseParts", "ParentId", Id);
            foreach (SteelBolt bolt in SteelBolts)
            {
                bolt.DeleteFromDataSet(dataSet);
            }
            EntityOperation.DeleteEntity(dataSet, this);
        }
        //IClonable
        /// <summary>
        /// Возвращает полную копию элемента
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            SteelBase steelBase = this.MemberwiseClone() as SteelBase;
            steelBase.SteelBolts = new ObservableCollection<SteelBolt>();
            foreach (SteelBolt bolt in SteelBolts)
            {
                steelBase.SteelBolts.Add(bolt.Clone() as SteelBolt);
            }
            steelBase.SteelBaseParts = new ObservableCollection<SteelBasePart>();
            foreach (SteelBasePart part in SteelBaseParts)
            {
                steelBase.SteelBaseParts.Add(part.Clone() as SteelBasePart);
            }
            return steelBase; 
        }
        /// <summary>
        /// Регистрация родителя
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            Level level = parent as Level;
            ParentMember = level;
            level.SteelBases.Add(this);
        }
        /// <summary>
        /// Удаление регистрации родителя
        /// </summary>
        public void UnRegisterParent()
        {
            Level level = ParentMember as Level;
            level.SteelBases.Remove(this);
        }
        #endregion
    }
}
