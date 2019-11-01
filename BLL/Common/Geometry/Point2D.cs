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
        public double Coord_X { get; set; }
        public double Coord_Y { get; set; }

        public String PointText()
        {
            return "(" + Convert.ToString(Coord_X) + ";" + Convert.ToString(Coord_Y) + ")";
        }

        public Point2D EndPoint (double Angle, double Length)
        {
            Point2D EndPoint = new Point2D(0,0);
            EndPoint.Coord_X = this.Coord_X + Convert.ToDouble(Math.Cos(Convert.ToDouble(Angle))) * Length;
            EndPoint.Coord_Y = this.Coord_Y + Convert.ToDouble(Math.Sin(Convert.ToDouble(Angle))) * Length;
            return EndPoint;
        }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument, String NodeName)
        {
            XmlElement xmlNode = xmlDocument.CreateElement(NodeName);
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Coord_X", Convert.ToString(Coord_X));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Coord_Y", Convert.ToString(Coord_Y));
            return xmlNode;
        }

        public Point2D(double coord_X, double coord_Y)
        {
            Coord_X = coord_X;
            Coord_Y = coord_Y;
        }

        public Point2D(XmlNode xmlNode)
        {
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "Coord_X") Coord_X = Convert.ToDouble(obj.Value);
                if (obj.Name == "Coord_Y") Coord_Y = Convert.ToDouble(obj.Value);
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
            return (StartPoint.Coord_X - EndPoint.Coord_X);
                //Math.Sqrt(Convert.ToDouble(StartPoint.coord_X - EndPoint.coord_X));
        }
    }

    public class Geometry2D
    {
        public double GetDistance(Point2D StartPoint, Point2D EndPoint)
        {
            double dX = EndPoint.Coord_X - StartPoint.Coord_X;
            double dY = EndPoint.Coord_Y - StartPoint.Coord_Y;
            return Convert.ToDouble(Math.Sqrt(Convert.ToDouble(dX*dX + dY*dY)));
        }
    }
}
