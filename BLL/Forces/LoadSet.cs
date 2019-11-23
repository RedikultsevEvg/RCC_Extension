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
        public int Id { get; set; }
        public List<ForcesGroup> ForcesGroups { get; set; } //Обратная ссылка на родительскую группу нагруок
        public string Name { get; set; } //Наименование
        public double PartialSafetyFactor { get; set; } //Коэффициент надежности по нагрузке
        public bool IsLiveLoad {get; set; }//Флаг временной нагрузки
        public bool IsCombination { get; set; } //Флаг комбинации
        public bool BothSign { get; set; } //Флаг знакопеременной нагрузки
        public ObservableCollection<ForceParameter> ForceParameters { get; set; }
        #endregion       
        #region Constructors
        public LoadSet()
        {
            ForcesGroups = new List<ForcesGroup>();
            ForceParameters = new ObservableCollection<ForceParameter>();
        }

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
