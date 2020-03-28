using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM;
using System.Windows.Forms;


namespace RDBLL.Entity.Common.NDM.Processors
{
    /// <summary>
    /// Основной процессор элементарных участков
    /// </summary>
    public static class NdmProcessor
    {
        /// <summary>
        /// Возвращает кривизну по заданным усилиям, коллекции элементарных участков, начальной кривизне
        /// </summary>
        /// <param name="sumForces">Заданные усилия</param>
        /// <param name="ndmAreas">Коллекция элементарных участков</param>
        /// <param name="curvature">Начальная кривизна (null - начальная кривизна не определена)</param>
        /// <param name="maxIterationCount"></param>
        /// <param name="accuracy"></param>
        /// <returns>Кривизна нелинейного расчета</returns>
        public static Curvature GetCurvature (SumForces sumForces, List<NdmArea> ndmAreas, Curvature curvature = null, int maxIterationCount = 20, double accuracy = 0.01)
        {
            string errorText = "Ошибка нелинейного расчета";
            StiffnessCoefficient stiffnessCoefficient = new StiffnessCoefficient(ndmAreas);
            if (curvature == null) { curvature = new Curvature(sumForces, stiffnessCoefficient); }
            //Определяем новые жесткостные коэффициенты по полученной кривизне
            StiffnessCoefficient newStiffnessCoefficient = new StiffnessCoefficient(ndmAreas, curvature);
            Curvature newCurvature = new Curvature(sumForces, newStiffnessCoefficient);
            try
            {
                SumForces sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
                int i = 0;
                double curAccuracy = double.PositiveInfinity;
                while (curAccuracy>accuracy)
                {
                    newCurvature = new Curvature(sumForces, newStiffnessCoefficient);
                    newStiffnessCoefficient = new StiffnessCoefficient(ndmAreas, newCurvature);
                    sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
                    curAccuracy = SumForcesProcessor.GetSigmaAccuracy(sumForces, sumForces2);
                    i++;
                    if (i > maxIterationCount)
                    {
                        errorText = $"Достигнуто максимальное количество итераций, актуальная точность {curAccuracy}";
                    }
                }
                sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
            }
            catch
            {
                //Пока показываем сообщени, в будущем надо будет делать нормальное окно хода нелинейного расчета
                MessageBox.Show("Проверьте исходные данные", errorText);
                //Генерируем новое исключение, для перехвата в вызывающей функции
                throw new Exception($"Nonlinear model operation error: {errorText}");
            }
            return newCurvature;
        }
    }
}
