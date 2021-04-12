using RDBLL.Common.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RDStartWPF.ResourcesDicts.Parameters
{
    public class ParamTemplSelector : DataTemplateSelector
    {
        public DataTemplate ParamDouble { get; set; }
        public DataTemplate ParamBool { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is StoredParam)) { return null; }
            StoredParam param = item as StoredParam;
            switch (param.ValueType)
            {
                case "double" : return ParamDouble;
                case "bool": return ParamBool;
                default: return null;
            }
        }
    }
}
