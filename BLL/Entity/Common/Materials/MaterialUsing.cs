using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using DAL.Common;
using System.Data;
using RDBLL.Entity.Common.Materials.Interfaces;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Common.Service;
using System.Collections.ObjectModel;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.RFExtenders;

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
                if (this is ConcreteUsing) materialKind = MaterialProcessor.GetMaterialKindById("Concrete", SelectedId);
                else if (this is ReinforcementUsing) materialKind = MaterialProcessor.GetMaterialKindById("Reinforcement", SelectedId);
                else throw new NotImplementedException("Material kind is not valid");
                return materialKind;
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
        public MaterialUsing()
        {
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
            DataTable dataTable = dataSet.Tables[GetTableName()];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            row.SetField("Name", Name);
            row.SetField("Purpose", Purpose);
            if (this is ConcreteUsing) row.SetField("Materialkindname", "Concrete");
            else if (this is ReinforcementUsing) row.SetField("Materialkindname", "Reinforcement");
            else throw new NotImplementedException("Material kind is not valid");
            row.SetField("SelectedId", SelectedId);
            row.SetField("ParentId", ParentMember.Id);
            #endregion
            dataTable.AcceptChanges();
            foreach (SafetyFactor safetyFactor in SafetyFactors)
            {
                safetyFactor.SaveToDataSet(dataSet, createNew);
            }
            if (this is ReinforcementUsing)
            {
                ReinforcementUsing rfUsing = (this) as ReinforcementUsing;
                row.SetField("Diameter", rfUsing.Diameter);
                row.SetField("Prestrain", rfUsing.Prestrain);
                rfUsing.Placement.SaveToDataSet(dataSet, createNew);              
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
            if (this is ReinforcementUsing)
            {
                ReinforcementUsing rfUsing = (this) as ReinforcementUsing;
                rfUsing.Diameter = dataRow.Field<double>("Diameter");
                rfUsing.Prestrain = dataRow.Field<double>("Prestrain");
            }
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (SafetyFactor safetyFactor in SafetyFactors) {safetyFactor.DeleteFromDataSet(dataSet);}
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
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
            return materialUsing;
        }
         #region Methods
        /// <summary>
        /// Регистрация ссылки на родительскую сущность
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            ParentMember = parent;
        }
        /// <summary>
        /// Удаление регистрации ссылки на родительскую сущность
        /// </summary>
        public void UnRegisterParent()
        {
            ParentMember = null;
        }
        public double[] GetTotalSafetyFactor()
        {
            double[] safetyFactors = new double[] { 1, 1, 1, 1 };
            foreach (SafetyFactor safetyFactor in SafetyFactors)
            {
                safetyFactors[0] *= safetyFactor.PsfFst;
                safetyFactors[1] *= safetyFactor.PsfSnd;
                safetyFactors[2] *= safetyFactor.PsfFstLong;
                safetyFactors[3] *= safetyFactor.PsfSndLong;
            }
            return safetyFactors;
        }
        #endregion
    }
}
