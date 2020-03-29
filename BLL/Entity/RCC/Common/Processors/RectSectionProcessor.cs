using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Common.Processors
{
    /// <summary>
    /// Процессов обработки прямоугольных сечений
    /// </summary>
    public static class RectSectionProcessor
    {
        /// <summary>
        /// Возвращает предельный момент для прямоугольного сечения для одностороннего армирования
        /// </summary>
        /// <param name="b">CrossSection width</param>
        /// <param name="h0">Effective depth</param>
        /// <param name="As">Reinforcement area</param>
        /// <param name="Rs">Reinforcement strength</param>
        /// <param name="Rc">Concrete strength</param>
        /// <param name="Es">Reinforcement Young's modulus</param>
        /// <param name="Eps_b2">Limit strain of concrete</param>
        /// <returns>Limit moment for cross section</returns>
        public static double GetUltMoment (double b, double h0, double As, double Rs, double Rc, double Es = 2e11, double Eps_b2 = 0.0035)
        {
            double ns = Rs * As;
            double x = ns / (Rc * b);
            double ksi = x / h0;
            double ksiR = 0.8 / (1 + (Rs / Es) / Eps_b2);
            if (ksi > ksiR)
            {
                x = ksiR * h0;
                ns = Rc * b * x;
            }
            double z = h0 - x / 2;
            return ns * z;
        }
        /// <summary>
        /// Return area of reinforcement for rectangle cross section
        /// </summary>
        /// <param name="M">Actual bending moment</param>
        /// <param name="b">Cross Section width</param>
        /// <param name="h0">Effective depth</param>
        /// <param name="Rs">Reinforcement strength</param>
        /// <param name="Rc">Concrete strength</param>
        /// <param name="Es">Reinforcement Young's modulus</param>
        /// <param name="Eps_b2">Limit strain of concrete</param>
        /// <returns>Area of reinforcement</returns>
        public static double GetReinforcementArea(double M, double b, double h0, double Rs, double Rc, double Es = 2e11, double Eps_b2 = 0.0035)
        {
            double alphaM = M / (Rc * b * h0 * h0) ;
            double ksi = 1 - Math.Sqrt(1 - 2 * alphaM);
            double ksiR = 0.8 / (1 + (Rs / Es) / Eps_b2);
            if (ksi > ksiR) throw new Exception("Требуется сжатая арматура");
            return Rc * b * ksi * h0 / Rs;
        }
    }
}
