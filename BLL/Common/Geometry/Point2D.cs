using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;
using System.Xml;

namespace RDBLL.Common.Geometry

{
    public class Point2D : ICloneable
    {
        public double X { get; set; }
        public double Y { get; set; }

        public String PointText()
        {
            return "(" + Convert.ToString(X) + ";" + Convert.ToString(Y) + ")";
        }

        public Point2D EndPoint (double Angle, double Length)
        {
            Point2D EndPoint = new Point2D(0,0);
            EndPoint.X = this.X + Convert.ToDouble(Math.Cos(Convert.ToDouble(Angle))) * Length;
            EndPoint.Y = this.Y + Convert.ToDouble(Math.Sin(Convert.ToDouble(Angle))) * Length;
            return EndPoint;
        }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument, String NodeName)
        {
            XmlElement xmlNode = xmlDocument.CreateElement(NodeName);
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Coord_X", Convert.ToString(X));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Coord_Y", Convert.ToString(Y));
            return xmlNode;
        }

        public Point2D()
        {
            X = 0;
            Y = 0;
        }

        public Point2D(double coord_X, double coord_Y)
        {
            X = coord_X;
            Y = coord_Y;
        }

        public Point2D(XmlNode xmlNode)
        {
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "Coord_X") X = Convert.ToDouble(obj.Value);
                if (obj.Name == "Coord_Y") Y = Convert.ToDouble(obj.Value);
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

        public double GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            return (StartPoint.X - EndPoint.X);
                //Math.Sqrt(Convert.ToDouble(StartPoint.coord_X - EndPoint.coord_X));
        }
    }
}
