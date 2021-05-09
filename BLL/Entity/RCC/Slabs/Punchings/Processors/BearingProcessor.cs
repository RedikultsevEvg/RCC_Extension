using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Slabs.Punchings.Results;
using RDBLL.Entity.RCC.Slabs.Punchings.Results.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Processors
{
    public class BearingProcessor : IBearingProcessor
    {
        /// <summary>
        /// Возвращает коэффициент несущей способности (отношение действующих усилий к предельным)
        /// </summary>
        /// <param name="contour">Расчетный контур продавливания</param>
        /// <param name="Nz">Продольная сила</param>
        /// <param name="Mx">Изгибающий момент относительно оси X</param>
        /// <param name="My">Tо же относительно оси Y</param>
        /// <param name="fullLoad">Флаг полной нагрузки</param>
        /// <returns></returns>
        public double GetBearingCapacityCoefficient(PunchingContour contour, double Nz, double Mx, double My, bool fullLoad = true)
        {
            double A = GetForceResistance(contour, fullLoad);
            double[] w = GetMomentResistance(contour, fullLoad);
            double WxPos = w[0];
            double WxNeg = w[1];
            double WyPos = w[2];
            double WyNeg = w[3];
            double forceStress = Nz / A;
            double momentStressXPos = Mx / WxPos;
            double momentStressXNeg = Mx / WxNeg;
            double momentStressYPos = My / WyPos;
            double momentStressYNeg = My / WyNeg;

            double momentX;
            if (Math.Sign(Nz/A) == Math.Sign(momentStressXPos))
            {
                momentX = momentStressXPos;
            }
            else
            {
                momentX = momentStressXNeg;
            }
            double momentY;
            if (Math.Sign(Nz / A) == Math.Sign(momentStressYPos))
            {
                momentY = momentStressYPos;
            }
            else
            {
                momentY = momentStressYNeg;
            }
            forceStress = Math.Abs(forceStress);
            momentX = Math.Abs(momentX);
            momentY = Math.Abs(momentX);
            double maxMoment = momentX + momentY;
            //Ограничение по вкладу моментов
            //Вклад момента не может более чем в 0,5 раза превышать вклад продольной силы
            double limitMoment = forceStress * 0.5;
            if (maxMoment > limitMoment)
            {
                return 1.5 * forceStress;
            }
            return forceStress + maxMoment;
        }

        public double[] GetMomentResistance(PunchingContour contour, bool fullLoad = true)
        {
            double rXPos = 0;
            double rXNeg = 0;
            double rYPos = 0;
            double rYNeg = 0;
            //Находим момент инерции контура
            double[] moments = GeomProcessor.GetMomentOfInertia(contour);
            double Ix = moments[0];
            double Iy = moments[1];

            Point2D center = GeomProcessor.GetContourCenter(contour);

            double totalHeight = GeomProcessor.GetContourHeight(contour);

            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                //Расчетное сопротивление бетона растяжению для полных нагрузок
                double RbtFull = GetConcreteRbt(subContour)[0];
                //Расчетное сопротивление бетона растяжению для нагрузок продолжительного действия
                double Rbtlong = GetConcreteRbt(subContour)[1];
                //Расчетное сопротивление бетона растяжению
                double Rbt = fullLoad ? RbtFull : Rbtlong;
                double[] dists = GeomProcessor.GetMaxDistFromContour(contour);
                //Прибавляем несущую способность субконтура
                double[] capacity = GetSubContourMomentCapacity(subContour, center, dists, Rbt);

                rXPos += capacity[0];
                rXNeg += capacity[1];
                rYPos += capacity[2];
                rYNeg += capacity[3];

            }
            return new double[] { rXPos, rXNeg, rYPos, rYNeg};
        }

        private double[] GetSubContourMomentCapacity(PunchingSubContour subContour, Point2D center, double[] maxSizes, double rbt)
        {
            double maxCounterX = maxSizes[0];
            double minCounterX = maxSizes[1];
            double maxCounterY = maxSizes[2];
            double minCounterY = maxSizes[3];

            double maxXmoment = 0;
            double minXmoment = 0;
            double maxYmoment = 0;
            double minYmoment = 0;

            double[] I = GeomProcessor.GetMomentOfInertia(subContour, center);
            double Ix = I[0];
            double Iy = I[1];

            maxXmoment += Ix / maxCounterY * subContour.Height * rbt;
            minXmoment += Ix / minCounterY * subContour.Height * rbt;
            maxYmoment += Iy / maxCounterX * subContour.Height * rbt;
            minYmoment += Iy / minCounterX * subContour.Height * rbt;

            return new double[] { maxXmoment, minXmoment, maxYmoment, minYmoment }; 
        }

        /// <summary>
        /// Возвращает несущую способность контура на действие продольной силы
        /// </summary>
        /// <param name="contour">Расчетный контур продавливания</param>
        /// <param name="fullLoad">Флаг полной нагрузки (по умолчанию), иначе - нагрузка длительного действия</param>
        /// <returns></returns>
        private double GetForceResistance(PunchingContour contour, bool fullLoad = true)
        {
            double capacity = 0;
            //Для каждого субконтура в контуре
            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                //Расчетное сопротивление бетона растяжению для полных нагрузок
                double RbtFull = GetConcreteRbt(subContour)[0];
                //Расчетное сопротивление бетона растяжению для нагрузок продолжительного действия
                double Rbtlong = GetConcreteRbt(subContour)[1];
                //Расчетное сопротивление бетона растяжению
                double Rbt = fullLoad ? RbtFull : Rbtlong;
                //Прибавляем несущую способность субконтура
                capacity += GetSubContourForceCapacity(subContour, Rbt);
            }
            //Несущая способность контура с учетом длительности нагрузки
            return capacity;
        }
        /// <summary>
        /// Возвращает несущую способность контура на действие продольной силы
        /// </summary>
        /// <param name="subContour"></param>
        /// <param name="Rbt"></param>
        /// <returns></returns>
        private double GetSubContourForceCapacity(PunchingSubContour subContour, double Rbt)
        {
            double length = GeomProcessor.GetSubContourLength(subContour);
            double forceCapacity = GetForceCapacity(length, subContour.Height, Rbt);
            return forceCapacity;
        }
        /// <summary>
        /// Возвращает несущую способность субконтура на действие продольной силы
        /// </summary>
        /// <param name="contourLength">Длина расчетного контура продавливания</param>
        /// <param name="depth">Рабочая (эффективная) высота расчетного контура</param>
        /// <param name="Rbt">Прочность бетона на растяжение</param>
        /// <returns></returns>
        private double GetForceCapacity(double contourLength, double depth, double Rbt)
        {
            double forceCapacity = contourLength * depth * Rbt;
            return forceCapacity;
        }
        /// <summary>
        /// Возвращает пару расчетных сопротивлений 0 - для полных нагрузок, 1 - для нагрузок продолжительного действия;
        /// </summary>
        /// <param name="subContour"></param>
        /// <returns></returns>
        private double[] GetConcreteRbt(PunchingSubContour subContour)
        {
            double[] Rbt = new double[] { 0, 0 };
            ConcreteUsing mat = subContour.Concrete;
            Rbt[0] = (mat.MaterialKind as ConcreteKind).FstTensStrength * mat.TotalSafetyFactor.PsfFstTens;
            Rbt[1] = (mat.MaterialKind as ConcreteKind).FstTensStrength * mat.TotalSafetyFactor.PsfFstLongTens;
            return Rbt;
        }
        /// <summary>
        /// Вычисление результата
        /// </summary>
        /// <param name="punching"></param>
        public void CalcResult(Punching punching)
        {
            try
            {
                IPunchingResultBuilder resultBuilder = new PunchingResultBuilder(punching);
                punching.Result = resultBuilder.GetPunchingResult();
                punching.IsActive = true;
                //punching.IsActive = false;
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage($"Ошибка расчета элемента: " + punching.Name, ex);
                punching.IsActive = false;
            }
            
        }
    }
}
