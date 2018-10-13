using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Forces
{
    public class ForceParameter
    {
        private int _kind_id;
        private ForceParamKind _forceParamKind;

        public int Id { get; set; }
        public double Value { get; set; }
        public int Kind_id
        {
            get { return _kind_id; }
            set
            {
                _kind_id = value;
                _forceParamKind = ProgrammSettings.ForceParamKinds[_kind_id-1];
                ForceParamKind = _forceParamKind;
            }
        }
        public ForceParamKind ForceParamKind
        {
            get
            {
                return _forceParamKind;
            }
            set { _forceParamKind = value; }
        }
    }


}
