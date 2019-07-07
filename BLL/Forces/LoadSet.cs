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
    public class LoadSet
    {
        #region 
        public String Name { get; set; } //Наименование
        public double PartialSafetyFactor { get; set; } //Коэффициент надежности по нагрузке
        public bool IsDeadLoad { get; set; } //Флаг постоянной нагрузки
        public bool IsCombination { get; set; } //Флаг комбинации
        public bool IsDesignLoad { get; set; } //Флаг расчетной нагрузки
        public bool BothSign { get; set; } //Флаг знакопеременной нагрузки
        public ObservableCollection<ForceParameter> ForceParameters { get; set; }
        #endregion
        //Constructors
        #region
        public LoadSet()
        {
            ForceParameters = new ObservableCollection<ForceParameter>();
        }
        #endregion
    }
}
