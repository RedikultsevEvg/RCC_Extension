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


namespace RDBLL.Processors.SC
{
    public static class SteelColumnBaseProcessor
    {
        public static void SolveSteelColumnBase(SteelColumnBase columnBase)
        {
            columnBase.ActualSteelBaseParts = new List<SteelBasePart>();
            foreach (SteelBasePart steelBasePart in columnBase.SteelBaseParts)
            {
                columnBase.ActualSteelBaseParts.AddRange(SteelColumnBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart));
            }
            ActualizeSteelBolts(columnBase);
            ActualizeLoadCases(columnBase);
            columnBase.IsActual = true;

            GetNdmAreas(columnBase);
            foreach (LoadSet loadCase in columnBase.LoadCases)
            {
                SumForces sumForces = new SumForces(loadCase);
                StiffnessCoefficient stiffnessCoefficient = new StiffnessCoefficient(columnBase.NdmAreas);
                Curvature curvature = new Curvature(sumForces, stiffnessCoefficient);
                //Определяем новые жесткостные коэффициенты по полученной кривизне
                StiffnessCoefficient newStiffnessCoefficient = new StiffnessCoefficient(columnBase.NdmAreas, curvature);
                Curvature newCurvature = new Curvature(sumForces, newStiffnessCoefficient);
                SumForces sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
                for (int i = 1; i<=20; i++)
                {
                    newCurvature = new Curvature(sumForces, newStiffnessCoefficient);
                    newStiffnessCoefficient = new StiffnessCoefficient(columnBase.NdmAreas, newCurvature);
                    sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
                }            
                sumForces2 = new SumForces(newStiffnessCoefficient, newCurvature);
                Console.WriteLine("Ok");
            }
            Console.WriteLine("Ok");
        }

        public static void ActualizeLoadCases(SteelColumnBase columnBase)
        {
            if (! columnBase.IsLoadCasesActual)
            {
                columnBase.LoadCases = LoadSetProcessor.GetLoadCases(columnBase.LoadsGroup);
                columnBase.IsLoadCasesActual = true;
            }   
        }

        public static void ActualizeSteelBolts(SteelColumnBase columnBase)
        {
            columnBase.ActualSteelBolts = new List<SteelBolt>();
            foreach (SteelBolt steelBolt in columnBase.SteelBolts)
            {
                columnBase.ActualSteelBolts.AddRange(SteelBoltProcessor.GetSteelBoltsFromBolt(steelBolt));
            }
        }

        public static ColumnBaseResult GetResult(SteelColumnBase columnBase)
        {
            ColumnBaseResult columnBaseResult = new ColumnBaseResult();
            ActualizeLoadCases(columnBase);
            columnBaseResult.LoadCases = columnBase.LoadCases;
            RectCrossSection rect = new RectCrossSection(columnBase.Width, columnBase.Length);
            MassProperty massProperty = RectProcessor.GetRectMassProperty(rect);
            double minStress = double.PositiveInfinity;
            double maxStress = double.NegativeInfinity;

            double dx = massProperty.Xmax;
            double dy = massProperty.Ymax;
            double stress;

            foreach (LoadSet loadCase in columnBaseResult.LoadCases)
            {
                for (int i = -1; i <= 1; i+=2)
                {
                    for (int j = -1; j <= 1; j+=2)
                    {
                        stress = LoadSetProcessor.StressInBarSection(loadCase, massProperty, dx * i, dy * j);
                        if (stress > maxStress) { maxStress = stress; }
                        if (stress < minStress) { minStress = stress; }
                    }
                }
            }
            columnBaseResult.Stresses.MinStress = minStress;
            columnBaseResult.Stresses.MaxStress = maxStress;
            return columnBaseResult;
        }
        public static void GetNdmAreas(SteelColumnBase columnBase)
        {
            columnBase.NdmAreas = new List<NdmArea>();

            foreach (SteelBasePart steelBasePart in columnBase.ActualSteelBaseParts)
            {
                SteelColumnBasePartProcessor.GetSubParts(steelBasePart);
                foreach (NdmConcreteArea ndmConcreteArea in steelBasePart.SubParts)
                {
                    columnBase.NdmAreas.Add(ndmConcreteArea.ConcreteArea);
                }
            }
            foreach (SteelBolt steelBolt in columnBase.ActualSteelBolts)
            {
                SteelBoltProcessor.GetSubParts(steelBolt);
                columnBase.NdmAreas.Add(steelBolt.SubPart.SteelArea);
            }
        }
    }
}

