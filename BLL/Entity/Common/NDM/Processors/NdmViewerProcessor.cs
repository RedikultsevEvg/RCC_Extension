using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestList03;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.MeasureUnits;

namespace RDBLL.Entity.Common.NDM.Processors
{
    /// <summary>
    /// Процессор отображения изополей напряжений и деформаций по элементарным участкам НДМ
    /// </summary>
    public static class NdmViewerProcessor
    {
        /// <summary>
        /// Возвращает коллекцию прямоугольников для отображения во вьювере напряжений
        /// </summary>
        /// <param name="forceCurvatures">Список наборов нагрузок и кривизн</param>
        /// <param name="ndmAreas">Список элементарных участков</param>
        /// <param name="code">0 - 1 группа ПС, 1 - 2 группа ПС</param>
        /// <param name="index">0 - деформации, 1 - напряжения</param>
        /// <returns></returns>
        public static List<LoadCaseRectangleValue> GetSressStrainValues(List<ForceCurvature> forceCurvatures, List<NdmArea> ndmAreas, int code, int index)
        {
            double sizeCoff = 100;
            double valCoff = 1;
            if (index == 1) valCoff = MeasureUnitConverter.GetCoefficient(3);
            List<LoadCaseRectangleValue> loadCaseRectangleValues = new List<LoadCaseRectangleValue>();
            int id = 1;
            foreach (ForceCurvature forceCurvature in forceCurvatures)
            {
                LoadCaseRectangleValue loadCaseRectangleValue = new LoadCaseRectangleValue();
                loadCaseRectangleValue.LoadCase = new LoadSet();
                loadCaseRectangleValue.LoadCase.Id = id;
                loadCaseRectangleValue.LoadCase.Name =  $"{id}: {forceCurvature.LoadSet.Name}";
                if (index == 1)
                {
                    loadCaseRectangleValue.LoadCase.Name += ", " + MeasureUnitConverter.GetUnitLabelText(3);
                }
                foreach (NdmArea ndmArea in ndmAreas)
                {
                    if (ndmArea is NdmRectangleArea)
                    {
                        NdmRectangleArea ndmRectangleArea = ndmArea as NdmRectangleArea;
                        RectangleValue aRV = new RectangleValue();
                        aRV.CenterX = ndmRectangleArea.CenterX * sizeCoff;
                        aRV.CenterY = ndmRectangleArea.CenterY * sizeCoff;
                        aRV.Width = ndmRectangleArea.Width * sizeCoff;
                        aRV.Length = ndmRectangleArea.Length * sizeCoff;
                        if (code == 1)
                        {
                            aRV.Value = NdmAreaProcessor.GetStrainFromCuvature(ndmArea, forceCurvature.CrcCurvature)[index];
                        }
                        else
                        {
                            aRV.Value = NdmAreaProcessor.GetStrainFromCuvature(ndmArea, forceCurvature.DesignCurvature)[index];
                        }
                        aRV.Value = Math.Round(aRV.Value * valCoff, 3);
                        loadCaseRectangleValue.RectangleValues.Add(aRV);
                    }
                }
                loadCaseRectangleValues.Add(loadCaseRectangleValue);
                id++;
            }
            return loadCaseRectangleValues;
        }

        public static List<LoadCaseRectangleValue> GetSressValues(List<ForceCurvature> forceCurvatures, List<NdmArea> ndmAreas, int code)
        {
            return GetSressStrainValues(forceCurvatures, ndmAreas, code, 1);
        }

        public static List<LoadCaseRectangleValue> GetSrainValues(List<ForceCurvature> forceCurvatures, List<NdmArea> ndmAreas, int code)
        {
            return GetSressStrainValues(forceCurvatures, ndmAreas, code, 0);
        }
    }
}
