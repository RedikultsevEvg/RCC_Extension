using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM.Processors
{
    /// <summary>
    /// Процессор элементарных участков
    /// </summary>
    public static class NdmAreaProcessor
    {
        /// <summary>
        /// Возвращает массив пары деформации-напряжения по элементарному участку и кривизне
        /// </summary>
        /// <param name="ndmArea">Элементарный участок</param>
        /// <param name="curvature">Кривизна</param>
        /// <returns>Пара напряжения-деформации</returns>
        public static double[] GetStrainFromCuvature(NdmArea ndmArea, Curvature curvature)
        {
            double strain = ndmArea.CenterY * curvature.CurvMatrix[0, 0];
            strain += ndmArea.CenterX * curvature.CurvMatrix[1, 0];
            strain += curvature.CurvMatrix[2, 0];
            double stress = ndmArea.GetSecantModulus(strain) * strain;
            return new double[2] { strain, stress }; 
        }

        /// <summary>
        /// Возвращает коллекцию прямоугольных элементарных участков по модели материалов, заданному прямоугольнику, размеру элемента
        /// </summary>
        /// <param name="materialModel">Модель материала</param>
        /// <param name="width">Ширина исходного участка</param>
        /// <param name="length">Длина исходного участка</param>
        /// <param name="centerX">Центр участка по X</param>
        /// <param name="centerY">Центр участка по Y</param>
        /// <param name="elementSize">Максимальнй размер элемента</param>
        /// <returns>Коллекция прямоугольных участков</returns>
        public static List<NdmRectangleArea> MeshRectangle(IMaterialModel materialModel, double width, double length, double centerX, double centerY, double elementSize = 0.02)
        {
            List<NdmRectangleArea> ndmRectangleAreas = new List<NdmRectangleArea>();
            //Определяем количество делений по каждой стороне
            int numX = Convert.ToInt32(Math.Ceiling(width / elementSize));
            int numY = Convert.ToInt32(Math.Ceiling(length / elementSize));
            //В любом случае участок делится не менее чем на 10 элементов по каждой стороне
            if (numX < 10) { numX = 10; }
            if (numY < 10) { numY = 10; }
            //Шаг элементарных участков (совпадает с соответствующим размером участка)
            double stepX = width / numX;
            double stepY = length / numY;
            //Определяем положение первого участка по каждому из направлений
            double startCenterX = centerX - width / 2 + stepX / 2;
            double startCenterY = centerY - length / 2 + stepY / 2;

            for (int i = 0; i < numX; i++)
            {
                for (int j = 0; j < numY; j++)
                {
                    NdmRectangleArea subPart;
                    subPart = new NdmRectangleArea(materialModel);
                    subPart.Width = stepX;
                    subPart.Length = stepY;
                    subPart.CenterX = startCenterX + stepX * i;
                    subPart.CenterY = startCenterY + stepY * j;
                    ndmRectangleAreas.Add(subPart);
                }
            }
            //Возвращаем коллекцию элементов
            return ndmRectangleAreas;
        }
    }
}
