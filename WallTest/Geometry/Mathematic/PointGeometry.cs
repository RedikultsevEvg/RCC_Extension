using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;

namespace Test.Geometry.Mathematic
{
    [TestClass]
    public class PointGeometry
    {
        [TestMethod]
        public void WithFilling()
        {
            Point2D Center = new Point2D(0, 0);
            double SizeX = 1.2;
            double SizeY = 2.4;
            int QuantityX = 4;
            int QuantityY = 6;
            bool FillArray = true;
            List<Point2D> points = GeometryProcessor.GetRectArrayPoints(Center, SizeX, SizeY, QuantityX, QuantityY, FillArray);
            int count = points.Count();
            Assert.AreEqual(24, count);
        }
        [TestMethod]
        public void WithoutFilling()
        {
            Point2D Center = new Point2D(0, 0);
            double SizeX = 1.2;
            double SizeY = 2.4;
            int QuantityX = 4;
            int QuantityY = 6;
            bool FillArray = false;
            List<Point2D> points = GeometryProcessor.GetRectArrayPoints(Center, SizeX, SizeY, QuantityX, QuantityY, FillArray);
            int count = points.Count();
            Assert.AreEqual(16, count);
        }
        [TestMethod]
        public void WithFillingBottomLeftPoint()
        {
            Point2D center = new Point2D(0, 0);
            double sizeX = 1.2;
            double sizeY = 2.4;
            int quantityX = 4;
            int quantityY = 6;
            bool FillArray = true;
            List<Point2D> points = GeometryProcessor.GetRectArrayPoints(center, sizeX, sizeY, quantityX, quantityY, FillArray);
            Assert.AreEqual(-sizeX/2, points[0].X, sizeX * 0.001);
        }
    }
}
