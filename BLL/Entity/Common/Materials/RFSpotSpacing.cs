using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials.Interfaces;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Базовый класс для распределения арматуры, имеющий диаметр
    /// </summary>
    public abstract class RFSpotSpacing : RFSpacingBase
    {
        /// <summary>
        /// Диаметр стержня
        /// </summary>
        public double Diameter
        { get { return RFSpacingParameters[0].GetDoubleValue(); }
            set { RFSpacingParameters[0].SetDoubleValue(value); }
        }
        /// <summary>
        /// Площадь одного стержная, вычисляется через диаметр
        /// </summary>
        public double Area { get { return Diameter * Diameter * Math.PI / 4; } }
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public RFSpotSpacing() : base()
        {

        }
        /// <summary>
        /// Конструктор по родительскому элементу
        /// </summary>
        /// <param name="parent"></param>
        public RFSpotSpacing(ReinforcementUsing parentMember) :base(parentMember)
        {
            RFSpacingParameter newParam = new RFSpacingParameter(this);
            newParam.SetParameterValue("double", 0.012);
            RFSpacingParameters.Add(newParam);
        }
        #endregion
        #region Methods
        /// <summary>
        /// Возвращает общее количество стержней в пределах заданного массива
        /// </summary>
        /// <returns></returns>
        public abstract int GetTotalBarsQuantity();
        #endregion
    }
}
