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
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.SC.Column.SteelBases.Factories;

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
        /// <summary>
        /// Паттерн для стальной базы
        /// </summary>
        public PatternBase Pattern { get; set; }
        /// <summary>
        /// Коллекция групп нагрузок
        /// </summary>
        public ObservableCollection<ForcesGroup> ForcesGroups { get; set; }
        /// <summary>
        /// Коллекция участков
        /// </summary>
        public ObservableCollection<SteelBasePart> SteelBaseParts { get; set; }
        /// <summary>
        /// Коллекция болтов
        /// </summary>
        public ObservableCollection<SteelBolt> SteelBolts { get; set; }
        /// <summary>
        /// Использование класса бетона
        /// </summary>
        public ConcreteUsing Concrete { get; set; }
        /// <summary>
        /// Использование класса стали
        /// </summary>
        public SteelUsing Steel { get; set; }
        /// <summary>
        /// Наименование единиц измерения
        /// </summary>
        public MeasureUnitList Measures { get => new MeasureUnitList(); }
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
            //Добавляем материалы
            MatFactProc.GetMatType(this, MatType.SteelBase);
        }
        /// <summary>
        /// Создает базу стальной колонны по указанному уровню
        /// </summary>
        /// <param name="parent">Ссылка на родительский элемент</param>
        public SteelBase(IDsSaveable parent)
        {
            SetDefault();
            RegisterParent(parent);
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
            row.SetField("IsActual", IsActual);
            row.SetField("UseSimpleMethod", UseSimpleMethod);
            row.AcceptChanges();
            if (Pattern is null)
            {
                foreach (SteelBasePart steelBasePart in SteelBaseParts)
                {
                    steelBasePart.SaveToDataSet(dataSet, createNew);
                }
                foreach (SteelBolt steelBolt in SteelBolts)
                {
                    steelBolt.SaveToDataSet(dataSet, createNew);
                }
            }
            else EntityOperation.SaveEntity(dataSet, true, Pattern);
        }
        /// <summary>
        /// Обновляет запись по датасету
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, this));
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
            //Участки являются простым элементом, удаляем просто так
            DsOperation.DeleteRow(dataSet, "SteelBaseParts", "ParentId", Id);
            //Болт не является простым элементом, поэтому удалять его просто из таблицы нельзя
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
            SteelBase newObj = this.MemberwiseClone() as SteelBase;
            newObj.Id = ProgrammSettings.CurrentId;
            newObj.ForcesGroups = new ObservableCollection<ForcesGroup>();
            foreach (ForcesGroup load in ForcesGroups)
            {
                ForcesGroup newLoad = load.Clone() as ForcesGroup;
                newLoad.Owners.Add(newObj);
                newObj.ForcesGroups.Add(newLoad);
            }
            newObj.SteelBolts = new ObservableCollection<SteelBolt>();
            foreach (SteelBolt bolt in SteelBolts)
            {
                newObj.SteelBolts.Add(bolt.Clone() as SteelBolt);
            }
            newObj.SteelBaseParts = new ObservableCollection<SteelBasePart>();
            foreach (SteelBasePart part in SteelBaseParts)
            {
                newObj.SteelBaseParts.Add(part.Clone() as SteelBasePart);
            }
            newObj.Steel = this.Steel.Clone() as SteelUsing;
            newObj.Concrete = this.Concrete.Clone() as ConcreteUsing;
            if (this.Pattern != null) newObj.Pattern = Pattern.Clone() as PatternBase;
            return newObj; 
        }
        /// <summary>
        /// Регистрация родителя
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            Level level = parent as Level;
            ParentMember = level;
            level.Children.Add(this);
        }
        /// <summary>
        /// Удаление регистрации родителя
        /// </summary>
        public void UnRegisterParent()
        {
            Level level = ParentMember as Level;
            level.Children.Remove(this);
        }
        #endregion
    }
}
