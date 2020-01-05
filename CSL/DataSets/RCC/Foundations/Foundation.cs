using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DAL.Common;
using CSL.Common;

namespace CSL.DataSets.RCC.Foundations
{
    /// <summary>
    /// Датасет для отчета по фундаментам
    /// </summary>
    public class FoundationDataSet
    {
        public static DataSet GetDataSet()
        {
            DataSet dataSet = new DataSet();
            DataTable newTable;
            #region Foundations
            newTable = new DataTable("Foundations");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddNameColumn(newTable);
            DsOperation.AddByteColumn(newTable, "Picture");
            DsOperation.AddDoubleColumn(newTable, "Width");
            DsOperation.AddDoubleColumn(newTable, "Length");
            DsOperation.AddDoubleColumn(newTable, "Height");
            DsOperation.AddDoubleColumn(newTable, "SoilVolumeWeight");
            DsOperation.AddDoubleColumn(newTable, "ConcreteVolumeWeight");
            DsOperation.AddDoubleColumn(newTable, "SoilVolume");
            DsOperation.AddDoubleColumn(newTable, "ConcreteVolume");
            DsOperation.AddDoubleColumn(newTable, "Area");
            DsOperation.AddDoubleColumn(newTable, "Wx");
            DsOperation.AddDoubleColumn(newTable, "Wy");
            #endregion
            #region FoundationParts
            newTable = new DataTable("FoundationParts");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn("Foundations", "FoundationId", newTable);
            DsOperation.AddNameColumn(newTable);
            DsOperation.AddDoubleColumn(newTable, "Width");
            DsOperation.AddDoubleColumn(newTable, "Length");
            DsOperation.AddDoubleColumn(newTable, "Heigth");
            DsOperation.AddDoubleColumn(newTable, "Volume");
            DsOperation.AddDoubleColumn(newTable, "CenterX");
            DsOperation.AddDoubleColumn(newTable, "CentrY");
            #endregion
            return dataSet;
        }
    }
}
