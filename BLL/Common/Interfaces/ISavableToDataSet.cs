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
        void SaveToDataSet(DataSet dataSet, bool createNew);
        void OpenFromDataSet(DataSet dataSet);
        void OpenFromDataSet(DataRow dataRow);
        void DeleteFromDataSet(DataSet dataSet);
    }
}
