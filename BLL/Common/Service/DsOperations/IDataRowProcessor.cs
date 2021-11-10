using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations
{
    internal interface IDataRowProcessor
    {
        DataRow FindDataRowById(DataTable dataTable, int id);
        IEnumerable<DataRow> FindDataRowsByParentId(DataTable dataTable, int parentId);
        void DeleteDataRowById(DataTable dataTable, int id);
        DataRow CreateDataRow(DataTable dataTable, int? id = null);
    }
}
