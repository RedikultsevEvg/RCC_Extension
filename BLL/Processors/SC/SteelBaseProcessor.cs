using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using RDBLL.Entity.Results.Forces;
using RDBLL.Entity.SC.Column;
using RDBLL.Entity.Results.SC;
using RDBLL.Processors.Forces;
using RDBLL.Common.Geometry;
using RDBLL.Entity.Common.NDM;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using RDBLL.Entity.Results.NDM;
using RDBLL.Entity.Common.NDM.Processors;


namespace RDBLL.Processors.SC
{
    /// <summary>
    /// Класс-процессор для операций со стальной базой
    /// (для разгрузки основного класса)
    /// </summary>
    public static class SteelBaseProcessor
    {
        /// <summary>
        /// Актуализирует все данные стальной базы
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        public static void SolveSteelColumnBase(SteelBase steelBase)
        {
            ActualizeBaseParts(steelBase);
            ActualizeSteelBolts(steelBase);
            ActualizeLoadCases(steelBase);
            GetNdmAreas(steelBase);
            steelBase.ForceCurvatures.Clear();
            if (steelBase.UseSimpleMethod) { SolveSimpleMethod(steelBase); }
            else { SolveNDMMethod (steelBase); }
            steelBase.IsActual = true;
        }
        /// <summary>
        /// Актуализирует наборы нагрузок
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        public static void ActualizeLoadCases(SteelBase steelBase)
        {
            if (! steelBase.IsLoadCasesActual)
            {
                steelBase.LoadCases = LoadSetProcessor.GetLoadCases(steelBase.LoadsGroup);
                steelBase.IsLoadCasesActual = true;
            }   
        }
        /// <summary>
        /// Актуализирует участки базы стальной колонны
        /// с учетом возможной симметрии
        /// т.е. заносит все участки как параметр стальной базы
        /// </summary>
        /// <param name="steelBase"></param>
        public static void ActualizeBaseParts(SteelBase steelBase)
        {
            if (steelBase.IsBasePartsActual) { return;}
            steelBase.ActualSteelBaseParts = new List<SteelBasePart>();
            foreach (SteelBasePart steelBasePart in steelBase.SteelBaseParts)
            {
                steelBase.ActualSteelBaseParts.AddRange(SteelBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart));
            }
            steelBase.IsBasePartsActual = true;
        }
        /// <summary>
        /// Актуализирует болты базы стальной колонны
        /// с учетом возможной симметрии
        /// т.е. заносит все болты как параметр стальной базы
        /// </summary>
        /// <param name="steelBase"></param>
        public static void ActualizeSteelBolts(SteelBase steelBase)
        {
            if (steelBase.IsBoltsActual) { return;}
            steelBase.ActualSteelBolts = new List<SteelBolt>();
            foreach (SteelBolt steelBolt in steelBase.SteelBolts)
            {
                steelBase.ActualSteelBolts.AddRange(SteelBoltProcessor.GetSteelBoltsFromBolt(steelBolt));
            }
            steelBase.IsBoltsActual = true;
        }
        /// <summary>
        /// Заносит коллекцию элементарных участков для базы стальной колонны
        /// как параметр стальной базы
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        public static void GetNdmAreas(SteelBase steelBase)
        {
            steelBase.ConcreteNdmAreas = GetConcreteNdmAreas(steelBase);
            steelBase.SteelNdmAreas=GetSteelNdmAreas(steelBase);
            steelBase.NdmAreas.Clear();
            steelBase.NdmAreas.AddRange(steelBase.ConcreteNdmAreas);
            steelBase.NdmAreas.AddRange(steelBase.SteelNdmAreas);
        }
        public static ForceCurvature GetCurvature(LoadSet loadCase, SteelBase columnBase)
        {
            SumForces sumForces = new SumForces(loadCase);
            StiffnessCoefficient stiffnessCoefficient = new StiffnessCoefficient(columnBase.NdmAreas);
            Curvature curvature = new Curvature(sumForces, stiffnessCoefficient);
            //Определяем новые жесткостные коэффициенты по полученной кривизне
            StiffnessCoefficient newStiffnessCoefficient = new StiffnessCoefficient(columnBase.NdmAreas, curvature);
            Curvature newCurvature = new Curvature(sumForces, newStiffnessCoefficient);
            //try
            //{
                SumForces sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
            //}
            //catch ()
            //{
            //    MessageBox.Show("Проверьте исходные данные", "Ошибка нелинейного расчета");
            //    return;
            //}

            for (int i = 1; i <= 20; i++)
            {
                newCurvature = new Curvature(sumForces, newStiffnessCoefficient);
                newStiffnessCoefficient = new StiffnessCoefficient(columnBase.NdmAreas, newCurvature);
                sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
            }
            sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
            return new ForceCurvature(loadCase, newCurvature);
        }
        /// <summary>
        /// Врзвращает коллекцию элементарных участков бетона для всех участков стальной базы
        /// </summary>
        /// <param name="steelBase">Стальная база</param>
        /// <returns>Коллекция элементарных участков</returns>
        public static List<NdmArea> GetConcreteNdmAreas(SteelBase steelBase)
        {
            List<NdmArea>  NdmAreas = new List<NdmArea>();
            foreach (SteelBasePart steelBasePart in steelBase.ActualSteelBaseParts)
            {
                if (steelBase.UseSimpleMethod) { SteelBasePartProcessor.GetSubParts(steelBasePart); }
                else { SteelBasePartProcessor.GetSubParts(steelBasePart, steelBase.ConcreteStrength); }
                foreach (NdmConcreteArea ndmConcreteArea in steelBasePart.SubParts)
                {
                    NdmAreas.Add(ndmConcreteArea.ConcreteArea);
                }
            }
            return NdmAreas;
        }
        /// <summary>
        /// Возвращает коллекцию элементарных участков стали для всех участков стальной базы
        /// </summary>
        /// <param name="columnBase">Стальная база</param>
        /// <returns>Коллекция элементарных участков</returns>
        public static List<NdmArea> GetSteelNdmAreas(SteelBase columnBase)
        {
            List<NdmArea> NdmAreas = new List<NdmArea>();
            foreach (SteelBolt steelBolt in columnBase.ActualSteelBolts)
            {
                SteelBoltProcessor.GetSubParts(steelBolt);
                NdmAreas.Add(steelBolt.SubPart.SteelArea);
            }
            return NdmAreas;
        }
        /// <summary>
        /// Возвращает набор усилий и кривизн стальной базы
        /// </summary>
        /// <param name="loadCase">Набор сочетаний</param>
        /// <param name="steelBase">Стальная база</param>
        /// <returns>Набор усилий и кривизн</returns>
        public static ForceCurvature GetCurvatureSimpleMethod(LoadSet loadCase, SteelBase steelBase)
        {
            SumForces sumForces = new SumForces(loadCase);
             StiffnessCoefficient stiffnessCoefficient = new StiffnessCoefficient(steelBase.ConcreteNdmAreas);
            Curvature curvature = new Curvature(sumForces, stiffnessCoefficient);
            return new ForceCurvature(loadCase, curvature);
        }
        public static void SolveSimpleMethod(SteelBase steelBase)
        {
            foreach (LoadSet loadCase in steelBase.LoadCases)
            {
                try //Запускаем нелинейный расчет
                {
                    //Получаем кривизну соответствующую начальному модулю упругости
                    //Кривизна будет единой для бетона и стали
                    ForceCurvature forceCurvature = GetCurvatureSimpleMethod(loadCase, steelBase);
                    //Заносим кривизну как параметр стальной базы
                    steelBase.ForceCurvatures.Add(forceCurvature);
                    //Создаем локальную переменную для кривизны соответствующей бетоны
                    //как кривизну, полученную для базы с начальным модулем упругости
                    Curvature concreteCurvature = forceCurvature.ConcreteCurvature;
                    //Для начала итерационного расчета кривизну стальных участков
                    //как кривизну базы с начальным модулем упругости
                    Curvature steelCurvature = forceCurvature.SteelCurvature;
                    //Получаем матрицу жесткостных коэффициентов для бетона с учетом кривизны, полученной на первом этапе
                    //таким образом будут вычислены участки растянутого бетона если они есть
                    //дальнейшие итерации для бетона не требуются
                    StiffnessCoefficient concreteStiffnessCoefficient = new StiffnessCoefficient(steelBase.ConcreteNdmAreas, concreteCurvature);
                    //Получаем матрицу усилий по набору нагрузок
                    SumForces initForces = new SumForces(loadCase);
                    //Вычисляем усилия в бетоне с учетом жесткостных коэффициентов учитывающих растянутую зону
                    SumForces concreteForces = new SumForces(concreteStiffnessCoefficient, concreteCurvature);
                    //Вычисляем усилие, которе должно быть воспринято болтами как разницу
                    //между начальным усилием и усилием, воспринимаемым бетоном
                    SumForces deltaForces = new SumForces(initForces, concreteForces);
                    //deltaForces.ForceMatrix[2, 0] = deltaForces.ForceMatrix[2, 0]/2;
                    //Делаем 20 итерация для более точного расчета
                    //практика показала, что достаточная точность достигается примерно за 5-7 итераций
                    //В будущем надо будет сделать проверку погрешности для возможного уменьшения количества итераций
                    //и вывод хода нелинейного расчета
                    for (int i = 1; i <= 5; i++)
                    {
                        //Уточняем жесткостные коэффициенты с учетом кривизны, полученной на предыдущем этапе
                        StiffnessCoefficient steelStiffnessCoefficient = new StiffnessCoefficient(steelBase.SteelNdmAreas, steelCurvature);
                        //Получаем новое значение кривизны для болтов с учетом жесткостных коэффициентов, полученных
                        //на предыдущем этапе
                        steelCurvature = new Curvature(deltaForces, steelStiffnessCoefficient);
                    }
                    //заносим полученное значение кривизны стальных участков как параметр базы
                    forceCurvature.SteelCurvature = steelCurvature;
                }
                catch //Ошибка нелинейного расчета
                {
                    //Ошибка нелинейного расчета говорит о том, что сходимость не достигнута,
                    //например, происходит опрокидывание базы из-за недостаточного количества болтов
                    MessageBox.Show("Проверьте исходные данные", "Ошибка нелинейного расчета");
                }
            }
        }

        public static void SolveNDMMethod(SteelBase steelBase)
        {
            foreach (LoadSet loadCase in steelBase.LoadCases)
            {
                SumForces sumForces = new SumForces(loadCase);
                StiffnessCoefficient stiffnessCoefficient = new StiffnessCoefficient(steelBase.NdmAreas);
                Curvature curvature = new Curvature(sumForces, stiffnessCoefficient);
                //Определяем новые жесткостные коэффициенты по полученной кривизне
                StiffnessCoefficient newStiffnessCoefficient = new StiffnessCoefficient(steelBase.NdmAreas, curvature);
                Curvature newCurvature = new Curvature(sumForces, newStiffnessCoefficient);
                try
                {
                    SumForces sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
                    for (int i = 1; i <= 20; i++)
                    {
                        newCurvature = new Curvature(sumForces, newStiffnessCoefficient);
                        newStiffnessCoefficient = new StiffnessCoefficient(steelBase.NdmAreas, newCurvature);
                        sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
                    }
                    sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
                    ForceCurvature forceCurvature = new ForceCurvature(loadCase, newCurvature);
                    steelBase.ForceCurvatures.Add(forceCurvature);
                }
                catch
                {
                    MessageBox.Show("Проверьте исходные данные", "Ошибка нелинейного расчета");
                }
            }
        }
        /// <summary>
        /// Возвращает коллекцию прямоугольных участков со значениями и комбинации нагрузок,
        /// к которой они относятся
        /// </summary>
        /// <param name="steelBase"></param>
        /// <returns></returns>
        public static List<LoadCaseRectangleValue> GetRectangleValues(SteelBase steelBase)
        {
            if (! steelBase.IsActual) { SteelBaseProcessor.SolveSteelColumnBase(steelBase); }
            List<LoadCaseRectangleValue> loadCaseRectangleValues = new List<LoadCaseRectangleValue>();
            foreach (ForceCurvature forceCurvature in steelBase.ForceCurvatures)
            {
                LoadCaseRectangleValue loadCaseRectangleValue = new LoadCaseRectangleValue();
                loadCaseRectangleValue.LoadCase = forceCurvature.LoadSet;
                foreach (SteelBasePart steelBasePart in steelBase.ActualSteelBaseParts)
                {
                    foreach (NdmConcreteArea ndmConcreteArea in steelBasePart.SubParts)
                    {
                        RectangleValue rectangleValue = new RectangleValue();
                        rectangleValue.CenterX = ndmConcreteArea.ConcreteArea.CenterX;
                        rectangleValue.CenterY = ndmConcreteArea.ConcreteArea.CenterY;
                        rectangleValue.Width = ndmConcreteArea.Width;
                        rectangleValue.Length = ndmConcreteArea.Length;
                        rectangleValue.Value = NdmAreaProcessor.GetStrainFromCuvature(ndmConcreteArea.ConcreteArea, forceCurvature.ConcreteCurvature)[1];
                        loadCaseRectangleValue.RectangleValues.Add(rectangleValue);
                    }
                }
                loadCaseRectangleValues.Add(loadCaseRectangleValue);
            }
            return loadCaseRectangleValues;
        }


    }
}

