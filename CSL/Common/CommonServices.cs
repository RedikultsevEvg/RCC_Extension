using FastReport;
using RDBLL.Entity.MeasureUnits;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data;
using DAL.Common;

namespace CSL.Common
{
    public class CommonServices
    {
        public static void PrepareMeasureUnit(Report report)
        {
            using (report)
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
            }


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

        public static void AddLoadsTableToDataSet(DataSet dataSet, string parentTableName, string ParentNameId)
        {
            DataTable newTable;
            #region Loads
            newTable = new DataTable("LoadSets");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddFkIdColumn(parentTableName, ParentNameId, newTable);
            DsOperation.AddNameColumn(newTable);
            DsOperation.AddDoubleColumn(newTable, "PartialSafetyFactor");
            DsOperation.AddStringColumn(newTable, "CrcForceDescription");
            DsOperation.AddStringColumn(newTable, "DesignForceDescription");
            #endregion
            #region LoadForceParameters
            newTable = new DataTable("LoadSetsForceParameters");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIntColumn(newTable, "Id");
            DsOperation.AddFkIdColumn(parentTableName, ParentNameId, newTable);
            DsOperation.AddStringColumn(newTable, "ElementName");
            DsOperation.AddIntColumn(newTable, "LoadSetId");
            DsOperation.AddStringColumn(newTable, "LoadSetName");
            DsOperation.AddStringColumn(newTable, "Description");
            DsOperation.AddStringColumn(newTable, "LongLabel");
            DsOperation.AddStringColumn(newTable, "ShortLabel");
            DsOperation.AddStringColumn(newTable, "Unit");
            DsOperation.AddDoubleColumn(newTable, "CrcValue");
            DsOperation.AddDoubleColumn(newTable, "DesignValue");
            #endregion
            #region LoadCases
            newTable = new DataTable("LoadCases");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddIntColumn(newTable, ParentNameId);
            DsOperation.AddStringColumn(newTable, "Description");
            DsOperation.AddDoubleColumn(newTable, "PartialSafetyFactor");
            DsOperation.AddStringColumn(newTable, "ForceDescription");
            #endregion
            #region ForceParameters
            newTable = new DataTable("LoadCasesForceParameters");
            dataSet.Tables.Add(newTable);
            DsOperation.AddIdColumn(newTable);
            DsOperation.AddStringColumn(newTable, "LoadCaseId");
            DsOperation.AddStringColumn(newTable, "LongLabel");
            DsOperation.AddStringColumn(newTable, "ShortLabel");
            DsOperation.AddStringColumn(newTable, "Unit");
            DsOperation.AddDoubleColumn(newTable, "CrcValue");
            DsOperation.AddDoubleColumn(newTable, "DesignValue");
            #endregion
            dataSet.Relations.Add("LoadCases", dataSet.Tables[parentTableName].Columns["Id"], dataSet.Tables["LoadCases"].Columns[ParentNameId]);
        }
    }
}
