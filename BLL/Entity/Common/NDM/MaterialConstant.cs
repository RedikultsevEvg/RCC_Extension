using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.NDM
{
    /// <summary>
    /// Константа материала
    /// </summary>
    public class MaterialConstant
    { 
        /// <summary>
        /// Наименование константы
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значение константы
        /// </summary>
        public double ConstantValue { get; set; } 
    }
}
