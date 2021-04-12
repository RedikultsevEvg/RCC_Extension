using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Placements
{
    public interface IHasPlacement
    {
        /// <summary>
        /// Свойство для сохранения расположения элементов
        /// </summary>
        Placement Placement { get; }
        /// <summary>
        /// Метод добавления расположения элементов
        /// </summary>
        /// <param name="placement"></param>
        void SetPlacement(Placement placement);
    }
}
