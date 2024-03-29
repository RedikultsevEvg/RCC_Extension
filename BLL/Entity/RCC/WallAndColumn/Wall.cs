﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Geometry;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.Reinforcement;
using System.Xml;
using RDBLL.Common.Geometry.Mathematic;


namespace RDBLL.Entity.RCC.WallAndColumn
{
    public class Wall :ICloneable
    {
        public String Name { get; set; }
        public WallType WallType { get; set; }     
        public Level Level { get; set; }
        public List<OpeningPlacing> OpeningPlacingList { get; set; } //Поменять на OpeningPlacing
        public Point2D StartPoint { get; set; }
        public Point2D EndPoint { get; set; }
        public bool ReWriteHeight { get; set; }
        public double Height { get; set; }
        public double ConcreteStartOffset { get; set; }
        public double ConcreteEndOffset { get; set; }
        public double ReiforcementStartOffset { get; set; }
        public double ReiforcementEndOffset { get; set; }
        public bool OverrideVertSpacing { get; set; }
        public bool OverrideHorSpacing { get; set; }
        public BarSpacingSettings VertSpacingSetting { get; set; }
        public BarSpacingSettings HorSpacingSetting { get; set; }

        //Start of methods
        public void SetDefault()
        {
            Name = "Новая стена";
            StartPoint = new Point2D(0, 0);
            EndPoint = new Point2D(6000, 0);
            ReWriteHeight = false;
            Height = 3000;
            ConcreteStartOffset = 0;
            ConcreteEndOffset = 0;
            ReiforcementStartOffset = 0;
            ReiforcementEndOffset = 0;
            OpeningPlacingList = new List<OpeningPlacing>();
            OverrideVertSpacing = false;
            OverrideHorSpacing = false;
            VertSpacingSetting = new BarSpacingSettings(1);
            HorSpacingSetting = new BarSpacingSettings(2);
        }
        public double GetLength()
        {
            return GeometryProc.GetDistance(this.StartPoint, this.EndPoint);
        }
        public double GetConcreteLength()
        {
            return GetLength() + ConcreteStartOffset + ConcreteEndOffset;
        }
        public double GetHeight()
        {
            if (ReWriteHeight) { return Height; }
            else { return WallType.GetHeight(Level); }
        }
        public double GetConcreteAreaBrutto()
        {
            double Area = 0;
            Area = GetConcreteLength() * GetHeight();
            return Area;
        }
        public double GetConcreteAreaNetto()
        {
            double Area = 0;
            Area = GetConcreteAreaBrutto() - GetOpeningsArea();
            return Area;
        }
        public double GetOpeningsArea()
        {
            double Area = 0;
            //Возвращает суммарную площадь всех проемов
            foreach (OpeningPlacing _openingPlacing in OpeningPlacingList)
            {
                Area += _openingPlacing.OpeningType.GetArea();
            }
            return Area;
        }
        public double GetConcreteVolumeBrutto()
        {
            double Volume = GetConcreteAreaBrutto() * WallType.Thickness;
            return Volume;
        }
        public double GetConcreteVolumeNetto()
        {
            double Volume = GetConcreteAreaNetto() * WallType.Thickness;
            return Volume;
        }
        public double ReinforcementLength()
        {
            return GetLength() + ReiforcementStartOffset + ReiforcementEndOffset;
        }
        public double ReinforcementArea()
        {
            return ReinforcementLength() * GetHeight();
        }
        public String GetStringSizes()
        {
            String sizes = "";
            //sizes += WallType.Thickness + " x ";
            sizes += GetLength() + " x ";
            sizes += GetHeight() + " (h)";
            return sizes;
        }
        public String GetStringOpenings()
        {
            String openings = "";
            foreach (OpeningPlacing obj in OpeningPlacingList)
            {
                openings += obj.OpeningType.Name + "; ";
            }
            return openings;
        }
        public int VertBarQuantity()
        {
            int quant, opening_quant;
            BarSpacingSettings barSpacingSettings;
            if (OverrideVertSpacing) { barSpacingSettings = VertSpacingSetting; } else { barSpacingSettings = WallType.VertSpacingSetting; }
            double length = GetLength() + ReiforcementStartOffset + ReiforcementEndOffset - 50 - 50;
            quant = barSpacingSettings.BarQuantity(length);
            foreach (OpeningPlacing obj in OpeningPlacingList)
            {
                if (obj.OpeningType.MoveVert)
                {
                    opening_quant = obj.OpeningType.QuantVertLeft + obj.OpeningType.QuantVertRight;
                    opening_quant -= Convert.ToInt32(Math.Ceiling(obj.OpeningType.Width / barSpacingSettings.MainSpacing)) - 1;
                    quant -= opening_quant;
                }
            }
            quant *= 2;
            return quant;
        } 
        public int HorBarQuantity()
        {
            int quant;
            BarSpacingSettings barSpacingSettings;
            if (OverrideHorSpacing) { barSpacingSettings = HorSpacingSetting; } else { barSpacingSettings = WallType.HorSpacingSetting; }
            double length = GetHeight() - 50 - 50;
            quant = barSpacingSettings.BarQuantity(length);
            return quant;
        }
        public double HorBarLength()
        {
            double length;
            double area = ReinforcementArea();
            double openingsArea = GetOpeningsArea();
            double lapCoefficient = 1;
            if (WallType.HorLapping) { lapCoefficient += WallType.HorLappingLength / WallType.HorBaseLength; }
            length = HorBarQuantity() * ReinforcementLength() * lapCoefficient * (area- openingsArea) / area;
            length *= 2;
            return length;
        }
        public int IncBarQuantity()
        {
            int quant = 0;
            foreach (OpeningPlacing obj in OpeningPlacingList)
            {
                if (obj.OpeningType.AddIncTopLeft) { quant += obj.OpeningType.QuantInclined; }
                if (obj.OpeningType.AddIncTopRight) { quant += obj.OpeningType.QuantInclined; }
                if (obj.OpeningType.AddIncBottomLeft) { quant += obj.OpeningType.QuantInclined; }
                if (obj.OpeningType.AddIncBottomRight) { quant += obj.OpeningType.QuantInclined; }
            }
            quant *= 2;
            return quant;
        }
        public int VertEdgeQuant()
        {
            int quant = 0;
            BarSpacingSettings barSpacingSettings;
            if (OverrideVertSpacing) { barSpacingSettings = VertSpacingSetting; } else { barSpacingSettings = WallType.VertSpacingSetting; }
            quant += VertBarQuantity();
            quant += VertBarQuantity();
            foreach (OpeningPlacing _openingPlacing in OpeningPlacingList)
            {
                quant += _openingPlacing.OpeningType.AddEdgeQuant(3, barSpacingSettings.MainSpacing)+ _openingPlacing.OpeningType.AddEdgeQuant(4, barSpacingSettings.MainSpacing);
            }
            return quant;
        }
        public int HorEdgeQuant()
        {
            int quant = 0;
            BarSpacingSettings barSpacingSettings;
            if (OverrideHorSpacing) { barSpacingSettings = HorSpacingSetting; } else { barSpacingSettings = WallType.HorSpacingSetting; }
            quant += HorBarQuantity();
            quant += HorBarQuantity();
            foreach (OpeningPlacing _openingPlacing in OpeningPlacingList)
            {
                quant += _openingPlacing.OpeningType.AddEdgeQuant(1, barSpacingSettings.MainSpacing) + _openingPlacing.OpeningType.AddEdgeQuant(2, barSpacingSettings.MainSpacing);
            }
            return quant;
        }
        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement xmlNode = xmlDocument.CreateElement("Wall");
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "Name", Name);
            //int counter = 0;
            //foreach (WallType wallTypeItem in Level.ParentMember.WallTypeList)
            //{
            //    if (ReferenceEquals(wallTypeItem, WallType))
            //    { XMLOperations.AddAttribute(xmlNode, xmlDocument, "WallTypeNumber", Convert.ToString(counter)); }
            //    counter ++;
            //}
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "ReWriteHeight", Convert.ToString(ReWriteHeight));
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "Height", Convert.ToString(Height));
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "ConcreteStartOffset", Convert.ToString(ConcreteStartOffset));
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "ConcreteEndOffset", Convert.ToString(ConcreteEndOffset));
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "ReiforcementStartOffset", Convert.ToString(ReiforcementStartOffset));
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "ReiforcementEndOffset", Convert.ToString(ReiforcementEndOffset));
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "OverrideVertSpacing", Convert.ToString(OverrideVertSpacing));
            //XMLOperations.AddAttribute(xmlNode, xmlDocument, "OverrideHorSpacing", Convert.ToString(OverrideHorSpacing));
            //XmlElement StartPointNode = StartPoint.SaveToXMLNode(xmlDocument, "StartPoint");
            //xmlNode.AppendChild(StartPointNode);
            //XmlElement EndPointNode = EndPoint.SaveToXMLNode(xmlDocument, "EndPoint");
            //xmlNode.AppendChild(EndPointNode);
            //foreach (OpeningPlacing obj in OpeningPlacingList)
            //{
            //    xmlNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            //}
            //XmlElement VertSpacingSettingNode = VertSpacingSetting.SaveToXMLNode(xmlDocument, "VertSpacingSetting");
            //xmlNode.AppendChild(VertSpacingSettingNode);
            //XmlElement HorSpacingSettingNode = HorSpacingSetting.SaveToXMLNode(xmlDocument, "HorSpacingSetting");
            //xmlNode.AppendChild(HorSpacingSettingNode);
            return xmlNode;
        }
        //Конструктор стены по уровню (выбирается первый из списка или создается)
        public Wall(Level level)
        {
            //Building building = level.ParentMember;
            //WallType wallType;
            //if (building.WallTypeList.Count == 0)
            //{ wallType = new WallType(building); }
            //else { wallType = building.WallTypeList[0]; }
            //Level = level;
            //WallType = wallType;
            //SetDefault();
            //level.Walls.Add(this);

        }
        public Wall(Level level, XmlNode xmlNode)
        {
            //Level = level;
            //SetDefault();
            //foreach (XmlAttribute obj in xmlNode.Attributes)
            //{
            //    if (obj.Name == "WallTypeNumber") WallType = level.ParentMember.WallTypeList[Convert.ToInt16(obj.Value)];
            //    if (obj.Name == "Name") Name = obj.Value; 
            //    if (obj.Name == "ReWriteHeight") ReWriteHeight = Convert.ToBoolean(obj.Value);
            //    if (obj.Name == "Height") Height = Convert.ToDouble(obj.Value);
            //    if (obj.Name == "ConcreteStartOffset") ConcreteStartOffset = Convert.ToDouble(obj.Value);
            //    if (obj.Name == "ConcreteEndOffset") ConcreteEndOffset = Convert.ToDouble(obj.Value);
            //    if (obj.Name == "ReiforcementStartOffset") ReiforcementStartOffset = Convert.ToDouble(obj.Value);
            //    if (obj.Name == "ReiforcementEndOffset") ReiforcementEndOffset = Convert.ToDouble(obj.Value);
            //    if (obj.Name == "OverrideVertSpacing") OverrideVertSpacing = Convert.ToBoolean(obj.Value);
            //    if (obj.Name == "OverrideHorSpacing") OverrideHorSpacing = Convert.ToBoolean(obj.Value);
            //}
            //OpeningPlacingList = new List<OpeningPlacing>();
            //foreach (XmlNode childNode in xmlNode.ChildNodes)
            //{
            //    if (childNode.Name == "StartPoint") StartPoint = new Point2D(childNode);
            //    if (childNode.Name == "EndPoint") EndPoint = new Point2D(childNode);
            //    if (childNode.Name == "OpeningPlacing") OpeningPlacingList.Add(new OpeningPlacing(this, childNode));
            //    if (childNode.Name == "VertSpacingSetting") VertSpacingSetting = new BarSpacingSettings(childNode);
            //    if (childNode.Name == "HorSpacingSetting") HorSpacingSetting = new BarSpacingSettings(childNode);
            //}

        }
            //Конструктор стены по уровню и типу стены
        public Wall(Level level, WallType wallType)
        {  
            Name = "Новая стена";
            StartPoint = new Point2D(0, 0);
            EndPoint = new Point2D(6, 0);
            Level = level;
            WallType = wallType;
            level.Walls.Add(this);
        }
        //Построение стены по двум точкам
        public Wall(Point2D StartPoint, Point2D EndPoint)
        {    
            Name = "Новая стена";
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;

        }
        //Построение стены по начальной точке, углу и длине
        public Wall(Point2D StartPoint, double Angle, double Length)
        {       
            Name = "Новая стена";
            this.StartPoint = StartPoint;
            this.EndPoint = StartPoint.EndPoint(Angle, Length);
        }
        //Клонирование объекта
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
