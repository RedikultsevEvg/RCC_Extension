using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces.Shapes
{
    public interface IRectangle : IShape
    {
        double Width { get; set; }
        double Length { get; set; }
    }
}
