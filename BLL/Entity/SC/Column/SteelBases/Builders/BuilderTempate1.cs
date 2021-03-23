using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Placements.Factory;
using RDBLL.Entity.SC.Column.SteelBases.Factorys;
using RDBLL.Entity.Common.Materials;
using RDBLL.Forces;
using RDBLL.Common.Service;


namespace RDBLL.Entity.SC.Column.SteelBases.Builders
{
    /// <summary>
    /// Создает шаблон стальной базы
    /// </summary>
    public class BuilderTempate1 : BuilderBase
    {
        private SteelBase _SteelBase = new SteelBase(true);
        /// <summary>
        /// Добавление болтов
        /// </summary>
        public override void AddBolts()
        {
            double width = 0.4;
            double length = 0.7;
            BoltFactProc.GetBoltsType1(_SteelBase, width, length, 2, 2);
        }

        /// <summary>
        /// Добавление нагрузок
        /// </summary>
        public override void AddLoads()
        {
            LoadSet loadSet = new LoadSet(_SteelBase.ForcesGroups[0]);
            _SteelBase.ForcesGroups[0].LoadSets.Add(loadSet);
            loadSet.Name = "Постоянная";
            loadSet.ForceParameters.Add(new ForceParameter(loadSet));
            loadSet.ForceParameters[0].KindId = 1; //Продольная сила
            loadSet.ForceParameters[0].CrcValue = -100000; //Продольная сила
            loadSet.ForceParameters.Add(new ForceParameter(loadSet));
            loadSet.ForceParameters[1].KindId = 2; //Изгибающий момент
            loadSet.ForceParameters[1].CrcValue = 200000; //Изгибающий момент
            loadSet.IsLiveLoad = false;
            loadSet.BothSign = false;
            loadSet.PartialSafetyFactor = 1.1;
        }
        /// <summary>
        /// Добавление материалов
        /// </summary>
        public override void AddMaterial()
        {
            SteelUsing steel = new SteelUsing(_SteelBase);
            steel.Name = "Сталь";
            steel.Purpose = "BaseSteel";
            steel.SelectedId = ProgrammSettings.SteelKinds[0].Id;
            _SteelBase.Steel = steel;
            ConcreteUsing concrete = new ConcreteUsing(_SteelBase);
            concrete.Name = "Бетон";
            concrete.Purpose = "Filling";
            concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
            concrete.AddGammaB1();
            _SteelBase.Concrete = concrete;

        }
        /// <summary>
        /// Добавление участков
        /// </summary>
        public override void AddParts()
        {
            double width = 0.6;
            double length = 0.9;
            double interLength = 0.5;
            PartFactProc.GetPartsType1(_SteelBase, width, length, interLength);
        }
        /// <summary>
        /// Создание геометрии
        /// </summary>
        public override void CreateGeometry()
        {
            _SteelBase.Name = "Новая база";
            _SteelBase.Height = 0.06;
            _SteelBase.IsActual = false;
        }

        public override SteelBase GetSteelBase()
        {
            CreateGeometry();
            AddMaterial();
            AddParts();
            AddBolts();
            AddLoads();
            return _SteelBase;
        }
    }
}
