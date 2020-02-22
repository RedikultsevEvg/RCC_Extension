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
            DsOperation.AddDoubleColumn(newTable, "MinSndAvgStressesWithWeight");
            DsOperation.AddDoubleColumn(newTable, "MinSndMiddleStressesWithWeight");
            DsOperation.AddDoubleColumn(newTable, "MinSndCornerStressesWithWeight");
            DsOperation.AddDoubleColumn(newTable, "MaxSndCornerStressesWithWeight");
            DsOperation.AddDoubleColumn(newTable, "MaxSndTensionAreaRatioWithWeight");
            DsOperation.AddDoubleColumn(newTable, "SndResistance");
            DsOperation.AddStringColumn(newTable, "AvgStressConclusion");
            DsOperation.AddStringColumn(newTable, "MiddleStressConclusion");
            DsOperation.AddStringColumn(newTable, "CornerStressConclusion");
            DsOperation.AddDoubleColumn(newTable, "SettlementMin");
            DsOperation.AddStringColumn(newTable, "SettlementConclusion");
            DsOperation.AddDoubleColumn(newTable, "CompressionHeightMax");
            DsOperation.AddDoubleColumn(newTable, "IncXMax");
            DsOperation.AddDoubleColumn(newTable, "IncYMax");
            DsOperation.AddDoubleColumn(newTable, "IncXYMax");
            DsOperation.AddStringColumn(newTable, "NzStiffnessStringMin");
            DsOperation.AddStringColumn(newTable, "MxStiffnessStringMin");
            DsOperation.AddStringColumn(newTable, "MyStiffnessStringMin");
            DsOperation.AddStringColumn(newTable, "NzStiffnessStringMax");
            DsOperation.AddStringColumn(newTable, "MxStiffnessStringMax");
            DsOperation.AddStringColumn(newTable, "MyStiffnessStringMax");
            DsOperation.AddStringColumn(newTable, "GeneralConclusion");
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
            #region StressesWithWeight
            newTable = new DataTable("FoundationStressesWithWeight");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn("Foundations", "FoundationId", newTable);
            DsOperation.AddDoubleColumn(newTable, "crcAvgStress");
            DsOperation.AddDoubleColumn(newTable, "crcCenterStress");
            DsOperation.AddDoubleColumn(newTable, "crcMiddleSresses");
            DsOperation.AddDoubleColumn(newTable, "crcCornerSressesMin");
            DsOperation.AddDoubleColumn(newTable, "crcCornerSressesMax");
            DsOperation.AddDoubleColumn(newTable, "designAvgStress");
            DsOperation.AddDoubleColumn(newTable, "designCenterStress");
            DsOperation.AddDoubleColumn(newTable, "designMiddleSresses");
            DsOperation.AddDoubleColumn(newTable, "designCornerSressesMin");
            DsOperation.AddDoubleColumn(newTable, "designCornerSressesMax");
            DsOperation.AddDoubleColumn(newTable, "CrcTensionAreaRatio");
            DsOperation.AddDoubleColumn(newTable, "DesignTensionAreaRatio");
            #endregion
            #region StressesWithoutWeight
            newTable = new DataTable("FoundationStressesWithoutWeight");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn("Foundations", "FoundationId", newTable);
            DsOperation.AddDoubleColumn(newTable, "crcAvgStress");
            DsOperation.AddDoubleColumn(newTable, "crcCenterStress");
            DsOperation.AddDoubleColumn(newTable, "crcMiddleSresses");
            DsOperation.AddDoubleColumn(newTable, "crcCornerSressesMin");
            DsOperation.AddDoubleColumn(newTable, "crcCornerSressesMax");
            DsOperation.AddDoubleColumn(newTable, "designAvgStress");
            DsOperation.AddDoubleColumn(newTable, "designCenterStress");
            DsOperation.AddDoubleColumn(newTable, "designMiddleSresses");
            DsOperation.AddDoubleColumn(newTable, "designCornerSressesMin");
            DsOperation.AddDoubleColumn(newTable, "designCornerSressesMax");
            DsOperation.AddDoubleColumn(newTable, "CrcTensionAreaRatio");
            DsOperation.AddDoubleColumn(newTable, "DesignTensionAreaRatio");
            #endregion
            #region SettlementSets
            newTable = new DataTable("SettlementSets");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn("Foundations", "FoundationId", newTable);
            DsOperation.AddNameColumn(newTable);
            DsOperation.AddDoubleColumn(newTable, "MinSettlement");
            DsOperation.AddDoubleColumn(newTable, "CompressedHeight");
            DsOperation.AddDoubleColumn(newTable, "SumRotateX");
            DsOperation.AddDoubleColumn(newTable, "SumRotateY");
            DsOperation.AddDoubleColumn(newTable, "SumRotateXY");
            DsOperation.AddStringColumn(newTable, "NzStiffness");
            DsOperation.AddStringColumn(newTable, "MxStiffness");
            DsOperation.AddStringColumn(newTable, "MyStiffness");
            #endregion
            #region ComressedLayers
            newTable = new DataTable("ComressedLayers");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn("SettlementSets", "SettlementSetId", newTable);
            DsOperation.AddDoubleColumn(newTable, "ZLevel");
            DsOperation.AddDoubleColumn(newTable, "TopLevel");
            DsOperation.AddDoubleColumn(newTable, "BtmLevel");
            DsOperation.AddDoubleColumn(newTable, "Alpha");
            DsOperation.AddDoubleColumn(newTable, "SigmZg");
            DsOperation.AddDoubleColumn(newTable, "SigmZgamma");
            DsOperation.AddDoubleColumn(newTable, "SigmZp");
            DsOperation.AddDoubleColumn(newTable, "LocalSettlement");
            DsOperation.AddDoubleColumn(newTable, "SumSettlement");
            #endregion
            return dataSet;
        }
    }
}
