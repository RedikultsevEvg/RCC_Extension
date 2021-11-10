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
using RDBLL.Common.Geometry;

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

        public static void AddLoadsToDataset(DataSet dataSet, string dataTableName, string parentTableName, ObservableCollection<LoadSet> loadSets, int parentId, string parentName)
        {
            //Таблица для набора нагрузок
            DataTable LoadSetTable = DsOperation.GetDataTable(dataSet, dataTableName, parentTableName);
            //Добавляем набор нагрузок в датасет
            foreach (LoadSet loadSet in loadSets)
            {
                string loadSetDescription, crcForceDescription = "", designForceDescription = "";
                string forceDescription = "";
                //Добавляем строку только с основными параметрами чтобы было куда добавлять дочерние элементы
                DataRow newLoadSet = LoadSetTable.NewRow();
                DsOperation.SetField(newLoadSet, "Id", ProgrammSettings.CurrentTmpId);// loadSet.Id);
                DsOperation.SetField(newLoadSet, "ParentId", parentId);
                LoadSetTable.Rows.Add(newLoadSet);
                //Таблица для параметров нагрузки
                DataTable ForceParamTable = DsOperation.GetDataTable(dataSet, dataTableName + "ForceParameters", dataTableName);
                //Добавляем параметры нагрузки в набор нагрузок
                foreach (ForceParameter forceParameter in loadSet.ForceParameters)
                {
                    DataRow newForceParameter = ForceParamTable.NewRow();
                    loadSetDescription = loadSet.Name;
                    loadSetDescription += " (n=" + loadSet.PartialSafetyFactor;
                    if (loadSet.BothSign) { loadSetDescription += " знакопеременная"; }
                    loadSetDescription += ")";
                    //Получаем ссылку на описание параметра нагрузки
                    var tmpForceParamLabels = from t in ProgrammSettings.ForceParamKinds where t.Id == forceParameter.KindId select t;
                    MeasureUnitLabel measureUnitLabel = tmpForceParamLabels.First().MeasureUnit.GetCurrentLabel();

                    DsOperation.SetField(newForceParameter, "Id", ProgrammSettings.CurrentTmpId);
                    DsOperation.SetField(newForceParameter, "ForceParamId", tmpForceParamLabels.First().Id);
                    DsOperation.SetField(newForceParameter, "ParentId", loadSet.Id);
                    DsOperation.SetField(newForceParameter, "ElementName", parentName);
                    DsOperation.SetField(newForceParameter, "LoadSetId", loadSet.Id);
                    DsOperation.SetField(newForceParameter, "LoadSetName", loadSet.Name);
                    DsOperation.SetField(newForceParameter, "Description", loadSetDescription);
                    DsOperation.SetField(newForceParameter, "LongLabel", tmpForceParamLabels.First().LongLabel);
                    DsOperation.SetField(newForceParameter, "ShortLabel", tmpForceParamLabels.First().ShortLabel);
                    DsOperation.SetField(newForceParameter, "Unit", measureUnitLabel.UnitName);
                    DsOperation.SetField(newForceParameter, "CrcValue", MathOperation.Round(forceParameter.CrcValue * measureUnitLabel.AddKoeff));
                    DsOperation.SetField(newForceParameter, "DesignValue", MathOperation.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff));

                    ForceParamTable.Rows.Add(newForceParameter);
                    crcForceDescription += tmpForceParamLabels.First().ShortLabel + "=";
                    crcForceDescription += Math.Round(forceParameter.CrcValue * measureUnitLabel.AddKoeff, 3);
                    crcForceDescription += measureUnitLabel.UnitName + "; ";

                    designForceDescription += tmpForceParamLabels.First().ShortLabel + "=";
                    designForceDescription += Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3);
                    designForceDescription += measureUnitLabel.UnitName + "; ";

                    forceDescription += tmpForceParamLabels.First().ShortLabel + "=" + Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3) + measureUnitLabel.UnitName + "; ";
                    //Добавляем остальные параметры к набору нагрузок
                    DsOperation.SetField(newLoadSet, "Name", loadSet.Name);
                    DsOperation.SetField(newLoadSet, "Description", "");
                    DsOperation.SetField(newLoadSet, "PartialSafetyFactor", loadSet.PartialSafetyFactor);
                    DsOperation.SetField(newLoadSet, "CrcForceDescription", crcForceDescription);
                    DsOperation.SetField(newLoadSet, "DesignForceDescription", designForceDescription);
                    DsOperation.SetField(newLoadSet, "ForceDescription", forceDescription);

                }


            }
        }
    }
}
