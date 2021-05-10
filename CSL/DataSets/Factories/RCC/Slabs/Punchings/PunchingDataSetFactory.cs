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
            //Таблица продавливаний
            newTable = new DataTable("Punchings");
            DsOperation.AddIdNameParentIdColumn(newTable);
            dataSet.Tables.Add(newTable);
            //Таблица 
            
            //Таблица расчетных контуров
            newTable = new DataTable("PunchingContours");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdNameParentIdColumn(newTable, "Punchings");
            //Таблица расчетных субконтуров
            newTable = new DataTable("PunchingSubContours");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdNameParentIdColumn(newTable, "PunchingContours");
            //Таблица линий расчетного контура
            newTable = new DataTable("PunchingLines");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdNameParentIdColumn(newTable, "PunchingSubContours");
            //Таблица результатов расчета по контурам
            newTable = new DataTable("PunchingLoadSetContours");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdNameParentIdColumn(newTable, "Punchings");
            return dataSet;
        }
    }
}
