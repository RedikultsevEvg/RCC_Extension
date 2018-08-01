﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.BuildingAndSite;
using RCC_Extension.BLL.Geometry;
using RCC_Extension.BLL.Service;
using System.Xml;


namespace RCC_Extension.BLL.WallAndColumn
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
        public decimal Height { get; set; }
        public decimal ConcreteStartOffset { get; set; }
        public decimal ConcreteEndOffset { get; set; }
        public decimal ReiforcementStartOffset { get; set; }
        public decimal ReiforcementEndOffset { get; set; }

        //Start of methods
        public decimal GetLength()
        {
            Geometry2D Geometry2D = new Geometry2D();
            return Geometry2D.GetDistance(this.StartPoint, this.EndPoint);
        }
        public decimal GetConcreteLength()
        {
            return GetLength() + ConcreteStartOffset + ConcreteEndOffset;
        }
        public decimal GetHeight()
        {
            if (ReWriteHeight) { return Height; }
            else { return WallType.GetHeight(Level); }
        }
        public decimal GetConcreteAreaBrutto()
        {
            decimal Area = 0;
            Area = GetConcreteLength() * GetHeight();
            return Area;
        }
        public decimal GetConcreteAreaNetto()
        {
            decimal Area = 0;
            Area = GetConcreteAreaBrutto() - GetOpeningsArea();
            return Area;
        }
        public decimal GetOpeningsArea()
        {
            decimal Area = 0;
            //Возвращает суммарную площадь всех проемов
            foreach (OpeningPlacing _openingPlacing in OpeningPlacingList)
            {
                Area += _openingPlacing.OpeningType.GetArea();
            }
            return Area;
        }
        public decimal GetConcreteVolumeBrutto()
        {
            decimal Volume = GetConcreteAreaBrutto() * WallType.Thickness;
            return Volume;
        }
        public decimal GetConcreteVolumeNetto()
        {
            decimal Volume = GetConcreteAreaNetto() * WallType.Thickness;
            return Volume;
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
            decimal length = GetLength() + ReiforcementStartOffset + ReiforcementEndOffset - 50 - 50;
            quant = WallType.VertSpacingSetting.BarQuantity(length);
            foreach (OpeningPlacing obj in OpeningPlacingList)
            {
                if (obj.OpeningType.MoveVert)
                {
                    opening_quant = obj.OpeningType.QuantVertLeft + obj.OpeningType.QuantVertRight;
                    opening_quant -= Convert.ToInt32(Math.Ceiling(obj.OpeningType.Width / WallType.VertSpacingSetting.MainSpacing)) - 1;
                    quant -= opening_quant;
                }
            }
            quant *= 2;
            return quant;
        }
        public int HorBarQuantity()
        {
            int quant;
            decimal length = GetHeight() - 50 - 50;
            quant = WallType.VertSpacingSetting.BarQuantity(length);
            return quant;
        }
        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement xmlNode = xmlDocument.CreateElement("Wall");
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Name", Name);
            int counter = 0;
            foreach (WallType wallTypeItem in Level.Building.WallTypeList)
            {
                if (ReferenceEquals(wallTypeItem, WallType))
                { XMLOperations.AddAttribute(xmlNode, xmlDocument, "WallTypeNumber", Convert.ToString(counter)); }
                counter ++;
            }
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "ReWriteHeight", Convert.ToString(ReWriteHeight));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Height", Convert.ToString(Height));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "ConcreteStartOffset", Convert.ToString(ConcreteStartOffset));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "ConcreteEndOffset", Convert.ToString(ConcreteEndOffset));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "ReiforcementStartOffset", Convert.ToString(ReiforcementStartOffset));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "ReiforcementEndOffset", Convert.ToString(ReiforcementEndOffset));
            XmlElement StartPointNode = StartPoint.SaveToXMLNode(xmlDocument, "StartPoint");
            xmlNode.AppendChild(StartPointNode);
            XmlElement EndPointNode = EndPoint.SaveToXMLNode(xmlDocument, "EndPoint");
            xmlNode.AppendChild(EndPointNode);
            foreach (OpeningPlacing obj in OpeningPlacingList)
            {
                xmlNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            return xmlNode;
        }
        //Конструктор стены по уровню (выбирается первый из списка или создается)
        public Wall(Level level)
        {
            Building building = level.Building;
            WallType wallType;
            if (building.WallTypeList.Count == 0)
            { wallType = new WallType(building); }
            else { wallType = building.WallTypeList[0]; }
            Name = "Новая стена";
            StartPoint = new Point2D(0, 0);
            EndPoint = new Point2D(6000, 0);
            ReWriteHeight = false;
            Height = 3000;
            ConcreteStartOffset = 0;
            ConcreteEndOffset = 0;
            ReiforcementStartOffset = 0;
            ReiforcementEndOffset = 0;
            Level = level;
            WallType = wallType;
            OpeningPlacingList = new List<OpeningPlacing>();
            level.WallList.Add(this);

        }
        public Wall(Level level, XmlNode xmlNode)
        {
            Level = level;
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {     
                if (obj.Name == "Name") Name = obj.Value; 
                if (obj.Name == "WallTypeNumber") WallType = level.Building.WallTypeList[Convert.ToInt16(obj.Value)];
                if (obj.Name == "ReWriteHeight") ReWriteHeight = Convert.ToBoolean(obj.Value);
                if (obj.Name == "Height") Height = Convert.ToDecimal(obj.Value);
                if (obj.Name == "ConcreteStartOffset") ConcreteStartOffset = Convert.ToDecimal(obj.Value);
                if (obj.Name == "ConcreteEndOffset") ConcreteEndOffset = Convert.ToDecimal(obj.Value);
                if (obj.Name == "ReiforcementStartOffset") ReiforcementStartOffset = Convert.ToDecimal(obj.Value);
                if (obj.Name == "ReiforcementEndOffset") ReiforcementEndOffset = Convert.ToDecimal(obj.Value);             
            }
            OpeningPlacingList = new List<OpeningPlacing>();
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name == "StartPoint") StartPoint = new Point2D(childNode);
                if (childNode.Name == "EndPoint") EndPoint = new Point2D(childNode);
                if (childNode.Name == "OpeningPlacing") OpeningPlacingList.Add(new OpeningPlacing(this, childNode));
            }

        }
            //Конструктор стены по уровню и типу стены
        public Wall(Level level, WallType wallType)
        {  
            Name = "Новая стена";
            StartPoint = new Point2D(0, 0);
            EndPoint = new Point2D(6, 0);
            Level = level;
            WallType = wallType;
            level.WallList.Add(this);
        }
        //Построение стены по двум точкам
        public Wall(Point2D StartPoint, Point2D EndPoint)
        {    
            Name = "Новая стена";
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;

        }
        //Построение стены по начальной точке, углу и длине
        public Wall(Point2D StartPoint, decimal Angle, decimal Length)
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
