using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс для хранения списка сжатых слоев грунта
    /// </summary>
    public class CompressedLayerList
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Список сжатых слоев грунта
        /// </summary>
        public List<CompressedLayer> CompressedLayers {get;set;}
    }
}
