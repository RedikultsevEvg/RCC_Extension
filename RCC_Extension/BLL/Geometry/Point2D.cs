﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.Service;
using System.Xml;

namespace RCC_Extension.BLL.Geometry

{
    public class Point2D : ICloneable
    {
        public decimal Coord_X { get; set; }
        public decimal Coord_Y { get; set; }

        public String PointText()
        {
            return "(" + Convert.ToString(Coord_X) + ";" + Convert.ToString(Coord_Y) + ")";
        }

        public Point2D EndPoint (decimal Angle, decimal Length)
        {
            Point2D EndPoint = new Point2D(0,0);
            EndPoint.Coord_X = this.Coord_X + Convert.ToDecimal(Math.Cos(Convert.ToDouble(Angle))) * Length;
            EndPoint.Coord_Y = this.Coord_Y + Convert.ToDecimal(Math.Sin(Convert.ToDouble(Angle))) * Length;
            return EndPoint;
        }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument, String NodeName)
        {
            XmlElement xmlNode = xmlDocument.CreateElement(NodeName);
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Coord_X", Convert.ToString(Coord_X));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Coord_Y", Convert.ToString(Coord_Y));
            return xmlNode;
        }

        public Point2D(decimal coord_X, decimal coord_Y)
        {
            Coord_X = coord_X;
            Coord_Y = coord_Y;
        }

        public Point2D(XmlNode xmlNode)
        {
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "Coord_X") Coord_X = Convert.ToDecimal(obj.Value);
                if (obj.Name == "Coord_Y") Coord_Y = Convert.ToDecimal(obj.Value);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Vector2D
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public decimal GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            return (StartPoint.Coord_X - EndPoint.Coord_X);
                //Math.Sqrt(Convert.ToDouble(StartPoint.coord_X - EndPoint.coord_X));
        }
    }

    public class Geometry2D
    {
        public decimal GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            decimal dX = EndPoint.Coord_X - StartPoint.Coord_X;
            decimal dY = EndPoint.Coord_Y - StartPoint.Coord_Y;
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(dX*dX + dY*dY)));
        }
    }
}
