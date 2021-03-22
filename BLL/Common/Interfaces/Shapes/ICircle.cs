using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Shapes
{
    public interface ICircle :IShape
    {
        double Diameter { get; set; }
    }
}
