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
                sizes[0] = foundationPart.Width + Math.Abs(foundationPart.CenterX)*2;
                sizes[1] = foundationPart.Length + Math.Abs(foundationPart.CenterY)*2;

                //Высоту определяем как сумму высот ступеней
                foreach (FoundationPart foundationPartH in foundation.Parts)
                {
                    sizes[2] += foundationPartH.Height;
                }

            }
            return sizes;
        }
        /// <summary>
        /// Возвращает тройку дистанций от верха фундамента до центра нижней ступени
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns></returns>
        public static double[] GetDeltaDistance(Foundation foundation)
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
            return delta;
        }
        /// <summary>
        /// Основной решатель фундамента
        /// </summary>
        /// <param name="foundation"></param>
        public static void SolveFoundation(Foundation foundation)
        {
            if (!foundation.IsLoadCasesActual || !foundation.IsPartsActual)
            {
                if (!foundation.IsLoadCasesActual)
                {
                    foundation.LoadCases = LoadSetProcessor.GetLoadCases(foundation.ForcesGroups);
                    //Загружения с учетом веса фундамента и грунта
                    foundation.btmLoadSetsWithWeight = GetBottomLoadCasesWithWeight(foundation);
                    //Загружения без учета веса фундамента и грунта
                    foundation.btmLoadSetsWithoutWeight = foundation.LoadCases;
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

            forcesGroups.Add(GetFoundationWeight(foundation));
            foreach (ForcesGroup forcesGroup in foundation.ForcesGroups)
            {
                forcesGroups.Add(forcesGroup);
            }            
            ObservableCollection<LoadSet> loadSets = LoadSetProcessor.GetLoadCases(forcesGroups);
            return loadSets;
        }

        /// <summary>
        /// Возвращает коллекцию кривизн по коллекции комбинаций нагрузок
        /// </summary>
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
            double centerX = foundation.Parts[foundation.Parts.Count - 1].CenterX;
            double centerY = foundation.Parts[foundation.Parts.Count - 1].CenterY;
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangle(materialModel, sizes[0], sizes[1], centerX, centerY, 0.05);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            foreach (NdmRectangleArea ndmRectangleArea in ndmRectangleAreas)
            {
                ndmAreas.Add(ndmRectangleArea);
            }
            return ndmAreas;
        }

        public static double[] GetFoundationCenter(Foundation foundation)
        {
            double[] delta = GetDeltaDistance(foundation);
            return new double[2] { delta[0], delta[1]};
        }

        public static List<double[]> GetFoundationMidllePoints(Foundation foundation)
        {
            List<double[]> points = new List<double[]>();
            FoundationPart foundationPart = foundation.Parts[foundation.Parts.Count - 1];
            double[] point1 = new double[2] { foundationPart.CenterX + foundationPart.Width / 2, foundationPart.CenterY };
            double[] point2 = new double[2] { foundationPart.CenterX - foundationPart.Width / 2, foundationPart.CenterY };
            double[] point3 = new double[2] { foundationPart.CenterX, foundationPart.CenterY + foundationPart.Length / 2 };
            double[] point4 = new double[2] { foundationPart.CenterX, foundationPart.CenterY - foundationPart.Length / 2 };
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
            double[] point1 = new double[2] { foundationPart.CenterX + foundationPart.Width / 2, foundationPart.CenterY + foundationPart.Length / 2 };
            double[] point2 = new double[2] { foundationPart.CenterX - foundationPart.Width / 2, foundationPart.CenterY - foundationPart.Length / 2 };
            double[] point3 = new double[2] { foundationPart.CenterX + foundationPart.Width / 2, foundationPart.CenterY - foundationPart.Length / 2 };
            double[] point4 = new double[2] { foundationPart.CenterX - foundationPart.Width / 2, foundationPart.CenterY + foundationPart.Length / 2 };
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);
            points.Add(point4);
            return points;
        }

        /// <summary>
        /// Возвращает коллекцию срединных, краевых, угловых напряжения для фундамента
        /// </summary>
        /// <param name="foundation"></param>
        /// <param name="forceCurvatures"></param>
        /// <returns></returns>
        public static List<double[]> GetStresses(Foundation foundation, List<ForceCurvature> forceCurvatures)
        {
            List<double[]> stresses = new List<double[]>();
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            double[] centerPoint = GetFoundationCenter(foundation);
            List<double[]> midllePoints = GetFoundationMidllePoints(foundation);
            List<double[]> cornerPoints = GetFoundationCornerPoints(foundation);
            foreach (ForceCurvature forceCurvature in forceCurvatures)
            {
                double crcCenterStress = NdmAreaProcessor.GetStrainFromCuvature(materialModel, centerPoint[0], centerPoint[1], forceCurvature.CrcCurvature)[1];
                double designCenterStress = NdmAreaProcessor.GetStrainFromCuvature(materialModel, centerPoint[0], centerPoint[1], forceCurvature.DesignCurvature)[1];
                List<double> crcMiddleSresses = new List<double>();
                List<double> designMiddleSresses = new List<double>();
                foreach (double[] point in midllePoints)
                {
                    crcMiddleSresses.Add(NdmAreaProcessor.GetStrainFromCuvature(materialModel, point[0], point[1], forceCurvature.CrcCurvature)[1]);
                    designMiddleSresses.Add(NdmAreaProcessor.GetStrainFromCuvature(materialModel, point[0], point[1], forceCurvature.DesignCurvature)[1]);
                }
                List<double> crcCornerSresses = new List<double>();
                List<double> designCornerSresses = new List<double>();
                foreach (double[] point in cornerPoints)
                {
                    crcCornerSresses.Add(NdmAreaProcessor.GetStrainFromCuvature(materialModel, point[0], point[1], forceCurvature.CrcCurvature)[1]);
                    designCornerSresses.Add(NdmAreaProcessor.GetStrainFromCuvature(materialModel, point[0], point[1], forceCurvature.DesignCurvature)[1]);
                }
                double[] stress = new double[8]
                { crcCenterStress, crcMiddleSresses.Min(), crcCornerSresses.Min(), crcCornerSresses.Max(),
                    designCenterStress, designMiddleSresses.Min(), designCornerSresses.Min(), designCornerSresses.Max()
                };
                stresses.Add(stress);
            }
            return stresses;
        }
    }
}
