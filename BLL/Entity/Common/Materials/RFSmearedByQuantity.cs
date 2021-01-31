using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс для арматуры, распределенной по линии.
    /// Задается количество стержней, шаг определяется делением на равные расстояния.
    /// </summary>
    public class RFSmearedByQuantity : RFSmearedSpacing
    {
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public RFSmearedByQuantity() : base()
        {

        }
        /// <summary>
        /// Конструктор по родительскому элементу
        /// </summary>
        /// <param name="parent"></param>
        public RFSmearedByQuantity(ReinforcementUsing parentMember) : base (parentMember)
        {
            RFSpacingParameter barQuantity = new RFSpacingParameter(this);
            barQuantity.SetParameterValue("int", 10);
            RFSpacingParameters.Add(barQuantity);
        }
        #endregion
        #region methods
        /// <summary>
        /// Возвращает количество стержней
        /// </summary>
        /// <returns></returns>
        public override int GetTotalBarsQuantity()
        {
            return RFSpacingParameters[1].GetIntValue();
        }
        #endregion
    }
}
