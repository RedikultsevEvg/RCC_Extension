using RDBLL.Common.Params;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column.SteelBases.Factorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Patterns
{
    public class PatternType1 : PatternBase
    {
        public override string Type { get => "SteelBasePatternType1"; }
        public PatternType1(bool genId = false) : base(genId)
        {
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Ширина базы, B" });
            StoredParams[0].SetDoubleValue(0.36);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Длина базы, L" });
            StoredParams[1].SetDoubleValue(0.66);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Внутренний размер, L1" });
            StoredParams[2].SetDoubleValue(0.35);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Размер по болтам, B2" });
            StoredParams[3].SetDoubleValue(0.2);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Размер по болтам, L2" });
            StoredParams[4].SetDoubleValue(0.49);
        }
        public override void GetBaseParts()
        {
            SteelBase steelBase = ParentMember as SteelBase;
            double baseWidth = StoredParams[0].GetDoubleValue();
            double baseLength = StoredParams[1].GetDoubleValue();
            double baseIntLength = StoredParams[2].GetDoubleValue();
            double width = StoredParams[3].GetDoubleValue();
            double length = StoredParams[4].GetDoubleValue();
            steelBase.SteelBolts.Clear();
            steelBase.SteelBaseParts.Clear();
            BoltFactProc.GetBoltsType1(steelBase, width, length, 2, 2);
            PartFactProc.GetPartsType1(steelBase, baseWidth, baseLength, baseIntLength);
        }
    }
}
