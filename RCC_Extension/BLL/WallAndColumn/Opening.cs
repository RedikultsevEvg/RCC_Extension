using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RCC_Extension.BLL.Service;
using RCC_Extension.BLL.BuildingAndSite;


namespace RCC_Extension.BLL.WallAndColumn
{
    public class OpeningType : ICloneable
    {
        public string Name { get; set; } //Наименование отверстия
        public Building Building { get; set; }
        public string Purpose { get; set; } //Назначение отверстия
        public decimal Width { get; set; } //Ширина отверстия
        public decimal Height { get; set; } //Высота отверстия
        public decimal Bottom { get; set; } //Привязка снизу (координата Y)
        public bool AddEdgeLeft { get; set; } //Устанавливать обрамление слева
        public bool AddEdgeRight { get; set; } //Справа
        public bool AddEdgeTop { get; set; } //Сверху
        public bool AddEdgeBottom { get; set; } //Снизу
        public bool MoveVert { get; set; }
        public int QuantVertLeft { get; set; } //Количество вертикальных стержней обрамления, если 0 - Стержни не учащаются
        public int QuantVertRight { get; set; } //Количество вертикальных стержней обрамления, если 0 - Стержни не учащаются

        public decimal GetArea()
        {
            //Возвращает площадь проема
            return Width * Height;
        }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement xmlNode = xmlDocument.CreateElement("OpeningType");
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Name", Name);
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Purpose", Purpose);
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Width", Convert.ToString(Width));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Height", Convert.ToString(Height));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Bottom", Convert.ToString(Bottom));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddEdgeLeft", Convert.ToString(AddEdgeLeft));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddEdgeRight", Convert.ToString(AddEdgeRight));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddEdgeTop", Convert.ToString(AddEdgeTop));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddEdgeBottom", Convert.ToString(AddEdgeBottom));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "MoveVert", Convert.ToString(MoveVert));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "QuantVertLeft", Convert.ToString(QuantVertLeft));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "QuantVertRight", Convert.ToString(QuantVertRight));
            return xmlNode;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int AddEdgeQuant(int Type, decimal Spacing)
        {
            //Возвращает количество элементов обрамления с соответствующей стороны проема
            //Type
            //1 - обрамление слева
            //2 - обрамление справа
            //3 - обрамление сверху
            //4 - обрамление снизу
            int Quant=0;
            if (Type == 1 || AddEdgeLeft) Quant = Convert.ToInt32(Math.Ceiling(Height / Spacing))-1;
            if (Type == 2 || AddEdgeRight) Quant = Convert.ToInt32(Math.Ceiling(Height / Spacing)) - 1;
            if (Type == 3 || AddEdgeTop) Quant = Convert.ToInt32(Math.Ceiling(Width / Spacing)) - 1;
            if (Type == 4 || AddEdgeRight) Quant = Convert.ToInt32(Math.Ceiling(Width / Spacing)) - 1;
            return Quant;
        }

        public OpeningType(Building building)
        {
            Name = "Новый проем";
            Building = building;
            Purpose = "АР";
            Height = 2200;
            Width = 1000;
            Bottom = 0;
            AddEdgeLeft = true;
            AddEdgeRight = true;
            AddEdgeTop = true;
            AddEdgeBottom = false;
            MoveVert = true;
            QuantVertLeft = 2;
            QuantVertRight = 2;

            building.OpeningTypeList.Add(this);
        }

    }
    //Класс для вставки проема в стену
    public class OpeningPlacing
    {
        public Wall Wall { get; set; }
        public OpeningType OpeningType { get; set; }
        public decimal Left { get; set; }
        public bool OverrideBottom { get; set; }
        public decimal Bottom { get; set; }

        public decimal GetBottom()
        {
            decimal bottom = OpeningType.Bottom;
            if (OverrideBottom) { bottom = Bottom; }
            return bottom;
        }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement xmlNode = xmlDocument.CreateElement("OpeningPlacing");
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "WallNumber", Convert.ToString(Wall.Level.WallList.IndexOf(Wall)));
            int counter = 0;
            foreach (OpeningType OpeningTypeItem in Wall.Level.Building.OpeningTypeList)
            {
                if (ReferenceEquals(OpeningTypeItem, OpeningType))
                { XMLOperations.AddAttribute(xmlNode, xmlDocument, "OpeningTypeNumber", Convert.ToString(counter)); }
                counter++;
            }
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Left", Convert.ToString(Left));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "OverrideBottom", Convert.ToString(OverrideBottom));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Bottom", Convert.ToString(Bottom));
            return xmlNode;
        }

        public OpeningPlacing(Wall wall)
        {
            Wall = wall;
            Building building = Wall.Level.Building;
            if (building.OpeningTypeList.Count == 0)
            { OpeningType = new OpeningType(building); }
            else { OpeningType = building.OpeningTypeList[0]; }
            Left = 1000;
            OverrideBottom = false;
            Bottom = 0;
            wall.OpeningPlacingList.Add(this);
        }
    }
}
