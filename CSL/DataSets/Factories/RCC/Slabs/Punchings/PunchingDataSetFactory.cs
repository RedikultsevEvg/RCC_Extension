using RDBLL.Common.Service.DsOperations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSL.DataSets.Factories.RCC.Slabs.Punchings
{
    /// <summary>
    /// Фабрика датасета для расчета на продавливание
    /// </summary>
    public class PunchingDataSetFactory : IReportDataSetFactory
    {
        public DataSet CreateDataSet()
        {
            DataSet dataSet = new DataSet();
            DataTable newTable;
            newTable = new DataTable("Punchings");
            DsOperation.AddIdNameParentIdColumn(newTable);
            dataSet.Tables.Add(newTable);
            newTable = new DataTable("PunchingContours");
            DsOperation.AddIdNameParentIdColumn(newTable);
            dataSet.Tables.Add(newTable);
            return dataSet;
        }
    }
}
