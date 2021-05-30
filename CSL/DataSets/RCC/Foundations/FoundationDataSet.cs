using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CSL.Common;
using RDBLL.Common.Service.DsOperations;

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
            DsOperation.AddCommonColumns(newTable);
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
            DsOperation.AddDoubleColumn(newTable, "AsBottomXAct");
            DsOperation.AddDoubleColumn(newTable, "AsBottomYAct");
            DsOperation.AddDoubleColumn(newTable, "AsBottomXMax");
            DsOperation.AddDoubleColumn(newTable, "AsBottomYMax");
            DsOperation.AddDoubleColumn(newTable, "AsCrcBottomXMax");
            DsOperation.AddDoubleColumn(newTable, "AsCrcBottomYMax");
            DsOperation.AddStringColumn(newTable, "AsFstConclusionX");
            DsOperation.AddStringColumn(newTable, "AsFstConclusionY");
            DsOperation.AddStringColumn(newTable, "GeneralConclusion");
            #endregion
            #region FoundationParts
            newTable = new DataTable("FoundationParts");
            dataSet.Tables.Add(newTable);
            DsOperation.AddCommonColumns(newTable, "Foundations");
            DsOperation.AddDoubleColumn(newTable, "Width");
            DsOperation.AddDoubleColumn(newTable, "Length");
            DsOperation.AddDoubleColumn(newTable, "Heigth");
            DsOperation.AddDoubleColumn(newTable, "Volume");
            DsOperation.AddDoubleColumn(newTable, "CenterX");
            DsOperation.AddDoubleColumn(newTable, "CenterY");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentXMax");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentYMax");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentXMin");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentYMin");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentXMax");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentYMax");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentXMin");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentYMin");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentXMaxDistr");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentYMaxDistr");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentXMinDistr");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentYMinDistr");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentXMaxDistr");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentYMaxDistr");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentXMinDistr");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentYMinDistr");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentX");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentY");
            DsOperation.AddDoubleColumn(newTable, "AsBottomXMax");
            DsOperation.AddDoubleColumn(newTable, "AsBottomYMax");
            DsOperation.AddDoubleColumn(newTable, "AsCrcBottomXMax");
            DsOperation.AddDoubleColumn(newTable, "AsCrcBottomYMax");
            #endregion
            #region StressesWithWeight
            newTable = new DataTable("FoundationStressesWithWeight");
            dataSet.Tables.Add(newTable);
            AddFoundStress(newTable);
            #endregion
            #region StressesWithoutWeight
            newTable = new DataTable("FoundationStressesWithoutWeight");
            dataSet.Tables.Add(newTable);
            AddFoundStress(newTable);
            #endregion
            #region SettlementSets
            newTable = new DataTable("SettlementSets");
            dataSet.Tables.Add(newTable);
            DsOperation.AddCommonColumns(newTable, "Foundations");
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
            DsOperation.AddCommonColumns(newTable, "SettlementSets");
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
            #region FoundationPartMoments
            newTable = new DataTable("PartMoments");
            dataSet.Tables.Add(newTable);
            DsOperation.AddCommonColumns(newTable, "FoundationParts");
            DsOperation.AddDoubleColumn(newTable, "CrcMomentX");
            DsOperation.AddDoubleColumn(newTable, "DesignMomentY");
            #endregion
            return dataSet;
        }
        private static void AddFoundStress(DataTable dataTable)
        {
            DsOperation.AddCommonColumns(dataTable, "Foundations");
            DsOperation.AddDoubleColumn(dataTable, "crcAvgStress");
            DsOperation.AddDoubleColumn(dataTable, "crcCenterStress");
            DsOperation.AddDoubleColumn(dataTable, "crcMiddleSresses");
            DsOperation.AddDoubleColumn(dataTable, "crcCornerSressesMin");
            DsOperation.AddDoubleColumn(dataTable, "crcCornerSressesMax");
            DsOperation.AddDoubleColumn(dataTable, "designAvgStress");
            DsOperation.AddDoubleColumn(dataTable, "designCenterStress");
            DsOperation.AddDoubleColumn(dataTable, "designMiddleSresses");
            DsOperation.AddDoubleColumn(dataTable, "designCornerSressesMin");
            DsOperation.AddDoubleColumn(dataTable, "designCornerSressesMax");
            DsOperation.AddDoubleColumn(dataTable, "CrcTensionAreaRatio");
            DsOperation.AddDoubleColumn(dataTable, "DesignTensionAreaRatio");
        }
    }
}
