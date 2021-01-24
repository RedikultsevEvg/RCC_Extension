﻿using System;
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
    /// Класс применения бетона в элементах
    /// </summary>
    public class MaterialUsing: ISavableToDataSet
    {
        /// <summary>
        /// Код применения
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код материала
        /// </summary>
        public int MaterialId { get; set; }
        /// <summary>
        /// Ссылка на материал
        /// </summary>
        public IMaterialKind MaterialKind { get; set; }
        /// <summary>
        /// Список коэффициентов надежности
        /// </summary>
        public ObservableCollection<SafetyFactor> SafetyFactors { get; set; }
        public string Purpose { get; set; }
        public List<IMaterialKind> MaterialKinds
        {
            get
            {
                List<IMaterialKind> materialKinds = new List<IMaterialKind>();
                if (MaterialKind is ConcreteKind) materialKinds.AddRange(ProgrammSettings.ConcreteKinds);
                else if ((MaterialKind is ReinforcementKind) || (MaterialKind is ReinforcementUsing)) materialKinds.AddRange(ProgrammSettings.ReinforcementKinds);
                else throw new NotImplementedException("Material kind is not valid");
                return materialKinds;
            }
        }

        private ISavableToDataSet ParentMember;
        #region Constructors
        public MaterialUsing()
        {
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
            dataTable = dataSet.Tables["Materialusings"];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            if (MaterialKind is ConcreteKind) row.SetField("Materialkindname", "Concrete");
            else if (MaterialKind is ReinforcementKind) row.SetField("Materialkindname", "Reinforcement");
            else if (MaterialKind is ReinforcementUsing)
            {
                row.SetField("Materialkindname", "Reinforcementusing");
                ReinforcementUsing reinforcementUsing = MaterialKind as ReinforcementUsing;
                reinforcementUsing.SaveToDataSet(dataSet, createNew);
            }
            else throw new NotImplementedException("Material kind is not valid");
            row.SetField("MaterialId", MaterialId);
            if (ParentMember is Foundation) row.SetField("Membertype", "Foundation");
            else throw new NotImplementedException("Member type is not valid");
            row.SetField("ParentId", ParentMember.Id);
            row.SetField("Purpose", Purpose);
            #endregion
            dataTable.AcceptChanges();
            foreach (SafetyFactor safetyFactor in SafetyFactors)
            {
                safetyFactor.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Обновляет запись в соответствии с сохраненной в датасете
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, "Materialusings", Id));
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            string materialKindName = dataRow.Field<string>("Materialkindname");
            switch (materialKindName)
                {
                case "Concrete":
                    MaterialKind = MaterialProcessor.GetMaterialKindById("Concrete", Id);
                    break;
                case "Reinforcement":
                    MaterialKind = MaterialProcessor.GetMaterialKindById("Reinforcement", Id);
                    break;
                default:
                    throw new NotImplementedException("Material name is not valid");
            }             
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, "Materialusings", Id);
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
            double[] safetyFactors = new double[] { 1, 1 };
            foreach (SafetyFactor safetyFactor in SafetyFactors)
            {
                safetyFactors[0] *= safetyFactor.PsfFst;
                safetyFactors[1] *= safetyFactor.PsfSnd;
            }
            return safetyFactors;
        }
        public void RenewMaterialKind()
        {
            if (MaterialKind is ConcreteKind) MaterialKind = MaterialProcessor.GetMaterialKindById("Concrete", MaterialId);
            else if (MaterialKind is ReinforcementKind) MaterialKind = MaterialProcessor.GetMaterialKindById("Reinforcement", MaterialId);
            else if (MaterialKind is ReinforcementUsing)
            {
                ReinforcementUsing reinforcementUsing = MaterialKind as ReinforcementUsing;
                reinforcementUsing.RenewKind();
            }
            else throw new NotImplementedException("Material kind is not valid");
        }
        #endregion
    }
}
