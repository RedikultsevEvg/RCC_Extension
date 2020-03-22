using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.Common.NDM.Interfaces;
using RDBLL.Entity.Common.NDM.MaterialModels;
using RDBLL.Entity.Common.NDM.Processors;
using RDBLL.Forces;
using System.Collections.Generic;
using System.Linq;

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
            public List<LoadCombination> loadCombinationsX { get; set; }
            /// <summary>
            /// Коллекция комбинаций усилий в ступени фундамента
            /// </summary>
            public List<LoadCombination> loadCombinationsY { get; set; }
            /// <summary>
            /// Коллекция элементарных участков бетона и арматуры в ступени фундамента
            /// </summary>
            public List<NdmArea> ndmAreasX { get; set; }
            /// <summary>
            /// Коллекция элементарных участков бетона и арматуры в ступени фундамента
            /// </summary>
            public List<NdmArea> ndmAreasY { get; set; }
        }
        /// <summary>
        /// Возвращает коллекцию моментов и элементарных участков по ступеням
        /// </summary>
        /// <param name="foundation"></param>
        /// <returns></returns>
        public static List<PartMomentAreas> GetBottomMomentAreas(Foundation foundation)
        {
            List<PartMomentAreas> partMomentAreas = new List<PartMomentAreas>();
            #region Settings
            double elemSize = 0.1;
            #endregion
            #region MaterialModels
            double Rc = -1.5e7;
            double Rct = 1.05e6;
            double Rsc = -3.5e8;
            double Rs = 3.5e8;
            double Es = 2e11;
            //SoilModel
            IMaterialModel soilModel = foundation.NdmAreas[0].MaterialModel;
            //Concrete Model
            List<double> constantConcreteList = new List<double> { Rc, -0.0015, -0.0035, Rct, 0.00008, 0.00015 };
            IMaterialModel concreteModel = new DoubleLinear(constantConcreteList);
            //Reinforcement Model
            List<double> constantSteelList = new List<double> { Rsc, Rsc / Es, -0.025, Rs, Rs / Es, 0.025 };
            IMaterialModel SteelModel = new DoubleLinear(constantSteelList);
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
                    int matrixIndex;
                    if (normalX)
                    {
                        MomentParameter.KindId = 2;
                        matrixIndex = 1;
                    }
                    else
                    {
                        MomentParameter.KindId = 3;
                        matrixIndex = 0;
                    }

                    SumForces sumForces;
                    sumForces = NdmAreaProcessor.GetSumForces(ndmSoilAreas, forceCurvature.CrcCurvature);
                    sumForces = new SumForces(sumForces, coordX, coordY);
                    MomentParameter.LongCrcValue = sumForces.ForceMatrix[matrixIndex, 0] * coff;
                    MomentParameter.CrcValue = sumForces.ForceMatrix[matrixIndex, 0] * coff;

                    sumForces = NdmAreaProcessor.GetSumForces(ndmSoilAreas, forceCurvature.DesignCurvature);
                    sumForces = new SumForces(sumForces, coordX, coordY);
                    MomentParameter.LongDesignValue = sumForces.ForceMatrix[matrixIndex, 0] * coff;
                    MomentParameter.DesignValue = sumForces.ForceMatrix[matrixIndex, 0] * coff;

                    loadCombinations.Add(loadCombination);
                }
                //Возвращаем 

                return loadCombinations;
            }
            List<NdmArea> concreteXNdmAreas = new List<NdmArea>();
            List<NdmArea> concreteYNdmAreas = new List<NdmArea>();
            double currentZ = 0;
            //Возвращает коллекцию элементарных участков для ступени и нижележащих ступеней
            List<NdmArea> GetPartAreas(RectFoundationPart part, bool normalX)
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
                List<NdmRectangleArea> newNdmAreas = NdmAreaProcessor.MeshRectangleByCoord(concreteModel, -width / 2, +width / 2, currentZ, currentZ + part.Height, elemSize);
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
                locPartMomentAreas.ndmAreasX = GetPartAreas(part, true);
                locPartMomentAreas.ndmAreasY = GetPartAreas(part, false);

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

                    locPartMomentAreas.loadCombinationsY = MomentXLeftCombinations;
                    locPartMomentAreas.loadCombinationsY.AddRange(MomentXRightCombinations);
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

                    locPartMomentAreas.loadCombinationsX = MomentYBottomCombinations;
                    locPartMomentAreas.loadCombinationsX.AddRange(MomentYTopCombinations);
                    #endregion
                    //Добавляем высоту
                    currentZ += part.Height;
                }
                else //Для самой верхней ступени момент определяем только по центру
                {
                    //Момент определяем в середине ступени
                    ndmSoilRectAreas = NdmAreaProcessor.MeshRectangleByCoord(soilModel, minMaxX[0], centerX, minMaxY[0], minMaxY[1], elemSize);
                    List<LoadCombination> MomentYCombinations = GetMoment(centerX, 0, ndmSoilRectAreas, foundation.ForceCurvaturesWithoutWeight, true, -1);
                    locPartMomentAreas.loadCombinationsX = MomentYCombinations;
                    ndmSoilRectAreas = NdmAreaProcessor.MeshRectangleByCoord(soilModel, minMaxX[0], minMaxX[1], minMaxY[0], centerY, elemSize);
                    List<LoadCombination> MomentXCombinations = GetMoment(0, centerY, ndmSoilRectAreas, foundation.ForceCurvaturesWithoutWeight, false, 1);
                    locPartMomentAreas.loadCombinationsY = MomentXCombinations;
                }
                partMomentAreas.Add(locPartMomentAreas);
            }
            return partMomentAreas;
        }
    }
}
