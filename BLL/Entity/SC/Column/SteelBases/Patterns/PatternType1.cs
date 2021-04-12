using RDBLL.Common.Params;
using RDBLL.Common.Service;
using RDBLL.Entity.SC.Column.SteelBases.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Patterns
{
    /// <summary>
    /// Создание стальной базы по паттерну 1
    /// </summary>
    public class PatternType1 : PatternBase
    {
        public override string Type { get => "SteelBasePatternType1"; }
        public PatternType1(bool genId = false) : base(genId)
        {
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Ширина базы, B" });
            StoredParams[0].SetDoubleValue(0.36, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Длина базы, L" });
            StoredParams[1].SetDoubleValue(0.66, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Внутренний размер, L1" });
            StoredParams[2].SetDoubleValue(0.35, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Размер по болтам, B2" });
            StoredParams[3].SetDoubleValue(0.2, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Размер по болтам, L2" });
            StoredParams[4].SetDoubleValue(0.49, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Диаметр болтов, d" });
            StoredParams[5].SetDoubleValue(0.03, 0, 0.3, 0);
        }
        public override void GetBaseParts()
        {
            SteelBase steelBase = ParentMember as SteelBase;
            double baseWidth = StoredParams[0].GetDoubleValue();
            double baseLength = StoredParams[1].GetDoubleValue();
            double baseIntLength = StoredParams[2].GetDoubleValue();
            double width = StoredParams[3].GetDoubleValue();
            double length = StoredParams[4].GetDoubleValue();
            double diam = StoredParams[5].GetDoubleValue();
            steelBase.SteelBolts.Clear();
            DataSet dataSet = ProgrammSettings.CurrentDataSet;
            foreach (SteelBasePart part in steelBase.SteelBaseParts)
            {
                EntityOperation.DeleteEntity(dataSet, part);
            }
            foreach (SteelBolt part in steelBase.SteelBolts)
            {
                EntityOperation.DeleteEntity(dataSet, part);
            }
            steelBase.SteelBaseParts.Clear();
            BoltFactProc.GetBoltsType1(steelBase, diam, width, length, 2, 2);
            PartFactProc.GetPartsType1(steelBase, baseWidth, baseLength, baseIntLength);
        }
    }
}
