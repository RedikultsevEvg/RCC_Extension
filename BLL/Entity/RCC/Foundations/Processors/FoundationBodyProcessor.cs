using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.Common.NDM.Interfaces;
using RDBLL.Entity.Common.NDM.MaterialModels;
using RDBLL.Entity.Common.NDM.Processors;
using RDBLL.Forces;
using System.Collections.Generic;
using System.Linq;
using System;
using RDBLL.Entity.RCC.Common.Processors;
using RDBLL.Entity.Common.Materials;

namespace RDBLL.Entity.RCC.Foundations.Processors
{
    /// <summary>
    /// Процессор расчетов тела фундамента
    /// </summary>
    public static class FoundationBodyProcessor
    {
        /// <summary>
        /// Класс комбинаций усилий в ступенях и элементарных участков ступеней
        /// </summary>
        public class PartMomentAreas
        {
            /// <summary>
            /// Код ступени фундамента
            /// </summary>
            public int FoundationPartId { get; set; }
            /// <summary>
            /// Ссылка на ступень фундамента, для которой получены моменты
            /// </summary>
            public FoundationPart FoundationPart { get; set; }
            /// <summary>
            /// Коллекция комбинаций усилий в ступени фундамента
            /// </summary>
            public List<LoadCombination> LoadCombinationsX { get; set; }
            /// <summary>
            /// Коллекция комбинаций усилий в ступени фундамента
            /// </summary>
            public List<LoadCombination> LoadCombinationsY { get; set; }
            /// <summary>
            /// Коллекция элементарных участков бетона и арматуры в ступени фундамента
            /// </summary>
            public List<NdmArea> CrcNdmAreasX { get; set; }
            /// <summary>
            /// Коллекция элементарных участков бетона и арматуры в ступени фундамента
            /// </summary>
            public List<NdmArea> DesignNdmAreasX { get; set; }
            /// <summary>
            /// Коллекция элементарных участков бетона и арматуры в ступени фундамента
            /// </summary>
            public List<NdmArea> CrcNdmAreasY { get; set; }
            /// <summary>
            /// Коллекция элементарных участков бетона и арматуры в ступени фундамента
            /// </summary>
            public List<NdmArea> DesignNdmAreasY { get; set; }
        }
        /// <summary>
        /// Возвращает коллекцию моментов и элементарных участков по ступеням
        /// </summary>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static bool CalcBottomMomentAreas(Foundation foundation)
        {
            #region Settings
            double elemSize = 0.1;
            #endregion
            #region MaterialModels
            double Rc = -1.5e7;
            double Rct = 1.05e6;
            double Rc_ser = -1.5e7;
            double Rct_ser = 1.05e6;
            double Ec = 3e10;
            double Rsc = -3.5e8;
            double Rs = 3.5e8;
            double Rsc_ser = -4.0e8;
            double Rs_ser = 4.0e8;
            double Es = 2e11;
            //SoilModel
            IMaterialModel soilModel = foundation.NdmAreas[0].MaterialModel;
            #region  Concrete Model
            //Модель материала бетона с нормативными характеристиками
            List<double> constCrcConcreteList = new List<double> { Rc_ser, -0.0015, -0.0035, Rct_ser, 0.00008, 0.00015 };
            IMaterialModel concreteCrcModel = new DoubleLinear(constCrcConcreteList);
            concreteCrcModel.ElasticModulus = Ec;
            //Модель материала бетона с расчетными характеристиками
            List<double> constDesignConcreteList = new List<double> { Rc, -0.0015, -0.0035, Rct, 0.00008, 0.00015 };
            IMaterialModel concreteDesignModel = new DoubleLinear(constDesignConcreteList);
            concreteDesignModel.ElasticModulus = Ec;
            #endregion
            #region Reinforcement Model
            //Модель материала арматуры с нормативными характеристиками
            List<double> constCrcSteelList = new List<double> { Rsc_ser, Rsc_ser / Es, -0.025, Rs_ser, Rs_ser / Es, 0.025 };
            IMaterialModel steelCrcModel = new DoubleLinear(constCrcSteelList);
            steelCrcModel.ElasticModulus = Es;
            //Модель материала арматуры с расчетными характеристиками
            List<double> consDesigntSteelList = new List<double> { Rsc, Rsc / Es, -0.025, Rs, Rs / Es, 0.025 };
            IMaterialModel steelDesignModel = new DoubleLinear(consDesigntSteelList);
            steelDesignModel.ElasticModulus = Es;
            #endregion
            #endregion

            List<LoadCombination> GetMoment(double coordX, double coordY, List<NdmRectangleArea> ndmSoilRectAreas, List<ForceCurvature> forceCurvatures, bool normalX, double coff)
            {
                List<LoadCombination> loadCombinations = new List<LoadCombination>();
                List<NdmArea> ndmSoilAreas = NdmAreaProcessor.ConvertFromRectToBase(ndmSoilRectAreas);

                List<double> tmpCrcMoments = new List<double>();
                List<double> tmpDesignMoments = new List<double>();
                //По всем комбинациям нагрузок и кривизн определяем изгибающий момент
                foreach (ForceCurvature forceCurvature in forceCurvatures)
                {
                    LoadCombination loadCombination = new LoadCombination();
                    loadCombination.Id = ProgrammSettings.CurrentTmpId;

                    ForceParameter MomentParameter = new ForceParameter(loadCombination);
                    loadCombination.ForceParameters.Add(MomentParameter);
                    MomentParameter.KindId = 2;
                    int index;
                    if (normalX) index = 1;
                    else index = 0;
                    SumForces sumForces;
                    sumForces = NdmAreaProcessor.GetSumForces(ndmSoilAreas, forceCurvature.CrcCurvature);
                    sumForces = new SumForces(sumForces, coordX, coordY);
                    MomentParameter.LongCrcValue = sumForces.ForceMatrix[index, 0] * coff;
                    MomentParameter.CrcValue = sumForces.ForceMatrix[index, 0] * coff;

                    sumForces = NdmAreaProcessor.GetSumForces(ndmSoilAreas, forceCurvature.DesignCurvature);
                    sumForces = new SumForces(sumForces, coordX, coordY);
                    MomentParameter.LongDesignValue = sumForces.ForceMatrix[index, 0] * coff;
                    MomentParameter.DesignValue = sumForces.ForceMatrix[index, 0] * coff;

                    loadCombinations.Add(loadCombination);
                }
                //Возвращаем 

                return loadCombinations;
            }
            List<NdmArea> concreteXNdmAreas = new List<NdmArea>();
            List<NdmArea> concreteYNdmAreas = new List<NdmArea>();
            double currentZ = 0;
            //Возвращает коллекцию элементарных участков для ступени и нижележащих ступеней
            List<NdmArea> GetPartAreas(RectFoundationPart part, IMaterialModel materialModel, bool normalX)
            {
                double width;
                List<NdmArea> curNdmAreas;
                if (normalX)
                {
                    width = part.Length;
                    curNdmAreas = concreteXNdmAreas;
                }
                else
                {
                    width = part.Width;
                    curNdmAreas = concreteYNdmAreas;
                }
                //Коллекция новых элементарных участков только для данной ступени
                List<NdmRectangleArea> newNdmAreas = NdmAreaProcessor.MeshRectangleByCoord(materialModel, -width / 2, +width / 2, currentZ, currentZ + part.Height, elemSize);
                //Коллекция элементарных участков данной ступени и всех нижележащих
                List<NdmArea> partNdmAreas = new List<NdmArea>();
                //Добавляем все ранее созданные элементарные участки
                partNdmAreas.AddRange(curNdmAreas);
                //Добавляем элементарные участки данной ступени
                partNdmAreas.AddRange(newNdmAreas);
                //Обновляем перечень элементарных участков с учетом данной ступени
                curNdmAreas.AddRange(newNdmAreas);
                //Возвращаем коллекцию элементарных участков
                return partNdmAreas;
            }
            double[] reinfAreas = new double[2] { 0, 0 };

            int partCount = foundation.Parts.Count();
            //В цикле проходим по всем ступеням
            //Высота центра тяжести

            RectFoundationPart bottomPart = foundation.Parts[foundation.Parts.Count() - 1];
            double[] minMaxX = new double[2] { bottomPart.Width / 2 * (-1D), bottomPart.Width / 2 };
            double[] minMaxY = new double[2] { bottomPart.Length / 2 * (-1D), bottomPart.Length / 2 };


            for (int i = partCount - 1; i >= 0; i--)
            {
                RectFoundationPart part = foundation.Parts[i];
                double centerX = part.CenterX - bottomPart.CenterX;
                double centerY = part.CenterY - bottomPart.CenterY;
                //Элементарные участки грунта
                List<NdmRectangleArea> ndmSoilRectAreas;
                PartMomentAreas locPartMomentAreas = new PartMomentAreas();
                locPartMomentAreas.FoundationPartId = part.Id;
                locPartMomentAreas.FoundationPart = part;
                locPartMomentAreas.CrcNdmAreasX = GetPartAreas(part, concreteCrcModel, true);
                locPartMomentAreas.CrcNdmAreasY = GetPartAreas(part, concreteCrcModel, false);
                locPartMomentAreas.DesignNdmAreasX = GetPartAreas(part, concreteDesignModel, true);
                locPartMomentAreas.DesignNdmAreasY = GetPartAreas(part, concreteDesignModel, false);

                //Если рассматриваемая ступень не является самой верхней
                if (i > 0)
                {
                    double currentX;
                    double currentY;
                    //Получаем следующую ступень
                    RectFoundationPart nextPart = foundation.Parts[i - 1];
                    #region Moments X
                    //Определяем моменты по грани ступени слева от центра
                    currentX = centerX - nextPart.Width / 2;
                    ndmSoilRectAreas = NdmAreaProcessor.MeshRectangleByCoord(soilModel, minMaxX[0], currentX, minMaxY[0], minMaxY[1], elemSize);
                    List<LoadCombination> MomentXLeftCombinations = GetMoment(currentX, 0, ndmSoilRectAreas, foundation.ForceCurvaturesWithoutWeight, true, -1);

                    //Определяем моменты по грани ступени справа от центра
                    currentX = centerX + nextPart.Width / 2;
                    ndmSoilRectAreas = NdmAreaProcessor.MeshRectangleByCoord(soilModel, currentX, minMaxX[1], minMaxY[0], minMaxY[1], elemSize);
                    List<LoadCombination> MomentXRightCombinations = GetMoment(currentX, 0, ndmSoilRectAreas, foundation.ForceCurvaturesWithoutWeight, true, 1);

                    locPartMomentAreas.LoadCombinationsY = MomentXLeftCombinations;
                    locPartMomentAreas.LoadCombinationsY.AddRange(MomentXRightCombinations);
                    #endregion
                    #region Moment Y
                    //Определяем моменты снизу от ступени
                    currentY = centerY - nextPart.Length / 2;
                    ndmSoilRectAreas = NdmAreaProcessor.MeshRectangleByCoord(soilModel, minMaxX[0], minMaxX[1], minMaxY[0], currentY, elemSize);
                    List<LoadCombination> MomentYBottomCombinations = GetMoment(0, currentY, ndmSoilRectAreas, foundation.ForceCurvaturesWithoutWeight, false, 1);
                    //Определяем моменты сверху от ступени
                    currentY = centerY + nextPart.Length / 2;
                    ndmSoilRectAreas = NdmAreaProcessor.MeshRectangleByCoord(soilModel, minMaxX[0], minMaxX[1], currentY, minMaxY[1], elemSize);
                    List<LoadCombination> MomentYTopCombinations = GetMoment(0, currentY, ndmSoilRectAreas, foundation.ForceCurvaturesWithoutWeight, false, -1);

                    locPartMomentAreas.LoadCombinationsX = MomentYBottomCombinations;
                    locPartMomentAreas.LoadCombinationsX.AddRange(MomentYTopCombinations);
                    #endregion
                    //Добавляем высоту
                    currentZ += part.Height;
                }
                else //Для самой верхней ступени момент определяем только по центру
                {
                    //Момент определяем в середине ступени
                    ndmSoilRectAreas = NdmAreaProcessor.MeshRectangleByCoord(soilModel, minMaxX[0], centerX, minMaxY[0], minMaxY[1], elemSize);
                    List<LoadCombination> MomentXCombinations = GetMoment(centerX, 0, ndmSoilRectAreas, foundation.ForceCurvaturesWithoutWeight, true, -1);
                    locPartMomentAreas.LoadCombinationsY = MomentXCombinations;
                    ndmSoilRectAreas = NdmAreaProcessor.MeshRectangleByCoord(soilModel, minMaxX[0], minMaxX[1], minMaxY[0], centerY, elemSize);
                    List<LoadCombination> MomentYCombinations = GetMoment(0, centerY, ndmSoilRectAreas, foundation.ForceCurvaturesWithoutWeight, false, 1);
                    locPartMomentAreas.LoadCombinationsX = MomentYCombinations;
                }
                part.Result.partMomentAreas = locPartMomentAreas;
            }
            return true;
        }
        /// <summary>
        /// Вычисление моментов для ступеней фундамента
        /// </summary>
        /// <param name="foundation"></param>
        public static void CalcCrcMoment (Foundation foundation)
        {
            double Rct = 1.5e6;
            
            foreach (FoundationPart part in foundation.Parts)
            {
                PartMomentAreas partMomentAreas = part.Result.partMomentAreas;
                part.Result.Mcrc = GetCrcMoments(partMomentAreas, Rct, 3e10);
            }
        }
        /// <summary>
        /// Возвращает пару моментов образования трещин для ступеней
        /// </summary>
        /// <param name="partMomentAreas"></param>
        /// <param name="Rct"></param>
        /// <param name="Ec"></param>
        /// <returns></returns>
        public static double[] GetCrcMoments(PartMomentAreas partMomentAreas, double Rct, double Ec = 3e10)
        {
            double[] crcMoments = new double[2];
            double sndMomentInertiaX = NdmConcreteProcessor.GetSndMomentInertia(partMomentAreas.CrcNdmAreasY, Ec)[0, 1];
            double sndMomentInertiaY = NdmConcreteProcessor.GetSndMomentInertia(partMomentAreas.CrcNdmAreasX, Ec)[0, 1];
            crcMoments = new double[2] { sndMomentInertiaX * Rct * (-1D), sndMomentInertiaY * Rct * (-1D) };
            return crcMoments;
        }
        private static double[] GetReinforcementArea(RectFoundationPart part)
        {
            double GetMaxMoment(List<LoadCombination> loadCombinations)
            {
                List<double> mxList = new List<double>();
                foreach (LoadCombination loadCombination in loadCombinations)
                {
                    foreach (ForceParameter forceParameter in loadCombination.ForceParameters)
                    {
                        if (forceParameter.KindId == 2) mxList.Add(forceParameter.DesignValue);
                    }
                }
                return mxList.Max();
            }
            //Получаем ссылки на армирование подошвы вдоль оси X и вдоль оси Y
            MaterialContainer materialContainer = part.Foundation.BottomReinforcement;
            ReinforcementUsing rfX = (materialContainer.MaterialUsings[0]) as ReinforcementUsing;
            ReinforcementUsing rfY = (materialContainer.MaterialUsings[1]) as ReinforcementUsing;
            //Получаем ссылки на материалы армирования вдоль оси
            ReinforcementKind rfKindX = (rfX.MaterialKind) as ReinforcementKind;
            ReinforcementKind rfKindY = (rfY.MaterialKind) as ReinforcementKind;
            //Получаем ссылки на раскладку армирования подошвы вдоль оси X и вдоль оси Y
            RFSmearedBySpacing rfSpacingX = (rfX.RFSpacing) as RFSmearedBySpacing;
            RFSmearedBySpacing rfSpacingY = (rfY.RFSpacing) as RFSmearedBySpacing;

            ConcreteKind concreteKind = part.Foundation.Concrete.MaterialKind as ConcreteKind;
            double mx = GetMaxMoment(part.Result.partMomentAreas.LoadCombinationsX);
            double my = GetMaxMoment(part.Result.partMomentAreas.LoadCombinationsY);
            double RsX = rfKindX.FstTensStrength;
            double RsY = rfKindY.FstTensStrength;
            double Rc = concreteKind.FstCompStrength;
            double bx = part.Length;
            double by = part.Width;
            double h0x = part.Result.ZMax - rfSpacingX.CoveringLayer;
            double h0y = part.Result.ZMax - rfSpacingX.CoveringLayer;
            double ax = RectSectionProcessor.GetReinforcementArea(my, bx, h0x, RsX, Rc);
            double ay = RectSectionProcessor.GetReinforcementArea(mx, by, h0y, RsY, Rc);
            part.Result.AsRec = new double[2]; 
            part.Result.AsRec[0] = ax;
            part.Result.AsRec[1] = ay;
            return new double[2] { ax, ay };
        }
        public static double[] GetReinforcementArea(Foundation foundation)
        {
            List<double> ax = new List<double>();
            List<double> ay = new List<double>();
            foreach (FoundationPart part in foundation.Parts)
            {
                if (part is RectFoundationPart)
                {
                    RectFoundationPart rectPart = part as RectFoundationPart;
                    double[] areas = GetReinforcementArea(rectPart);
                    ax.Add(areas[0]);
                    ay.Add(areas[1]);
                }
            }
            foundation.Result.AsRec = new double[2] { ax.Max(), ay.Max() };
            return foundation.Result.AsRec;
        }
        //Возвращает фактическую площадь армирования подошвы фундамента
        public static double[] GetActualReinforcementArea(Foundation foundation)
        {
            //Длина раскладки для арматуры вдоль оси X и Y
            double lengthX = foundation.Parts[foundation.Parts.Count - 1].Length;
            double lengthY = foundation.Parts[foundation.Parts.Count - 1].Width;
            //Получаем ссылки на армирование подошвы вдоль оси X и вдоль оси Y
            MaterialContainer materialContainer = foundation.BottomReinforcement;
            ReinforcementUsing rfX = (materialContainer.MaterialUsings[0]) as ReinforcementUsing;
            ReinforcementUsing rfY = (materialContainer.MaterialUsings[1]) as ReinforcementUsing;
            //Получаем ссылки на раскладку армирования подошвы вдоль оси X и вдоль оси Y
            RFSmearedBySpacing rfSpacingX = (rfX.RFSpacing) as RFSmearedBySpacing;
            RFSmearedBySpacing rfSpacingY = (rfY.RFSpacing) as RFSmearedBySpacing;
            //Фактическая площадь арм
            double areaX = rfSpacingX.GetTotalBarsArea(lengthX - 0.1);
            double areaY = rfSpacingY.GetTotalBarsArea(lengthY - 0.1);
            //Заносим полученные площади армирования в результаты расчета
            foundation.Result.AsAct = new double[2] { areaX, areaY };
            return foundation.Result.AsAct;
        }
    }
}
