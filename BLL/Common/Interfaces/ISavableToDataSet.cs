using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RDBLL.Common.Interfaces
{
    interface ISavableToDataSet
    {
        void SaveToDataSet(DataSet dataSet);
        void OpenFromDataSet(DataSet dataSet, int id);
    }
}
