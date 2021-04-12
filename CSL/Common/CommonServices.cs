using FastReport;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Forces;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using RDBLL.Common.Service.DsOperations;

namespace CSL.Common
{
    public class CommonServices
    {
        public static void PrepareMeasureUnit(Report report)
        {
            report.SetParameterValue("Units.LinearSize", MeasureUnitConverter.GetUnitLabelText(0));
            report.SetParameterValue("Units.Force", MeasureUnitConverter.GetUnitLabelText(1));
            report.SetParameterValue("Units.Moment", MeasureUnitConverter.GetUnitLabelText(2));
            report.SetParameterValue("Units.Stress", MeasureUnitConverter.GetUnitLabelText(3));
            report.SetParameterValue("Units.Geometry.Area", MeasureUnitConverter.GetUnitLabelText(4));
            report.SetParameterValue("Units.Geometry.SecMoment", MeasureUnitConverter.GetUnitLabelText(5));
            report.SetParameterValue("Units.Geometry.Moment", MeasureUnitConverter.GetUnitLabelText(6));
            report.SetParameterValue("Units.Mass", MeasureUnitConverter.GetUnitLabelText(7));
            report.SetParameterValue("Units.Density", MeasureUnitConverter.GetUnitLabelText(8));
            report.SetParameterValue("Units.VolumeWeight", MeasureUnitConverter.GetUnitLabelText(9));
            report.SetParameterValue("Units.SizeArea", MeasureUnitConverter.GetUnitLabelText(10));
            report.SetParameterValue("Units.SizeVolume", MeasureUnitConverter.GetUnitLabelText(11));
            report.SetParameterValue("Units.DisributedForce", MeasureUnitConverter.GetUnitLabelText(12));
            report.SetParameterValue("Units.DisributedLoad", MeasureUnitConverter.GetUnitLabelText(13));
            report.SetParameterValue("Units.Filtration", MeasureUnitConverter.GetUnitLabelText(14));
        }

        public static byte[] ExportToByte(Canvas surface)
        {
            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(surface.Width, surface.Height);
            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            // Create a file stream for saving image
            //using (FileStream outStream = new FileStream(path.LocalPath, FileMode.Create))
            //{
            //    // Use png encoder for our data
            //    PngBitmapEncoder encoder = new PngBitmapEncoder();
            //    // push the rendered bitmap to it
            //    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            //    // save the data to the stream
            //    encoder.Save(outStream);
            //}

            // Use png encoder for our data
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            // push the rendered bitmap to it
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            // save the data to the stream

            // Restore previously saved layout
            surface.LayoutTransform = transform;

            using (MemoryStream s = new MemoryStream())
            {
                encoder.Save(s);
                //imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                //SDraw.Bitmap myBitmap = new SDraw.Bitmap(s);
                return s.ToArray();
            }
        }

        private static void AddLoadsTableToDataSet(DataSet dataSet, string dataTableName, string parentTableName)
        {
            DataTable newTable;
            #region Loads
            //Таблица нагрузок
            try
            {
                newTable = new DataTable(dataTableName);
                dataSet.Tables.Add(newTable);
                DsOperation.AddIdColumn(newTable, true);
                DsOperation.AddFkIdColumn(parentTableName, "ParentId", newTable);
                DsOperation.AddStringColumn(newTable, "Description");
                DsOperation.AddDoubleColumn(newTable, "PartialSafetyFactor");
                DsOperation.AddStringColumn(newTable, "CrcForceDescription");
                DsOperation.AddStringColumn(newTable, "DesignForceDescription");
                DsOperation.AddStringColumn(newTable, "ForceDescription");
            }
            catch { }
            #endregion
            #region LoadForceParameters
            //Таблица параметров нагрузок
            try
            {
                newTable = new DataTable(dataTableName+"ForceParameters");
                dataSet.Tables.Add(newTable);
                DsOperation.AddIntColumn(newTable, "Id");
                DsOperation.AddFkIdColumn(parentTableName, "ParentId", newTable);
                DsOperation.AddStringColumn(newTable, "ElementName");
                DsOperation.AddIntColumn(newTable, "LoadSetId");
                DsOperation.AddStringColumn(newTable, "LoadSetName");
                DsOperation.AddStringColumn(newTable, "Description");
                DsOperation.AddStringColumn(newTable, "LongLabel");
                DsOperation.AddStringColumn(newTable, "ShortLabel");
                DsOperation.AddStringColumn(newTable, "Unit");
                DsOperation.AddDoubleColumn(newTable, "CrcValue");
                DsOperation.AddDoubleColumn(newTable, "DesignValue");
            }
            catch { }
            #endregion
        }

        public static void AddLoadsToDataset(DataSet dataSet, string dataTableName, string parentTableName, ObservableCollection<LoadSet> loadSets, int parentId, string parentName)
        {
            AddLoadsTableToDataSet(dataSet, dataTableName, parentTableName);

            DataTable dataTable = dataSet.Tables[dataTableName];
            foreach (LoadSet loadSet in loadSets)
            {
                string loadSetDescription, crcForceDescription = "", designForceDescription = "";
                string forceDescription = "";
                DataRow newLoadSet = dataTable.NewRow();
                DataTable ForceParameters = dataSet.Tables[dataTableName + "ForceParameters"];
                foreach (ForceParameter forceParameter in loadSet.ForceParameters)
                {
                    DataRow newForceParameter = ForceParameters.NewRow();
                    loadSetDescription = loadSet.Name;
                    loadSetDescription += " (n=" + loadSet.PartialSafetyFactor;
                    if (loadSet.BothSign) { loadSetDescription += " знакопеременная"; }
                    loadSetDescription += ")";
                    var tmpForceParamLabels = from t in ProgrammSettings.ForceParamKinds where t.Id == forceParameter.KindId select t;
                    MeasureUnitLabel measureUnitLabel = tmpForceParamLabels.First().MeasureUnit.GetCurrentLabel();
                    newForceParameter.ItemArray = new object[] { tmpForceParamLabels.First().Id,
                        parentId,
                        parentName,
                        loadSet.Id,
                        loadSet.Name,
                        loadSetDescription,
                        tmpForceParamLabels.First().LongLabel,
                        tmpForceParamLabels.First().ShortLabel,
                        measureUnitLabel.UnitName,
                        Math.Round(forceParameter.CrcValue * measureUnitLabel.AddKoeff, 3),
                        Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3)
                    };
                    ForceParameters.Rows.Add(newForceParameter);
                    crcForceDescription += tmpForceParamLabels.First().ShortLabel + "=";
                    crcForceDescription += Math.Round(forceParameter.CrcValue * measureUnitLabel.AddKoeff, 3);
                    crcForceDescription += measureUnitLabel.UnitName + "; ";

                    designForceDescription += tmpForceParamLabels.First().ShortLabel + "=";
                    designForceDescription += Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3);
                    designForceDescription += measureUnitLabel.UnitName + "; ";

                    forceDescription += tmpForceParamLabels.First().ShortLabel + "=" + Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3) + measureUnitLabel.UnitName + "; ";
                }
                newLoadSet.ItemArray = new object[]
                    {   loadSet.Id,
                        loadSet.Name,
                        parentId,
                        "",
                        loadSet.PartialSafetyFactor,
                        crcForceDescription,
                        designForceDescription,
                        forceDescription
                    };
                dataTable.Rows.Add(newLoadSet);
            }
        }
    }
}
