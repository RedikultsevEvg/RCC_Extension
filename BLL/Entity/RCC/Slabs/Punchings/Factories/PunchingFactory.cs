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
        TestType1_400х400х200,
        TestType2_400х600х300,
        TestType3_600х400х400
    }
    public static class PunchingFactory
    {
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
                        punching.CoveringLayerX = 0.03;
                        punching.CoveringLayerY = 0.04;
                        PunchingLayer layer = new PunchingLayer(true);
                        layer.RegisterParent(punching);
                        layer.Name = "Верхний слой";
                        layer.Height = 0.2;
                        ConcreteUsing concrete = new ConcreteUsing(layer);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Plate";
                        concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
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
                        punching.CoveringLayerX = 0.03;
                        punching.CoveringLayerY = 0.04;
                        PunchingLayer layer = new PunchingLayer(true);
                        layer.RegisterParent(punching);
                        layer.Name = "Верхний слой";
                        layer.Height = 0.3;
                        ConcreteUsing concrete = new ConcreteUsing(layer);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Plate";
                        concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
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
                        punching.CoveringLayerX = 0.03;
                        punching.CoveringLayerY = 0.04;
                        PunchingLayer layer = new PunchingLayer(true);
                        layer.RegisterParent(punching);
                        layer.Name = "Верхний слой";
                        layer.Height = 0.4;
                        ConcreteUsing concrete = new ConcreteUsing(layer);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Plate";
                        concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
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
