using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Placements.Factory;

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
            _SteelBase.SteelBolts = new ObservableCollection<SteelBolt>();
            SteelBolt steelBolt = new SteelBolt();
            steelBolt.Name = "Болт №_";
            BoltUsing boltUsing = new BoltUsing(steelBolt);
            steelBolt.BoltUsing = boltUsing;
            boltUsing.Diameter = 0.03;
            RectArrayPlacement placement = PlacementFactory.GetPlacement(PcmType.Rect2x2x0) as RectArrayPlacement;
            boltUsing.SetPlacement(placement);
        }

        /// <summary>
        /// Добавление нагрузок
        /// </summary>
        public override void AddLoads()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Добавление материалов
        /// </summary>
        public override void AddMaterial()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Добавление участков
        /// </summary>
        public override void AddParts()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Создание геометрии
        /// </summary>
        public override void CreateGeometry()
        {
            _SteelBase.Name = "Новая база";
            _SteelBase.Thickness = 0.06;
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
