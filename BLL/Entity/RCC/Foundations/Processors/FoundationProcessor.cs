using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.Common.NDM.Interfaces;
using RDBLL.Entity.Common.NDM.MaterialModels;
using RDBLL.Entity.Common.NDM.Processors;
using RDBLL.Forces;
using RDBLL.Processors.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RDBLL.Entity.RCC.Foundations.Processors
{
    /// <summary>
    /// Процессор фундамента
    /// </summary>
    public class FoundationProcessor
    {
        /// <summary>
        /// Возвращает габаритные размеры фундамента
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns></returns>
        public static double[] GetContourSize(Foundation foundation)
        {
            double[] sizes = new double[3] { 0, 0, 0 };
            if (foundation.Parts.Count>0)
            {
                //Размеры в плане принимаем по самой нижней ступени
                FoundationPart foundationPart = foundation.Parts[foundation.Parts.Count - 1];
                sizes[0] = foundationPart.Width;
                sizes[1] = foundationPart.Length;

                //Высоту определяем как сумму высот ступеней
                foreach (FoundationPart foundationPartH in foundation.Parts)
                {
                    sizes[2] += foundationPartH.Height;
                }

            }
            return sizes;
        }
        /// <summary>
        /// Получает геометрические характеристики подошвы фундамента
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns></returns>
        public static double[] GetBtmGeometryProperties(Foundation foundation)
        {
            double foundationWidth = FoundationProcessor.GetContourSize(foundation)[0];
            double foundationLength = FoundationProcessor.GetContourSize(foundation)[1];
            double Area = foundationWidth * foundationLength;
            double Wx = foundationWidth * foundationLength * foundationLength / 6;
            double Wy = foundationWidth * foundationWidth * foundationLength / 6;
            double Ix = foundationWidth * foundationLength * foundationLength * foundationLength / 12;
            double Iy = foundationWidth * foundationWidth * foundationWidth * foundationLength / 12;

            double[] properties = new double[7] { foundationWidth, foundationLength, Area, Wx, Wy, Ix, Iy };
            return properties;
        }
        /// <summary>
        /// Возвращает тройку дистанций от верха фундамента до центра нижней ступени
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns></returns>
        public static double[] GetDeltaDistance(Foundation foundation, bool isPositive = true)
        {
            double[] delta = new double[3] { 0, 0, 0 };
            int partCount = foundation.Parts.Count;
            if (partCount == 0) { return delta; }
            //Получаем X и Y как расстояние до центра самой нижней ступени
            delta[0] = foundation.Parts[partCount - 1].CenterX;
            delta[1] = foundation.Parts[partCount - 1].CenterY;
            //Сдвижку по Z получаем как суммарную высоту фундамента
            foreach (FoundationPart foundationPart in foundation.Parts)
            {
                //Знак - принимаем, так как фундамент располагается вниз от верхней точки
                delta[2] -= foundationPart.Height;
            }
            if (! isPositive)
            {
                delta[0] = (-1D) * delta[0];
                delta[1] = (-1D) * delta[1];
                delta[2] = (-1D) * delta[2];
            }
            return delta;
        }
        /// <summary>
        /// Основной решатель фундамента
        /// </summary>
        /// <param name="foundation"></param>
        public static void SolveFoundation(Foundation foundation)
        {
            if (foundation.Parts.Count == 0) { throw new Exception("Не заданы ступени фундамента"); }
            if (!foundation.IsLoadCasesActual || !foundation.IsPartsActual)
            {
                if (!foundation.IsLoadCasesActual)
                {
                    foundation.LoadCases = LoadSetProcessor.GetLoadCases(foundation.ForcesGroups);
                    //Загружения с учетом веса фундамента и грунта
                    foundation.btmLoadSetsWithWeight = GetBottomLoadCasesWithWeight(foundation);
                    //Загружения без учета веса фундамента и грунта
                    if (foundation.LoadCases == null) { foundation.LoadCases = new ObservableCollection<LoadSet>(); }
                    double[] delta = GetDeltaDistance(foundation);
                    foundation.btmLoadSetsWithoutWeight = LoadSetProcessor.GetLoadSetsTransform(foundation.LoadCases, delta);
                    foundation.IsLoadCasesActual = true;
                }
                foundation.IsPartsActual = true;

                foundation.NdmAreas = GetNdmAreas(foundation);
                foundation.ForceCurvaturesWithoutWeight = GetForceCurvatures(foundation, foundation.btmLoadSetsWithoutWeight);
                foundation.ForceCurvaturesWithWeight = GetForceCurvatures(foundation, foundation.btmLoadSetsWithWeight);
            }
        }
        /// <summary>
        /// Возвращает группу нагрузок с учетом веса фундамент и грунта на его уступах
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns></returns>
        public static ForcesGroup GetFoundationWeight(Foundation foundation)
        {
            double[] Nz = new double[2] { 0, 0 };
            double PartialSafetyFactor = 1.2;
            Nz[0] -= GetConcreteVolume(foundation) * foundation.ConcreteVolumeWeight;
            Nz[0] -= GetSoilVolume(foundation) * foundation.SoilVolumeWeight;
            Nz[0] -= foundation.ConcreteFloorLoad * GetContourSize(foundation)[0] * GetContourSize(foundation)[1];
            Nz[1] -= GetConcreteVolume(foundation) * foundation.ConcreteVolumeWeight * 1.1;
            Nz[1] -= GetSoilVolume(foundation) * foundation.SoilVolumeWeight * 1.2;
            Nz[1] -= foundation.ConcreteFloorLoad * foundation.ConcreteFloorLoadFactor * GetContourSize(foundation)[0] * GetContourSize(foundation)[1];

            if (Nz[0] != 0 & Nz[0] != 0) { PartialSafetyFactor = Math.Round(Nz[1] / Nz[0], 3); }
            ForcesGroup forcesGroup  = new ForcesGroup();
            LoadSet loadSet = new LoadSet(forcesGroup);
            forcesGroup.LoadSets.Add(loadSet);
            loadSet.Name = "Вес фундамента и грунта на уступах";
            loadSet.PartialSafetyFactor = PartialSafetyFactor;
            loadSet.IsLiveLoad = false;
            ForceParameter newForceParameter = new ForceParameter(loadSet);
            loadSet.ForceParameters.Add(newForceParameter);
            newForceParameter.KindId = 1;
            newForceParameter.CrcValue = Nz[0];
            newForceParameter.DesignValue = Nz[1];

            if (!(foundation.FloorLoad == 0))
            {
                Nz = new double[2] { 0, 0 };
                Nz[0] -= foundation.FloorLoad * GetContourSize(foundation)[0] * GetContourSize(foundation)[1];
                Nz[1] -= foundation.FloorLoad * foundation.FloorLoadFactor * GetContourSize(foundation)[0] * GetContourSize(foundation)[1];
                LoadSet liveLoadSet = new LoadSet(forcesGroup);
                forcesGroup.LoadSets.Add(liveLoadSet);
                liveLoadSet.Name = "Нагрузка на пол";
                liveLoadSet.PartialSafetyFactor = foundation.FloorLoadFactor;
                liveLoadSet.IsLiveLoad = false;
                ForceParameter newLiveForceParameter = new ForceParameter(liveLoadSet);
                liveLoadSet.ForceParameters.Add(newLiveForceParameter);
                newLiveForceParameter.KindId = 1;
                newLiveForceParameter.CrcValue = Nz[0];
                newLiveForceParameter.DesignValue = Nz[1];
            }
            return forcesGroup;
        }

        /// <summary>
        /// Возвращает объем фундамента
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns></returns>
        public static double GetConcreteVolume(Foundation foundation)
        {
            double volume = 0;
            foreach (FoundationPart foundationPart in foundation.Parts)
            {
                volume += foundationPart.Width * foundationPart.Length * foundationPart.Height;
            }
            return volume;
        }

        /// <summary>
        /// Возвращает объем грунта на уступах фундамента
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns></returns>
        public static double GetSoilVolume(Foundation foundation)
        {
            double volume = 0;
            double[] sizes = GetContourSize(foundation);
            volume = sizes[0] * sizes[1] * sizes[2] - GetConcreteVolume(foundation);
            return volume;
        }

        /// <summary>
        /// Возвращает усилия к подошве фундамента с учетом веса фундамента и грунта на его уступах
        /// </summary>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static ObservableCollection<LoadSet> GetBottomLoadCasesWithWeight(Foundation foundation)
        {
            ObservableCollection<ForcesGroup> forcesGroups = new ObservableCollection<ForcesGroup>();
            double[] delta = GetDeltaDistance(foundation);

            ForcesGroup forcesGroupSelfWeight = GetFoundationWeight(foundation);
            //Приводим нагрузку от веса фундамента от центра нижней ступени к точке приложения остальных нагрузок
            //точка с координатами 0,0,0
            double[] deltaNegative = GetDeltaDistance(foundation, false);
            forcesGroupSelfWeight.LoadSets = LoadSetProcessor.GetLoadSetsTransform(forcesGroupSelfWeight.LoadSets, deltaNegative);
            forcesGroups.Add(forcesGroupSelfWeight);
            //Добавляем остальные нагрузки
            foreach (ForcesGroup forcesGroup in foundation.ForcesGroups)
            {
                forcesGroups.Add(forcesGroup);
            }            
            ObservableCollection<LoadSet> loadSets = LoadSetProcessor.GetLoadCases(forcesGroups);
            //Приводим комбинацию нагрузок к подошве фундамента
            loadSets = LoadSetProcessor.GetLoadSetsTransform(loadSets, delta);
            return loadSets;
        }

        /// <summary>
        /// Возвращает коллекцию кривизн по коллекции комбинаций нагрузок
        /// </summary>
        /// <param name="foundation"></param>
        /// <param name="loadSets"></param>
        /// <returns></returns>
        public static List<ForceCurvature> GetForceCurvatures(Foundation foundation, ObservableCollection<LoadSet> loadSets)
        {

            List<ForceCurvature> forceCurvatures = new List<ForceCurvature>();
            

            foreach (LoadSet loadSet in loadSets)
            {
                ForceCurvature forceCurvature = new ForceCurvature(loadSet);
                SumForces crcSumForces = new SumForces(loadSet, false);
                SumForces designSumForces = new SumForces(loadSet, true);
                forceCurvature.CrcCurvature = NdmProcessor.GetCurvature(crcSumForces, foundation.NdmAreas);
                forceCurvature.DesignCurvature = NdmProcessor.GetCurvature(designSumForces, foundation.NdmAreas);
                forceCurvatures.Add(forceCurvature);
            }
            return forceCurvatures;
        }
        /// <summary>
        /// Возвращает коллекцию элементарных участков для подошвы фундамента
        /// </summary>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static List<NdmArea> GetNdmAreas(Foundation foundation)
        {
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            double[] sizes = GetContourSize(foundation);
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangle(materialModel, sizes[0], sizes[1], 0, 0, 0.05);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            foreach (NdmRectangleArea ndmRectangleArea in ndmRectangleAreas)
            {
                ndmAreas.Add(ndmRectangleArea);
            }
            return ndmAreas;
        }
        /// <summary>
        /// Возвращает смещение центра нижней ступени фундамента по горизонтали
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns>Массив из двух чисел - смещение по X и Y</returns>
        public static double[] GetFoundationCenter(Foundation foundation)
        {
            double[] delta = GetDeltaDistance(foundation);
            return new double[2] { delta[0], delta[1]};
        }

        public static List<double[]> GetFoundationMidllePoints(Foundation foundation)
        {
            List<double[]> points = new List<double[]>();
            FoundationPart foundationPart = foundation.Parts[foundation.Parts.Count - 1];
            double[] point1 = new double[2] { foundationPart.Width / 2, foundationPart.CenterY };
            double[] point2 = new double[2] { (-1D) * foundationPart.Width / 2, foundationPart.CenterY };
            double[] point3 = new double[2] { foundationPart.CenterX, foundationPart.Length / 2 };
            double[] point4 = new double[2] { foundationPart.CenterX, (-1D) * foundationPart.Length / 2 };
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);
            points.Add(point4);
            return points;
        }

        public static List<double[]> GetFoundationCornerPoints(Foundation foundation)
        {
            List<double[]> points = new List<double[]>();
            FoundationPart foundationPart = foundation.Parts[foundation.Parts.Count - 1];
            double[] point1 = new double[2] { foundationPart.Width / 2, foundationPart.Length / 2 };
            double[] point2 = new double[2] { (-1D) * foundationPart.Width / 2, (-1D) * foundationPart.Length / 2 };
            double[] point3 = new double[2] { foundationPart.Width / 2, (-1D) * foundationPart.Length / 2 };
            double[] point4 = new double[2] { (-1D) * foundationPart.Width / 2, foundationPart.Length / 2 };
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);
            points.Add(point4);
            return points;
        }

        /// <summary>
        /// Возвращает коллекцию срединных, краевых, угловых напряжений для фундамента
        /// </summary>
        /// <param name="foundation"></param>
        /// <param name="forceCurvatures"></param>
        /// <returns></returns>
        public static List<double[]> GetStresses(Foundation foundation, List<ForceCurvature> forceCurvatures)
        {
            double[] GetStressedAreas(List<NdmArea> ndmAreas, ForceCurvature forceCurvature)
            {
                //Площади с положительными и отрицательными напряжениями
                double crcPosArea = 0, crcNegArea = 0, designPosArea = 0, designNegArea = 0;
                foreach (NdmArea ndmArea in ndmAreas)
                {
                    double stress;
                    stress = NdmAreaProcessor.GetStrainFromCuvature(ndmArea, forceCurvature.CrcCurvature)[1];
                    if (!(stress<0)) { crcPosArea += ndmArea.Area; } else { crcNegArea += ndmArea.Area; }
                    stress = NdmAreaProcessor.GetStrainFromCuvature(ndmArea, forceCurvature.DesignCurvature)[1];
                    if (!(stress<0)) { designPosArea += ndmArea.Area; } else { designNegArea += ndmArea.Area; }
                }
                return new double[] { crcPosArea, crcNegArea, designPosArea, designNegArea};
            }
            List<double[]> stresses = new List<double[]>();
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
        
            //Получаем коллекцию краевых точек
            List<double[]> midllePoints = GetFoundationMidllePoints(foundation);
            //Получаем коллекцию угловых точек
            List<double[]> cornerPoints = GetFoundationCornerPoints(foundation);
            foreach (ForceCurvature forceCurvature in forceCurvatures)
            {

                //Вычисляем напряжения по центру нижней подошвы
                double crcCenterStress = NdmAreaProcessor.GetStrainFromCuvature(materialModel, 0, 0, forceCurvature.CrcCurvature)[1];
                double designCenterStress = NdmAreaProcessor.GetStrainFromCuvature(materialModel, 0, 0, forceCurvature.DesignCurvature)[1];
                //Создаем списки для напряжений в краевых точках
                List<double> crcMiddleSresses = new List<double>();
                List<double> designMiddleSresses = new List<double>();
                //Получаем напряжения для краевых точек
                foreach (double[] point in midllePoints)
                {
                    crcMiddleSresses.Add(NdmAreaProcessor.GetStrainFromCuvature(materialModel, point[0], point[1], forceCurvature.CrcCurvature)[1]);
                    designMiddleSresses.Add(NdmAreaProcessor.GetStrainFromCuvature(materialModel, point[0], point[1], forceCurvature.DesignCurvature)[1]);
                }
                //Создаем списки для напряжений в угловых точках
                List<double> crcCornerSresses = new List<double>();
                List<double> designCornerSresses = new List<double>();
                //Получаем напряжения для угловых точек
                foreach (double[] point in cornerPoints)
                {
                    crcCornerSresses.Add(NdmAreaProcessor.GetStrainFromCuvature(materialModel, point[0], point[1], forceCurvature.CrcCurvature)[1]);
                    designCornerSresses.Add(NdmAreaProcessor.GetStrainFromCuvature(materialModel, point[0], point[1], forceCurvature.DesignCurvature)[1]);
                }
                /*Формируем массив напряжений из 10 величин
                Среднее напряжение,
                напряжение по центру нижней подошвы,
                минимальное напряжение в краевых точках,
                минимальное напряжение в угловых точках,
                максимальное напряжение в угловых точках
                Отрыв в краевых точках определять не нужно, так как если он возникает, то он будет и в угловой точке
                */
                double[] avgStress = GetAvgStresses(foundation, forceCurvature);
                double[] areas = GetStressedAreas(foundation.NdmAreas, forceCurvature);
                double[] stress = new double[14]
                {avgStress[0], crcCenterStress, crcMiddleSresses.Min(), crcCornerSresses.Min(), crcCornerSresses.Max(),
                     avgStress[1], designCenterStress, designMiddleSresses.Min(), designCornerSresses.Min(), designCornerSresses.Max(),
                     areas[0], areas[1], areas[2], areas[3]
                };
                stresses.Add(stress);
            }
            return stresses;
        }
        /// <summary>
        /// Возвращает средние напряжения под подошвой фундамента
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <param name="forceCurvature">Кривизна</param>
        /// <returns></returns>
        public static double[] GetAvgStresses(Foundation foundation, ForceCurvature forceCurvature)
        {

            double Area = GetBtmGeometryProperties(foundation)[2];
            //Вычисляем средние напряжения
            double crcAvgStress = forceCurvature.CrcSumForces.ForceMatrix[2, 0] / Area;
            double designAvgStress = forceCurvature.DesignSumForces.ForceMatrix[2, 0] / Area;
            double[] stress = new double[2] { crcAvgStress, designAvgStress};
            return stress;
        }
    }
}
