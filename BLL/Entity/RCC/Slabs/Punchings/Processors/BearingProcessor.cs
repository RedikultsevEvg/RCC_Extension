using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Entity.Common.Materials;
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
        /// Возвращает коэффициент несущей спсо
        /// </summary>
        /// <param name="contour">Расчетный контур продавливания</param>
        /// <param name="Nz">Продольная сила</param>
        /// <param name="Mx">Изгибающий момент относительно оси X</param>
        /// <param name="My">Tо же относительно оси Y</param>
        /// <param name="fullLoad">Флаг полной нагрузки</param>
        /// <returns></returns>
        public double GetBearindCapacityCoefficient(PunchingContour contour, double Nz, double Mx, double My, bool fullLoad = true)
        {
            double A = GetForceResistance(contour, fullLoad);
            double WxPos = GetMomentResistance(contour, fullLoad)[0];
            double WxNeg = GetMomentResistance(contour, fullLoad)[1];
            double WyPos = GetMomentResistance(contour, fullLoad)[2];
            double WyNeg = GetMomentResistance(contour, fullLoad)[3];
            double forceStress = Math.Abs(Nz / A);
            double momentStress = Mx / WxPos + My / WyPos;
            //Ограничение по вкладу моментов
            //Вклад момента не может более чем в 1,5 раза превышать вклад продольной силы
            double maxMomentStress = forceStress * 1.5;
            momentStress = momentStress < maxMomentStress ? momentStress : maxMomentStress;
            return forceStress + momentStress;
        }

        private double[] GetMomentResistance(PunchingContour contour, bool fullLoad = true)
        {
            double rXPos = 0;
            double rXNeg = 0;
            double rYPos = 0;
            double rYNeg = 0;
            //Находим момент инерции контура
            double[] momentOfInertia = GeomProcessor.GetMomentOfInertia(contour);

            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                //Расчетное сопротивление бетона растяжению для полных нагрузок
                double RbtFull = GetConcreteRbt(subContour)[0];
                //Расчетное сопротивление бетона растяжению для нагрузок продолжительного действия
                double Rbtlong = GetConcreteRbt(subContour)[1];
                //Расчетное сопротивление бетона растяжению
                double Rbt = fullLoad ? RbtFull : Rbtlong;
                //Прибавляем несущую способность субконтура
                double capacityX = GetSubContourMomentCapacityX(subContour, Rbt);
            }
            return new double[] { rXPos, rXNeg, rYPos, rYNeg};
        }

        private double GetSubContourMomentCapacityX(PunchingSubContour subContour, double rbt)
        {
            throw new NotImplementedException();
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
    }
}
