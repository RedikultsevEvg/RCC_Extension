using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.Common.Materials.Processors.Strength;
using RDBLL.Entity.RCC.Reinforcements.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Ancorages
{
    /// <summary>
    /// Класс логики для расчета длин анкеровки
    /// </summary>
    public class AncorageLogic : IAncorageLogic
    {
        /// <summary>
        /// Возвращает длину анкеровки арматуры в бетоне
        /// </summary>
        /// <param name="concrete">Использование бетона</param>
        /// <param name="barSection">Сечение арматурного стержня</param>
        /// <param name="loadRate">Коэффициент длительности нагрузки</param>
        /// <param name="inCompression">Флаг напряженного состояния (сжатие = true)</param>
        /// <param name="areaCofficient">Oтношения требуемой и фактической площади армирования</param>
        /// <returns></returns>
        public double GetAncorageLenth(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression, double areaCofficient = 1)
        {
            //Базовая длина анкеровки
            double ancorageLength = GetBaseAncorageLength(concrete, barSection, loadRate, inCompression);
            //Учет отношения требуемой и фактической площади армирования
            ancorageLength *= areaCofficient;
            //Коэффициент, учитывающий напряженное состояние (сжатие или растяжение)
            double alpha = inCompression ? 0.75: 1.0 ;
            ancorageLength *= alpha;
            //Диаметр арматуры
            double ds = barSection.Circle.Diameter;
            //Дополнительные ограничения на длину анкеровки
            if (ancorageLength < 15 * ds) { ancorageLength = 15 * ds; }
            if (ancorageLength < 0.2) { ancorageLength = 0.2; }
            return ancorageLength;
        }
        /// <summary>
        /// Возвращает длину нахлеста арматуры для случая без разбежки
        /// </summary>
        /// <param name="concrete"></param>
        /// <param name="barSection"></param>
        /// <param name="loadRate"></param>
        /// <param name="inCompression"></param>
        /// <param name="areaCofficient"></param>
        /// <returns></returns>
        public double GetDoubleLappingLenth(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression, double areaCofficient = 1)
        {
            //Базовая длина анкеровки
            double ancorageLength = GetBaseAncorageLength(concrete, barSection, loadRate, inCompression);
            //Учет отношения требуемой и фактической площади армирования
            ancorageLength *= areaCofficient;
            //Коэффициент, учитывающий напряженное состояние (сжатие или растяжение)
            double alpha = inCompression ? 1.2 : 2.0;
            ancorageLength *= alpha;
            return ancorageLength;
        }
        /// <summary>
        /// Возвращает необходимую длину разбежки для нахлеста арматуры
        /// </summary>
        /// <param name="concrete"></param>
        /// <param name="barSection"></param>
        /// <param name="loadRate"></param>
        /// <param name="inCompression"></param>
        /// <param name="areaCofficient"></param>
        /// <returns></returns>
        public double GetSimpleLappingGapLenth(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression, double areaCofficient = 1)
        {
            return 1.3 * GetSimpleLappingLenth(concrete, barSection, loadRate, inCompression, areaCofficient);
        }
        /// <summary>
        /// Возвращает длину нахлеста арматуры для случая с разбежкой
        /// </summary>
        /// <param name="concrete"></param>
        /// <param name="barSection"></param>
        /// <param name="loadRate"></param>
        /// <param name="inCompression"></param>
        /// <param name="areaCofficient"></param>
        /// <returns></returns>
        public double GetSimpleLappingLenth(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression, double areaCofficient = 1)
        {
            //Базовая длина анкеровки
            double ancorageLength = GetBaseAncorageLength(concrete, barSection, loadRate, inCompression);
            //Учет отношения требуемой и фактической площади армирования
            ancorageLength *= areaCofficient;
            //Коэффициент, учитывающий напряженное состояние (сжатие или растяжение)
            double alpha = inCompression ? 0.9 : 1.2;
            ancorageLength *= alpha;
            return ancorageLength;
        }
        /// <summary>
        /// возвращает пару расчетного сопротивления бетона растяжения - для полных и длительных нагрузок
        /// </summary>
        /// <param name="concrete">Использование бетона</param>
        /// <param name="barSection">Сечение арматурного стержня</param>
        /// <returns></returns>
        private double[] GetRbond(ConcreteUsing concrete, IBarSection barSection)
        {
            //Расчетное сопротивление бетона растяжению для всех нагрузок
            double RbtFull = StrengthProcessor.GetConcreteStrength(concrete, true, false, true);
            //Расчетное сопротивление бетона растяженияю для длительных нагрузок
            double RbtLong = StrengthProcessor.GetConcreteStrength(concrete, true, false, false);
            //Вспомогательный коэффициент, зависящий от вида арматуры
            double eta1 = (barSection.Reinforcement.MaterialKind as ReinforcementKind).BondCoefficient;
            //Вспомогательный коэффициент, зависящий от диаметра
            double eta2 = 1.0;
            if (barSection.Circle.Diameter > 0.032)
            {
                eta2 = 0.9;
            }
            //Расчетное сопротивление арматуры с бетоном для полных нагрузок
            double RbondFull = eta1 * eta2 * RbtFull;
            //тоже для длительных нагрузок
            double RbondLong = eta1 * eta2 * RbtLong;
            //Расчетное сопротивление сцепления
            return new double[2] { RbondFull, RbondLong };
        }
        /// <summary>
        /// Возвращает базовую длину анкеровки
        /// </summary>
        /// <param name="concrete">Использование бетона</param>
        /// <param name="barSection">Сечение арматурного стержня</param>
        /// <param name="loadRate">Коэффициент длительности д</param>
        /// <param name="inCompression"></param>
        /// <returns></returns>
        private double GetBaseAncorageLength(ConcreteUsing concrete, IBarSection barSection, double loadRate, bool inCompression)
        {
            double[] Rbonds = GetRbond(concrete, barSection);
            //Расчетное сопротивление арматуры с бетоном для полных нагрузок
            double RbondFull = Rbonds[0];
            //тоже для длительных нагрузок
            double RbondLong = Rbonds[1];
            ReinforcementUsing reinforcement = barSection.Reinforcement;
            ReinforcementKind kind = reinforcement.MaterialKind as ReinforcementKind;
            //Расчетное сопротивление арматуры
            double Rs = StrengthProcessor.GetReinforcementStrength(reinforcement, true, inCompression, true);
            //Площадь сечения арматуры
            double As = barSection.Circle.GetArea();
            //Периметр арматуры
            double Ps = barSection.Circle.GetPerimeter();
            //Длина анкеровки от полной нагрузки
            double ancorageLengthFull = Rs * As / (RbondFull * Ps);
            //Длина анкеровки от длительной нагрузки
            double ancorageLengthLong = Rs * As / (RbondLong * Ps) * loadRate;
            //Длина анкеровки
            double ancorageLength = Math.Max(ancorageLengthFull, ancorageLengthLong);
            return ancorageLength;
        }

    }
}
