using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RDBLL.Common.Service;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Forces
{
    /// <summary>
    ///Клас комбинации загружений 
    /// </summary>
    public class LoadSet : IEquatable<LoadSet>, ISavableToDataSet
    {
        #region
        /// <summary>
        /// Код комбинации загружений
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на родительскую группу нагруок
        /// </summary>
        public List<ForcesGroup> ForcesGroups { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
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
        /// <summary>
        /// Коллекция нагрузок, входящих в загружение
        /// </summary>
        public ObservableCollection<ForceParameter> ForceParameters { get; set; }
        #endregion       
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public LoadSet()
        {
            ForcesGroups = new List<ForcesGroup>();
            ForceParameters = new ObservableCollection<ForceParameter>();
        }

        /// <summary>
        /// Конструктор по группе нагрузок
        /// </summary>
        /// <param name="forcesGroup"></param>
        public LoadSet(ForcesGroup forcesGroup)
        {
            Id = ProgrammSettings.CurrentId;
            ForcesGroups = new List<ForcesGroup>();
            ForceParameters = new ObservableCollection<ForceParameter>();
            ForcesGroups.Add(forcesGroup);
            forcesGroup.SetParentsNotActual();
            Name = "Новая нагрузка";
            PartialSafetyFactor = 1.1;
            IsLiveLoad = false;
            IsCombination = false;
            BothSign = false;
            
        }
        #endregion
        #region Methods
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["LoadSets"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
            { Id, Name, PartialSafetyFactor, IsLiveLoad, IsCombination, BothSign
            };
            dataTable.Rows.Add(dataRow);

            dataTable = dataSet.Tables["ForcesGroupLoadSets"];
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                dataRow = dataTable.NewRow();
                dataRow.ItemArray = new object[]
                { forcesGroup.Id, this.Id
                };
                dataTable.Rows.Add(dataRow);
            }
            foreach (ForceParameter forceParameter in ForceParameters)
            {
                forceParameter.SaveToDataSet(dataSet);
            }
        }

        public void OpenFromDataSet(DataSet dataSet, int Id)
        {

        }
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
        public bool CompareForceParameters(LoadSet other)
        {
            if (!(other.ForceParameters.Count == ForceParameters.Count)) { return false; }
            for (int i = 0; i < ForceParameters.Count; i++)
            {
                if (!this.ForceParameters[i].Equals(other.ForceParameters[i])) { return false; }
            }
            return true;
        }
    }
}
