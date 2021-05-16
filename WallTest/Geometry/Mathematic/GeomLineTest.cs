using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;

namespace Test.Geometry.Mathematic
{
    [TestClass]
    public class GeomLineTest
    {
        [TestMethod]
        public void LineMomentofInertia1()
        {
            Point2D startPoint = new Point2D(0, -2);
            Point2D endPoint = new Point2D(0, 2);
            Point2D center = new Point2D(0, 0);
            double[] moments = GeometryProcessor.GetLineMomentInertia(startPoint, endPoint, center);
            double Ix = moments[0];
            double Iy = moments[1];
            double expectedIx = 4.0 * 4.0 * 4.0 / 12;
            double expectedIy = 0;
            Assert.AreEqual(expectedIx, Ix, 0.0001);
            Assert.AreEqual(expectedIy, Iy, 0.0001);
        }
        [TestMethod]
        public void LineMomentofInertia2()
        {
            Point2D startPoint = new Point2D(0, 0);
            Point2D endPoint = new Point2D(0, 2);
            Point2D center = new Point2D(0, 0);
            double[] moments = GeometryProcessor.GetLineMomentInertia(startPoint, endPoint, center);
            double Ix = moments[0];
            double Iy = moments[1];
            double expectedIx = 2.0 * 2.0 * 2.0 / 12 + 2.0 * 1.0 * 1.0;
            double expectedIy = 0;
            Assert.AreEqual(expectedIx, Ix, 0.0001);
            Assert.AreEqual(expectedIy, Iy, 0.0001);
        }
        [TestMethod]
        public void LineMomentofInertia3()
        {
            Point2D startPoint = new Point2D(0, 2);
            Point2D endPoint = new Point2D(0, 4);
            Point2D center = new Point2D(0, 0);
            double[] moments = GeometryProcessor.GetLineMomentInertia(startPoint, endPoint, center);
            double Ix = moments[0];
            double Iy = moments[1];
            double expectedIx = 2.0 * 2.0 * 2.0 / 12 + 2.0 * 3.0 * 3.0;
            double expectedIy = 0;
            Assert.AreEqual(expectedIx, Ix, 0.0001);
            Assert.AreEqual(expectedIy, Iy, 0.0001);
        }
        //Отрезок вдоль оси X
        [TestMethod]
        public void LineMomentofInertia4()
        {
            Point2D startPoint = new Point2D(-4, 2);
            Point2D endPoint = new Point2D(4, 2);
            Point2D center = new Point2D(0, 0);
            double[] moments = GeometryProcessor.GetLineMomentInertia(startPoint, endPoint, center);
            double Ix = moments[0];
            double Iy = moments[1];
            double expectedIx = 8.0 * 2.0 * 2.0;
            double expectedIy = 8.0 * 8.0 * 8.0 / 12;
            Assert.AreEqual(expectedIx, Ix, 0.0001);
            Assert.AreEqual(expectedIy, Iy, 0.0001);
        }
    }
}
