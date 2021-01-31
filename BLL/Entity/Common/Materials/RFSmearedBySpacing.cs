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
    /// Задается максимальное расстояние между стержнями (шаг).
    /// </summary>
    public class RFSmearedBySpacing : RFSmearedSpacing
    {
        /// <summary>
        /// Количество дополнительных стержней
        /// </summary>
        public int AddQuantity
        {
            get { return RFSpacingParameters[2].GetIntValue(); }
            set { RFSpacingParameters[2].SetIntValue(value); }
        }
        /// <summary>
        /// Шаг арматуры
        /// </summary>
        public double Spacing
        {
            get { return RFSpacingParameters[3].GetDoubleValue(); }
            set { RFSpacingParameters[3].SetDoubleValue(value); }
        }
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public RFSmearedBySpacing() : base() {;}
        /// <summary>
        /// Конструктор по родительскому элементу
        /// </summary>
        /// <param name="parent"></param>
        public RFSmearedBySpacing(ReinforcementUsing parentMember) : base(parentMember)
        {
            //Создаем параметр для количество добавленных стержней
            RFSpacingParameter barAddQuantity = new RFSpacingParameter(this);
            barAddQuantity.SetParameterValue("int", 1);
            RFSpacingParameters.Add(barAddQuantity);
            //Создаем параметр для шага арматуры
            RFSpacingParameter barSpacing = new RFSpacingParameter(this);
            barSpacing.SetParameterValue("double", 0.200);
            RFSpacingParameters.Add(barSpacing);
        }
        #endregion
        #region Methods
        public override int GetTotalBarsQuantity()
        {
            throw new Exception("Couldn't obtain bars quantity without length");
        }
        public int GetTotalBarsQuantity(double length)
        {
            double spacing = RFSpacingParameters[2].GetDoubleValue();
            int additionalBars = RFSpacingParameters[1].GetIntValue();
            int spacingQuantity = Convert.ToInt32(Math.Ceiling(length / spacing));
            return spacingQuantity + additionalBars;
        }
        public double GetTotalBarsArea(double length)
        {
            return GetTotalBarsQuantity(length) * Area;
        }
        #endregion
    }
}
