using RDBLL.Common.ErrorProcessing.Messages;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Reinforcements.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.Reinforcements.Bars.Factories;
using RDBLL.Common.Service;

namespace RDBLL.Entity.RCC.Reinforcements.Ancorages.Factories
{
    /// <summary>
    /// Типы анкеровки для фабрики
    /// </summary>
    public enum AncorageType
    {
        /// <summary>
        /// Пустая анкеровка
        /// </summary>
        Empty,
        /// <summary>
        /// Анкеровка для одного стержня
        /// </summary>
        OneBar,
        /// <summary>
        /// Анкеровка для нескольких стержней
        /// </summary>
        MultyBar,
    }
    /// <summary>
    /// Фабрика расчетов длины анкеровки
    /// </summary>
    public static class AncorageFactory
    {
        /// <summary>
        /// Возвращает расчет длины анкеровки
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IAncorage GetAncorage(AncorageType type)
        {
            if (type == AncorageType.Empty)
            {
                return GetEmptyAncorage();
            }
            //Возвращаем расчет для одного стержня
            if (type == AncorageType.OneBar)
            {
                return GetOneBarAncorage();
            }
            //Возвращает расчет для нескольких стержней
            else if (type == AncorageType.MultyBar)
            {
                return GetMultyBarAncorage();
            }
            //Если тип не распознан
            else
            {
                throw new Exception(CommonMessages.TypeIsUknown);
            }

        }

        private static IAncorage GetEmptyAncorage()
        {
            IAncorageLogic ancorageLogic = new AncorageLogic();
            IAncorage ancorage = new Ancorage(ancorageLogic, true);
            ancorage.Name = "Расчет анкеровки";
            ancorage.LongLoadRate = 0.9;
            return ancorage;
        }

        /// <summary>
        /// Возвращает расчет анкеровки для нескольких стержней
        /// </summary>
        /// <returns></returns>
        private static IAncorage GetMultyBarAncorage()
        {
            IAncorageLogic ancorageLogic = new AncorageLogic();
            IAncorage ancorage = new Ancorage(ancorageLogic, true);
            ancorage.Name = "Расчет анкеровки";
            ancorage.LongLoadRate = 0.9;
            ConcreteUsing concrete = new ConcreteUsing(true);
            concrete.SelectedId = 5;
            concrete.AddGammaB1();
            concrete.RegisterParent(ancorage);
            ancorage.Concrete = concrete;
            IBarSection barSection;
            //Добавляем стержень диаметром 12мм
            barSection = BarSectionFactory.GetBarSection(BarType.BarWithoutPlace);
            barSection.Circle.Diameter = 0.012;
            barSection.Name = $"Стержень диаметром {barSection.Circle.Diameter * 1000}мм";
            barSection.RegisterParent(ancorage);
            //Добавляем стержень диаметром 16мм
            barSection = BarSectionFactory.GetBarSection(BarType.BarWithoutPlace);
            barSection.Circle.Diameter = 0.016;
            barSection.Name = $"Стержень диаметром {barSection.Circle.Diameter * 1000}мм";
            barSection.RegisterParent(ancorage);
            //Добавляем стержень диаметром 20мм
            barSection = BarSectionFactory.GetBarSection(BarType.BarWithoutPlace);
            barSection.Circle.Diameter = 0.020;
            barSection.Name = $"Стержень диаметром {barSection.Circle.Diameter * 1000}мм";
            barSection.RegisterParent(ancorage);
            return ancorage;
        }
        /// <summary>
        /// Возвращает расчет анкеровки для одного стержня
        /// </summary>
        /// <returns></returns>
        private static IAncorage GetOneBarAncorage()
        {
            IAncorageLogic ancorageLogic = new AncorageLogic();
            IAncorage ancorage = new Ancorage(ancorageLogic, true);
            ancorage.Name = "Расчет анкеровки";
            ancorage.LongLoadRate = 0.9;
            ConcreteUsing concrete = new ConcreteUsing(true);
            concrete.SelectedId = 5;
            concrete.AddGammaB1();
            concrete.RegisterParent(ancorage);
            ancorage.Concrete = concrete;
            IBarSection barSection = BarSectionFactory.GetBarSection(BarType.BarWithoutPlace);
            barSection.Name = $"Стержень диаметром {barSection.Circle.Diameter * 1000}мм";
            barSection.RegisterParent(ancorage);
            return ancorage;
        }
    }
}
