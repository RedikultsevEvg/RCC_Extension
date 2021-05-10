using CSL.Common;
using CSL.DataSets.Factories;
using CSL.DataSets.Factories.RCC.Slabs.Punchings;
using CSL.Reports.Interfaces;
using FastReport;
using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.DrawUtils.Interfaces;
using RDBLL.DrawUtils.RCC.Slabs.Punchings;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors;
using RDBLL.Entity.RCC.Slabs.Punchings.Results;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CSL.Reports.RCC.Slabs.Punchings
{
    public class PunchingReport : IReport
    {
        private BuildingSite _BuildingSite;

        private double linearSizeCoefficient = MeasureUnitConverter.GetCoefficient(0);
        private double forceCoefficient = MeasureUnitConverter.GetCoefficient(1);
        private double momentCoefficient = MeasureUnitConverter.GetCoefficient(2);
        private double stressCoefficient = MeasureUnitConverter.GetCoefficient(3);
        private double geometryAreaCoefficient = MeasureUnitConverter.GetCoefficient(4);
        private double geometrySecMomentCoefficient = MeasureUnitConverter.GetCoefficient(5);
        private double geometryMomentCoefficient = MeasureUnitConverter.GetCoefficient(6);
        private double MassCoefficient = MeasureUnitConverter.GetCoefficient(7);

        public DataSet dataSet { get; set; }

        public void ShowReport(string fileName)
        {
            PrepareReport();
            Report report = new Report();
            try
            {
                report.Load(fileName);
                CommonServices.PrepareMeasureUnit(report);
                report.RegisterData(dataSet);
#if DEBUG
                {
                    report.Design();
                }
#else
                {
                    //Если отчет подготовлен, выводим его
                    if (report.Prepare()) report.Show();
                    else MessageBox.Show("Ошибка подготовки отчета");
                }
#endif
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage($"Ошибка вывода отчета", ex);
            }
            finally
            {
                report.Dispose();
            }
        }

        public PunchingReport(BuildingSite buildingSite)
        {
            _BuildingSite = buildingSite;
            IReportDataSetFactory dataSetFactory = new PunchingDataSetFactory();
            dataSet = dataSetFactory.CreateDataSet();
        }

        private void PrepareReport()
        {
            foreach (Building building in _BuildingSite.Children)
            {
                foreach (Level level in building.Children)
                {
                    foreach (IHasParent child in level.Children)
                    {
                        if (child is Punching)
                        {
                            Punching punching = child as Punching;
                            ProcessPunching(punching);
                        }
                    }
                }
            }
        }

        private void ProcessPunching(Punching punching)
        {
            DataTable dataTable = DsOperation.GetDataTable(dataSet, "Punchings");
            //Если расчет не был выполнен ранее, то выполняем расчет
            //if (!(punching.IsActive))
            //{
                IBearingProcessor processor = new BearingProcessor();
                processor.CalcResult(punching);
            //}
            //Если результат был получен успешно, то используем его
            if (punching.IsActive)
            {
                bool isBearing = true;
                PunchingResult punchingResult = punching.Result;
                DataRow row = dataTable.NewRow();
                DsOperation.SetField(row, "Id", punching.Id);
                DsOperation.SetField(row, "ParentId", punching.ParentMember.Id);
                DsOperation.SetField(row, "Name", punching.Name);
                DsOperation.SetField(row, "Height", MathOperation.Round(GeomProcessor.GetTotalHeight(punching) * linearSizeCoefficient));
                DsOperation.SetField(row, "ColumnSizeX", punching.Width * linearSizeCoefficient);
                DsOperation.SetField(row, "ColumnSizeY", punching.Length * linearSizeCoefficient);
                DsOperation.SetField(row, "CoveringLayerX", punching.CoveringLayerX * 1000);
                DsOperation.SetField(row, "CoveringLayerY", punching.CoveringLayerY * 1000);
                #region Picture
                Canvas canvas = new Canvas();
                canvas.Width = 600;
                canvas.Height = 600;
                IDrawScatch drawScatch = new PunchingDrawProcessor();
                drawScatch.DrawTopScatch(canvas, punching);
                byte[] b = CommonServices.ExportToByte(canvas);
                DsOperation.SetField(row, "TopScatch", b);
                #endregion
                dataTable.Rows.Add(row);
                ProcessLoadCases(dataSet, punchingResult);
                ProcessPunchingContours(dataSet, punchingResult);
                double maxBearing = ProcessLoadContour(dataSet, punchingResult);
                if (maxBearing > 1) { isBearing = false; }
                string addc = isBearing ? "" : "не ";
                string gc = $"В соответствии с выполненным расчетом несущая способность на продавливание {addc}обеспечена. ";
                gc += $"Максимальное значение коэффициента использования {MathOperation.Round(maxBearing)}";
                DsOperation.SetField(row, "GenConclusion", gc);
            }
        }

        private double ProcessLoadContour(DataSet dataSet, PunchingResult punchingResult)
        {
            double maxBearing = 0;
            DataTable dataTable = DsOperation.GetDataTable(dataSet, "PunchingLoadSetContours");
            foreach (PunchingCalcResult result in punchingResult.ContourResults)
            {
                DataRow row = dataTable.NewRow();
                DsOperation.SetField(row, "Id", result.Id);
                DsOperation.SetField(row, "ParentId", result.Punching.Id);
                DsOperation.SetField(row, "BearingCoef", result.BearingCoef);
                dataTable.Rows.Add(row);
                if (result.BearingCoef > maxBearing) { maxBearing = result.BearingCoef; }
            }
            return maxBearing;
        }

        private void ProcessPunchingContours(DataSet dataSet, PunchingResult punchingResult)
        {
            DataTable dataTable = DsOperation.GetDataTable(dataSet, "PunchingContours");
            foreach (ContourResult result in punchingResult.PunchingContours)
            {
                DataRow row = dataTable.NewRow();
                DsOperation.SetField(row, "Id", result.Id);
                DsOperation.SetField(row, "ParentId", punchingResult.Punching.Id);
                PunchingContour contour = result.PunchingContour;
                Point2D center = GeomProcessor.GetContourCenter(contour);
                DsOperation.SetField(row, "CenterX", MathOperation.Round(center.X * linearSizeCoefficient));
                DsOperation.SetField(row, "CenterY", MathOperation.Round(center.Y * linearSizeCoefficient));
                dataTable.Rows.Add(row);
                ProcessSubContours(result, result.Id);
            }
        }

        private void ProcessSubContours(ContourResult result, int parentId)
        {
            DataTable dataTable = DsOperation.GetDataTable(dataSet, "PunchingSubContours");
            PunchingContour contour = result.PunchingContour;
            foreach (PunchingSubContour subContour in contour.SubContours)
            {
                DataRow row = dataTable.NewRow();
                int Id = ProgrammSettings.CurrentTmpId;
                DsOperation.SetField(row, "Id", Id);
                DsOperation.SetField(row, "ParentId", parentId);
                //Высота субконтура
                DsOperation.SetField(row, "Height", MathOperation.Round(subContour.Height * linearSizeCoefficient));
                //Длина субконтура
                DsOperation.SetField(row, "Length", MathOperation.Round(GeomProcessor.GetSubContourLength(subContour) * linearSizeCoefficient));
                DsOperation.SetField(row, "ConcreteName", subContour.Concrete.MaterialKind.Name);
                dataTable.Rows.Add(row);
                ProcessLines(subContour, Id);
            }
        }

        private void ProcessLines(PunchingSubContour subContour, int parentId)
        {
            DataTable dataTable = DsOperation.GetDataTable(dataSet, "PunchingLines");
            foreach (PunchingLine line in subContour.Lines)
            {
                DataRow row = dataTable.NewRow();
                int Id = ProgrammSettings.CurrentTmpId;
                DsOperation.SetField(row, "Id", Id);
                DsOperation.SetField(row, "ParentId", parentId);
                DsOperation.SetField(row, "StartX", MathOperation.Round(line.StartPoint.X * linearSizeCoefficient));
                DsOperation.SetField(row, "StartY", MathOperation.Round(line.StartPoint.Y * linearSizeCoefficient));
                DsOperation.SetField(row, "EndX", MathOperation.Round(line.EndPoint.X * linearSizeCoefficient));
                DsOperation.SetField(row, "EndY", MathOperation.Round(line.EndPoint.Y * linearSizeCoefficient));
                dataTable.Rows.Add(row);
            }
        }

        private void ProcessLoadCases(DataSet dataSet, PunchingResult punchingResult)
        {
            Punching punching = punchingResult.Punching;
            CommonServices.AddLoadsToDataset(dataSet, "LoadSets", "Punchings", punching.ForcesGroups[0].LoadSets, punching.Id, punching.Name);
            ObservableCollection<LoadSet> loadSets = new ObservableCollection<LoadSet>();
            foreach (LoadCaseResult caseResult in punchingResult.LoadCases)
            {
                LoadSet loadSet = caseResult.LoadSet;
                loadSet.Id = ProgrammSettings.CurrentTmpId;
                loadSets.Add(loadSet);
            }
            CommonServices.AddLoadsToDataset(dataSet, "LoadCases", "Punchings", loadSets, punching.Id, punching.Name);
        }
    }
}
