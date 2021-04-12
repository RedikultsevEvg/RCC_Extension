using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.MaterialModels;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM
{
    /// <summary>
    /// Класс элементарного участка круглой формы
    /// </summary>
    public class NdmCircleArea :NdmArea
    {
        private double _Diametr;
        /// <summary>
        /// Диаметр
        /// </summary>
        public double Diametr
        {
            get { return _Diametr; }
            set
            {
                _Diametr = value;
                Area = _Diametr * _Diametr * 0.785;
            }
        }

        /// <summary>
        /// Конструктор по модели материала
        /// </summary>
        /// <param name="materialModel"></param>
        public NdmCircleArea(IMaterialModel materialModel)
        {
            MaterialModel = materialModel;
            //new LinearIsotropic(2e+11, 0.000001, 1)
        }
    }
}
