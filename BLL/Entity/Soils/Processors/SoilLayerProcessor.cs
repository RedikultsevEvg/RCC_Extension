using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Soils;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.RCC.Foundations.Processors;
using RDBLL.Common.Service;

namespace RDBLL.Entity.Soils.Processors
{
    /// <summary>
    /// Процессор слоев грунта
    /// </summary>
    public class SoilLayerProcessor
    {
        /// <summary>
        /// Класс сжатого слоя
        /// </summary>
        public class CompressedLayer
        {
            /// <summary>
            /// Ссылка на элементарный слой
            /// </summary>
            public SoilElementaryLayer SoilElementaryLayer { get; set; }
            public double SigmZg { get; set; }
            public double SigmZgamma { get; set; }
            public double SigmZp { get; set; }
            /// <summary>
            /// Осадка элементарного слоя
            /// </summary>
            public double LocalSettlement { get; set; }
            /// <summary>
            /// Осадка нарастающим итогом
            /// </summary>
            public double SumSettlement { get; set; }
        }
        /// <summary>
        /// Возвращает слои грунта по фундаменту и заданной глубине
        /// </summary>
        /// <param name="foundation">Фундамент</param>
        /// <param name="soilThickness">Заданная глубина, по умолчанию 50м</param>
        /// <param name="maxHeight">Максимальная толщина слоя грунта</param>
        /// <returns></returns>
        public static List<SoilElementaryLayer> LayersFromSection(Foundation foundation, double soilThickness = 50, double maxHeight = 0.2)
        {
            double[] levels = FoundationProcessor.FoundationLevels(foundation);
            double foundationAbsBtmLevel = levels[3];
            List<SoilElementaryLayer> soilElementaryLayers = new List<SoilElementaryLayer>();
            List<SoilElementaryLayer> tmpSoilElementaryLayers = new List<SoilElementaryLayer>();
            SoilSection soilSection = foundation.SoilSection;
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
            double alpha, alpha1, alpha2, D, R, K1, K2, K3;
            if (b < l)
            {
                double tmpb = l;
                l = b;
                b = tmpb;
            }
            R = Math.Sqrt(l * l + b * b + z * z);
            D = 2 * R;
            K1 = l * b * z / D;
            K2 = (l * l + b * b + 2 * z * z) / (D * D * z * z + l * l * b * b);
            alpha1 = K1 * K2;
            K3 = l * b / (Math.Sqrt((l * l + z * z) * (b * b + z * z)));
            alpha2 = Math.Asin(K3);
            alpha = 2 / (Math.PI) * (alpha1 + alpha2);
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

            if (width < length)
            {
                double tmpb = length;
                length = width;
                width = tmpb;
            }
            if ( width <= 10) { zMin = width / 2;}
            else if (width > 10 & width <= 60) { zMin = 4 + 0.1 * width; }
            else { zMin = 10; }

            foreach (SoilElementaryLayer soilElementaryLayer in soilElementaryLayers)
            {
                double layerHeight = soilElementaryLayer.TopLevel - soilElementaryLayer.BottomLevel;
                z += layerHeight / 2;
                //Давление от собственного веса грунта
                sigmZgI -= layerHeight * SoilWeight(soilElementaryLayer)[1];
                double alpha = GetAlphaRect(length, width, z);
                //Давление от внешней нагрузки для слоя
                sigmZpI = alpha * sigmZp0;
                //Давление от веса грунта выше подошвы фундамента для слоя
                sigmZgammaI = alpha * sigmZg0;
                //Отношение давления от внешней нагрузки к давлению от собственного веса грунта
                double sigmRatio = sigmZpI / sigmZgI;

                if (! (soilElementaryLayer.Soil is BearingSoil)) { throw new Exception("В основании залегает слой с ненормируемыми характеристиками"); }
                double elasiticModulus = (soilElementaryLayer.Soil as BearingSoil).ElasticModulus;
                double sndElasitcModulus = (soilElementaryLayer.Soil as BearingSoil).SndElasticModulus;
                //глубина сжимаемой толщи определяется по отношению напряжений
                if ((sigmRatio > 0.2) || (sigmRatio > minSigmRatio) || (z<zMin)) //если граница не достигнута по СП или указанная пользователем, то считаем
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
                    compressedLayer.SigmZg = sigmZgI;
                    compressedLayer.SigmZgamma = sigmZgammaI;
                    compressedLayer.SigmZp = sigmZpI;
                    compressedLayers.Add(compressedLayer);
                }

            }
            int count = compressedLayers.Count;
            for (int i = count -1; i >= 0; i--)
            {
                sumSettlement += compressedLayers[i].LocalSettlement;
                compressedLayers[i].SumSettlement = sumSettlement;
            }
            return compressedLayers;
        }
        public static double[] SoilWeight(SoilElementaryLayer soilElementaryLayer)
        {
            double [] weight = new double[2];
            weight[0] = soilElementaryLayer.Soil.FstDesignDensity;
            weight[1] = soilElementaryLayer.Soil.SndDesignDensity;
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
    }
}
