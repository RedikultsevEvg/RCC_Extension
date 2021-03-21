using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RDBLL.Common.Service;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Forces
{
    /// <summary>
    ///Клас комбинации загружений 
    /// </summary>
    public class LoadSet : Load, IEquatable<LoadSet>, IDsSaveable, IDuplicate
    {
        #region
        /// <summary>
        /// Обратная ссылка на родительскую группу нагруок
        /// </summary>
        public List<ForcesGroup> ForcesGroups { get; set; }
        /// <summary>
        /// Коэффициент надежности по нагрузке
        /// </summary>
        public double PartialSafetyFactor { get; set; }
        /// <summary>
        /// Флаг временной нагрузки
        /// </summary>
        public bool IsLiveLoad {get; set; }
        /// <summary>
        /// Флаг комбинации
        /// </summary>
        public bool IsCombination { get; set; }
        /// <summary>
        /// Флаг знакопеременной нагрузки
        /// </summary>
        public bool BothSign { get; set; }
        #endregion       
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public LoadSet() :base()
        {
            ForcesGroups = new List<ForcesGroup>();

        }

        /// <summary>
        /// Конструктор по группе нагрузок
        /// </summary>
        /// <param name="forcesGroup"></param>
        public LoadSet(ForcesGroup forcesGroup) :base()
        {
            Id = ProgrammSettings.CurrentId;
            ForcesGroups = new List<ForcesGroup>();
            ForcesGroups.Add(forcesGroup);
            Name = "Новая нагрузка";
            PartialSafetyFactor = 1.1;
            IsLiveLoad = false;
            IsCombination = false;
            BothSign = false;
            
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "LoadSets"; }
        /// <summary>
        /// Сохраняет лоадсет в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            #region
            row.SetField("PartialSafetyFactor", PartialSafetyFactor);
            row.SetField("IsLiveLoad", IsLiveLoad);
            row.SetField("IsCombination", IsCombination);
            row.SetField("BothSign", BothSign);
            #endregion
            row.AcceptChanges();

            DataTable dataTable = dataSet.Tables["ForcesGroupLoadSets"];
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                if (createNew)
                {
                    row = dataTable.NewRow();
                    dataTable.Rows.Add(row);
                }
                else
                {
                    var tmpRow = (from dataRow in dataTable.AsEnumerable()
                                  where dataRow.Field<int>("LoadSetId") == Id
                                  select dataRow).Single();
                    row = tmpRow;
                }
                #region
                row.SetField("Id", ProgrammSettings.CurrentId);
                row.SetField("ForcesGroupId", forcesGroup.Id);
                row.SetField("LoadSetId",Id);
                #endregion
            }
            dataTable.AcceptChanges();
            foreach (ForceParameter forceParameter in ForceParameters)
            {
                forceParameter.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Обновляет запись в соответствии с датасетом
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (ForceParameter forceParameter in ForceParameters)
            {
                forceParameter.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        #region Methods
        #endregion
        //IEquatable
        public bool Equals(LoadSet other)
        {
            if (this.Name == other.Name
                & this.PartialSafetyFactor==other.PartialSafetyFactor
                & CompareForceParameters(other))
            {
                return true;
            }
            else { return false; }
        }
        /// <summary>
        /// Сравнивает все значения нагрузок при сравнении текущего лоадсета с другим
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true, если лоадсеты имеют одинаковые наборы нагрузок</returns>
        public bool CompareForceParameters(LoadSet other)
        {
            if (!(other.ForceParameters.Count == ForceParameters.Count)) { return false; }
            for (int i = 0; i < ForceParameters.Count; i++)
            {
                if (!this.ForceParameters[i].Equals(other.ForceParameters[i])) { return false; }
            }
            return true;
        }
        #region IDuplicate
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            LoadSet loadSet = this.MemberwiseClone() as LoadSet;
            loadSet.Id = ProgrammSettings.CurrentId;
            loadSet.ForcesGroups = new List<ForcesGroup>();
            loadSet.ForceParameters = new ObservableCollection<ForceParameter>();
            //Копируем параметры нагрузки
            foreach (ForceParameter forceParameter in ForceParameters)
            {
                ForceParameter newForceParameter = forceParameter.Clone() as ForceParameter;
                newForceParameter.LoadId = loadSet.Id;
                newForceParameter.LoadSet = loadSet;
                loadSet.ForceParameters.Add(newForceParameter);
            }
            return loadSet;
        }
        #endregion
    }
}
