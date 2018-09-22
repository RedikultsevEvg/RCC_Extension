using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;

namespace RDBLL.Forces
{
    public class ForcesGroup
    {
        public SteelColumnBase SteelColumnBase { get; set; }
        public ObservableCollection<BarLoadSet> Loads { get; set; }
        public Point2D Excentricity { get; set; }

        #region Constructors
        public ForcesGroup(SteelColumnBase steelColumnBase)
        {
            Loads = new ObservableCollection<BarLoadSet>();
            Loads.Add(new BarLoadSet(this));
            Excentricity = new Point2D(0, 0);
            SteelColumnBase = steelColumnBase;
        }
        #endregion
    }
}
