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
using System.Windows;
using RDBLL.Entity.Soils.Processors;
using RDBLL.Entity.Soils;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Geometry;

namespace RDBLL.Entity.RCC.Foundations.Processors
{
    /// <summary>
    /// Процессор фундамента
    /// </summary>
    public class FoundationProcessor
    {
        public class SettleMentResult
        {
            public double Settlement { get; set; }
            public double CompressionHeight { get; set; }
            public double IncX { get; set; }
            public double IncY { get; set; }
            public double IncXY { get; set; }
            public double NzStiffnessMin { get; set; }
            public double MxStiffnessMin { get; set; }
            public double MyStiffnessMin { get; set; }
            public double NzStiffnessMax { get; set; }
            public double MxStiffnessMax { get; set; }
            public double MyStiffnessMax { get; set; }
            public string NzStiffnessStringMin { get; set; }
            public string MxStiffnessStringMin { get; set; }
            public string MyStiffnessStringMin { get; set; }
            public string NzStiffnessStringMax { get; set; }
            public string MxStiffnessStringMax { get; set; }
            public string MyStiffnessStringMax { get; set; }
        }
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
                RectFoundationPart foundationPart = foundation.Parts[foundation.Parts.Count - 1];
                sizes[0] = foundationPart.Width;
                sizes[1] = foundationPart.Length;

                //Высоту определяем как сумму высот ступеней
                foreach (RectFoundationPart foundationPartH in foundation.Parts)
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
            foreach (RectFoundationPart foundationPart in foundation.Parts)
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
        /// Возвращает характерные отметки фундамента
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns>Набор абсолютных отметок - нуль, верх фундамента, низ фундамента, верх грунта обратной засыпки</returns>
        public static double[] FoundationLevels(Foundation foundation)
        {
            double[] levels = new double[5];
            double[] foundationSizes = GetDeltaDistance(foundation);
            double absZeroLevel = foundation.Level.Building.AbsoluteLevel - foundation.Level.Building.RelativeLevel;
            double AbsTopLevel = absZeroLevel + foundation.RelativeTopLevel;
            double AbsBtmLevel = AbsTopLevel + foundationSizes[2];
            double SoilTopLevel = absZeroLevel + foundation.SoilRelativeTopLevel;
            if (foundation.SoilSection is null) { throw new Exception("Для фундамента не назначена скважина"); }
            SoilSection soilSection = foundation.SoilSection;
            if (soilSection.SoilLayers.Count == 0) { throw new Exception("Скважина не содержит грунтов"); }
            levels[0] = absZeroLevel; // - абсолютная отметка нуля
            levels[1] = AbsTopLevel; // - абсолютная отметка верха фундамента
            levels[2] = AbsBtmLevel; // - абсолютная отметка подошвы фундамента
            levels[3] = SoilTopLevel; // - абсолютная отметка поверхности грунта обратной засыпки
            levels[4] = soilSection.SoilLayers[0].TopLevel; // - абсолютная отметка поверхности природного грунта
            return levels;
        }
        /// <summary>
        /// Основной решатель фундамента
        /// </summary>
        /// <param name="foundation"></param>
        public static bool SolveFoundation(Foundation foundation)
        {
            bool result = false;
            #region checking
            if (foundation.SoilSectionId is null)
            {
                MessageBox.Show("Не задана скважина для расчета");
                return false;
            }
            if (foundation.SoilSection.SoilLayers.Count ==0)
            {
                MessageBox.Show("Скважина не содержит грунтов");
                return false;
            }
            if (foundation.Parts.Count == 0)
            {
                MessageBox.Show("Не заданы ступени фундамента");
                return false;
            }
            #endregion
            try
            {
                //if (!foundation.IsLoadCasesActual || !foundation.IsPartsActual)
                //{
                //    if (!foundation.IsLoadCasesActual)
                //    {
                        foundation.LoadCases = LoadSetProcessor.GetLoadCases(foundation.ForcesGroups);
                        //Загружения с учетом веса фундамента и грунта
                        foundation.btmLoadSetsWithWeight = GetBottomLoadCasesWithWeight(foundation);
                        //Загружения без учета веса фундамента и грунта
                        if (foundation.LoadCases == null) { foundation.LoadCases = new ObservableCollection<LoadSet>(); }
                        double[] delta = GetDeltaDistance(foundation);
                        foundation.btmLoadSetsWithoutWeight = LoadSetProcessor.GetLoadSetsTransform(foundation.LoadCases, delta);
                        foundation.IsLoadCasesActual = true;
                    //}
                    foundation.IsPartsActual = true;
                    foundation.NdmAreas = GetNdmAreas(foundation);
                    foundation.ForceCurvaturesWithoutWeight = GetForceCurvatures(foundation, foundation.btmLoadSetsWithoutWeight);
                    foundation.ForceCurvaturesWithWeight = GetForceCurvatures(foundation, foundation.btmLoadSetsWithWeight);
                    foundation.Result.CompressedLayers = CompressedLayers(foundation);
                SettleMentResult SettleMentResult = GetSettleMentResult(foundation);
                foundation.Result.MaxSettlement = SettleMentResult.Settlement;
                foundation.Result.sndResistance = SndResistance(foundation);
                //}
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                MessageBox.Show("Ошибка расчета :" + ex);
            }
            return result;
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
            foreach (RectFoundationPart foundationPart in foundation.Parts)
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
        /// <summary>
        /// Возвращает краевые точки фундамента
        /// Каждая точка - пара координат
        /// </summary>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static List<double[]> GetFoundationMidllePoints(Foundation foundation)
        {
            List<double[]> points = new List<double[]>();
            RectFoundationPart foundationPart = foundation.Parts[foundation.Parts.Count - 1];
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
        /// <summary>
        /// Возвращает угловые точки фундамента
        /// Каждая точка - пара координат
        /// </summary>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static List<double[]> GetFoundationCornerPoints(Foundation foundation)
        {
            List<double[]> points = new List<double[]>();
            RectFoundationPart foundationPart = foundation.Parts[foundation.Parts.Count - 1];
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
                {
                    //Для 2-й группы ПС
                    avgStress[0], //[0] средние напряжения
                    crcCenterStress, //[1] напряжения в центре 
                    crcMiddleSresses.Min(), //[2] минимальные напряжения в середине грани
                    crcCornerSresses.Min(), //[3] минимальные напряжения в углу
                    crcCornerSresses.Max(), //[4] максимальные напряжения в углу (для оценки отрыва)
                    //Для 1-й группы ПС
                    avgStress[1], //[5] 
                    designCenterStress, //[6] 
                    designMiddleSresses.Min(), //[7] 
                    designCornerSresses.Min(), //[8] 
                    designCornerSresses.Max(), //[9] 
                    //Площади сжатой и растянутой части
                    areas[0], //[10] 
                    areas[1], //[11] 
                    areas[2], //[12] 
                    areas[3] //[13] 
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
        /// <summary>
        /// Возвращает список средних напряжений под подошвой фундаментов для всех загружений
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns>Список давлений 0 - Напряжения от давления грунта выше подошвы фундамента, 1 - Напряжения от внешней нагрузки</returns>
        public static List<double[]> GetMidlleStresses(Foundation foundation)
        {
            List<double[]> stressesList = new List<double[]>();
            double[] levels = FoundationLevels(foundation);
            double Area = GetBtmGeometryProperties(foundation)[2];
            double sigmZg;
            double d = levels[3] - levels[2];
            double dn = levels[4] - levels[2];
            if (d < dn) { sigmZg = foundation.SoilVolumeWeight * d *(-1D); }
            else { sigmZg = foundation.SoilVolumeWeight * dn * (-1D); }

            List<double[]> stressesWithWeigth = FoundationProcessor.GetStresses(foundation, foundation.ForceCurvaturesWithWeight);
            foreach (double[] stressesArray in stressesWithWeigth)
            {
                double[] stresses = new double[2];
                stresses[0] = sigmZg; //Напряжения от давления грунта выше подошвы фундамента
                stresses[1] = stressesArray[0]; //Напряжения от внешней нагрузки
                stressesList.Add(stresses);
            }
            return stressesList;
        }
        public static List<CompressedLayerList> CompressedLayers(Foundation foundation)
        {
            List<CompressedLayerList> mainCompressedLayers = new List<CompressedLayerList>();
            double l, b;
            double[] foundationSizes = FoundationProcessor.GetContourSize(foundation);
            l = foundationSizes[1];
            b = foundationSizes[0];
            
            List<SoilElementaryLayer> soilElementaryLayers = SoilLayerProcessor.LayersFromSection(foundation);
            List<double[]> midlleStresses = GetMidlleStresses(foundation);
            foreach (double[] stresses in midlleStresses)
            {
                List<CompressedLayer> compressedLayers = SoilLayerProcessor.CompressedLayers(soilElementaryLayers, l, b, stresses[0], stresses[1]);
                CompressedLayerList compressedLayerList = new CompressedLayerList();
                compressedLayerList.Id = ProgrammSettings.CurrentTmpId;
                compressedLayerList.CompressedLayers = compressedLayers;
                mainCompressedLayers.Add(compressedLayerList);
            }   
            return mainCompressedLayers;
        }
        /// <summary>
        /// Возвращает суммарные результаты по массиву напряжений
        /// </summary>
        /// <param name="stressesList"></param>
        /// <returns></returns>
        public static double[] MinMaxStresses(List<double[]> stressesList)
        {
            double[] minMaxStresses = new double[5];
            List<double> MinSndAvgStressesWithWeight = new List<double>();
            List<double> MinSndMiddleStressesWithWeight = new List<double>();
            List<double> MinSndCornerStressesWithWeight = new List<double>();
            List<double> MaxSndCornerStressesWithWeight = new List<double>();
            List<double> MaxSndTensionAreaRatioWithWeight = new List<double>();
            foreach (double[] stresses in stressesList)
            {
                MinSndAvgStressesWithWeight.Add(stresses[0]);
                MinSndMiddleStressesWithWeight.Add(stresses[2]);
                MinSndCornerStressesWithWeight.Add(stresses[3]);
                MaxSndCornerStressesWithWeight.Add(stresses[4]);
                MaxSndTensionAreaRatioWithWeight.Add(stresses[10] / (stresses[10] + stresses[11]));
            }

            minMaxStresses[0] = MinSndAvgStressesWithWeight.Min(); //средние напряжения
            minMaxStresses[1] = MinSndMiddleStressesWithWeight.Min(); //краевое напряжение
            minMaxStresses[2] = MinSndCornerStressesWithWeight.Min(); //минимальное угловое напряжение
            minMaxStresses[3] = MaxSndCornerStressesWithWeight.Max(); //максимальное угловое напряжение
            minMaxStresses[4] = MaxSndTensionAreaRatioWithWeight.Max(); //максимальный процент площади отрыва

            return minMaxStresses;
        }
        /// <summary>
        /// Возвращает результирующие данные по осадке для отчета
        /// </summary>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static SettleMentResult GetSettleMentResult (Foundation foundation)
        {
            //Функция перевода жесткости в строку
            string[] GetStiffnessString(List<double> stiffnessList, double coff)
            {
                string[] s = new string[2];
                s[0] = "---";
                s[1] = "---";
                if (stiffnessList.Count > 0)
                {
                    double stiffnessMax = stiffnessList.Max();
                    double StiffnessMaxRound = Math.Round(stiffnessMax * coff, 3);
                    s[0] = Convert.ToString(StiffnessMaxRound);

                    double stiffnessMin = stiffnessList.Min();
                    double StiffnessMinRound = Math.Round(stiffnessMin * coff, 3);
                    s[1] = Convert.ToString(StiffnessMinRound);
                }
                return s;
            }
            #region Списки для хранения
            SettleMentResult settleMentResult = new SettleMentResult();
            List<CompressedLayerList> mainCompressedLayers = foundation.Result.CompressedLayers;
            //Список хранения осадки
            List<double> Settlements = new List<double>();
            //Список для хранения глубины сжимаемой толщи
            List<double> ComressionHeights = new List<double>();
            //Списки для хранения кренов относительно каждой из осей
            List<double> IncXs = new List<double>();
            List<double> IncYs = new List<double>();
            List<double> IncXYs = new List<double>();
            //Списки для хранения жесткостей по каждой степени свободы
            List<double> NzStiffnesses = new List<double>();
            List<double> MxStiffnesses = new List<double>();
            List<double> MyStiffnesses = new List<double>();
            #endregion
            int count = mainCompressedLayers.Count;
            //Заполнение списков
            for (int i = 0; i<count; i++)
            {
                CompressedLayerList compressedLayersList = mainCompressedLayers[i];
                double sumSettlement = compressedLayersList.CompressedLayers[0].SumSettlement;
                Settlements.Add(sumSettlement);
                double compressionHeight = SoilLayerProcessor.ComressedHeight(compressedLayersList.CompressedLayers);
                ComressionHeights.Add(compressionHeight);

                SumForces sumForces = new SumForces(foundation.LoadCases[i], false);
                double Nz = sumForces.ForceMatrix[2, 0];
                double Mx = sumForces.ForceMatrix[0, 0];
                double My = sumForces.ForceMatrix[1, 0];
                double[] rotates = SoilLayerProcessor.Inclination(Mx, My, compressedLayersList.CompressedLayers, foundation);
                double MxInc = rotates[0];
                double MyInc = rotates[1];
                double MxyInc = Math.Sqrt(MxInc * MxInc + MyInc * MyInc);
                IncXs.Add(Math.Abs(MxInc));
                IncYs.Add(Math.Abs(MyInc));
                IncXYs.Add(MxyInc);

                double NzStiffness;
                double MxStiffness;
                double MyStiffness;

                if (sumSettlement != 0)
                {
                    NzStiffness = Nz / sumSettlement;
                    NzStiffnesses.Add(Math.Abs(NzStiffness));
                    if (MxInc != 0)
                    {
                        MxStiffness = Mx / MxInc;
                        MxStiffnesses.Add(Math.Abs(MxStiffness));
                    }
                    if (MyInc != 0)
                    {
                        MyStiffness = My / MyInc;
                        MyStiffnesses.Add(Math.Abs(MyStiffness));
                    }
                }
            }
            //Нахождение минимумов и максимумов по спискам
            settleMentResult.Settlement = Settlements.Min();
            settleMentResult.CompressionHeight = ComressionHeights.Max();
            settleMentResult.IncX = IncXs.Max();
            settleMentResult.IncY = IncYs.Max();
            settleMentResult.IncXY = IncXYs.Max();

            string[] stiffness;
            double measureCoff;
            measureCoff = MeasureUnitConverter.GetCoefficient(1) / MeasureUnitConverter.GetCoefficient(0);
            stiffness = GetStiffnessString(NzStiffnesses, measureCoff);
            settleMentResult.NzStiffnessStringMax = stiffness[0];
            settleMentResult.NzStiffnessStringMin = stiffness[1];

            measureCoff = MeasureUnitConverter.GetCoefficient(2);
            stiffness = GetStiffnessString(MxStiffnesses, measureCoff);
            settleMentResult.MxStiffnessStringMax = stiffness[0];
            settleMentResult.MxStiffnessStringMin = stiffness[1];

            stiffness = GetStiffnessString(MyStiffnesses, measureCoff);
            settleMentResult.MyStiffnessStringMax = stiffness[0];
            settleMentResult.MyStiffnessStringMin = stiffness[1];
           
            return settleMentResult;
        }
        /// <summary>
        /// Возвращает расчетное сопротивление дисперсного грунта
        /// </summary>
        /// <param name="gammaC1"></param>
        /// <param name="gammaC2"></param>
        /// <param name="k"></param>
        /// <param name="fi2">Угол внутреннего трения, радиан</param>
        /// <param name="c2"></param>
        /// <param name="width"></param>
        /// <param name="kZ"></param>
        /// <param name="d1"></param>
        /// <param name="db"></param>
        /// <param name="gamma2"></param>
        /// <param name="gamma2Dash"></param>
        /// <returns>Расчетное сопротивлени дисперсного грунта</returns>
        public static double LinearResistance(double gammaC1, double gammaC2, double k, double fi2, double c2, double width, double d1, double db, double gamma2, double gamma2Dash)
        {
            double[] cofficients = SndResistanceCoff(fi2);

            double m_Gamma = cofficients[0];
            double m_Q = cofficients[1];
            double m_C = cofficients[2];
            double kZ;
            if (width < 10) kZ = 1; else kZ = 8 / width + 0.2;
            double resistance;
            resistance = gammaC1 * gammaC2 / k * (m_Gamma * kZ * width * gamma2 + gamma2Dash * (m_Q * (d1 + db) - db) + m_C * c2);
            return resistance;
        }
        /// <summary>
        /// Возвращает коэффициенты для вычисления расчетного сопротивления дисперсного грунта
        /// </summary>
        /// <param name="fi">Угол внутреннего трения в градусах!!!!</param>
        /// <returns>Массив из 3-х коэффциентов</returns>
        public static double[] SndResistanceCoff(double fi)
        {
            fi = fi / 180 * Math.PI;
            double[] cofficients = new double[3];
            double psi = Math.PI / (1 / (Math.Tan(fi)) + fi - Math.PI / 2);
            cofficients[0] = psi / 4; //M_Gamma
            cofficients[1] = 1 + psi; //M_Q
            cofficients[2] = psi / (Math.Tan(fi)); //M_C
            return cofficients;
        }
        /// <summary>
        /// Возвращает коэффициент по СП
        /// </summary>
        /// <param name="dispersedSoil"></param>
        /// <returns></returns>
        private static double GetGammaC1(DispersedSoil dispersedSoil)
        {
            //Если грунт крупнообломочный
            //Если грунт песок мелкий
            //Если грунт песок маловлажный
            //Если грунт песок насыщенный водой
            //Глинистый грунт с Il<0.25
            //Глинистый грунт с 0.25<Il<0.5
            //Глинистый грунт с Il>0.5
            return 1.1;
        }
        /// <summary>
        /// Возвращает коэффициент по СП
        /// </summary>
        /// <param name="building"></param>
        /// <param name="dispersedSoil"></param>
        /// <returns></returns>
        private static double GetGammaC2(BuildingAndSite.Building building, DispersedSoil dispersedSoil)
        {
            double gammaC2 = 1;
            double ratio = building.RigidRatio;
            if ((!building.IsRigid) & (ratio < 4))
            {
                if (dispersedSoil is ClaySoil)
                {
                    ClaySoil claySoil = dispersedSoil as ClaySoil;
                    if (claySoil.IL > 0.5) return 1;
                    return MathOperation.InterpolateNumber(1.5, 1.1, 4.0, 1.0, claySoil.IL);
                }
            }
            return gammaC2;
        }
        /// <summary>
        /// Возвращает расчетное сопротивление грунта по 2-й группе предельных состояний
        /// </summary>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static double SndResistance (Foundation foundation)
        {

            double resistance = 0;
            List<CompressedLayerList> compressedLayerList = foundation.Result.CompressedLayers;
            List<CompressedLayer> compressedLayers = compressedLayerList[0].CompressedLayers;
            Soil soil = compressedLayers[0].SoilElementaryLayer.Soil;
            //Если грунт не является несущим, выдаем исключение
            if (!(soil is BearingSoil)) throw new Exception("Невозможно определить расчетное сопротивление ненесущего грунта");
            //Если грунт является скальным
            if (soil is RockSoil)
            {
                RockSoil rockSoil = soil as RockSoil;
                resistance = rockSoil.SndDesignStrength;
                return resistance;
            }
            //Если грунт является дисперсным
            if (soil is DispersedSoil)
            {
                DispersedSoil dispSoil = soil as DispersedSoil;
                double fi2 = dispSoil.SndDesignFi;
                double c2 = dispSoil.SndDesignCohesion;
                //Коэффициент по СП
                double gammaC1 = GetGammaC1(dispSoil);
                //Коэффициент по СП
                double gammaC2 = GetGammaC2(foundation.Level.Building, dispSoil);
                //Если характеристики грунта определены испытаниями, то коэффициент 1.0, иначе 1.1
                double k;
                if (dispSoil.IsDefinedFromTest) k = 1.0; else k = 1.1;
                double[] sizes = FoundationProcessor.GetContourSize(foundation);
                double width = Math.Min(sizes[0], sizes[1]);
                //Абсолютная отметка планировки для здания
                double planingLevel = foundation.Level.Building.AbsolutePlaningLevel;
                //Характерные отметки здания
                double[] levels = FoundationLevels(foundation);
                //Абсолютная отметка подошвы
                double btmLevel = levels[2];
                //Абсолютная отметка засыпки
                double soilLevel = levels[3];
                //Расстояние от уровня засыпки до подошвы фундамента
                double d1 = soilLevel- btmLevel;
                //Добавляем высоту пола, приведенную по объемному весу к засыпке
                d1 += foundation.FloorLoad / foundation.SoilVolumeWeight;
                //Глубина подвала принимается не более 2м
                double db = Math.Max(0, planingLevel - soilLevel);
                db = Math.Min(2, db);
                double gamma2;
                //Характеристики грунта ниже подошвы фундамента осредняем в пределах зоны, определяемой по СП
                double zMax;
                if (width < 10) zMax = btmLevel-width / 2; else zMax = btmLevel - (4 + 0.1 * width);
                List<double> gammas = new List<double>();
                foreach (CompressedLayer compressedLayer in compressedLayers)
                {
                    if (compressedLayer.SoilElementaryLayer.TopLevel < zMax)
                    {
                        double gamma = SoilLayerProcessor.SoilWeight(compressedLayer.SoilElementaryLayer)[3];
                        gammas.Add(gamma);
                    }
                }
                gamma2 = gammas.Average();
                //Объемный вес грунта обратной засыпки
                double gamma2Dash = foundation.SoilVolumeWeight;
                resistance = LinearResistance(gammaC1, gammaC2, k, fi2, c2, width, d1, db, gamma2, gamma2Dash);
                double maxSettlement = foundation.Result.MaxSettlement * (-1D);
                double maxLimitSettlement = foundation.Level.Building.MaxFoundationSettlement;
                //Если осадка не превышает 40% предельной
                if (maxSettlement < (0.4 * maxLimitSettlement)) return 1.2 * resistance;
                //Если осадка не превышает 70% предельной
                if (maxSettlement < (0.7 * maxLimitSettlement)) return MathOperation.InterpolateNumber(0.4, 1.2, 0.7, 1.0, maxSettlement / maxLimitSettlement) * resistance;
                //Иначе возварщаем без повышения
                return resistance;
            }
            return resistance;
        }
    }
}
