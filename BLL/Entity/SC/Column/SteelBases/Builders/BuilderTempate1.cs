using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Placements.Factory;
using RDBLL.Entity.SC.Column.SteelBases.Factories;
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
            BoltFactProc.GetBoltsType1(_SteelBase, 0.03, width, length, 2, 2);
        }

        /// <summary>
        /// Добавление нагрузок
        /// </summary>
        public override void AddLoads()
        {
            Forces.Factories.Factory.ForceGroupFactory(_SteelBase, Forces.Factories.ForceType.N100MX200);
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
            AddParts();
            AddBolts();
            AddLoads();
            return _SteelBase;
        }
    }
}
