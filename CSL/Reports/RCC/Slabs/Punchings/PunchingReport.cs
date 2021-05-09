using CSL.Common;
using CSL.DataSets.Factories;
using CSL.DataSets.Factories.RCC.Slabs.Punchings;
using CSL.Reports.Interfaces;
using FastReport;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
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

namespace CSL.Reports.RCC.Slabs.Punchings
{
    public class PunchingReport : IReport
    {
        private BuildingSite _BuildingSite;
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
            DataTable dataTable = dataSet.Tables["Punchings"];
            //Если расчет не был выполнен ранее, то выполняем расчет
            //if (!(punching.IsActive))
            //{
                IBearingProcessor processor = new BearingProcessor();
                processor.CalcResult(punching);
            //}
            //Если результат был получен успешно, то используем его
            if (punching.IsActive)
            {
                PunchingResult punchingResult = punching.Result;
                DataRow row = dataTable.NewRow();
                DsOperation.SetField(row, "Id", punching.Id);
                DsOperation.SetField(row, "ParentId", punching.ParentMember.Id);
                DsOperation.SetField(row, "Name", punching.Name);
                dataTable.Rows.Add(row);
                ProcessLoadCases(dataSet, punchingResult);
                //ProcessPunchingContours(dataSet, punchingResult);
            }
        }

        private void ProcessPunchingContours(DataSet dataSet, PunchingResult punchingResult)
        {
            DataTable dataTable = dataSet.Tables["Punchings"];
            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);
            throw new NotImplementedException();
        }

        private void ProcessLoadCases(DataSet dataSet, PunchingResult punchingResult)
        {
            Punching punching = punchingResult.Punching;
            CommonServices.AddLoadsToDataset(dataSet, "LoadSets", "Punchings", punching.ForcesGroups[0].LoadSets, punching.Id, punching.Name);
            ObservableCollection<LoadSet> loadSets = new ObservableCollection<LoadSet>();
            foreach (LoadCaseResult caseResult in punchingResult.LoadCases)
            {
                loadSets.Add(caseResult.LoadSet);
            }
            CommonServices.AddLoadsToDataset(dataSet, "LoadCases", "Punchings", loadSets, punching.Id, punching.Name);
        }
    }
}
