using RDBLL.Common.Geometry;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors;
using RDBLL.Forces;
using RDBLL.Processors.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings.Results.Factories
{
    public class PunchingResultBuilder : IPunchingResultBuilder
    {
        private Punching _Punching;
        private PunchingResult _Result = new PunchingResult();
        public PunchingResult GetPunchingResult()
        {
            GetCommon();
            GetPunchingLoadCases();
            GetPunchingContours();
            GetPunchingContourResults();
            return _Result;
        }

        private void GetCommon()
        {
            _Result.Id = ProgrammSettings.CurrentTmpId;
            _Result.Punching = _Punching;
        }

        private void GetPunchingContourResults()
        {
            _Result.ContourResults = new List<PunchingCalcResult>();
            foreach (LoadCaseResult load in _Result.LoadCases)
            {

                foreach (ContourResult contourResult in _Result.PunchingContours)
                {
                    PunchingContour contour = contourResult.PunchingContour;
                    //Получаем центр тяжести контура
                    Point2D contourCenter = PunchingGeometryProcessor.GetContourCenter(contour);
                    //Приводим нагрузки к центру тяжести контура
                    LoadSet transformedSet = LoadSetProcessor.GetLoadSetTransform(load.LoadSet, contourCenter);              
                    //Процессор для вычисления несущей способности
                    IBearingProcessor bearingProcessor = new BearingProcessor();
                    //расчет на полную нагрузку
                    double coef = bearingProcessor.GetBearingCapacityCoefficient(contour, load.LoadSet);
                    //Расчет на длительные нагрузки
                    //double coef = bearingProcessor.GetBearingCapacityCoefficient(contour, Nz, Mx, My, false);
                    PunchingCalcResult calcResult  = new PunchingCalcResult();
                    //Добавляем код результата
                    calcResult.Id = ProgrammSettings.CurrentTmpId;
                    //Добавляем ссылку на продавливание
                    calcResult.Punching = _Punching;
                    //Добавляем ссылку на нагрузку
                    calcResult.LoadSet = load.LoadSet;
                    //Добавляем ссылку на преобразованную коллекцию нагрузок
                    calcResult.TransformedLoadSet = LoadSetProcessor.GetLoadSetTransform(load.LoadSet, contourCenter);
                    //Добавляем ссылку на контур
                    calcResult.PunchingContour = contour;
                    //Добавляем рассчитанный коэффициент
                    calcResult.BearingCoef = coef;
                    //Добавляем результат
                    _Result.ContourResults.Add(calcResult);
                }
            }
        }
        //Добавляем комбинации нагрузок
        private void GetPunchingLoadCases()
        {
            _Result.LoadCases = new List<LoadCaseResult>();
            ObservableCollection<LoadSet> loadSets = LoadSetProcessor.GetLoadCases(_Punching.ForcesGroups);
            foreach (LoadSet loadSet in loadSets)
            {
                LoadSet load = loadSet;
                LoadCaseResult loadCase = new LoadCaseResult();
                loadCase.Id = ProgrammSettings.CurrentTmpId;
                loadCase.LoadSet = load;
                _Result.LoadCases.Add(loadCase);
            }

        }
        //Добавляем контуры продавливания
        private void GetPunchingContours()
        {
            _Result.PunchingContours = new List<ContourResult>();
            ILayerProcessor layerProcessor = new MultiLayersProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(_Punching);
            foreach (PunchingContour contour in contours)
            {
                ContourResult contourResult = new ContourResult { Id = ProgrammSettings.CurrentTmpId };
                contourResult.PunchingContour = contour;
                contourResult.Center = PunchingGeometryProcessor.GetContourCenter(contour);
                _Result.PunchingContours.Add(contourResult);
            }         
        }

        public PunchingResultBuilder(Punching punching)
        {
            _Punching = punching;
        }
    }
}
