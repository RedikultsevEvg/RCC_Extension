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

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс применения материалов в элементах
    /// </summary>
    public abstract class MaterialUsing : IHasParent, IDuplicate
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
        public ISavableToDataSet ParentMember { get; private set; }
        private static string TableName { get { return "MaterialUsings"; } }
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
        public MaterialUsing(ISavableToDataSet parentMember)
        {
            Id = ProgrammSettings.CurrentId;
            RegisterParent(parentMember);
            SafetyFactors = new ObservableCollection<SafetyFactor>();
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables[TableName];
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
                rfUsing.RFSpacing.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Обновляет запись в соответствии с сохраненной в датасете
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, TableName, Id));
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
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (SafetyFactor safetyFactor in SafetyFactors)
            {
                safetyFactor.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, TableName, Id);
        }
        #endregion
        #region IDuplicate
        public object Duplicate()
        {
            MaterialUsing materialUsing;
            if (this is ConcreteUsing)
            {
                ConcreteUsing concrete = new ConcreteUsing();
                materialUsing = concrete;
            }
            else if (this is ReinforcementUsing)
            {
                ReinforcementUsing reinforcement = new ReinforcementUsing();
                reinforcement.RFSpacing = (this as ReinforcementUsing).RFSpacing.Duplicate() as RFSpacingBase;
                materialUsing = reinforcement;
            }
            else throw new Exception("Material kind is not valid");
            materialUsing.Id = ProgrammSettings.CurrentId;
            materialUsing.Name = Name;
            materialUsing.Purpose = Purpose;
            materialUsing.SelectedId = SelectedId;

            foreach (SafetyFactor safetyFactor in SafetyFactors)
            {
                SafetyFactor newObject = (safetyFactor.Duplicate()) as SafetyFactor;
                newObject.RegisterParent(materialUsing);
                materialUsing.SafetyFactors.Add(newObject);
            }
            return materialUsing;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Регистрация ссылки на родительскую сущность
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(ISavableToDataSet parent)
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
