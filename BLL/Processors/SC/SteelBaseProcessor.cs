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


namespace RDBLL.Processors.SC
{
    public static class SteelBaseProcessor
    {
        public static void SolveSteelColumnBase(SteelBase columnBase)
        {
            columnBase.ActualSteelBaseParts = new List<SteelBasePart>();
            ActualizeBaseParts(columnBase);
            ActualizeSteelBolts(columnBase);
            ActualizeLoadCases(columnBase);
            columnBase.IsActual = true;
            GetNdmAreas(columnBase);
            columnBase.ForceCurvatures.Clear();
            foreach (LoadSet loadCase in columnBase.LoadCases)
            {
                try
                {
                    ForceCurvature forceCurvature = GetCurvatureSimpleMethod(loadCase, columnBase);
                    columnBase.ForceCurvatures.Add(forceCurvature);
                    List<NdmArea> concreteNdmAreas = GetConcreteNdmAreas(columnBase);
                    List<NdmArea> steelNdmAreas = GetSteelNdmAreas(columnBase);
                    Curvature concreteCurvature = forceCurvature.ConcreteCurvature;
                    Curvature steelCurvature = forceCurvature.SteelCurvature;
                    StiffnessCoefficient concreteStiffnessCoefficient = new StiffnessCoefficient(concreteNdmAreas, concreteCurvature);
                    SumForces initForces = new SumForces(loadCase);
                    SumForces concreteForces = new SumForces(concreteStiffnessCoefficient, concreteCurvature);
                    SumForces deltaForces = new SumForces(initForces, concreteForces);
                    for (int i = 1; i <= 20; i++)
                    {
                        StiffnessCoefficient steelStiffnessCoefficient = new StiffnessCoefficient(steelNdmAreas, steelCurvature);
                        //SumForces steelForces = new SumForces(steelStiffnessCoefficient, steelCurvature);
                        steelCurvature = new Curvature(deltaForces, steelStiffnessCoefficient);
                    }
                    forceCurvature.SteelCurvature = steelCurvature;
                }
                catch
                {
                    MessageBox.Show("Проверьте исходные данные", "Ошибка нелинейного расчета");
                }
            }


        }

        public static void ActualizeLoadCases(SteelBase columnBase)
        {
            if (! columnBase.IsLoadCasesActual)
            {
                columnBase.LoadCases = LoadSetProcessor.GetLoadCases(columnBase.LoadsGroup);
                columnBase.IsLoadCasesActual = true;
            }   
        }
        public static void ActualizeBaseParts(SteelBase columnBase)
        {
            foreach (SteelBasePart steelBasePart in columnBase.SteelBaseParts)
            {
                columnBase.ActualSteelBaseParts.AddRange(SteelBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart));
            }
        }
        public static void ActualizeSteelBolts(SteelBase columnBase)
        {
            columnBase.ActualSteelBolts = new List<SteelBolt>();
            foreach (SteelBolt steelBolt in columnBase.SteelBolts)
            {
                columnBase.ActualSteelBolts.AddRange(SteelBoltProcessor.GetSteelBoltsFromBolt(steelBolt));
            }
        }

        /// <summary>
        /// Получает коллекцию элементарных участков для базы стальной колонны
        /// </summary>
        /// <param name="columnBase"></param>
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

