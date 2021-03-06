﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using System.Xml;
using RDBLL.Common.Service;
using RDBLL.Common.Geometry.Mathematic;

namespace RDBLL.Entity.RCC.Reinforcement
{
    public class Bar
    {
        public String Name { get; set; }
        public double Diametr { get; set;}
        public String ClassName { get; set; }
        public Path Path { get; set; }
    }

    public class BarSpacingSettings :ICloneable
    {
        public bool AddBarsLeft { get; set; }
        public bool AddBarsRight { get; set; }
        public double AddBarsLeftSpacing { get; set; }
        public double AddBarsRightSpacing { get; set; }
        public Int32 AddBarsLeftQuant { get; set; }
        public Int32 AddBarsRightQuant { get; set; }
        public double MainSpacing { get; set; }
        public bool AddBarsEdgeStart { get; set; }
        public bool AddBarsEdgeEnd { get; set; }

        public String SpacingText()
        {
            String S="Шаг ";
            if (AddBarsLeft) S += Convert.ToString(AddBarsLeftQuant - 1) + "*" + Convert.ToString(AddBarsLeftSpacing) + ";";
            S += Convert.ToString(MainSpacing) + ";";
            if (AddBarsRight) S += Convert.ToString(AddBarsRightQuant - 1) + "*" + Convert.ToString(AddBarsRightSpacing) + ";";
            return S;
        }
        public int BarQuantity(double length)
        {
            int quant = 0;
            if (AddBarsLeft)
            {
                quant += AddBarsLeftQuant;
                length -= (AddBarsLeftQuant - 1) * AddBarsLeftSpacing;
            }
            else { quant ++; }

            if (AddBarsRight)
            {
                quant += AddBarsRightQuant;
                length -= (AddBarsRightQuant - 1) * AddBarsRightSpacing;
            }
            else { quant++; }
            quant += Convert.ToInt32(Math.Ceiling(length / MainSpacing)) - 1;
            return quant;
        }
        public XmlElement SaveToXMLNode(XmlDocument xmlDocument, String NodeName)
        {
            XmlElement xmlNode = xmlDocument.CreateElement(NodeName);
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddBarsLeft", Convert.ToString(AddBarsLeft));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddBarsRight", Convert.ToString(AddBarsRight));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddBarsLeftSpacing", Convert.ToString(AddBarsLeftSpacing));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddBarsRightSpacing", Convert.ToString(AddBarsRightSpacing));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddBarsLeftQuant", Convert.ToString(AddBarsLeftQuant));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddBarsRightQuant", Convert.ToString(AddBarsRightQuant));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "MainSpacing", Convert.ToString(MainSpacing));
            return xmlNode;
        }
        public BarSpacingSettings(int Type)
        {
            switch (Type) //
            {
                case 1: //Вертикальная раскладка
                    {
                        AddBarsLeft = true;
                        AddBarsLeftQuant = 2;
                        AddBarsLeftSpacing = 100;
                        AddBarsRight = true;
                        AddBarsRightQuant = 2;
                        AddBarsRightSpacing = 100;
                        MainSpacing = 200;
                        break;
                    }
                case 2: //Горизонтальная раскладка
                    {
                        AddBarsLeft = false;
                        AddBarsLeftQuant = 2;
                        AddBarsLeftSpacing = 100;
                        AddBarsRight = false;
                        AddBarsRightQuant = 2;
                        AddBarsRightSpacing = 100;
                        MainSpacing = 200;
                        break;
                    }
            }
        }
        public BarSpacingSettings(XmlNode xmlNode)
        {
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "AddBarsLeft") AddBarsLeft = Convert.ToBoolean(obj.Value);
                if (obj.Name == "AddBarsRight") AddBarsRight = Convert.ToBoolean(obj.Value);
                if (obj.Name == "AddBarsLeftSpacing") AddBarsLeftSpacing = Convert.ToDouble(obj.Value);
                if (obj.Name == "AddBarsRightSpacing") AddBarsRightSpacing = Convert.ToDouble(obj.Value);
                if (obj.Name == "AddBarsLeftQuant") AddBarsLeftQuant = Convert.ToInt32(obj.Value);
                if (obj.Name == "AddBarsRightQuant") AddBarsRightQuant = Convert.ToInt32(obj.Value);
                if (obj.Name == "MainSpacing") MainSpacing = Convert.ToDouble(obj.Value);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class BarLineSpacing
    {
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }

        public double StartOffset { get; set; }
        public double EndOffset { get; set; }

        public bool AddStartBar { get; set; }
        public bool AddEndBar { get; set; }

        public BarSpacingSettings barSpacingSettings { get; set; }

        public Int32 BarQuantity ()
        {
            int Quant = 0;
            double length = GeometryProc.GetDistance(this.StartPoint, this.EndPoint);
            double mainLength = length - this.StartOffset-this.EndOffset;
             
            if (this.barSpacingSettings.AddBarsLeft)
            {
                Quant += this.barSpacingSettings.AddBarsLeftQuant;
                mainLength -= this.barSpacingSettings.AddBarsLeftSpacing * this.barSpacingSettings.AddBarsLeftQuant;
            }
            if (this.barSpacingSettings.AddBarsRight)
            {
                Quant += this.barSpacingSettings.AddBarsRightQuant;
                mainLength -= this.barSpacingSettings.AddBarsRightSpacing * this.barSpacingSettings.AddBarsRightQuant;
            }

            Quant += Convert.ToInt16(Math.Ceiling(mainLength / barSpacingSettings.MainSpacing));
            return Quant;
        }
    }
}
