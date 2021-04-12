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
    public class PatternType2 : PatternBase
    {
        public override string Type { get => "SteelBasePatternType2"; }
        public PatternType2(bool genId = false) : base(genId)
        {
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Ширина базы, B" });
            StoredParams[0].SetDoubleValue(0.5, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Длина базы, L" });
            StoredParams[1].SetDoubleValue(0.8, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Ширина свеса, B1" });
            StoredParams[2].SetDoubleValue(0.07, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Внутренний размер, L1" });
            StoredParams[3].SetDoubleValue(0.35, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Размер по болтам, B2" });
            StoredParams[4].SetDoubleValue(0.2, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Размер по болтам, L2" });
            StoredParams[5].SetDoubleValue(0.49, 0, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Диаметр болтов, d" });
            StoredParams[6].SetDoubleValue(0.03, 0.3, double.PositiveInfinity, 0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Наличие ребра по X" });
            StoredParams[7].SetBoolValue(true);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "Наличие ребра по Y" });
            StoredParams[8].SetBoolValue(true);
        }
        public override void GetBaseParts()
        {
            SteelBase steelBase = ParentMember as SteelBase;
            double baseWidth = StoredParams[0].GetDoubleValue();
            double baseLength = StoredParams[1].GetDoubleValue();
            double contilever = StoredParams[2].GetDoubleValue();
            double baseIntLength = StoredParams[3].GetDoubleValue();
            double width = StoredParams[4].GetDoubleValue();
            double length = StoredParams[5].GetDoubleValue();
            double diam = StoredParams[6].GetDoubleValue();
            bool edgeX = StoredParams[7].GetBoolValue();
            bool edgeY = StoredParams[8].GetBoolValue();

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
            PartFactProc.GetPartsType1(steelBase, baseWidth, baseLength, contilever, baseIntLength, edgeX, edgeY);
        }
    }
}
