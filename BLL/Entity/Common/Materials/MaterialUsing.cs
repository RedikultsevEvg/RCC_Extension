using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Entity.Common.Materials.Interfaces;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Common.Service;
using System.Collections.ObjectModel;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.RFExtenders;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс применения материалов в элементах
    /// </summary>
    public abstract class MaterialUsing : IHasParent, ICloneable
    {
        /// <summary>
        /// Код применения
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование применения
        /// </summary>
        public string Name { get; set; }
        public string ExpanderName
        {
            get
            {
                if (this is ConcreteUsing) return "Бетон";
                if (this is SteelUsing) return "Сталь";
                else return null;
            }
        }
        public string ClassName
        {
            get
            {
                if (this is ConcreteUsing) return "Класс бетона";
                if (this is SteelUsing) return "Класс стали";
                else return null;
            }
        }
        /// <summary>
        /// Назначение контейнера в родительском элементе
        /// </summary>
        public string Purpose { get; set; }
        /// <summary>
        /// Код выбранного материала
        /// </summary>
        public int SelectedId { get; set; }
        /// <summary>
        /// Ссылка на выбранный материал
        /// </summary>
        public IMaterialKind MaterialKind
        { get
            {
                IMaterialKind materialKind;
                if (this is ConcreteUsing) materialKind = MaterialProcessor.GetMaterialKindById(MaterialKindTypes.Concrete, SelectedId);
                else if (this is ReinforcementUsing) materialKind = MaterialProcessor.GetMaterialKindById(MaterialKindTypes.Reinforcement, SelectedId);
                else if (this is SteelUsing) materialKind = MaterialProcessor.GetMaterialKindById(MaterialKindTypes.Steel, SelectedId);
                else throw new NotImplementedException("Material kind is not valid");
                return materialKind;
            }
        }
        /// <summary>
        /// Суммарный коэффициент надежности
        /// </summary>
        public SafetyFactor TotalSafetyFactor
        {
            get
            {
                SafetyFactor safetyFactor = new SafetyFactor();
                foreach (SafetyFactor safetyFactorLoc in SafetyFactors)
                {
                    for (int i = 0; i < SafetyFactor.CoefCount; i++)
                    {
                        safetyFactor.Coefficients[i] *= safetyFactorLoc.Coefficients[i];
                    }
                }
                return safetyFactor;
            }
        }
        /// <summary>
        /// Список коэффициентов надежности
        /// </summary>
        public ObservableCollection<SafetyFactor> SafetyFactors { get; set; }
        /// <summary>
        /// Коллекция материалов, доступных для выбора
        /// </summary>
        public List<IMaterialKind> MaterialKinds
        {
            get
            {
                List<IMaterialKind> materialKinds = new List<IMaterialKind>();
                if (this is ConcreteUsing) materialKinds.AddRange(ProgrammSettings.ConcreteKinds);
                else if (this is ReinforcementUsing) materialKinds.AddRange(ProgrammSettings.ReinforcementKinds);
                else if (this is SteelUsing) materialKinds.AddRange(ProgrammSettings.SteelKinds);
                else if (this is BoltUsing) materialKinds.AddRange(ProgrammSettings.SteelKinds);
                else throw new Exception("Material kind is not valid");
                return materialKinds;
            }
        }
        /// <summary>
        /// Ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public MaterialUsing(bool genId = false)
        {
            if (genId) Id = ProgrammSettings.CurrentId;
            SafetyFactors = new ObservableCollection<SafetyFactor>();
        }
        /// <summary>
        /// Конструктор по родительскому элементу
        /// </summary>
        /// <param name="parentMember"></param>
        public MaterialUsing(IDsSaveable parentMember)
        {
            Id = ProgrammSettings.CurrentId;
            RegisterParent(parentMember);
            SafetyFactors = new ObservableCollection<SafetyFactor>();
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "MaterialUsings"; }
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region setFields
            DsOperation.SetField(row, "Purpose", Purpose);
            if (this is ConcreteUsing) DsOperation.SetField(row, "Materialkindname", "Concrete");
            else if (this is ReinforcementUsing) DsOperation.SetField(row, "Materialkindname", "Reinforcement");
            else if (this is SteelUsing) DsOperation.SetField(row, "Materialkindname", "Steel");
            else if (this is BoltUsing) DsOperation.SetField(row, "Materialkindname", "SteelBolt");
            else throw new NotImplementedException("Material kind is not valid");
            DsOperation.SetField(row, "SelectedId", SelectedId);
            #endregion
            row.Table.AcceptChanges();
            foreach (SafetyFactor safetyFactor in SafetyFactors)
            {
                safetyFactor.SaveToDataSet(dataSet, createNew);
            }
            if (this is CircleUsingBase)
            {
                CircleUsingBase crclUsing = (this) as CircleUsingBase;
                row.SetField("Diameter", crclUsing.Diameter);
                row.SetField("Prestrain", crclUsing.Prestrain);
                //crclUsing.Placement.SaveToDataSet(dataSet, createNew);              
            }
        }
        /// <summary>
        /// Обновляет запись в соответствии с сохраненной в датасете
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
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            Purpose = dataRow.Field<string>("Purpose");
            string materialKindName = dataRow.Field<string>("Materialkindname");
            SelectedId = dataRow.Field<int>("SelectedId");
            if (this is CircleUsingBase)
            {
                CircleUsingBase crclUsing = this as CircleUsingBase;
                crclUsing.Diameter = dataRow.Field<double>("Diameter");
                crclUsing.Prestrain = dataRow.Field<double>("Prestrain");
            }
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (SafetyFactor safetyFactor in SafetyFactors) {safetyFactor.DeleteFromDataSet(dataSet);}
            EntityOperation.DeleteEntity(dataSet, this);
        }
        #endregion
        public object Clone()
        {
            MaterialUsing materialUsing = MemberwiseClone() as MaterialUsing; ;
            materialUsing.Id = ProgrammSettings.CurrentId;
            materialUsing.SafetyFactors = new ObservableCollection<SafetyFactor>();
            foreach (SafetyFactor safetyFactor in SafetyFactors)
            {
                SafetyFactor newObject = (safetyFactor.Clone()) as SafetyFactor;
                newObject.RegisterParent(materialUsing);
                materialUsing.SafetyFactors.Add(newObject);
            }
            if (this is IHasPlacement)
            {
                IHasPlacement hasPlacement = this as IHasPlacement;
                IHasPlacement newHasPlacement = materialUsing as IHasPlacement;
                Placement placement = hasPlacement.Placement.Clone() as Placement;
                newHasPlacement.SetPlacement(placement);
            }
            return materialUsing;
        }
         #region Methods
        /// <summary>
        /// Регистрация ссылки на родительскую сущность
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            if (ParentMember != null) UnRegisterParent();
            ParentMember = parent;
        }
        /// <summary>
        /// Удаление регистрации ссылки на родительскую сущность
        /// </summary>
        public void UnRegisterParent()
        {
            ParentMember = null;
        }
        //public double[] GetTotalSafetyFactor()
        //{
        //    double[] safetyFactors = new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        //    foreach (SafetyFactor safetyFactor in SafetyFactors)
        //    {
        //        for (int i = 0; i<SafetyFactor.CoefCount; i++)
        //        {
        //            safetyFactors[i] *= safetyFactor.Coefficients[i];
        //        }
        //    }
        //    return safetyFactors;
        //}
        #endregion
    }
}
