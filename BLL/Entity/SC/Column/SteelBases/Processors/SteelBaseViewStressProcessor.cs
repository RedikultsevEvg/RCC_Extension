﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestList03;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.Common.NDM.Processors;
using RDBLL.Entity.SC.Column;
using System.Windows;
using RDBLL.Processors.SC;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Common.Interfaces.Shapes;

namespace RDBLL.Entity.SC.Column.SteelBases.Processors
{
    /// <summary>
    /// Процессор просмотра результатов расчета стальной базы
    /// </summary>
    public static class SteelBaseViewProcessor
    {
        /// <summary>
        /// Отображение изополя напряжений в бетоне подливки стальной базы
        /// </summary>
        /// <param name="steelBase"></param>
        public static void ShowStress(SteelBase steelBase)
        {
            //Создаем список кривизн и элементарных участков
            List<ForceCurvature> forceCurvatures = new List<ForceCurvature>();
            List<NdmArea> ndmAreas = steelBase.NdmAreas;
            //Получаем основную кривизну из двойной
            foreach (ForceDoubleCurvature forceDoubleCurvature in steelBase.ForceCurvatures)
            {
                forceCurvatures.Add(forceDoubleCurvature);
            }
            List<LoadCaseRectangleValue> loadCaseRectangleValues = NdmViewerProcessor.GetSressValues(forceCurvatures, ndmAreas, 2);
            //Показываем окно с напряжениями
            Window wndMain = new TestList03.MainWindow(loadCaseRectangleValues);
            wndMain.Show();
        }
        /// <summary>
        /// Отображение напряжений в участках базы стальной колонны
        /// </summary>
        /// <param name="steelBase"></param>
        public static void ShowPartStress(SteelBase steelBase)
        {
            double zoomCoff = 100;
            double sizeCoff = MeasureUnitConverter.GetCoefficient(0);
            double stressCoff = MeasureUnitConverter.GetCoefficient(3);
            double actualThickness = steelBase.Height;
            double steelStrength = (steelBase.Steel.MaterialKind as SteelKind).FstStrength;

            List<LoadCaseRectangleValue> loadCaseRectangleValues = new List<LoadCaseRectangleValue>();
            #region Stresses
            LoadCaseRectangleValue StressesValue = new LoadCaseRectangleValue();
            StressesValue.LoadCase = new LoadSet();
            StressesValue.LoadCase.Id = 1;
            StressesValue.LoadCase.Name = $"Напряжения_MAX, {MeasureUnitConverter.GetUnitLabelText(3)}";
            foreach (SteelBasePart steelBasePart in steelBase.SteelBaseParts)
            {
                double maxBedStress = SteelBasePartProcessor.GetGlobalMinStressNonLinear(steelBasePart) * (-1D);
                double maxMoment = SteelBasePartProcessor.GetMoment(steelBasePart, maxBedStress);
                double thickness = (steelBasePart.ParentMember as IHasHeight).Height;
                double maxStress = SteelBasePartProcessor.GetPlateStress(maxMoment, thickness);
                RectangleValue aRV = new RectangleValue();
                aRV.CenterX = steelBasePart.Center.X * zoomCoff;
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //Поправка с учетом ошибки во вьювере
                aRV.CenterY = steelBasePart.Center.Y * zoomCoff * (-1.0);
                aRV.Width = steelBasePart.Width * zoomCoff;
                aRV.Length = steelBasePart.Length * zoomCoff;
                aRV.Value = maxStress;
                aRV.Value = Math.Round(aRV.Value * stressCoff, 3);
                StressesValue.RectangleValues.Add(aRV);
            }
            loadCaseRectangleValues.Add(StressesValue);
            #endregion
            #region Thicknesses
            LoadCaseRectangleValue ThicknessValue = new LoadCaseRectangleValue();
            ThicknessValue.LoadCase = new LoadSet();
            ThicknessValue.LoadCase.Id = 2;
            ThicknessValue.LoadCase.Name = $"Рекомендуемая толщина, {MeasureUnitConverter.GetUnitLabelText(0)}";
            foreach (SteelBasePart steelBasePart in steelBase.SteelBaseParts)
            {
                double maxBedStress = SteelBasePartProcessor.GetGlobalMinStressNonLinear(steelBasePart) * (-1D);
                double maxMoment = SteelBasePartProcessor.GetMoment(steelBasePart, maxBedStress);
                double thickness = (steelBasePart.ParentMember as IHasHeight).Height;
                double maxStress = SteelBasePartProcessor.GetPlateStress(maxMoment, thickness);
                double recomendedThickness = 0;
                if (maxStress > 0) { recomendedThickness = actualThickness * Math.Pow((maxStress / steelStrength), 0.5); }
                RectangleValue aRV = new RectangleValue();
                aRV.CenterX = steelBasePart.Center.X * zoomCoff;
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //Поправка с учетом ошибки во вьювере
                aRV.CenterY = steelBasePart.Center.Y * zoomCoff * (-1);
                aRV.Width = steelBasePart.Width * zoomCoff;
                aRV.Length = steelBasePart.Length * zoomCoff;
                aRV.Value = Math.Round(recomendedThickness * sizeCoff, 3);
                ThicknessValue.RectangleValues.Add(aRV);
            }
            loadCaseRectangleValues.Add(ThicknessValue);
            #endregion
            Window wndMain = new TestList03.MainWindow(loadCaseRectangleValues);
            wndMain.Show();
        }
    }
}
