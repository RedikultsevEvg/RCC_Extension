using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Factories
{
    /// <summary>
    /// Фабрика расчета на продавливание для центральной колонны
    /// </summary>
    public class CentralColumnPunching : IPunchingFactory
    {
        /// <summary>
        /// Создание расчета на продавливание
        /// </summary>
        public Punching CreatePunching()
        {
            //Создаем новый расчет на продавливание
            Punching punching = new Punching(true);
            punching.Name = "Продавливание прямоугольной колонной";
            punching.Width = 0.4;
            punching.Length = 0.4;
            punching.CoveringLayerX = 0.03;
            punching.CoveringLayerY = 0.04;
            //Добавляем один слой расчета
            PunchingLayer layer = new PunchingLayer(true);
            layer.RegisterParent(punching);
            layer.Name = "Верхний слой";
            layer.Height = 0.2;
            //Добавляем бетон
            ConcreteUsing concrete = new ConcreteUsing(layer);
            concrete.Name = "Бетон";
            concrete.Purpose = "Plate";
            concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
            concrete.AddGammaB1();
            layer.Concrete = concrete;
            //Добавляем нагрузку
            Forces.Factories.Factory.ForceGroupFactory(punching, Forces.Factories.ForceType.N100MX200);
            return punching;
        }
    }
}
