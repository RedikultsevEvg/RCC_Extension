using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.ErrorProcessing.Messages
{
    public static class CommonMessages
    {
        public static string TypeIsUknown => "Type of option is uknown";
        public static string ParentTypeIsntValid => "Parent type is not valid";
        public static string TableNotContainColumn => "Table not contain required column";
    }
}
