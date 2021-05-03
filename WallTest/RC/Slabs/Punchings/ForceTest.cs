using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings.Factories;
using RDBLL.Entity.RCC.Slabs.Punchings.Processors;

namespace Test.RC.Slabs.Punchings
{
    [TestClass]
    public class ForceTest
    {
        [TestMethod]
        public void ForceOnlyTest1()
        {
            double Nz = 400000;
            double Mx = 0;
            double My = 0;

            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);

            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, true);
                double Rbt = 980000;
                double expected = Nz / (Rbt * um * h0) + Mx / (Rbt * wx * h0) + Mx / (Rbt * wx * h0);
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, false);
                double Rbt = 980000 * 0.9;
                double expected = Nz / (Rbt * um * h0) + Mx / (Rbt * wx * h0) + Mx / (Rbt * wx * h0);
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
        }
        [TestMethod]
        public void ForceAndMomentTest1()
        {
            double Nz = 400000;
            double Mx = 20000;
            double My = 20000;

            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);
            //Проверяем при полной нагрузке, включая кратковременные
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, true);
                double Rbt = 980000;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
            //Проверяем при длительных нагрузках
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, false);
                double Rbt = 980000 * 0.9;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01); ;
            }
        }
        [TestMethod]
        public void ForceAndMomentTest11()
        {
            //В тесте должно сработать ограничение по вкладу моментов
            double Nz = 200000;
            double Mx = 20000;
            double My = 20000;

            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);
            //Проверяем при полной нагрузке, включая кратковременные
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, true);
                double Rbt = 980000;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
            //Проверяем при длительных нагрузках
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, false);
                double Rbt = 980000 * 0.9;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01); ;
            }
        }
        [TestMethod]
        public void ForceAndMomentTest12()
        {
            //В тесте должно сработать ограничение по вкладу моментов
            //Значения моментов Mx отрицательные
            double Nz = 200000;
            double Mx = -20000;
            double My = 20000;

            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);
            //Проверяем при полной нагрузке, включая кратковременные
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, true);
                double Rbt = 980000;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
            //Проверяем при длительных нагрузках
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, false);
                double Rbt = 980000 * 0.9;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01); ;
            }
        }
        [TestMethod]
        public void ForceAndMomentTest13()
        {
            //В тесте должно сработать ограничение по вкладу моментов
            //Значения моментов My отрицательные
            double Nz = 200000;
            double Mx = 20000;
            double My = -20000;

            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);
            //Проверяем при полной нагрузке, включая кратковременные
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, true);
                double Rbt = 980000;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
            //Проверяем при длительных нагрузках
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, false);
                double Rbt = 980000 * 0.9;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01); ;
            }
        }
        [TestMethod]
        public void ForceAndMomentTest14()
        {
            //В тесте должно сработать ограничение по вкладу моментов
            //Значения моментов Mx и My отрицательные
            double Nz = 200000;
            double Mx = -20000;
            double My = -20000;

            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);
            //Проверяем при полной нагрузке, включая кратковременные
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, true);
                double Rbt = 980000;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01);
            }
            //Проверяем при длительных нагрузках
            foreach (PunchingContour contour in contours)
            {
                double actual = bearingProcessor.GetBearindCapacityCoefficient(contour, Nz, Mx, My, false);
                double Rbt = 980000 * 0.9;
                double exp_N = Math.Abs(Nz / (Rbt * um * h0));
                double exp_Mx = Math.Abs(Mx / (Rbt * wx * h0));
                double exp_My = Math.Abs(My / (Rbt * wy * h0));
                double exp_M = exp_Mx + exp_My;
                double expected;
                if (exp_M < (0.5 * exp_N))
                {
                    expected = exp_N + exp_Mx + exp_My;
                }
                else { expected = exp_N * 1.5; }
                Assert.AreEqual(expected, actual, expected * 0.01); ;
            }
        }
        [TestMethod]
        public void MomentOfResistanceTest1()
        {
            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType1_400х400х200);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);

            foreach (PunchingContour contour in contours)
            {
                double Rbt = 980000;
                double wxPos = Rbt * wx * h0;
                double wyPos = Rbt * wy * h0;
                double[] actualW = (bearingProcessor as BearingProcessor).GetMomentResistance(contour, true);
                double actualWxPos = actualW[0];
                double actualWxNeg = actualW[1];
                double actualWyPos = actualW[2];
                double actualWyNeg = actualW[3];
                Assert.AreEqual(wxPos, actualWxPos, wxPos * 0.01);
                Assert.AreEqual(-wxPos, actualWxNeg, wxPos * 0.01);
                Assert.AreEqual(wyPos, actualWyPos, wyPos * 0.01);
                Assert.AreEqual(-wyPos, actualWyNeg, wyPos * 0.01);
            }
        }
        [TestMethod]
        public void MomentOfResistanceTest2()
        {
            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType2_400х600х300);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);

            foreach (PunchingContour contour in contours)
            {
                double Rbt = 980000;
                double wxPos = Rbt * wx * h0;
                double wyPos = Rbt * wy * h0;
                double[] actualW = (bearingProcessor as BearingProcessor).GetMomentResistance(contour, true);
                double actualWxPos = actualW[0];
                double actualWxNeg = actualW[1];
                double actualWyPos = actualW[2];
                double actualWyNeg = actualW[3];
                Assert.AreEqual(wxPos, actualWxPos, wxPos * 0.01);
                Assert.AreEqual(-wxPos, actualWxNeg, wxPos * 0.01);
                Assert.AreEqual(wyPos, actualWyPos, wyPos * 0.01);
                Assert.AreEqual(-wyPos, actualWyNeg, wyPos * 0.01);
            }
        }
        [TestMethod]
        public void MomentOfResistanceTest3()
        {
            ProgrammSettings.InicializeNew();
            Punching punching = PunchingFactory.GetPunching(PunchingType.TestType3_600х400х400);
            ILayerProcessor layerProcessor = new OneLayerProcessor();
            List<PunchingContour> contours = layerProcessor.GetPunchingContours(punching);
            IBearingProcessor bearingProcessor = new BearingProcessor();
            double b = punching.Width;
            double h = punching.Length;
            double h0 = contours[0].SubContours[0].Height;

            double um = (b + h + h0 + h0) * 2;
            double wx = (h + h0) * ((h + h0) / 3 + b + h0);
            double wy = (b + h0) * ((b + h0) / 3 + h + h0);

            foreach (PunchingContour contour in contours)
            {
                double Rbt = 980000;
                double wxPos = Rbt * wx * h0;
                double wyPos = Rbt * wy * h0;
                double[] actualW = (bearingProcessor as BearingProcessor).GetMomentResistance(contour, true);
                double actualWxPos = actualW[0];
                double actualWxNeg = actualW[1];
                double actualWyPos = actualW[2];
                double actualWyNeg = actualW[3];
                Assert.AreEqual(wxPos, actualWxPos, wxPos * 0.01);
                Assert.AreEqual(-wxPos, actualWxNeg, wxPos * 0.01);
                Assert.AreEqual(wyPos, actualWyPos, wyPos * 0.01);
                Assert.AreEqual(-wyPos, actualWyNeg, wyPos * 0.01);
            }
        }
    }
}
