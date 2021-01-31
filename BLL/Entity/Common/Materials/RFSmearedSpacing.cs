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
    /// Базовый класс "размазанного" расположения армирования
    /// </summary>
    public abstract class RFSmearedSpacing :RFSpotSpacing
    {
        /// <summary>
        /// Защитный слой
        /// </summary>
        public double CoveringLayer
        {
            get { return RFSpacingParameters[1].GetDoubleValue(); }
            set { RFSpacingParameters[1].SetDoubleValue(value); }
        }
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public RFSmearedSpacing() : base()
            {          

            }
        /// <summary>
        /// Конструктор по родительскому элементу
        /// </summary>
        /// <param name="parent"></param>
        public RFSmearedSpacing(ReinforcementUsing parentMember) : base(parentMember)
        {
            SetInitialParam();
        }

        #endregion
        #region methods
        public double GetTotalBarsArea()
        {
            return GetTotalBarsQuantity() * Area;
        }
        private void SetInitialParam()
        {
            //Создаем параметр для защитного слоя
            RFSpacingParameter newParam = new RFSpacingParameter(this);
            newParam.SetParameterValue("double", 0.07);
            RFSpacingParameters.Add(newParam);
        }
        #endregion

    }
}
