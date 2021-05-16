using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Interfaces.Shapes;

namespace Test.Geometry.Mathematic.PointGeometryTests
{
    [TestClass]
    public class OffsetGeometryTest
    {
        [TestMethod]
        public void OffsetRectangleTest()
        {
            double addWidth = 0.1;
            IRectangle rect = new RectCrossSection(0.4, 0.6);
            IRectangle rect2 = GeometryProcessor.GetRectangleOffset(rect, addWidth);
            Assert.AreEqual(rect.Width + 2 * addWidth, rect2.Width, 0.0001);
            Assert.AreEqual(rect.Length + 2 * addWidth, rect2.Length, 0.0001);
            List<Point2D> points = GeometryProcessor.GetAnglePointsFromRectangle(rect2);
            //Левая нижняя точка
            Assert.AreEqual(-0.3, points[0].X, 0.0001);
            Assert.AreEqual(-0.4, points[0].Y, 0.0001);
            //Левая верхняя точка
            Assert.AreEqual(-0.3, points[1].X, 0.0001);
            Assert.AreEqual(0.4, points[1].Y, 0.0001);
            //Правая нижняя точка
            Assert.AreEqual(0.3, points[2].X, 0.0001);
            Assert.AreEqual(-0.4, points[2].Y, 0.0001);
            //Правая верхняя точка
            Assert.AreEqual(0.3, points[3].X, 0.0001);
            Assert.AreEqual(0.4, points[3].Y, 0.0001);
        }
        [TestMethod]
        public void OffsetRectangleTest2()
        {
            double[] addWidth = new double[4] { 0.1, 0.2, 0.3, 0.4};
            IRectangle rect = new RectCrossSection(0.4, 0.6);
            IRectangle rect2 = GeometryProcessor.GetRectangleOffset(rect, addWidth);
            Assert.AreEqual(rect.Width + addWidth[0] + addWidth[1], rect2.Width, 0.0001);
            Assert.AreEqual(rect.Length + addWidth[2] + addWidth[3], rect2.Length, 0.0001);
            List<Point2D> points = GeometryProcessor.GetAnglePointsFromRectangle(rect2);
            //Левая нижняя точка
            Assert.AreEqual(-0.3, points[0].X, 0.0001);
            Assert.AreEqual(-0.7, points[0].Y, 0.0001);
            //Левая верхняя точка
            Assert.AreEqual(-0.3, points[1].X, 0.0001);
            Assert.AreEqual(0.6, points[1].Y, 0.0001);
            //Правая нижняя точка
            Assert.AreEqual(0.4, points[2].X, 0.0001);
            Assert.AreEqual(-0.7, points[2].Y, 0.0001);
            //Правая верхняя точка
            Assert.AreEqual(0.4, points[3].X, 0.0001);
            Assert.AreEqual(0.6, points[3].Y, 0.0001);
        }
    }
}
