using RDBLL.Entity.SC.Column.SteelBases.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Builders
{
    public class BuilderPattern2 : BuilderBase
    {
        private SteelBase _SteelBase = new SteelBase(true);
        /// <summary>
        /// Добавление болтов
        /// </summary>
        public override void AddBolts() { throw new NotImplementedException(); }
        /// <summary>
        /// Добавление нагрузок
        /// </summary>
        public override void AddLoads() { Forces.Factories.Factory.ForceGroupFactory(_SteelBase, Forces.Factories.ForceType.N100MX200); }
        /// <summary>
        /// Добавление участков
        /// </summary>
        public override void AddParts() { throw new NotImplementedException(); }
        /// <summary>
        /// Создание геометрии
        /// </summary>
        public override void CreateGeometry()
        {
            PatternBase Pattern = new PatternType2(true);
            Pattern.RegisterParent(_SteelBase);
            _SteelBase.Pattern = Pattern; ;
        }

        public override SteelBase GetSteelBase()
        {
            CreateGeometry();
            AddLoads();
            return _SteelBase;
        }
    }
}
