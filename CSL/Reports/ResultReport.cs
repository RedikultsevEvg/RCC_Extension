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
using RDBLL.Entity.MeasureUnits;
using CSL.DataSets.SC;


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
                report.Design();
                //report.Show();
            }
        }
        public void PrepareReport()
        {
            int steelBaseId = 1;
            int LoadCaseId = 1;
            int ForceParameterId = 1;
            int steelBasePartId = 1;
            int steelBaseBoltId = 1;

            double linearSizeCoefficient = MeasureUnitConverter.GetCoefficient(0);
            double ForceCoefficient = MeasureUnitConverter.GetCoefficient(1);
            double MomentSizeCoefficient = MeasureUnitConverter.GetCoefficient(2);
            double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);

            foreach (Building building in _buildingSite.BuildingList)
            {
                foreach (Level level in building.LevelList)
                {
                    DataTable SteelBases = dataSet.Tables[0];
                    foreach (SteelColumnBase steelColumnBase in level.SteelColumnBaseList)
                    {
                        if (! steelColumnBase.IsActual) { SteelColumnBaseProcessor.SolveSteelColumnBase(steelColumnBase); }

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
                        foreach (LoadSet loadSet in steelColumnBase.LoadCases)
                        {
                            DataRow newLoadCase = LoadCases.NewRow();
                            newLoadCase.ItemArray = new object[] { LoadCaseId, steelBaseId, loadSet.Name, loadSet.PartialSafetyFactor};
                            LoadCases.Rows.Add(newLoadCase);

                            DataTable ForceParameters = dataSet.Tables[2];
                            foreach (ForceParameter forceParameter in loadSet.ForceParameters)
                            {
                                DataRow newForceParameter = ForceParameters.NewRow();
                                var tmpForceParamLabels = from t in ProgrammSettings.ForceParamKinds where t.Id == forceParameter.Kind_id select t;
                                MeasureUnitLabel measureUnitLabel = tmpForceParamLabels.First().MeasureUnit.GetCurrentLabel();
                                newForceParameter.ItemArray = new object[] { ForceParameterId,
                                    LoadCaseId,
                                    tmpForceParamLabels.First().LongLabel,
                                    tmpForceParamLabels.First().ShortLabel,
                                    measureUnitLabel.UnitName,
                                    forceParameter.CrcValueInCurUnit,
                                    forceParameter.DesignValue * measureUnitLabel.AddKoeff};
                                ForceParameters.Rows.Add(newForceParameter);
                                ForceParameterId++;
                            }
                            LoadCaseId++;
                        }
                        DataTable SteelBasesParts = dataSet.Tables[3];
                        foreach (SteelBasePart steelBasePart in steelColumnBase.ActualSteelBaseParts)
                        {
                            DataRow newSteelBasePart = SteelBasesParts.NewRow();
                            double maxBedStress = SteelColumnBasePartProcessor.GetGlobalMinStressLinear(steelBasePart) * (-1D);
                            double maxStress = SteelColumnBasePartProcessor.GetResult(steelBasePart, maxBedStress)[1];
                            maxBedStress = Math.Round(stressCoefficient * maxBedStress, 3);
                            maxStress = Math.Round(stressCoefficient * maxStress, 3);
                            #region Picture
                            Canvas canvasPart = new Canvas();
                            canvasPart.Width = 300;
                            canvasPart.Height = 300;
                            double zoom_factor_X = canvasPart.Width / steelBasePart.Width / 1.2;
                            double zoom_factor_Y = canvasPart.Height / steelBasePart.Length / 1.2;
                            double scale_factor;
                            if (zoom_factor_X < zoom_factor_Y) { scale_factor = zoom_factor_X; } else { scale_factor = zoom_factor_Y; }
                            canvasPart.Width = steelBasePart.Width * 1.2 * scale_factor;
                            canvasPart.Height = steelBasePart.Length * 1.2 * scale_factor;
                            double[] columnBaseCenter = new double[2] { canvasPart.Width/2 - steelBasePart.Center[0]*scale_factor, canvasPart.Height/2 + steelBasePart.Center[1] * scale_factor };
                            DrawSteelBase.DrawBasePart(steelBasePart, canvasPart, columnBaseCenter, scale_factor, 1, 1, 1, false);
                            byte[] bPart = ExportToByte(canvasPart);
                            #endregion
                            newSteelBasePart.ItemArray = new object[]
                            { steelBasePartId, steelBaseId,
                                bPart, steelBasePart.Name,
                                steelBasePart.Center[0] * linearSizeCoefficient,
                                steelBasePart.Center[1] * linearSizeCoefficient,
                                steelBasePart.Width * linearSizeCoefficient,
                                steelBasePart.Length * linearSizeCoefficient,
                                maxBedStress,
                                maxStress };
                            SteelBasesParts.Rows.Add(newSteelBasePart);
                            steelBasePartId++;
                        }
                        DataTable SteelBasesBolts = dataSet.Tables[4];
                        foreach (SteelBolt steelBolt in steelColumnBase.ActualSteelBolts)
                        {
                            DataRow newSteelBaseBolt = SteelBasesBolts.NewRow();
                            newSteelBaseBolt.ItemArray = new object[]
                            {steelBaseBoltId, steelBaseId, steelBolt.Name,
                                steelBolt.Diameter * linearSizeCoefficient,
                                steelBolt.CenterX * linearSizeCoefficient,
                                steelBolt.CenterY * linearSizeCoefficient,
                                0 * stressCoefficient,
                                0 * stressCoefficient };
                            SteelBasesBolts.Rows.Add(newSteelBaseBolt);
                            steelBaseBoltId++;
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
            dataSet = SteelColumnBaseDataSet.GetDataSet();
        }
        #endregion


    }
}
