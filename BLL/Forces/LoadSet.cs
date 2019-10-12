using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace RDBLL.Forces
{
    /// <summary>
    ///Клас комбинации загружений 
    /// </summary>
    public class LoadSet : IEquatable<LoadSet>
    {
        #region
        private bool _isDeadLoad;
        public int Id { get; set; }
        public ForcesGroup ForcesGroup { get; set; } //Обратная ссылка на родительскую группу нагруок
        public String Name { get; set; } //Наименование
        public double PartialSafetyFactor { get; set; } //Коэффициент надежности по нагрузке
        public bool IsDeadLoad //Флаг постоянной нагрузки
        {
            get { return _isDeadLoad; }
            set
            {
                if (value) { BothSign = false; }
                _isDeadLoad = value;
            }
        }
        public bool IsCombination { get; set; } //Флаг комбинации
        public bool BothSign { get; set; } //Флаг знакопеременной нагрузки
        public ObservableCollection<ForceParameter> ForceParameters { get; set; }
        #endregion
        //Constructors
        #region
        public LoadSet()
        {
            ForceParameters = new ObservableCollection<ForceParameter>();
        }

        public LoadSet(ForcesGroup forcesGroup)
        {
            ForcesGroup = forcesGroup;
            forcesGroup.SteelColumnBase.IsLoadCasesActual = false;
            Name = "Новая нагрузка";
            PartialSafetyFactor = 1.1;
            IsDeadLoad = true;
            IsCombination = false;
            BothSign = false;
            ForceParameters = new ObservableCollection<ForceParameter>();
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
