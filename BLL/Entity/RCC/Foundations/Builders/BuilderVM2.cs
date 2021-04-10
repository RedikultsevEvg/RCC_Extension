using RDBLL.Entity.RCC.Foundations.Factories;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Builders
{
    public class BuilderVM2 : BuilderBase
    {
        /// <summary>
        /// Закрытое поле для нового фундамента
        /// </summary>
        private Foundation _Foundation = new Foundation(true);
        /// <summary>
        /// Добавляет бетон к фундаменту
        /// </summary>
        public override void AddConcrete() { ConcreteFactProc.AddConcrete(_Foundation); }
        /// <summary>
        /// Добавляет нагрузки к фундаменту
        /// </summary>
        public override void AddLoads()
        {
            ForcesGroup forcesGroup = _Foundation.ForcesGroups[0];
            //Постоянная нагрузка
            LoadSet loadSet = new LoadSet(forcesGroup);
            loadSet.Name = "Постоянная";
            loadSet.PartialSafetyFactor = 1.1;
            ForceParameter forceParameter;
            forceParameter = new ForceParameter();
            forceParameter.KindId = 1; //Продольная сила
            forceParameter.CrcValue = -290000; //Продольная сила
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 2; //Момент Mx
            forceParameter.CrcValue = 20000; //Момент Mx
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 3; //Момент My
            forceParameter.CrcValue = 97000; //Момент My
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 4; //Поперечная сила Qx
            forceParameter.CrcValue = 90000; //Поперечная сила Qx
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 5; //Поперечная сила Qy
            forceParameter.CrcValue = -5000; //Поперечная сила Qy
            loadSet.ForceParameters.Add(forceParameter);
            forcesGroup.LoadSets.Add(loadSet);
        }
        /// <summary>
        /// Добавляет ступени
        /// </summary>
        public override void AddParts()
        {
            RectFoundationPart foundationPart;
            foundationPart = new RectFoundationPart(_Foundation);
            foundationPart.Width = 0.6;
            foundationPart.Length = 0.6;
            foundationPart.Height = 1.3;
            _Foundation.Parts.Add(foundationPart);
            foundationPart = new RectFoundationPart(_Foundation);
            foundationPart.Width = 2.4;
            foundationPart.Length = 1.8;
            foundationPart.Height = 0.3;
            _Foundation.Parts.Add(foundationPart);
        }
        /// <summary>
        /// Добавляет армирование к фундаменту
        /// </summary>
        public override void AddReinforcement() { ReinforcementFactProc.GetReinforcement(_Foundation); }
        /// <summary>
        /// Возвращает созданный фундамент
        /// </summary>
        /// <returns></returns>
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
