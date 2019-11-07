﻿using System;
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
        /// <param name="columnBase">База стальной колонны</param>
        public static void SolveSteelColumnBase(SteelBase columnBase)
        {
            ActualizeBaseParts(columnBase);
            ActualizeSteelBolts(columnBase);
            ActualizeLoadCases(columnBase);
            columnBase.IsActual = true;
            GetNdmAreas(columnBase);
            columnBase.ForceCurvatures.Clear();
            foreach (LoadSet loadCase in columnBase.LoadCases)
            {
                try //Запускаем нелинейный расчет
                {
                        //Получаем кривизну соответствующую начальному модулю упругости
                        //Кривизна будет единой для бетона и стали
                    ForceCurvature forceCurvature = GetCurvatureSimpleMethod(loadCase, columnBase);
                        //Заносим кривизну как параметр стальной базы
                    columnBase.ForceCurvatures.Add(forceCurvature);
                        //Получаем элементарные участки стальной базы
                    List<NdmArea> concreteNdmAreas = GetConcreteNdmAreas(columnBase);
                    List<NdmArea> steelNdmAreas = GetSteelNdmAreas(columnBase);
                        //Создаем локальную переменную для кривизны соответствующей бетоны
                        //как кривизну, полученную для базы с начальным модулем упругости
                    Curvature concreteCurvature = forceCurvature.ConcreteCurvature;
                        //Для начала итерационного расчета кривизну стальных участков
                        //как кривизну базы с начальным модулем упругости
                    Curvature steelCurvature = forceCurvature.SteelCurvature;
                        //Получаем матрицу жесткостных коэффициентов для бетона с учетом кривизны, полученной на первом этапе
                        //таким образом будут вычислены участки растянутого бетона если они есть
                        //дальнейшие итерации для бетона не требуются
                    StiffnessCoefficient concreteStiffnessCoefficient = new StiffnessCoefficient(concreteNdmAreas, concreteCurvature);
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
                        StiffnessCoefficient steelStiffnessCoefficient = new StiffnessCoefficient(steelNdmAreas, steelCurvature);
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
        /// <summary>
        /// Актуализирует наборы нагрузок
        /// </summary>
        /// <param name="columnBase">База стальной колонны</param>
        public static void ActualizeLoadCases(SteelBase columnBase)
        {
            if (! columnBase.IsLoadCasesActual)
            {
                columnBase.LoadCases = LoadSetProcessor.GetLoadCases(columnBase.LoadsGroup);
                columnBase.IsLoadCasesActual = true;
            }   
        }
        /// <summary>
        /// Актуализирует участки базы стальной колонны
        /// с учетом возможной симметрии
        /// т.е. заносит все участки как параметр стальной базы
        /// </summary>
        /// <param name="columnBase"></param>
        public static void ActualizeBaseParts(SteelBase columnBase)
        {
            columnBase.ActualSteelBaseParts = new List<SteelBasePart>();
            foreach (SteelBasePart steelBasePart in columnBase.SteelBaseParts)
            {
                columnBase.ActualSteelBaseParts.AddRange(SteelBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart));
            }
        }
        /// <summary>
        /// Актуализирует болты базы стальной колонны
        /// с учетом возможной симметрии
        /// т.е. заносит все болты как параметр стальной базы
        /// </summary>
        /// <param name="columnBase"></param>
        public static void ActualizeSteelBolts(SteelBase columnBase)
        {
            columnBase.ActualSteelBolts = new List<SteelBolt>();
            foreach (SteelBolt steelBolt in columnBase.SteelBolts)
            {
                columnBase.ActualSteelBolts.AddRange(SteelBoltProcessor.GetSteelBoltsFromBolt(steelBolt));
            }
        }
        /// <summary>
        /// Заносит коллекцию элементарных участков для базы стальной колонны
        /// как параметр стальной базы
        /// </summary>
        /// <param name="columnBase">База стальной колонны</param>
        public static void GetNdmAreas(SteelBase columnBase)
        {
            columnBase.NdmAreas = GetConcreteNdmAreas(columnBase);
            columnBase.NdmAreas.AddRange(GetSteelNdmAreas(columnBase));
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
        /// <param name="columnBase">Стальная база</param>
        /// <returns>Коллекция элементарных участков</returns>
        public static List<NdmArea> GetConcreteNdmAreas(SteelBase columnBase)
        {
            List<NdmArea>  NdmAreas = new List<NdmArea>();
            foreach (SteelBasePart steelBasePart in columnBase.ActualSteelBaseParts)
            {
                SteelBasePartProcessor.GetSubParts(steelBasePart);
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
        /// <param name="columnBase">Стальная база</param>
        /// <returns>Набор усилий и кривизн</returns>
        public static ForceCurvature GetCurvatureSimpleMethod(LoadSet loadCase, SteelBase columnBase)
        {
            SumForces sumForces = new SumForces(loadCase);
            List<NdmArea> NdmAreas = GetConcreteNdmAreas(columnBase);
            StiffnessCoefficient stiffnessCoefficient = new StiffnessCoefficient(NdmAreas);
            Curvature curvature = new Curvature(sumForces, stiffnessCoefficient);
            return new ForceCurvature(loadCase, curvature);
        }

    }
}

