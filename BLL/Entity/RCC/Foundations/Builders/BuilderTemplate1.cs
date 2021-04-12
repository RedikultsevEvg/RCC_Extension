using RDBLL.Entity.RCC.Foundations.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Builders
{
    /// <summary>
    /// Создает столбчатый фундамент с настройками по умолчанию
    /// </summary>
    public class BuilderTemplate1 : BuilderBase
    {
        /// <summary>
        /// Закрытое поле для нового фундамента
        /// </summary>
        private Foundation _Foundation = new Foundation(true);
        /// <summary>
        /// Добавляет бетон к фундаменту
        /// </summary>
        public override void AddConcrete() { ConcreteFactProc.AddConcrete(_Foundation);}
        /// <summary>
        /// Добавляет нагрузки к фундаменту
        /// </summary>
        public override void AddLoads() { Forces.Factories.Factory.ForceGroupFactory(_Foundation, Forces.Factories.ForceType.N1000MX200MY50); }
        /// <summary>
        /// Добавляет ступени
        /// </summary>
        public override void AddParts()
        {
            RectFoundationPart foundationPart;
            foundationPart = new RectFoundationPart(_Foundation);
            foundationPart.Width = 1.2;
            foundationPart.Length = 1.2;
            foundationPart.Height = 1.5;
            _Foundation.Parts.Add(foundationPart);
            foundationPart = new RectFoundationPart(_Foundation);
            foundationPart.Width = 2.4;
            foundationPart.Length = 2.4;
            foundationPart.Height = 0.3;
            _Foundation.Parts.Add(foundationPart);
        }
        /// <summary>
        /// Добавляет армирование к фундаменту
        /// </summary>
        public override void AddReinforcement() { ReinforcementFactProc.GetReinforcement(_Foundation);}
        public override Foundation GetFoundation()
        {
            AddCommnon(_Foundation);
            AddParts();
            AddReinforcement();
            AddConcrete();
            AddLoads();
            AddSoilSection(_Foundation);
            return _Foundation;
        }
    }
}
