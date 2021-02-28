using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using RDBLL.Entity.Common.Materials.Interfaces;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс использования бетона в конструкции
    /// </summary>
    public class ConcreteUsing : MaterialUsing
    {
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ConcreteUsing() : base()
        {

        }

        #region Methods
        public void AddGammaB1()
        {
            SafetyFactor safetyFactor = new SafetyFactor(true);
            safetyFactor.Name = "Коэффициент, учитывающий длительность действия нагрузки";
            safetyFactor.PsfFstLong = 0.9;
            safetyFactor.RegisterParent(this);
            SafetyFactors.Add(safetyFactor);
        }
        public void AddGammaB2()
        {
            SafetyFactor safetyFactor = new SafetyFactor(true);
            safetyFactor.Name = "Коэффициент, учитывающий бетонирование в вертикальном положении";
            safetyFactor.PsfFstLong = 0.85;
            safetyFactor.RegisterParent(this);
            SafetyFactors.Add(safetyFactor);
        }

        public object Clone()
        {
            return base.Clone();
        }
        #endregion
    }
}
