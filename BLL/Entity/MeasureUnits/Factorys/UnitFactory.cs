using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.MeasureUnits.Factorys
{
    public static class UnitFactory
    {
        public static void GetMeasureUnits(ref ObservableCollection<MeasureUnit> measureUnits, ref List<ForceParamKind> forceParamKinds)
        {
            #region Исходные данные единиц измерения
            #region
            MeasureUnit measureUnitLength = new MeasureUnit();
            measureUnitLength.MeasureUnitKind = "Линейные размеры";
            measureUnitLength.UnitLabels.Add(new MeasureUnitLabel { Id = 1, UnitName = "м", AddKoeff = 1.0 });
            measureUnitLength.UnitLabels.Add(new MeasureUnitLabel { Id = 2, UnitName = "мм", AddKoeff = 1000 });
            measureUnitLength.UnitLabels.Add(new MeasureUnitLabel { Id = 3, UnitName = "см", AddKoeff = 100 });
            measureUnitLength.UnitLabels.Add(new MeasureUnitLabel { Id = 4, UnitName = "дм", AddKoeff = 10 });
            measureUnitLength.UnitLabels.Add(new MeasureUnitLabel { Id = 5, UnitName = "км", AddKoeff = 0.1 });
            measureUnitLength.CurrentUnitLabelId = 2;
            #endregion
            #region
            MeasureUnit measureUnitForce = new MeasureUnit();
            measureUnitForce.MeasureUnitKind = "Усилия";
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 6, UnitName = "Н", AddKoeff = 1.0 });
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 7, UnitName = "кН", AddKoeff = 0.001 });
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 8, UnitName = "МН", AddKoeff = 0.000001 });
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 9, UnitName = "кгс", AddKoeff = 1 / 9.81 });
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 10, UnitName = "тс", AddKoeff = 0.001 / 9.81 });
            measureUnitForce.CurrentUnitLabelId = 7;
            #endregion
            #region
            MeasureUnit measureUnitMoment = new MeasureUnit();
            measureUnitMoment.MeasureUnitKind = "Изгибающие моменты";
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 11, UnitName = "Н*м", AddKoeff = 1.0 });
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 12, UnitName = "кН*м", AddKoeff = 0.001 });
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 13, UnitName = "МН*м", AddKoeff = 0.000001 });
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 14, UnitName = "кгс*м", AddKoeff = 1 / 9.81 });
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 15, UnitName = "тс*м", AddKoeff = 0.001 / 9.81 });
            measureUnitMoment.CurrentUnitLabelId = 12;
            #endregion
            #region
            MeasureUnit measureUnitStress = new MeasureUnit();
            measureUnitStress.MeasureUnitKind = "Напряжения";
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 16, UnitName = "Па", AddKoeff = 1.0 });
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 17, UnitName = "кПа", AddKoeff = 0.001 });
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 18, UnitName = "МПа", AddKoeff = 0.000001 });
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 19, UnitName = "кгс/см^2", AddKoeff = 0.0001 / 9.81 });
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 20, UnitName = "тс/м^2", AddKoeff = 0.001 / 9.81 });
            measureUnitStress.CurrentUnitLabelId = 18;
            #endregion
            #region
            MeasureUnit measureUnitGeometryArea = new MeasureUnit();
            measureUnitGeometryArea.MeasureUnitKind = "Геометрия. Площадь";
            measureUnitGeometryArea.UnitLabels.Add(new MeasureUnitLabel { Id = 21, UnitName = "м^2", AddKoeff = 1.0 });
            measureUnitGeometryArea.UnitLabels.Add(new MeasureUnitLabel { Id = 22, UnitName = "мм^2", AddKoeff = 1000000 });
            measureUnitGeometryArea.UnitLabels.Add(new MeasureUnitLabel { Id = 23, UnitName = "см^2", AddKoeff = 10000 });
            measureUnitGeometryArea.CurrentUnitLabelId = 22;
            #endregion
            #region
            MeasureUnit measureUnitGeometrySecMoment = new MeasureUnit();
            measureUnitGeometrySecMoment.MeasureUnitKind = "Геометрия. Момент сопротивления";
            measureUnitGeometrySecMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 26, UnitName = "м^3", AddKoeff = 1.0 });
            measureUnitGeometrySecMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 27, UnitName = "мм^3", AddKoeff = 1e9 });
            measureUnitGeometrySecMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 28, UnitName = "см^3", AddKoeff = 1e6 });
            measureUnitGeometrySecMoment.CurrentUnitLabelId = 27;
            #endregion
            #region
            MeasureUnit measureUnitGeometryMoment = new MeasureUnit();
            measureUnitGeometryMoment.MeasureUnitKind = "Геометрия. Момент инерции";
            measureUnitGeometryMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 31, UnitName = "м^4", AddKoeff = 1.0 });
            measureUnitGeometryMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 32, UnitName = "мм^4", AddKoeff = 1e12 });
            measureUnitGeometryMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 33, UnitName = "см^4", AddKoeff = 1e8 });
            measureUnitGeometryMoment.CurrentUnitLabelId = 32;
            #endregion
            #region
            MeasureUnit measureUnitMass = new MeasureUnit();
            measureUnitMass.MeasureUnitKind = "Масса";
            measureUnitMass.UnitLabels.Add(new MeasureUnitLabel { Id = 36, UnitName = "кг", AddKoeff = 1.0 });
            measureUnitMass.UnitLabels.Add(new MeasureUnitLabel { Id = 37, UnitName = "г", AddKoeff = 1000 });
            measureUnitMass.UnitLabels.Add(new MeasureUnitLabel { Id = 38, UnitName = "т", AddKoeff = 0.001 });
            measureUnitMass.CurrentUnitLabelId = 36;
            #endregion
            #region
            MeasureUnit measureUnitDensity = new MeasureUnit();
            measureUnitDensity.MeasureUnitKind = "Плотность";
            measureUnitDensity.UnitLabels.Add(new MeasureUnitLabel { Id = 41, UnitName = "кг/м^3", AddKoeff = 1.0 });
            measureUnitDensity.UnitLabels.Add(new MeasureUnitLabel { Id = 42, UnitName = "г/см^3", AddKoeff = 0.001 });
            measureUnitDensity.UnitLabels.Add(new MeasureUnitLabel { Id = 43, UnitName = "т/м^3", AddKoeff = 0.001 });
            measureUnitDensity.CurrentUnitLabelId = 41;
            #endregion
            #region
            MeasureUnit measureUnitVolumeWeight = new MeasureUnit();
            measureUnitVolumeWeight.MeasureUnitKind = "Объемный вес";
            measureUnitVolumeWeight.UnitLabels.Add(new MeasureUnitLabel { Id = 45, UnitName = "Н/м^3", AddKoeff = 1.0 });
            measureUnitVolumeWeight.UnitLabels.Add(new MeasureUnitLabel { Id = 46, UnitName = "кН/м^3", AddKoeff = 0.001 });
            measureUnitVolumeWeight.UnitLabels.Add(new MeasureUnitLabel { Id = 47, UnitName = "кг/м^3", AddKoeff = 1 / 9.81 });
            measureUnitVolumeWeight.UnitLabels.Add(new MeasureUnitLabel { Id = 48, UnitName = "г/см^3", AddKoeff = 0.001 / 9.81 });
            measureUnitVolumeWeight.UnitLabels.Add(new MeasureUnitLabel { Id = 49, UnitName = "т/м^3", AddKoeff = 0.001 / 9.81 });
            measureUnitVolumeWeight.CurrentUnitLabelId = 46;
            #endregion
            #region
            MeasureUnit measureUnitSizeArea = new MeasureUnit();
            measureUnitSizeArea.MeasureUnitKind = "Размеры. Площадь";
            measureUnitSizeArea.UnitLabels.Add(new MeasureUnitLabel { Id = 51, UnitName = "м^2", AddKoeff = 1.0 });
            measureUnitSizeArea.UnitLabels.Add(new MeasureUnitLabel { Id = 52, UnitName = "мм^2", AddKoeff = 1000000 });
            measureUnitSizeArea.UnitLabels.Add(new MeasureUnitLabel { Id = 53, UnitName = "см^2", AddKoeff = 10000 });
            measureUnitSizeArea.CurrentUnitLabelId = 51;
            #endregion
            #region
            MeasureUnit measureUnitSizeVolume = new MeasureUnit();
            measureUnitSizeVolume.MeasureUnitKind = "Размеры. Объем";
            measureUnitSizeVolume.UnitLabels.Add(new MeasureUnitLabel { Id = 56, UnitName = "м^3", AddKoeff = 1.0 });
            measureUnitSizeVolume.UnitLabels.Add(new MeasureUnitLabel { Id = 57, UnitName = "мм^3", AddKoeff = 1000000000 });
            measureUnitSizeVolume.UnitLabels.Add(new MeasureUnitLabel { Id = 58, UnitName = "см^3", AddKoeff = 1000000 });
            measureUnitSizeVolume.CurrentUnitLabelId = 56;
            #endregion
            #region
            MeasureUnit measureUnitDisributedForce = new MeasureUnit();
            measureUnitDisributedForce.MeasureUnitKind = "Распределенная нагрузка на погонный метр";
            measureUnitDisributedForce.UnitLabels.Add(new MeasureUnitLabel { Id = 60, UnitName = "Н/м", AddKoeff = 1.0 });
            measureUnitDisributedForce.UnitLabels.Add(new MeasureUnitLabel { Id = 61, UnitName = "кН/м", AddKoeff = 0.001 });
            measureUnitDisributedForce.UnitLabels.Add(new MeasureUnitLabel { Id = 62, UnitName = "МН/м", AddKoeff = 0.000001 });
            measureUnitDisributedForce.UnitLabels.Add(new MeasureUnitLabel { Id = 63, UnitName = "кгс/м", AddKoeff = 1 / 9.81 });
            measureUnitDisributedForce.UnitLabels.Add(new MeasureUnitLabel { Id = 64, UnitName = "тс/м", AddKoeff = 0.001 / 9.81 });
            measureUnitDisributedForce.CurrentUnitLabelId = 61;
            #endregion
            #region
            MeasureUnit measureUnitDisributedLoad = new MeasureUnit();
            measureUnitDisributedLoad.MeasureUnitKind = "Распределенная нагрузка на квадратный метр";
            measureUnitDisributedLoad.UnitLabels.Add(new MeasureUnitLabel { Id = 70, UnitName = "Н/м^2", AddKoeff = 1.0 });
            measureUnitDisributedLoad.UnitLabels.Add(new MeasureUnitLabel { Id = 71, UnitName = "кН/м^2", AddKoeff = 0.001 });
            measureUnitDisributedLoad.UnitLabels.Add(new MeasureUnitLabel { Id = 72, UnitName = "МН/м^2", AddKoeff = 0.000001 });
            measureUnitDisributedLoad.UnitLabels.Add(new MeasureUnitLabel { Id = 73, UnitName = "кгс/м^2", AddKoeff = 1 / 9.81 });
            measureUnitDisributedLoad.UnitLabels.Add(new MeasureUnitLabel { Id = 74, UnitName = "тс/м^2", AddKoeff = 0.001 / 9.81 });
            measureUnitDisributedLoad.CurrentUnitLabelId = 71;
            #endregion
            #region
            MeasureUnit measureUnitFiltration = new MeasureUnit();
            measureUnitFiltration.MeasureUnitKind = "Коэффициент фильтрации";
            measureUnitFiltration.UnitLabels.Add(new MeasureUnitLabel { Id = 80, UnitName = "м/с", AddKoeff = 1.0 });
            measureUnitFiltration.UnitLabels.Add(new MeasureUnitLabel { Id = 81, UnitName = "м/сут", AddKoeff = 3600 });
            measureUnitFiltration.CurrentUnitLabelId = 81;
            #endregion
            #region
            measureUnits = new ObservableCollection<MeasureUnit>();
            measureUnits.Add(measureUnitLength); //0
            measureUnits.Add(measureUnitForce); //1
            measureUnits.Add(measureUnitMoment); //2
            measureUnits.Add(measureUnitStress); //3
            measureUnits.Add(measureUnitGeometryArea); //4
            measureUnits.Add(measureUnitGeometrySecMoment); //5
            measureUnits.Add(measureUnitGeometryMoment); //6
            measureUnits.Add(measureUnitMass); //7
            measureUnits.Add(measureUnitDensity); //8
            measureUnits.Add(measureUnitVolumeWeight); //9
            measureUnits.Add(measureUnitSizeArea); //10
            measureUnits.Add(measureUnitSizeVolume); //11
            measureUnits.Add(measureUnitDisributedForce); //12
            measureUnits.Add(measureUnitDisributedLoad); //13
            measureUnits.Add(measureUnitFiltration); //14
            #endregion
            #endregion
            #region Исходные данные видов нагрузки

            forceParamKinds = new List<ForceParamKind>();
            forceParamKinds.Add(new ForceParamKind
            {
                Id = 1,
                LongLabel = "Продольная сила N",
                ShortLabel = "N",
                Addition = "Положительному значению силы соответствует направление вдоль оси Z (направлена вертикально вверх)",
                MeasureUnit = measureUnitForce
            });
            forceParamKinds.Add(new ForceParamKind
            {
                Id = 2,
                LongLabel = "Изгибающий момент Mx",
                ShortLabel = "Mx",
                Addition = "За положительное значение момента принят момент против часовой стрелки если смотреть с конца оси X",
                MeasureUnit = measureUnitMoment
            });
            forceParamKinds.Add(new ForceParamKind
            {
                Id = 3,
                LongLabel = "Изгибающий момент My",
                ShortLabel = "My",
                Addition = "За положительное значение момента принят момент против часовой стрелки если смотреть с конца оси Y",
                MeasureUnit = measureUnitMoment
            });
            forceParamKinds.Add(new ForceParamKind
            {
                Id = 4,
                LongLabel = "Поперечная сила Qx",
                ShortLabel = "Qx",
                Addition = "Положительному значению силы соответствует направление вдоль оси X (направлена вправо по плану)",
                MeasureUnit = measureUnitForce
            });
            forceParamKinds.Add(new ForceParamKind
            {
                Id = 5,
                LongLabel = "Поперечная сила Qy",
                ShortLabel = "Qy",
                Addition = "Положительному значению силы соответствует направление вдоль оси Y (направлена вверх по плану)",
                MeasureUnit = measureUnitForce
            });
            forceParamKinds.Add(new ForceParamKind
            {
                Id = 6,
                LongLabel = "Крутящий момент Mz",
                ShortLabel = "Mz",
                Addition = "За положительное значение момента принят момент против часовой стрелки если смотреть с конца оси Z",
                MeasureUnit = measureUnitMoment
            });
            #endregion
        }
    }
}
