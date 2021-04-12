using RDBLL.Entity.RCC.Foundations.Factories;
using RDBLL.Entity.Soils;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Builders
{
    /// <summary>
    /// Создает фундамент для верификационного примера VM1
    /// </summary>
    public class BuilderVM1 : BuilderBase
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
            forceParameter.CrcValue = -276000; //Продольная сила
            loadSet.ForceParameters.Add(forceParameter);
            forcesGroup.LoadSets.Add(loadSet);
            //Снеговая нагрузка
            loadSet = new LoadSet(forcesGroup);
            loadSet.Name = "Снеговая";
            loadSet.PartialSafetyFactor = 1.4;
            forceParameter = new ForceParameter();
            forceParameter.KindId = 1; //Продольная сила
            forceParameter.CrcValue = -106000; //Продольная сила
            loadSet.ForceParameters.Add(forceParameter);
            forcesGroup.LoadSets.Add(loadSet);
            //Кратковременная нагрузка
            loadSet = new LoadSet(forcesGroup);
            loadSet.Name = "Кратковременная";
            loadSet.PartialSafetyFactor = 1.2;
            forceParameter = new ForceParameter();
            forceParameter.KindId = 1; //Продольная сила
            forceParameter.CrcValue = -46000; //Продольная сила
            loadSet.ForceParameters.Add(forceParameter);
            forcesGroup.LoadSets.Add(loadSet);
            //Ветровая нагрузка по X
            loadSet = new LoadSet(forcesGroup);
            loadSet.Name = "Ветровая по X";
            loadSet.PartialSafetyFactor = 1.4;
            forceParameter = new ForceParameter();
            forceParameter.KindId = 3; //Момент
            forceParameter.CrcValue = -41300; //Момент
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 4; //Поперечная сила
            forceParameter.CrcValue = -2000; //Поперечная сила
            loadSet.ForceParameters.Add(forceParameter);
            forcesGroup.LoadSets.Add(loadSet);
            //Ветровая нагрузка по Y
            loadSet = new LoadSet(forcesGroup);
            loadSet.Name = "Ветровая по Y";
            loadSet.PartialSafetyFactor = 1.4;
            forceParameter = new ForceParameter();
            forceParameter.KindId = 2; //Момент
            forceParameter.CrcValue = -35000; //Момент
            loadSet.ForceParameters.Add(forceParameter);

            forceParameter = new ForceParameter();
            forceParameter.KindId = 5; //Поперечная сила
            forceParameter.CrcValue = -9100; //Поперечная сила
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
            foundationPart.Length = 0.8;
            foundationPart.Height = 2.9;
            _Foundation.Parts.Add(foundationPart);
            foundationPart = new RectFoundationPart(_Foundation);
            foundationPart.Width = 1.2;
            foundationPart.Length = 1.8;
            foundationPart.Height = 0.45;
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
