using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Soils;
using RDBLL.Entity.RCC.Foundations;
using RDBLL.Entity.RCC.Foundations.Processors;

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
        public static List<SoilElementaryLayer> LayersFromSection(Foundation foundation, double soilThickness = 50, double maxHeight = 0.2)
        {
            double[] foundationSizes = FoundationProcessor.GetDeltaDistance(foundation);
            double absZeroLevel = foundation.Level.Building.AbsoluteLevel - foundation.Level.Building.RelativeLevel;
            double foundationAbsTopLevel = absZeroLevel + foundation.RelativeTopLevel;
            double foundationAbsBtmLevel = foundationAbsTopLevel - foundationSizes[2];
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
    }
}
