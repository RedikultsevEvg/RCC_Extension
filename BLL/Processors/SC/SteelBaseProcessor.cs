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
using RDBLL.Entity.Common.Materials;

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
            if (steelBase.UseSimpleMethod) { steelBase.IsActual = SolveSimpleMethod(steelBase); }
            else { steelBase.IsActual = SolveNDMMethod(steelBase); }
        }
        /// <summary>
        /// Актуализирует наборы нагрузок
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        public static void ActualizeLoadCases(SteelBase steelBase)
        {
            steelBase.LoadCases = LoadSetProcessor.GetLoadCases(steelBase.ForcesGroups); 
        }
        /// <summary>
        /// Актуализирует участки базы стальной колонны
        /// с учетом возможной симметрии
        /// т.е. заносит все участки как параметр стальной базы
        /// </summary>
        /// <param name="steelBase"></param>
        public static void ActualizeBaseParts(SteelBase steelBase)
        {
            steelBase.ActualSteelBaseParts = new List<SteelBasePart>();
            foreach (SteelBasePart steelBasePart in steelBase.SteelBaseParts)
            {
                steelBase.ActualSteelBaseParts.AddRange(SteelBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart));
            }
        }
        /// <summary>
        /// Актуализирует болты базы стальной колонны
        /// с учетом возможной симметрии
        /// т.е. заносит все болты как параметр стальной базы
        /// </summary>
        /// <param name="steelBase"></param>
        public static void ActualizeSteelBolts(SteelBase steelBase)
        {
            steelBase.ActualSteelBolts = new List<SteelBolt>();
            foreach (SteelBolt steelBolt in steelBase.SteelBolts)
            {
                steelBase.ActualSteelBolts.AddRange(SteelBoltProcessor.GetSteelBoltsFromBolt(steelBolt));
            }
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
        public static ForceDoubleCurvature GetCurvature(LoadSet loadCase, SteelBase columnBase)
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
            return new ForceDoubleCurvature(loadCase, newCurvature);
        }
        /// <summary>
        /// Врзвращает коллекцию элементарных участков бетона для всех участков стальной базы
        /// </summary>
        /// <param name="steelBase">Стальная база</param>
        /// <returns>Коллекция элементарных участков</returns>
        public static List<NdmArea> GetConcreteNdmAreas(SteelBase steelBase)
        {
            List<NdmArea>  NdmAreas = new List<NdmArea>();
            ConcreteKind concreteKind = steelBase.Conrete.MaterialKind as ConcreteKind;
            double concreteStrength = concreteKind.FstCompStrength;
            foreach (SteelBasePart steelBasePart in steelBase.ActualSteelBaseParts)
            {
                if (steelBase.UseSimpleMethod) { SteelBasePartProcessor.GetSubParts(steelBasePart); }
                else { SteelBasePartProcessor.GetSubParts(steelBasePart, concreteStrength); }
                foreach (NdmRectangleArea ndmConcreteArea in steelBasePart.SubParts)
                {
                    NdmAreas.Add(ndmConcreteArea);
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
                NdmAreas.Add(steelBolt.SubPart);
            }
            return NdmAreas;
        }
        /// <summary>
        /// Возвращает набор усилий и кривизн стальной базы
        /// </summary>
        /// <param name="loadCase">Набор сочетаний</param>
        /// <param name="steelBase">Стальная база</param>
        /// <returns>Набор усилий и кривизн</returns>
        public static ForceDoubleCurvature GetCurvatureSimpleMethod(LoadSet loadCase, SteelBase steelBase)
        {
            SumForces sumForces = new SumForces(loadCase);
             StiffnessCoefficient stiffnessCoefficient = new StiffnessCoefficient(steelBase.ConcreteNdmAreas);
            Curvature curvature = new Curvature(sumForces, stiffnessCoefficient);
            return new ForceDoubleCurvature(loadCase, curvature);
        }
        public static bool SolveSimpleMethod(SteelBase steelBase)
        {
            bool result = true;
            foreach (LoadSet loadCase in steelBase.LoadCases)
            {
                try //Запускаем нелинейный расчет
                {
                    //Получаем кривизну соответствующую начальному модулю упругости
                    //Кривизна будет единой для бетона и стали
                    ForceDoubleCurvature forceCurvature = GetCurvatureSimpleMethod(loadCase, steelBase);
                    //Заносим кривизну как параметр стальной базы
                    steelBase.ForceCurvatures.Add(forceCurvature);
                    //Создаем локальную переменную для кривизны соответствующей бетоны
                    //как кривизну, полученную для базы с начальным модулем упругости
                    Curvature concreteCurvature = forceCurvature.DesignCurvature;
                    //Для начала итерационного расчета кривизну стальных участков
                    //как кривизну базы с начальным модулем упругости
                    Curvature steelCurvature = forceCurvature.SecondDesignCurvature;
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
                    forceCurvature.SecondDesignCurvature = steelCurvature;
                }
                catch //Ошибка нелинейного расчета
                {
                    //Ошибка нелинейного расчета говорит о том, что сходимость не достигнута,
                    //например, происходит опрокидывание базы из-за недостаточного количества болтов
                    MessageBox.Show("Проверьте исходные данные", "Ошибка нелинейного расчета");
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Решение базы стальной колонны нелинейным методом
        /// </summary>
        /// <param name="steelBase"></param>
        /// <returns>true - расчет выполнен успешно</returns>
        public static bool SolveNDMMethod(SteelBase steelBase)
        {
            bool result = true;
            foreach (LoadSet loadCase in steelBase.LoadCases)
            {
                SumForces sumForces = new SumForces(loadCase);
                List<NdmArea> ndmAreas = steelBase.NdmAreas;
                try
                {
                    //Если нелинейный расчет не выполнится, то будет сгенерировано исключение
                    ForceDoubleCurvature forceCurvature = new ForceDoubleCurvature(loadCase, NdmProcessor.GetCurvature(sumForces, ndmAreas));
                    steelBase.ForceCurvatures.Add(forceCurvature);
                }
                //Если хотя бы один случай нагружения даст ошибку, то общий результат будет false
                catch
                {
                    MessageBox.Show("Ошибка нелинейного расчета", $"Сочетание: {loadCase.Name}");
                    result = false;
                }
            }
            return result;
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
            foreach (ForceDoubleCurvature forceCurvature in steelBase.ForceCurvatures)
            {
                LoadCaseRectangleValue loadCaseRectangleValue = new LoadCaseRectangleValue();
                loadCaseRectangleValue.LoadCase = forceCurvature.LoadSet;
                foreach (SteelBasePart steelBasePart in steelBase.ActualSteelBaseParts)
                {
                    foreach (NdmRectangleArea ndmConcreteArea in steelBasePart.SubParts)
                    {
                        RectangleValue rectangleValue = new RectangleValue();
                        rectangleValue.CenterX = ndmConcreteArea.CenterX;
                        rectangleValue.CenterY = ndmConcreteArea.CenterY;
                        rectangleValue.Width = ndmConcreteArea.Width;
                        rectangleValue.Length = ndmConcreteArea.Length;
                        rectangleValue.Value = NdmAreaProcessor.GetStrainFromCuvature(ndmConcreteArea, forceCurvature.DesignCurvature)[1];
                        loadCaseRectangleValue.RectangleValues.Add(rectangleValue);
                    }
                }
                loadCaseRectangleValues.Add(loadCaseRectangleValue);
            }
            return loadCaseRectangleValues;
        }


    }
}

