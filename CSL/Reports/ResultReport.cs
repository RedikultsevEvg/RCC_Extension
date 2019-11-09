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
        private BuildingSite _buildingSite;
        private double linearSizeCoefficient;
        private double forceCoefficient;
        private double momentCoefficient;
        private double stressCoefficient;
        private double geometryAreaCoefficient;
        private double geometrySecMomentCoefficient;
        private double geometryMomentCoefficient;
        private double MassCoefficient;
        public DataSet dataSet { get; set; }

        public void ShowReport(string fileName)
        {
            using (Report report = new Report())
            {
                report.Load(fileName);
                report.SetParameterValue("Units.LinearSize", MeasureUnitConverter.GetUnitLabelText(0));
                report.SetParameterValue("Units.Force", MeasureUnitConverter.GetUnitLabelText(1));
                report.SetParameterValue("Units.Moment", MeasureUnitConverter.GetUnitLabelText(2));
                report.SetParameterValue("Units.Stress", MeasureUnitConverter.GetUnitLabelText(3));
                report.SetParameterValue("Units.Geometry.Area", MeasureUnitConverter.GetUnitLabelText(4));
                report.SetParameterValue("Units.Geometry.SecMoment", MeasureUnitConverter.GetUnitLabelText(5));
                report.SetParameterValue("Units.Geometry.Moment", MeasureUnitConverter.GetUnitLabelText(6));
                report.SetParameterValue("Units.Mass", MeasureUnitConverter.GetUnitLabelText(7));
                report.RegisterData(dataSet);
                //report.Design();
                report.Show();
            }
        }
        public void PrepareReport()
        {
            foreach (Building building in _buildingSite.Buildings)
            {
                foreach (Level level in building.Levels)
                {
                    DataTable SteelBases = dataSet.Tables["SteelBases"];
                    foreach (SteelBase steelBase in level.SteelBases)
                    {
                        //if (! steelColumnBase.IsActual)
                        //{
                            SteelBaseProcessor.SolveSteelColumnBase(steelBase);
                        //}

                        DataRow newSteelBase = SteelBases.NewRow();
                        Double A = steelBase.Width * steelBase.Length;
                        Double Wx = steelBase.Width * steelBase.Length * steelBase.Length / 6;
                        Double Wy = steelBase.Length * steelBase.Width * steelBase.Width / 6;
                        #region Picture
                        Canvas canvas = new Canvas();
                        canvas.Width = 600;
                        canvas.Height = 600;
                        DrawSteelBase.DrawBase(steelBase, canvas);
                        byte[] b = ExportToByte(canvas);
                        #endregion
                        newSteelBase.ItemArray = new object[]
                        { steelBase.Id, b, steelBase.Name,
                            steelBase.SteelStrength * stressCoefficient,
                            steelBase.ConcreteStrength * stressCoefficient,
                            steelBase.Width * linearSizeCoefficient,
                            steelBase.Length * linearSizeCoefficient,
                            steelBase.Thickness * linearSizeCoefficient,
                            A * geometryAreaCoefficient,
                            Wx * geometrySecMomentCoefficient,
                            Wy * geometrySecMomentCoefficient};
                        
                        SteelBases.Rows.Add(newSteelBase);

                        ProcessLoadSets(steelBase);
                        ProcessLoadCases(steelBase);
                        ProcessPart(steelBase);
                        ProcessBolt(steelBase);
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
            dataSet = SteelBaseDataSet.GetDataSet();

            linearSizeCoefficient = MeasureUnitConverter.GetCoefficient(0);
            forceCoefficient = MeasureUnitConverter.GetCoefficient(1);
            momentCoefficient = MeasureUnitConverter.GetCoefficient(2);
            stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
            geometryAreaCoefficient = MeasureUnitConverter.GetCoefficient(4);
            geometrySecMomentCoefficient = MeasureUnitConverter.GetCoefficient(5);
            geometryMomentCoefficient = MeasureUnitConverter.GetCoefficient(6);
            MassCoefficient = MeasureUnitConverter.GetCoefficient(7);
        }
        #endregion
        private void ProcessLoadSets(SteelBase steelBase)
        {
            DataTable LoadSets = dataSet.Tables["LoadSets"];
            foreach (LoadSet loadSet in steelBase.LoadsGroup[0].LoadSets)
            {
                string loadSetDescription, crcForceDescription = "", designForceDescription = "";
                DataRow newLoadSet = LoadSets.NewRow();
                DataTable ForceParameters = dataSet.Tables["LoadSetsForceParameters"];
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
                        steelBase.Id,
                        steelBase.Name,
                        loadSet.Id,
                        loadSet.Name,
                        loadSetDescription,
                        tmpForceParamLabels.First().LongLabel,
                        tmpForceParamLabels.First().ShortLabel,
                        measureUnitLabel.UnitName,
                        Math.Round(forceParameter.CrcValueInCurUnit, 3),
                        Math.Round(forceParameter.CrcValue * loadSet.PartialSafetyFactor * measureUnitLabel.AddKoeff, 3)
                    };
                    ForceParameters.Rows.Add(newForceParameter);
                    crcForceDescription += tmpForceParamLabels.First().ShortLabel + "=";
                    crcForceDescription += Math.Round(forceParameter.CrcValue * measureUnitLabel.AddKoeff, 3);
                    crcForceDescription += measureUnitLabel.UnitName + "; ";

                    designForceDescription += tmpForceParamLabels.First().ShortLabel + "=";
                    designForceDescription += Math.Round(forceParameter.CrcValue * loadSet.PartialSafetyFactor * measureUnitLabel.AddKoeff, 3);
                    designForceDescription += measureUnitLabel.UnitName + "; ";
                }
                newLoadSet.ItemArray = new object[]
                    {   loadSet.Id,
                        steelBase.Id,
                        loadSet.Name,
                        loadSet.PartialSafetyFactor,
                        crcForceDescription,
                        designForceDescription
                    };
                LoadSets.Rows.Add(newLoadSet);
            }
        }
        /// <summary>
        /// Добавление в датасет данных сочетаниям нагрузок
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        private void ProcessLoadCases(SteelBase steelBase)
        {
            DataTable LoadCases = dataSet.Tables["LoadCases"];
            foreach (LoadSet loadSet in steelBase.LoadCases)
            {
                string forceDescription = "";
                DataRow newLoadCase = LoadCases.NewRow();
                DataTable ForceParameters = dataSet.Tables["LoadCasesForceParameters"];
                foreach (ForceParameter forceParameter in loadSet.ForceParameters)
                {
                    DataRow newForceParameter = ForceParameters.NewRow();
                    var tmpForceParamLabels = from t in ProgrammSettings.ForceParamKinds where t.Id == forceParameter.KindId select t;
                    MeasureUnitLabel measureUnitLabel = tmpForceParamLabels.First().MeasureUnit.GetCurrentLabel();
                    newForceParameter.ItemArray = new object[] { tmpForceParamLabels.First().Id,
                        loadSet.Id,
                        tmpForceParamLabels.First().LongLabel,
                        tmpForceParamLabels.First().ShortLabel,
                        measureUnitLabel.UnitName,
                        Math.Round(forceParameter.CrcValueInCurUnit, 3),
                        Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3)
                    };
                    ForceParameters.Rows.Add(newForceParameter);
                    forceDescription += tmpForceParamLabels.First().ShortLabel +"="+ Math.Round(forceParameter.DesignValue * measureUnitLabel.AddKoeff, 3) + measureUnitLabel.UnitName + "; ";
                }
                newLoadCase.ItemArray = new object[]
                    { loadSet.Id, steelBase.Id,
                    loadSet.Name, loadSet.PartialSafetyFactor,
                    forceDescription
                    };
                LoadCases.Rows.Add(newLoadCase);
            }
        }
        /// <summary>
        /// Добавление в датасет данных по участку стальной базы
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        private void ProcessPart(SteelBase steelBase)
        {
            DataTable dataTable = dataSet.Tables["SteelBasesParts"];
            foreach (SteelBasePart steelBasePart in steelBase.ActualSteelBaseParts)
            {
                DataRow newSteelBasePart = dataTable.NewRow();
                //double maxBedStress = SteelColumnBasePartProcessor.GetGlobalMinStressLinear(steelBasePart) * (-1D);
                double maxBedStress = SteelBasePartProcessor.GetGlobalMinStressNonLinear(steelBasePart) * (-1D);
                double maxStress = SteelBasePartProcessor.GetResult(steelBasePart, maxBedStress)[1];
                double actualThickness = steelBasePart.SteelBase.Thickness;
                double steelStrength = steelBasePart.SteelBase.SteelStrength;
                double recomendedThickness = 0;
                if (maxStress>0) { recomendedThickness = actualThickness * Math.Pow((maxStress / steelStrength), 0.5); }
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
                double[] columnBaseCenter = new double[2] { canvasPart.Width / 2 - steelBasePart.CenterX * scale_factor, canvasPart.Height / 2 + steelBasePart.CenterY * scale_factor };
                DrawSteelBase.DrawBasePart(steelBasePart, canvasPart, columnBaseCenter, scale_factor, 1, 1, 1, false);
                byte[] bPart = ExportToByte(canvasPart);
                #endregion
                newSteelBasePart.ItemArray = new object[]
                { steelBasePart.Id, steelBase.Id,
                                bPart, steelBasePart.Name,
                                steelBasePart.CenterX * linearSizeCoefficient,
                                steelBasePart.CenterY * linearSizeCoefficient,
                                steelBasePart.Width * linearSizeCoefficient,
                                steelBasePart.Length * linearSizeCoefficient,
                                Math.Round(stressCoefficient * maxBedStress, 3),
                                Math.Round(stressCoefficient * maxStress, 3),
                                Math.Round(linearSizeCoefficient * recomendedThickness, 3)
            };
                dataTable.Rows.Add(newSteelBasePart);
            }

        }
        /// <summary>
        /// Добавление в датасет данных по болту стальной базы
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        private void ProcessBolt(SteelBase steelBase)
        {
            DataTable dataTable = dataSet.Tables["SteelBasesBolts"];
            foreach (SteelBolt steelBolt in steelBase.ActualSteelBolts)
            {
                DataRow newSteelBaseBolt = dataTable.NewRow();
                double area = steelBolt.Diameter * steelBolt.Diameter * 0.785;
                double maxStress = SteelBoltProcessor.GetMaxStressNonLinear(steelBolt);
                double force = maxStress * area;
                newSteelBaseBolt.ItemArray = new object[]
                {steelBolt.Id, steelBase.Id, steelBolt.Name,
                                steelBolt.Diameter * linearSizeCoefficient,
                                steelBolt.CenterX * linearSizeCoefficient,
                                steelBolt.CenterY * linearSizeCoefficient,
                                Math.Round(maxStress * stressCoefficient, 3),
                                Math.Round(force * forceCoefficient, 3) };
                dataTable.Rows.Add(newSteelBaseBolt);
            }
        }

    }
}
