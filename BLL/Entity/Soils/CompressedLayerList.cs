using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soils
{
    public class CompressedLayerList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CompressedLayer> CompressedLayers {get;set;}
    }
}
