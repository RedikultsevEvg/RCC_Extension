using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс крупнообломочного грунта
    /// </summary>
    public class GravelSoil :DispersedSoil
    {
        public bool HasIL { get; set; }
        /// <summary>
        /// Код заполнителя
        /// 0 - без заполнителя
        /// 1 - песчаный заполнитель
        /// 2 - глинистый заполнитель
        /// </summary>
        public int FillingId { get; set; }
        /// <summary>
        /// Количество заполнителя
        /// </summary>
        public double FillingQuantity { get; set; }
        /// <summary>
        /// Показатель текучести заполнителя
        /// </summary>
        public double FillingIL { get; set; }
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public GravelSoil(BuildingSite buildingSite) : base(buildingSite)
        {
            Description = "Щебенистый грунт";
            CrcFi = 30;
            FstDesignFi = 27;
            SndDesignFi = 28;
            CrcCohesion = 0;
            FstDesignCohesion = 0;
            SndDesignCohesion = 0;
            HasIL = false;
            //По умолчанию - без заполнителя
            FillingId = 0;
            /*BignessId
             * 0 - валунный
             * 1 - глыбовый
             * 2 - галечниковый
             * 3 - щебенистый
             * 4 - гравийный
             * 5 - дресвяный
             * */
            BignessId = 3;
        }
    }
}
