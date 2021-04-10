using RDBLL.Entity.Soils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Results
{
    /// <summary>
    /// Класс для хранения результатов расчета фундамента
    /// </summary>
    public class FoundationResult
    {
        public List<CompressedLayerList> CompressedLayers { get; set; }
        public List<double[]> StressesWithWeigth { get; set; }
        public double MaxSettlement { get; set; }
        public double CompressHeight { get; set; }
        public double IncX { get; set; }
        public double IncY { get; set; }
        public double IncXY { get; set; }
        public double MinSndAvgStressesWithWeight { get; set; }
        public double MinSndMiddleStressesWithWeight { get; set; }
        public double MinSndCornerStressesWithWeight { get; set; }
        public double MaxSndCornerStressesWithWeight { get; set; }
        public double MaxSndTensionAreaRatioWithWeight { get; set; }
        public double SndResistance { get; set; }
        public double[] AsAct { get; set; }
        public double[] AsRec { get; set; }
        public bool GeneralResult { get; set; }
    }
}
