using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service
{
    public class DetailObjectList
    {
        public String DataType { get; set; }
        public Object ParentObject { get; set; }
        public Object ObjectList { get; set; }
        public bool IsSelectable { get; set; }
        public List<Int16> BtnVisibilityList { get; set; }

        public DetailObjectList(String dataType, Object parentObject, Object objectList, bool isSelectable)
        {
            DataType = dataType;
            ParentObject = parentObject;
            ObjectList = objectList;
            IsSelectable = isSelectable;
        }
    }
}
