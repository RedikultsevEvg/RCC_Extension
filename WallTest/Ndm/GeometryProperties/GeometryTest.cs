using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.Common.NDM.Processors;
using System.Collections.Generic;
using RDBLL.Entity.Common.NDM.Interfaces;
using RDBLL.Entity.Common.NDM.MaterialModels;

namespace Test.Ndm.GeometryProperties
{
    [TestClass]
    public class GeometryTest
    {
        double tolerance = 0.01;
        [TestMethod]
        public void GravityCenter1()
        {
            double b = 2.0;
            double h = 0.5;
            double[] expected = new double[2] { b / 2, h / 2 };
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangle(materialModel, b, h, b / 2,  h / 2, 0.1);
            ndmAreas.AddRange(ndmRectangleAreas);
            double[] actual = NdmConcreteProcessor.GetGravityCenter(ndmAreas);
            Assert.AreEqual(expected[0], actual[0], expected[0] * tolerance);
        }
        [TestMethod]
        public void GravityCenter2()
        {
            double b = 2.0;
            double h = 0.5;
            double[] expected = new double[2] { b / 2, h / 2 };
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangle(materialModel, b, h, b / 2, h / 2, 0.1);
            ndmAreas.AddRange(ndmRectangleAreas);
            double[] actual = NdmConcreteProcessor.GetGravityCenter(ndmAreas);
            Assert.AreEqual(expected[1], actual[1], expected[1] * tolerance);
        }
        [TestMethod]
        public void MomentInertia1()
        {
            double b = 2.0;
            double h = 0.5;
            double Ix = b * h * h * h / 12;
            double Iy = h * b * b * b / 12;
            double[] expected = new double[2] { Ix, Iy };
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangle(materialModel, b, h, b / 2, h / 2, 0.1);
            ndmAreas.AddRange(ndmRectangleAreas);
            double[] actual = NdmConcreteProcessor.GetMomentInertia(ndmAreas, 1e10);
            Assert.AreEqual(expected[0], actual[0], expected[0] * tolerance);
        }
        [TestMethod]
        public void MomentInertia2()
        {
            double b = 2.0;
            double h = 0.5;
            double Ix = b * h * h * h / 12;
            double Iy = h * b * b * b / 12;
            double[] expected = new double[2] { Ix, Iy };
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangle(materialModel, b, h, b / 2, h / 2, 0.1);
            ndmAreas.AddRange(ndmRectangleAreas);
            double[] actual = NdmConcreteProcessor.GetMomentInertia(ndmAreas, 1e10);
            Assert.AreEqual(expected[1], actual[1], expected[1] * tolerance);
        }
        [TestMethod]
        public void SndMomentInertia1()
        {
            double b = 2.0;
            double h = 0.5;
            double Wx = b * h * h / 6;
            double Wy = h * b * b / 6;
            double[,] expected = new double[2, 2];
            expected[0, 0] = Wx;
            expected[0, 1] = Wx;
            expected[1, 0] = Wy;
            expected[1, 1] = Wy;
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangle(materialModel, b, h, b / 2, h / 2, 0.1);
            ndmAreas.AddRange(ndmRectangleAreas);
            double[,] actual = NdmConcreteProcessor.GetSndMomentInertia(ndmAreas, 1e10);
            Assert.AreEqual(expected[0,0], actual[0,0], expected[0,0] * tolerance);
        }
        [TestMethod]
        public void SndMomentInertia1_1()
        {
            double b = 2.0;
            double h = 0.5;
            double Wx = b * h * h / 6;
            double Wy = h * b * b / 6;
            double[,] expected = new double[2, 2];
            expected[0, 0] = Wx;
            expected[0, 1] = Wx;
            expected[1, 0] = Wy;
            expected[1, 1] = Wy;
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangleByCoord(materialModel, -b / 2, b / 2, 0, h, 0.1);
            ndmAreas.AddRange(ndmRectangleAreas);
            double[,] actual = NdmConcreteProcessor.GetSndMomentInertia(ndmAreas, 1e10);
            Assert.AreEqual(expected[0, 0], actual[0, 0], expected[0, 0] * tolerance);
        }
        [TestMethod]
        public void SndMomentInertia3()
        {
            double b = 2.0;
            double h = 0.5;
            double Wx = b * h * h / 6;
            double Wy = h * b * b / 6;
            double[,] expected = new double[2, 2];
            expected[0, 0] = Wx;
            expected[0, 1] = Wx;
            expected[1, 0] = Wy;
            expected[1, 1] = Wy;
            IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
            List<NdmArea> ndmAreas = new List<NdmArea>();
            List<NdmRectangleArea> ndmRectangleAreas = NdmAreaProcessor.MeshRectangle(materialModel, b, h, b / 2, h / 2, 0.1);
            ndmAreas.AddRange(ndmRectangleAreas);
            double[,] actual = NdmConcreteProcessor.GetSndMomentInertia(ndmAreas, 1e10);
            Assert.AreEqual(expected[1, 0], actual[1, 0], expected[1, 0] * tolerance);
        }
    }
}
