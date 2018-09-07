using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Geometry
{
    public class RectCrossSection
    {
        public double Lx { get; set; }
        public double Ly { get; set; }

        public RectCrossSection(double lx, double ly)
        {
            Lx = lx;
            Ly = ly;
        }
    }

    public static class RectProcessor
    {
        public static MassProperty GetRectMassProperty(RectCrossSection rect)
        {
            MassProperty massProperty = new MassProperty();
            massProperty.A = rect.Lx * rect.Ly;
            massProperty.Wx = rect.Lx * Math.Pow(rect.Ly, 2) / 6;
            massProperty.Wy = rect.Ly * Math.Pow(rect.Lx, 2) / 6;
            massProperty.Ix = rect.Lx * Math.Pow(rect.Ly, 3) / 12;
            massProperty.Iy = rect.Ly * Math.Pow(rect.Lx, 3) / 12;
            massProperty.Xmax = rect.Lx / 2;
            massProperty.Ymax = rect.Ly / 2;
            return massProperty;
        }
    }
}
