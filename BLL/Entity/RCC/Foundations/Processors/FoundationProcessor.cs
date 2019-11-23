using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Processors
{
    public class FoundationProcessor
    {
        public static double[] GetContourSize(Foundation foundation)
        {
            double[] sizes = new double[2] { 1, 1 };
            if (foundation.Parts.Count>0)
            {
                FoundationPart foundationPart = foundation.Parts[foundation.Parts.Count - 1];
                sizes[0] = foundationPart.Width + Math.Abs(foundationPart.CenterX)*2;
                sizes[1] = foundationPart.Length + Math.Abs(foundationPart.CenterY)*2;
            }
            return sizes;
        }
    }
}
