using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace RDBLL.Forces
{
    /// <summary>
    /// Абстрактный класс комбинации нагрузок иил усилий
    /// </summary>
    public abstract class Load
    {
        /// <summary>
        /// Код комбинации загружений
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Коллекция нагрузок, входящих в загружение
        /// </summary>
        public ObservableCollection<ForceParameter> ForceParameters { get; set; }

        public Load()
        {
            ForceParameters = new ObservableCollection<ForceParameter>();
        }
                        
    }
}
