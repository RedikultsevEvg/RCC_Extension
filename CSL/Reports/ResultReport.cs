using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastReport;
using System.Data;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.SC.Column;
using RDBLL.Processors.SC;
using RDBLL.Processors.Forces;
using RDBLL.Entity.Results.SC;
using RDBLL.Forces;
using RDBLL.Common.Service;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using  SDraw = System.Drawing;
using RDBLL.DrawUtils.SteelBase;

namespace CSL.Reports
{
    public class ResultReport
    {
        public DataSet dataSet { get; set; }
        private BuildingSite _buildingSite;


        public void ShowReport()
        {
            using (Report report = new Report())
            {
                report.Load(Directory.GetCurrentDirectory()+"\\Reports\\SteelBases.frx");
                report.RegisterData(dataSet);
                //report.Design();
                report.Show();
            }
        }
        public void PrepareReport()
        {
            int steelBaseId = 1;
            int LoadCaseId = 1;
            int ForceParameterId = 1;
            int steelBasePartId = 1;

            foreach (Building building in _buildingSite.BuildingList)
            {
                foreach (Level level in building.LevelList)
                {
                    DataTable SteelBases = dataSet.Tables[0];
                    foreach (SteelColumnBase steelColumnBase in level.SteelColumnBaseList)
                    {
                        DataRow newSteelBase = SteelBases.NewRow();
                        Double A = steelColumnBase.Width * steelColumnBase.Length;
                        Double Wx = steelColumnBase.Width * steelColumnBase.Length * steelColumnBase.Length / 6;
                        Double Wy = steelColumnBase.Length * steelColumnBase.Width * steelColumnBase.Width / 6;
                        #region Picture
                        Canvas canvas = new Canvas();
                        canvas.Width = 600;
                        canvas.Height = 600;
                        DrawSteelBase.DrawBase(steelColumnBase, canvas);
                        byte[] b = ExportToByte(canvas);
                        #endregion
                        newSteelBase.ItemArray = new object[] { steelBaseId, b, steelColumnBase.Name, steelColumnBase.Width, steelColumnBase.Length, A, Wx, Wy };
                        
                        SteelBases.Rows.Add(newSteelBase);
                        DataTable LoadCases = dataSet.Tables[1];                       
                        foreach (BarLoadSet barLoadSet in BarLoadSetProcessor.GetLoadCases(steelColumnBase.LoadsGroup))
                        {
                            DataRow newLoadCase = LoadCases.NewRow();
                            newLoadCase.ItemArray = new object[] { LoadCaseId, steelBaseId, barLoadSet.LoadSet.Name, barLoadSet.LoadSet.PartialSafetyFactor};
                            LoadCases.Rows.Add(newLoadCase);

                            DataTable ForceParameters = dataSet.Tables[2];
                            foreach (ForceParameter forceParameter in barLoadSet.LoadSet.ForceParameters)
                            {
                                DataRow newForceParameter = ForceParameters.NewRow();
                                var tmpForceParamLabels = from t in ProgrammSettings.ForceParamKinds where t.Id == forceParameter.Kind_id select t;
                                newForceParameter.ItemArray = new object[] { ForceParameterId, LoadCaseId, tmpForceParamLabels.First().LongLabel, tmpForceParamLabels.First().ShortLabel, tmpForceParamLabels.First().UnitLabel, forceParameter.CrcValue, forceParameter.DesignValue };
                                ForceParameters.Rows.Add(newForceParameter);
                                ForceParameterId++;
                            }
                            LoadCaseId++;
                        }
                        DataTable SteelBasesParts = dataSet.Tables[3];
                        foreach (SteelBasePart steelBasePart in steelColumnBase.SteelBaseParts)
                        {
                            foreach (SteelBasePart steelBasePartEh in SteelColumnBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart))
                            {
                                DataRow newSteelBasePart = SteelBasesParts.NewRow();
                                ColumnBasePartResult columnBasePartResult = SteelColumnBasePartProcessor.GetResult(steelBasePartEh);
                                double maxBedStress = columnBasePartResult.MaxBedStress;
                                double maxStress = columnBasePartResult.MaxStress;
                                #region Picture
                                Canvas canvasPart = new Canvas();
                                canvasPart.Width = 300;
                                canvasPart.Height = 300;
                                double zoom_factor_X = canvasPart.Width / steelBasePartEh.Width / 1.2;
                                double zoom_factor_Y = canvasPart.Height / steelBasePartEh.Length / 1.2;
                                double scale_factor;
                                if (zoom_factor_X < zoom_factor_Y) { scale_factor = zoom_factor_X; } else { scale_factor = zoom_factor_Y; }
                                canvasPart.Width = steelBasePartEh.Width * 1.2 * scale_factor;
                                canvasPart.Height = steelBasePartEh.Length * 1.2 * scale_factor;
                                double[] columnBaseCenter = new double[2] { canvasPart.Width/2 - steelBasePartEh.Center[0]*scale_factor, canvasPart.Height/2 + steelBasePartEh.Center[1] * scale_factor };
                                DrawSteelBase.DrawBasePart(steelBasePartEh, canvasPart, columnBaseCenter, scale_factor, 1, 1, 1, false);
                                byte[] bPart = ExportToByte(canvasPart);
                                #endregion
                                newSteelBasePart.ItemArray = new object[] { steelBasePartId, steelBaseId, bPart, steelBasePartEh.Name, steelBasePartEh.Center[0], steelBasePartEh.Center[1], steelBasePartEh.Width, steelBasePart.Length, Math.Round(maxBedStress/1000000,3), Math.Round(maxStress /1000000,3) };
                                SteelBasesParts.Rows.Add(newSteelBasePart);
                                steelBasePartId++;
                            }
                        }
                        steelBaseId++;
                    }
                }
            }


        }

        public byte[] ExportToByte(Canvas surface)
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

        #region Constructors
        public ResultReport(BuildingSite buildingSite)
        {
            _buildingSite = buildingSite;
            dataSet = new DataSet();
            #region SteelBases
            //Базы стальных колонн
            DataTable SteelBases = new DataTable("SteelBases");
            dataSet.Tables.Add(SteelBases);
            DataColumn SteelBaseId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBasePicture = new DataColumn("Picture", Type.GetType("System.Byte[]"));
            DataColumn SteelBaseName = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn SteelBaseWidth = new DataColumn("Width", Type.GetType("System.Double"));
            DataColumn SteelBaseLength = new DataColumn("Length", Type.GetType("System.Double"));
            DataColumn SteelBaseArea = new DataColumn("Area", Type.GetType("System.Double"));
            DataColumn SteelBaseWx = new DataColumn("Wx", Type.GetType("System.Double"));
            DataColumn SteelBaseWy = new DataColumn("Wy", Type.GetType("System.Double"));

            SteelBases.Columns.Add(SteelBaseId);
            SteelBases.Columns.Add(SteelBasePicture);
            SteelBases.Columns.Add(SteelBaseName);
            SteelBases.Columns.Add(SteelBaseWidth);
            SteelBases.Columns.Add(SteelBaseLength);
            SteelBases.Columns.Add(SteelBaseArea);
            SteelBases.Columns.Add(SteelBaseWx);
            SteelBases.Columns.Add(SteelBaseWy);
            #endregion
            #region LoadCases
            DataTable LoadCases = new DataTable("LoadCases");
            dataSet.Tables.Add(LoadCases);

            DataColumn LoadCaseId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBaseIdInLoadCase = new DataColumn("SteelBaseId", Type.GetType("System.Int32"));
            DataColumn LoadCaseDescription = new DataColumn("Description", Type.GetType("System.String"));
            DataColumn LoadCasePartialSafetyFactor = new DataColumn("PartialSafetyFactor", Type.GetType("System.Double"));

            LoadCases.Columns.Add(LoadCaseId);
            LoadCases.Columns.Add(SteelBaseIdInLoadCase);
            LoadCases.Columns.Add(LoadCaseDescription);
            LoadCases.Columns.Add(LoadCasePartialSafetyFactor);
            #endregion
            #region ForceParameters
            DataTable ForceParameters = new DataTable("ForceParameters");
            dataSet.Tables.Add(ForceParameters);

            DataColumn ForceParameterId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn LoadCaseIdInForceParameter = new DataColumn("LoadCaseId", Type.GetType("System.Int32"));
            DataColumn ForceParameterLongLabel = new DataColumn("LongLabel", Type.GetType("System.String"));
            DataColumn ForceParameterShortLabel = new DataColumn("ShortLabel", Type.GetType("System.String"));
            DataColumn ForceParameterUnit = new DataColumn("Unit", Type.GetType("System.String"));
            DataColumn ForceParameterCrcValue = new DataColumn("CrcValue", Type.GetType("System.Double"));
            DataColumn ForceParameterDesignValue = new DataColumn("DesignValue", Type.GetType("System.Double"));

            ForceParameters.Columns.Add(ForceParameterId);
            ForceParameters.Columns.Add(LoadCaseIdInForceParameter);
            ForceParameters.Columns.Add(ForceParameterLongLabel);
            ForceParameters.Columns.Add(ForceParameterShortLabel);
            ForceParameters.Columns.Add(ForceParameterUnit);
            ForceParameters.Columns.Add(ForceParameterCrcValue);
            ForceParameters.Columns.Add(ForceParameterDesignValue);
            #endregion
            #region SteelBasesParts
            DataTable SteelBasesParts = new DataTable("SteelBasesParts");
            dataSet.Tables.Add(SteelBasesParts);

            DataColumn SteelBasesPartId = new DataColumn("Id", Type.GetType("System.Int32"));
            DataColumn SteelBaseIdInPart = new DataColumn("SteelBaseId", Type.GetType("System.Int32"));
            DataColumn SteelBasePartPicture = new DataColumn("Picture", Type.GetType("System.Byte[]"));
            DataColumn SteelBasesPartName = new DataColumn("Name", Type.GetType("System.String"));
            DataColumn SteelBasesPartCenterX = new DataColumn("CenterX", Type.GetType("System.Double"));
            DataColumn SteelBasesPartCenterY = new DataColumn("CenterY", Type.GetType("System.Double"));
            DataColumn SteelBasesPartWidth = new DataColumn("Width", Type.GetType("System.Double"));
            DataColumn SteelBasesPartLength = new DataColumn("Length", Type.GetType("System.Double"));
            DataColumn SteelBasesPartMaxBedStrees = new DataColumn("MaxBedStrees", Type.GetType("System.Double"));
            DataColumn SteelBasesPartMaxSteelStrees = new DataColumn("MaxSteelStrees", Type.GetType("System.Double"));

            SteelBasesParts.Columns.Add(SteelBasesPartId);
            SteelBasesParts.Columns.Add(SteelBaseIdInPart);
            SteelBasesParts.Columns.Add(SteelBasePartPicture);
            SteelBasesParts.Columns.Add(SteelBasesPartName);
            SteelBasesParts.Columns.Add(SteelBasesPartCenterX);
            SteelBasesParts.Columns.Add(SteelBasesPartCenterY);
            SteelBasesParts.Columns.Add(SteelBasesPartWidth);
            SteelBasesParts.Columns.Add(SteelBasesPartLength);
            SteelBasesParts.Columns.Add(SteelBasesPartMaxBedStrees);
            SteelBasesParts.Columns.Add(SteelBasesPartMaxSteelStrees);
            #endregion
        }
        #endregion


    }
}
