using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSL.DataSets.Factories
{
    /// <summary>
    /// Интерфейс фабрики датасетов
    /// </summary>
    public interface IReportDataSetFactory
    {
        DataSet CreateDataSet();
    }
}
