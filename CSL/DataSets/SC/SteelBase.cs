using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CSL.Common;
using DAL.Common;


namespace CSL.DataSets.SC
{
    public class SteelBaseDataSet
    {
        public static DataSet GetDataSet()
        {
            DataSet dataSet = new DataSet();
            DataTable newTable;
            #region SteelBases
            //Базы стальных колонн
            newTable = new DataTable("SteelBases");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddByteColumn(newTable, "Picture");
            DsOperation.AddNameColumn(newTable);
            DsOperation.AddDoubleColumn(newTable, "SteelStrength");
            DsOperation.AddDoubleColumn(newTable, "ConcreteStrength");
            DsOperation.AddDoubleColumn(newTable, "Width");
            DsOperation.AddDoubleColumn(newTable, "Length");
            DsOperation.AddDoubleColumn(newTable, "Thickness");
            DsOperation.AddDoubleColumn(newTable, "Area");
            DsOperation.AddDoubleColumn(newTable, "Wx");
            DsOperation.AddDoubleColumn(newTable, "Wy");
            #endregion
            //Добавляем общие таблицы работы нагрузок и сочетаний
            //CommonServices.AddLoadsTableToDataSet(dataSet, "LoadSets", "SteelBases", "SteelBaseId");
            //CommonServices.AddLoadsTableToDataSet(dataSet, "LoadCases", "SteelBases", "SteelBaseId");
            #region SteelBasesParts
            newTable = new DataTable("SteelBasesParts");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn("SteelBases", "SteelBaseId", newTable);
            DsOperation.AddByteColumn(newTable, "Picture");
            DsOperation.AddNameColumn(newTable);
            DsOperation.AddDoubleColumn(newTable, "CenterX");
            DsOperation.AddDoubleColumn(newTable, "CenterY");
            DsOperation.AddDoubleColumn(newTable, "Width");
            DsOperation.AddDoubleColumn(newTable, "Length");
            DsOperation.AddDoubleColumn(newTable, "MaxBedStress");
            DsOperation.AddDoubleColumn(newTable, "MaxSteelStress");
            DsOperation.AddDoubleColumn(newTable, "RecomendedThickness");
            #endregion
            #region SteelBolts
            newTable = new DataTable("SteelBasesBolts");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn("SteelBases", "SteelBaseId", newTable);
            DsOperation.AddNameColumn(newTable);
            DsOperation.AddDoubleColumn(newTable, "Diameter");
            DsOperation.AddDoubleColumn(newTable, "CenterX");
            DsOperation.AddDoubleColumn(newTable, "CenterY");
            DsOperation.AddDoubleColumn(newTable, "MaxStress");
            DsOperation.AddDoubleColumn(newTable, "MaxForce");
            #endregion

            return dataSet;
        }
    }
}
