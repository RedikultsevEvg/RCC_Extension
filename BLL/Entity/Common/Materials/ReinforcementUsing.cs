using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Materials.Interfaces;
using RDBLL.Common.Interfaces;
using System.Data;
using DAL.Common;
using RDBLL.Common.Service;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс использования арматуры в конструкции
    /// </summary>
    public class ReinforcementUsing : MaterialUsing
    {
        /// <summary>
        /// Класс расположения арматуры
        /// </summary>
        public RFSpacingBase RFSpacing { get; set; }
        #region Constructors
        public ReinforcementUsing() : base()
        {

        }
        public ReinforcementUsing(ISavableToDataSet parentMember) : base(parentMember)
        {
            SelectedId = 2;
        }
        #endregion
        #region Method
        #endregion
    }
}
