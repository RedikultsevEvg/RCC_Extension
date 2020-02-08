using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces
{
    interface IRdObserver
    {
        void Update();
        void UnSubScribe();
    }
}
