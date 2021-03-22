using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Soils;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.RCC.Foundations.Processors;
using RDBLL.Common.Service;
using RDBLL.Common.Geometry;

namespace RDBLL.Entity.Soils.Processors
{
    /// <summary>
    /// Процессор слоев грунта
    /// </summary>
    public class SoilLayerProcessor
    {
        /// <summary>
        /// Возвращает слои грунта по фундаменту и заданной глубине
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <param name="soilThickness">Заданная глубина, по умолчанию 50м</param>
        /// <param name="maxHeight">Максимальная толщина слоя грунта</param>
        /// <returns></returns>
        public static List<SoilElementaryLayer> LayersFromSection(Foundation foundation, double soilThickness = 50, double maxHeight = 0.1)
        {
            double[] levels = FoundationProcessor.FoundationLevels(foundation);
            double foundationAbsBtmLevel = levels[2];
            List<SoilElementaryLayer> soilElementaryLayers = new List<SoilElementaryLayer>();
            List<SoilElementaryLayer> tmpSoilElementaryLayers = new List<SoilElementaryLayer>();
            SoilSection soilSection = foundation.SoilSectionUsing.SoilSection;
            //Проверка на опирание фундамента на грунт
            if (soilSection.SoilLayers[0].TopLevel< foundationAbsBtmLevel) { throw new Exception("Низ фундамента расположен выше слоев грунта"); }
            int soilCount = soilSection.SoilLayers.Count;
            if (soilCount == 0) { throw new Exception("Скважина не содержит грунтов"); }
            for (int i=0; i<soilCount; i++)
            {
                //Переменные для отметок верха и низа слоя грунта
                double soilLayerTopLevel, soilLayerBtmLevel;
                soilLayerTopLevel = soilSection.SoilLayers[i].TopLevel;
                //Если слой последний, то низ слоя грунта это граница сжимаемой толщи
                if (i == soilCount-1) { soilLayerBtmLevel = foundationAbsBtmLevel - soilThickness; }
                //Иначе низ слоя грунта это верх следующего слоя
                else { soilLayerBtmLevel = soilSection.SoilLayers[i+1].TopLevel; }
                //Если низ слоя ниже фундамента, учитываем данный слой
                if (soilLayerBtmLevel < foundationAbsBtmLevel)
                {
                    //Если верх слоя грунта выше уровня подошвы фундамента, выравниваем верх по фундаменту
                    if (soilLayerTopLevel > foundationAbsBtmLevel) { soilLayerTopLevel = foundationAbsBtmLevel; }
                    //Создаем новый элементарный слой и добавляем его в коллекцию
                    SoilElementaryLayer soilElementaryLayer = new SoilElementaryLayer();
                    soilElementaryLayer.SoilId = soilSection.SoilLayers[i].SoilId;
                    soilElementaryLayer.Soil = soilSection.SoilLayers[i].Soil;
                    soilElementaryLayer.TopLevel = soilLayerTopLevel;
                    soilElementaryLayer.BottomLevel = soilLayerBtmLevel;
                    tmpSoilElementaryLayers.Add(soilElementaryLayer);
                }
                //Если низ слоя выше подошвы грунта, ничего добавлять не нужно
            }
            //Разбиваем слои грунта на подслои
            foreach (SoilElementaryLayer soilElementaryLayer in tmpSoilElementaryLayers)
            {
                double layerHeight = soilElementaryLayer.TopLevel - soilElementaryLayer.BottomLevel;
                //Если высота слоя превышает максимальную заданную высоту, то разбиваем слой на дополниетельные слои грунта
                if (layerHeight > maxHeight)
                {
                    int division = Convert.ToInt32(Math.Ceiling(layerHeight / maxHeight));
                    double locLayerHeight = layerHeight / division;
                    for (int i = 0; i<division; i++)
                    {
                        double tmpTopLevel = soilElementaryLayer.TopLevel - locLayerHeight * i;
                        double tmpBtmLevel = tmpTopLevel - locLayerHeight;
                        SoilElementaryLayer tmpSoilElementaryLayer = new SoilElementaryLayer();
                        tmpSoilElementaryLayer.Id = ProgrammSettings.CurrentTmpId;
                        tmpSoilElementaryLayer.SoilId = soilElementaryLayer.SoilId;
                        tmpSoilElementaryLayer.Soil = soilElementaryLayer.Soil;
                        tmpSoilElementaryLayer.TopLevel = tmpTopLevel;
                        tmpSoilElementaryLayer.BottomLevel = tmpBtmLevel;
                        soilElementaryLayers.Add(tmpSoilElementaryLayer);
                    }
                }
                //Иначе просто добавляем исходный слой
                else
                {
                    soilElementaryLayer.Id = ProgrammSettings.CurrentTmpId;
                    soilElementaryLayers.Add(soilElementaryLayer);
                }
            }
            //Для каждого из слоев назначаем наличие воды
            foreach (SoilElementaryLayer soilElementaryLayer in soilElementaryLayers)
            {
                if ((soilElementaryLayer.TopLevel < soilSection.NaturalWaterLevel) & (soilSection.HasWater)) { soilElementaryLayer.HasGroundWater = true; }
                else { soilElementaryLayer.HasGroundWater = false; }
            }
            return soilElementaryLayers;
        }
        /// <summary>
        /// Возвращает коэффициент уменьшения напряжений под фундаментом по глубине
        /// </summary>
        /// <param name="foundation"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double GetAlpha (Foundation foundation, double z)
        {
            double alpha;
            double[] foundationSizes = FoundationProcessor.GetDeltaDistance(foundation);
            double l, b;
            l = foundationSizes[0];
            b = foundationSizes[1];
            alpha = GetAlphaRect(l, b, z);
            return alpha;
        }
        /// <summary>
        /// Возвращает коэффициент уменьшения напряжений под фундаментом по глубине
        /// </summary>
        /// <param name="l">Больший размер подошвы</param>
        /// <param name="b">Меньший размер фундамента</param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double GetAlphaRect(double l, double b, double z)
        {
            double alpha, R, K1;
            if (b > l)
            {
                double tmpb = l;
                l = b;
                b = tmpb;
            }
            if (b < (0.1 * l)) b = 0.1 * l;
            b *= 0.5;
            l *= 0.5;
            R = Math.Sqrt(l * l + b * b + z * z);
            K1 = l * l + b * b + 2 * z * z;
            alpha = 2 / (Math.PI) * (Math.Atan2(b * l, z * R) + b * l * z * K1 / ((b * b + z * z) * (l * l + z * z) * R));
            return alpha;
        }
        /// <summary>
        /// Возвращает коллекцию сжатых слоев
        /// </summary>
        /// <param name="soilElementaryLayers">Коллекция слоев под подошвой фундамента</param>
        /// <param name="length">Длина фундамента</param>
        /// <param name="width">Ширина фундамента</param>
        /// <param name="sigmZg0">Давление от грунта</param>
        /// <param name="sigmZp0">Давление от внешней нагрузки</param>
        /// <param name="minSigmRatio">Минимальное отношение для сжимаемой толщи</param>
        /// <returns></returns>
        public static List<CompressedLayer> CompressedLayers(List<SoilElementaryLayer> soilElementaryLayers, double length, double width, double sigmZg0, double sigmZp0, double minSigmRatio = 0.5)
        {
            List<CompressedLayer> compressedLayers = new List<CompressedLayer>();
            double z = 0;
            double sigmZgI;
            double sigmZgammaI;
            double sigmZpI;
            double sumSettlement = 0;
            sigmZgI = sigmZg0;
            double zMin;

            if (width > length)
            {
                double tmpb = length;
                length = width;
                width = tmpb;
            }
            if ( width <= 10) { zMin = width / 2;}
            else if (width > 10 & width <= 60) { zMin = 4 + 0.1 * width; }
            else { zMin = 10; }

            int count = soilElementaryLayers.Count;
            for (int i = 0; i<count; i++)
            {
                SoilElementaryLayer soilElementaryLayer = soilElementaryLayers[i];
                double layerHeight = soilElementaryLayer.TopLevel - soilElementaryLayer.BottomLevel;
                //Если рассматриваемый слой первый, то ординату центра получаем как половину высоты слоя
                if (i == 0) z = layerHeight / 2;
                //иначе получаем ординату как половина высоты предыдущего слоя + половина высоты текущего слоя
                else z += (soilElementaryLayers[i - 1].TopLevel - soilElementaryLayers[i - 1].BottomLevel) / 2 + layerHeight /2;
                //Давление от собственного веса грунта
                sigmZgI -= layerHeight * SoilWeight(soilElementaryLayer)[3];
                double alpha = GetAlphaRect(length, width, z);
                //Давление от внешней нагрузки для слоя
                sigmZpI = alpha * sigmZp0;
                //Давление от веса грунта выше подошвы фундамента для слоя
                sigmZgammaI = alpha * sigmZg0;
                //Отношение давления от внешней нагрузки к давлению от собственного веса грунта
                double sigmRatio = sigmZpI / sigmZgI;

                if (!(soilElementaryLayer.Soil is BearingSoil)) { throw new Exception("В основании залегает слой с ненормируемыми характеристиками"); }
                double elasiticModulus = (soilElementaryLayer.Soil as BearingSoil).ElasticModulus;
                double sndElasitcModulus = (soilElementaryLayer.Soil as BearingSoil).SndElasticModulus;
                //глубина сжимаемой толщи определяется по отношению напряжений
                if ((sigmRatio > 0.2) || (sigmRatio > minSigmRatio) || (z < zMin)) //если граница не достигнута по СП или указанная пользователем, то считаем
                {
                    CompressedLayer compressedLayer = new CompressedLayer();
                    compressedLayer.SoilElementaryLayer = soilElementaryLayer;
                    if (
                            (sigmRatio > 0.5) //Если больше 0,5 для обычного грунта
                            || //или
                            ((sigmRatio > 0.2) & (elasiticModulus <= 7e6)) //больше 0,2 для сильно сжимаемых с модулем меньше 7МПа
                            || //или
                            (sigmRatio > minSigmRatio)//отношение больше минимального
                            ||
                            (z < zMin) //Глубина сжимаемой толщи меньше минимальной
                        )
                    //то учитываем осадку для слоя
                    {
                        double localSettlement;
                        //Если давление от внешней нагрузки меньше природного
                        if (sigmZp0 > sigmZg0) //знак больше, потому что сжатие имеет знак минус
                        {
                            //то считаем только по вторичной ветви
                            localSettlement = 0.8 * layerHeight * (sigmZpI / sndElasitcModulus);
                        }
                        else //Иначе считаем с учетом первичной и вторичной ветви
                        {
                            localSettlement = 0.8 * layerHeight * ((sigmZpI - sigmZgammaI) / elasiticModulus + sigmZgammaI / sndElasitcModulus);
                        }
                        compressedLayer.LocalSettlement = localSettlement;
                    }
                    else //иначе осадка слоя не учитывается
                    {
                        compressedLayer.LocalSettlement = 0;
                    }
                    compressedLayer.Zlevel = z;
                    compressedLayer.Alpha = alpha;
                    compressedLayer.SigmZg = sigmZgI;
                    compressedLayer.SigmZgamma = sigmZgammaI;
                    compressedLayer.SigmZp = sigmZpI;
                    compressedLayers.Add(compressedLayer);
                }

            }

            count = compressedLayers.Count;
            for (int i = count -1; i >= 0; i--)
            {
                sumSettlement += compressedLayers[i].LocalSettlement;
                compressedLayers[i].SumSettlement = sumSettlement;
            }
            return compressedLayers;
        }
        /// <summary>
        /// Возвращает плотность и вес грунта по 1-й и 2-й группе ПС
        /// </summary>
        /// <param name="soilElementaryLayer"></param>
        /// <returns></returns>
        public static double[] SoilWeight(SoilElementaryLayer soilElementaryLayer)
        {
            double [] weight = new double[4];

            if (soilElementaryLayer.HasGroundWater)
            {
                weight[0] = (soilElementaryLayer.Soil.FstDesignDensity - 1000) / (1 + soilElementaryLayer.Soil.PorousityCoef); //Плотность по 1й группе ПС с учетом взвешивающего действия воды
                weight[1] = (soilElementaryLayer.Soil.SndDesignDensity - 1000) / (1 + soilElementaryLayer.Soil.PorousityCoef); //Плотность по 2й группе ПС с учетом взвешивающего действия воды
                weight[2] = (soilElementaryLayer.Soil.FstDesignDensity * 9.81 - 9810) / (1 + soilElementaryLayer.Soil.PorousityCoef);  //Объемный вес по 1й группе ПС с учетом взвешивающего действия воды
                weight[3] = (soilElementaryLayer.Soil.SndDesignDensity * 9.81 - 9810) / (1 + soilElementaryLayer.Soil.PorousityCoef);  //Объемный вес по 2й группе ПС с учетом взвешивающего действия воды
            }
            else
            {
                weight[0] = soilElementaryLayer.Soil.FstDesignDensity;
                weight[1] = soilElementaryLayer.Soil.SndDesignDensity;
                weight[2] = soilElementaryLayer.Soil.FstDesignDensity * 9.81;
                weight[3] = soilElementaryLayer.Soil.SndDesignDensity * 9.81;
            }

            return weight;
        }
        /// <summary>
        /// Возвращает глубину сжимаемой толщи
        /// </summary>
        /// <param name="compressedLayers">Коллекция слоев грунта</param>
        /// <returns></returns>
        public static double ComressedHeight(List<CompressedLayer> compressedLayers)
        {
            double comressedHeight = 0;
            int i = compressedLayers.Count-1;
            while (compressedLayers[i].SumSettlement == 0)
            {
                i--;
            }
            comressedHeight = compressedLayers[0].SoilElementaryLayer.TopLevel - compressedLayers[i].SoilElementaryLayer.BottomLevel;
            return comressedHeight;
        }
        /// <summary>
        /// Возвращает значения коэффициента K по таблице 5.9 СП22.13330.2011
        /// </summary>
        /// <param name="length">Размер фундамента вдоль оси Y</param>
        /// <param name="width">Размер фундамента вдоль оси X</param>
        /// <returns>Массив 0 - коэффициент вдоль оси X, 1- вдоль оси Y</returns>
        private static double[] GetK_5_9_Coff(double length, double width)
        {
            double[] cofficient = new double[2] { 0, 0 };
            double lengthTmp = length;
            double widthTmp = width;
            if (width > length)
            {
                lengthTmp = width;
                widthTmp = length;
            }
            double betta = lengthTmp / widthTmp;
            if (betta > 10) betta = 10;
            List<double> coff_5_9_0 = new List<double> { 1, 1.2, 1.5, 2.0, 3.0, 5.0, 10.0 };
            List<double> coff_5_9_1 = new List<double> { 0.5, 0.57, 0.68, 0.82, 1.17, 1.42, 2.0 };
            List<double> coff_5_9_2 = new List<double> { 0.5, 0.43, 0.36, 0.28, 0.20, 0.12, 0.07 };
            if (width > length)
            {
                cofficient[0] = MathOperation.InterpolateList(coff_5_9_0, coff_5_9_1, betta);
                cofficient[1] = MathOperation.InterpolateList(coff_5_9_0, coff_5_9_2, betta);
            }
            else
            {
                cofficient[0] = MathOperation.InterpolateList(coff_5_9_0, coff_5_9_2, betta);
                cofficient[1] = MathOperation.InterpolateList(coff_5_9_0, coff_5_9_1, betta);
            }
            return cofficient;
        }
        /// <summary>
        /// Возвращает значения коэффициента K по таблице 5.9 СП22.13330.2011
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <returns>Массив 0 - коэффициент вдоль оси X, 1- вдоль оси Y</returns>
        private static double[] GetK_5_9_Coff(Foundation foundation)
        {
            double[] sizes = FoundationProcessor.GetContourSize(foundation);
            double[] cofficient = GetK_5_9_Coff(sizes[1], sizes[0]);
            return cofficient;
        }
        /// <summary>
        /// Возвращает осредненное значение жесткости слоев грунта в пределах сжимаемой толщи в соответствии с п.5.6.45 СП22.1330.2011
        /// </summary>
        /// <param name="compressedLayers">Коллекция сжатых слоев</param>
        /// <returns></returns>
        private static double AvgPoisonCoff(List<CompressedLayer> compressedLayers)
        {
            double sumA = 0;
            double sumD = 0;
            foreach (CompressedLayer compressedLayer in compressedLayers)
            {
                if (compressedLayer.LocalSettlement != 0)
                {
                    double ai = compressedLayer.SigmZp * (compressedLayer.SoilElementaryLayer.TopLevel - compressedLayer.SoilElementaryLayer.BottomLevel);
                    BearingSoil bearingSoil = compressedLayer.SoilElementaryLayer.Soil as BearingSoil;
                    double pR = bearingSoil.PoissonRatio;
                    double di = ai * (1 - pR * pR) / bearingSoil.ElasticModulus;
                    sumD += di;
                    sumA += ai;
                }
            }
            //Если сжатых слоев в коллекции вообще нет, выдаем исключение
            if (sumA == 0) throw new Exception("В основании отсутствуют сжатые слои");
            double dAvg = sumD/sumA;
            return dAvg;
        }
        /// <summary>
        /// Возвращает пару кренов для фундамента
        /// </summary>
        /// <param name="Mx">Момент относительно оси X</param>
        /// <param name="My">Момент относительно оси Y</param>
        /// <param name="compressedLayers">Коллекция сжатых слоев грунта</param>
        /// <param name="foundation">Фундамент</param>
        /// <returns></returns>
        public static double[] Inclination(double Mx, double My, List<CompressedLayer> compressedLayers, Foundation foundation)
        {
            double[] inclination = new double[2] { 0, 0 };
            double[] sizes = FoundationProcessor.GetContourSize(foundation);
            double[] k = GetK_5_9_Coff(foundation);
            double d = AvgPoisonCoff(compressedLayers);
            if (Mx != 0 )
            {
                double length = sizes[1];
                length /= 2;
                inclination[0] = d * k[1] * Mx / (length * length * length);
            }
            if (My != 0)
            {
                double length = sizes[0];
                length /= 2;
                inclination[1] = d * k[0] * My / (length * length * length);
            }
            return inclination;
        }
    }
}
