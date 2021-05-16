using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Factories
{
    public enum PunchingType
    {
        /// <summary>
        /// Средняя колонна 400х400мм, толщина перекрытия 200мм
        /// </summary>
        TestType1_400х400х200,
        /// <summary>
        /// Средняя колонна 400х600мм, толщина перекрытия 300мм.
        /// </summary>
        TestType2_400х600х300,
        /// <summary>
        /// Средняя колонна 600х400мм, толщина переккрытия 400мм.
        /// </summary>
        TestType3_600х400х400,
        /// <summary>
        /// Колонна 400х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани
        /// </summary>
        Edge1,
        /// <summary>
        /// Колонна 400х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани с отступом 50мм
        /// </summary>
        Edge1_offset50,
        /// <summary>
        /// Колонна 400х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани с отступом 100мм
        /// </summary>
        Edge1_offset100,
        /// <summary>
        /// Колонна 400х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани с отступом 150мм
        /// </summary>
        Edge1_offset150,
        /// <summary>
        /// Колонна 400х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани с отступом 200мм
        /// </summary>
        Edge1_offset200,
        /// <summary>
        /// Колонна 400х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани с отступом 250мм
        /// </summary>
        Edge1_offset250,
        /// <summary>
        /// Колонна 400х600мм, толщина перекрытия 200мм, колонна расположена у нижней грани
        /// </summary>
        Edge2,
        /// <summary>
        /// Колонна 600х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани
        /// </summary>
        Edge3,
        /// <summary>
        /// Колонна 600х400мм, толщина перекрытия 200мм, колонна расположена у левой грани
        /// </summary>
        Edge4,
        Angle1,
        Angle2,
        Angle3,
        DropPanel1,
        DropPanel2,
        DropPanelEdge1,
        DropPanelEdge2,
        DropPanelAngle1,
    }
    /// <summary>
    /// Фабрика создания тестовых вариантов для расчета на продавливание
    /// </summary>
    public static class TestCaseFactory
    {
        /// <summary>
        /// Статический класс простой фабрики
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Punching GetPunching(PunchingType type)
        {
            switch (type)
            {
                case PunchingType.TestType1_400х400х200:
                    {
                        Punching punching = new Punching(true);
                        punching.Name = "Продавливание прямоугольной колонной";
                        punching.Width = 0.4;
                        punching.Length = 0.4;
                        punching.LeftEdge = false;
                        punching.RightEdge = false;
                        punching.TopEdge = false;
                        punching.BottomEdge = false;
                        punching.CoveringLayerX = 0.03;
                        punching.CoveringLayerY = 0.04;
                        PunchingLayer layer = new PunchingLayer(true);
                        layer.RegisterParent(punching);
                        layer.Name = "Верхний слой";
                        layer.Height = 0.2;
                        ConcreteUsing concrete = new ConcreteUsing(layer);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Plate";
                        concrete.SelectedId = 6;
                        concrete.AddGammaB1();
                        layer.Concrete = concrete;
                        return punching;
                    }
                case PunchingType.TestType2_400х600х300:
                    {
                        Punching punching = new Punching(true);
                        punching.Name = "Продавливание прямоугольной колонной";
                        punching.Width = 0.4;
                        punching.Length = 0.6;
                        punching.LeftEdge = false;
                        punching.RightEdge = false;
                        punching.TopEdge = false;
                        punching.BottomEdge = false;
                        punching.CoveringLayerX = 0.03;
                        punching.CoveringLayerY = 0.04;
                        PunchingLayer layer = new PunchingLayer(true);
                        layer.RegisterParent(punching);
                        layer.Name = "Верхний слой";
                        layer.Height = 0.3;
                        ConcreteUsing concrete = new ConcreteUsing(layer);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Plate";
                        concrete.SelectedId = 6;
                        concrete.AddGammaB1();
                        layer.Concrete = concrete;
                        return punching;
                    }
                case PunchingType.TestType3_600х400х400:
                    {
                        Punching punching = new Punching(true);
                        punching.Name = "Продавливание прямоугольной колонной";
                        punching.Width = 0.6;
                        punching.Length = 0.4;
                        punching.LeftEdge = false;
                        punching.RightEdge = false;
                        punching.TopEdge = false;
                        punching.BottomEdge = false;
                        punching.CoveringLayerX = 0.03;
                        punching.CoveringLayerY = 0.04;
                        PunchingLayer layer = new PunchingLayer(true);
                        layer.RegisterParent(punching);
                        layer.Name = "Верхний слой";
                        layer.Height = 0.4;
                        ConcreteUsing concrete = new ConcreteUsing(layer);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Plate";
                        concrete.SelectedId = 6;
                        concrete.AddGammaB1();
                        layer.Concrete = concrete;
                        return punching;
                    }
                // Колонна 400х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани
                case PunchingType.Edge1:
                    {
                        Punching punching = new Punching(true);
                        punching.Name = "Продавливание прямоугольной колонной";
                        punching.Width = 0.4;
                        punching.Length = 0.4;
                        punching.LeftEdge = false;
                        punching.RightEdge = false;
                        punching.TopEdge = false;
                        punching.BottomEdge = true;
                        punching.CoveringLayerX = 0.03;
                        punching.CoveringLayerY = 0.04;
                        PunchingLayer layer = new PunchingLayer(true);
                        layer.RegisterParent(punching);
                        layer.Name = "Верхний слой";
                        layer.Height = 0.2;
                        ConcreteUsing concrete = new ConcreteUsing(layer);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Plate";
                        concrete.SelectedId = 6;
                        concrete.AddGammaB1();
                        layer.Concrete = concrete;
                        return punching;
                    }
                // Колонна 400х400мм, толщина перекрытия 200мм, колонна расположена у нижней грани
                case PunchingType.Edge1_offset250:
                    {
                        Punching punching = new Punching(true);
                        punching.Name = "Продавливание прямоугольной колонной";
                        punching.Width = 0.4;
                        punching.Length = 0.4;
                        punching.LeftEdge = false;
                        punching.RightEdge = false;
                        punching.TopEdge = false;
                        punching.BottomEdge = true;
                        punching.BottomEdgeDist = 0.25;
                        punching.CoveringLayerX = 0.03;
                        punching.CoveringLayerY = 0.04;
                        PunchingLayer layer = new PunchingLayer(true);
                        layer.RegisterParent(punching);
                        layer.Name = "Верхний слой";
                        layer.Height = 0.2;
                        ConcreteUsing concrete = new ConcreteUsing(layer);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Plate";
                        concrete.SelectedId = 6;
                        concrete.AddGammaB1();
                        layer.Concrete = concrete;
                        return punching;
                    }
                default:
                    {
                        throw new Exception("Type is not valid");
                    }
            }
        }
    }
}
