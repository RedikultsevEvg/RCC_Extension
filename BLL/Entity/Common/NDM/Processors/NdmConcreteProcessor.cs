using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM;

namespace RDBLL.Entity.Common.NDM.Processors
{
    /// <summary>
    /// Процессор элементарных участков для бетонных сечений
    /// </summary>
    public static class NdmConcreteProcessor
    {
        /// <summary>
        /// Возвращает координаты центра тяжести для коллекции элементарных участков
        /// </summary>
        /// <param name="ndmAreas">Коллекция элементарных участков</param>
        /// <returns>координаты центра тяжести X, Y</returns>
        public static double[] GetGravityCenter(List<NdmArea> ndmAreas)
        {           
            //Все значения будут умножены на модуль упругости, который потом сократится
            double a_red = 0;
            double sX_red = 0;
            double sY_red = 0;
            foreach (NdmArea ndmArea in ndmAreas)
            {
                double a = ndmArea.Area * ndmArea.MaterialModel.ElasticModulus;
                a_red += a;
                sX_red += a * ndmArea.CenterY;
                sY_red += a * ndmArea.CenterX;
            }
            double[] gravityCenter = new double[2] { sY_red / a_red, sX_red / a_red };
            return gravityCenter;
        }
        /// <summary>
        /// Возвращает моменты инерции, приведенные к центру и приведенные к модулю
        /// </summary>
        /// <param name="center">Координаты центра тяжести</param>
        /// <param name="ndmAreas">Коллекция элементарных участков</param>
        /// <param name="modulus">Модуль упругости, к которому выполняется приведение</param>
        /// <returns></returns>
        public static double[] GetMomentInertia (List<NdmArea> ndmAreas, double modulus, double[] center = null)
        {
            if (center is null) center = GetGravityCenter(ndmAreas);
            double Ix = 0;
            double Iy = 0;
            foreach (NdmArea ndmArea in ndmAreas)
            {
                double x = ndmArea.CenterX - center[0];
                double y = ndmArea.CenterY - center[1];
                Ix += ndmArea.Area * ndmArea.MaterialModel.ElasticModulus * y * y;
                Iy += ndmArea.Area * ndmArea.MaterialModel.ElasticModulus * x * x;
            }
            return new double[2] {Ix / modulus, Iy / modulus};
        }
        /// <summary>
        /// Возвращает моменты сопротивления, приведенные к центру и модулю
        /// </summary>
        /// <param name="ndmAreas">Коллекция элементарных участков</param>
        /// <param name="modulus">Модуль деформации, к которому выполняетя приведение</param>
        /// <param name="center">Массив координат центра тяжести</param>
        /// <returns>Массив моментов сопротивления - верх, низ, право, лево</returns>
        public static double[,] GetSndMomentInertia(List<NdmArea> ndmAreas, double modulus, double[] center = null)
        {
            //Если центр тяжести не передан, находим центр тяжести
            if (center is null) center = GetGravityCenter(ndmAreas);
            //Массив для расстояний от центра тяжести сечения до центра тяжести каждого участка
            double[,] distances = new double[2, 2];
            foreach (NdmArea ndmArea in ndmAreas)
            {
                double delta_b = 0, delta_h = 0;
                if (ndmArea is NdmRectangleArea)
                {
                    NdmRectangleArea ndmRectangleArea = ndmArea as NdmRectangleArea;
                    delta_b = ndmRectangleArea.Width / 2;
                    delta_h = ndmRectangleArea.Length / 2;
                }
                distances[0, 0] = Math.Max(ndmArea.CenterY + delta_h, distances[0, 0]);
                distances[0, 1] = Math.Min(ndmArea.CenterY - delta_h, distances[0, 1]);
                distances[1, 0] = Math.Max(ndmArea.CenterX + delta_b, distances[1, 0]);
                distances[1, 1] = Math.Min(ndmArea.CenterX - delta_b, distances[1, 1]);
            }
            //Вычисляем относительно центра тяжести
            distances[0, 0] -= center[1];
            distances[0, 1] -= center[1];
            distances[1, 0] -= center[0];
            distances[1, 1] -= center[0];
            //Получаем моменты инерции
            double[] momentInertia = GetMomentInertia(ndmAreas, modulus, center);
            //Массив моментов сопротивления
            double[,] moments = new double[2, 2];
            //Относительно X - верх, низ
            moments[0, 0] = momentInertia[0] / distances[0, 0];
            moments[0, 1] = momentInertia[0] / distances[0, 1];
            //Относительно Y - право, лево
            moments[1, 0] = momentInertia[1] / distances[1, 0];
            moments[1, 1] = momentInertia[1] / distances[1, 1];
            return moments;
        }
    }
}
