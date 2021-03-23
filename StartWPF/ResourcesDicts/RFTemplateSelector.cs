using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.Common.Materials.RFExtenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RDStartWPF.ResourcesDicts
{

    public class RFTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RFBySpacing { get; set; }
        public DataTemplate RFByRectArray { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (! (item is ReinforcementUsing)) { return null; }
            ReinforcementUsing reinforcement = item as ReinforcementUsing;
            if (reinforcement.Extender is null) { return null; }

            RFExtender extender = reinforcement.Extender;
            if (extender is LineToSurfBySpacing) { return RFBySpacing; }
            if (extender is CoveredArray) { return RFByRectArray; }
            else return null;
        }
    }
}
