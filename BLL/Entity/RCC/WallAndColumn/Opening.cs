using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.BuildingAndSite;


namespace RDBLL.Entity.RCC.WallAndColumn
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
        public bool MoveVert { get; set; } //Смешать вертикальные стержни
        public int QuantVertLeft { get; set; } //Количество вертикальных стержней обрамления, если 0 - Стержни не учащаются
        public int QuantVertRight { get; set; } //Количество вертикальных стержней обрамления, если 0 - Стержни не учащаются
        public int QuantInclined { get; set; } //Количество диагональных стержней обрамления
        public bool AddIncTopLeft { get; set; } //Флаг установки диагональных стержней
        public bool AddIncTopRight { get; set; } //Флаг установки диагональных стержней
        public bool AddIncBottomLeft { get; set; } //Флаг установки диагональных стержней
        public bool AddIncBottomRight { get; set; } //Флаг установки диагональных стержней

        public void SetDefault()
        {
            Name = "Новый проем";
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
            QuantInclined = 2;
            AddIncTopLeft = true;
            AddIncTopRight = true;
            AddIncBottomLeft = false;
            AddIncBottomRight = false;
        }
        //Возвращает площадь проема
        public decimal GetArea()
        {
            
            return Width * Height;
        }

        //Возвращает строку полного наименования
        public String FullName()
        {
            String S;
            S = Name + " - " + Purpose + " - " + Convert.ToString(Width) + "*" + Convert.ToString(Height) + "(h)";
            return S;
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
            if (Type == 4 || AddEdgeBottom) Quant = Convert.ToInt32(Math.Ceiling(Width / Spacing)) - 1;
            return Quant;
        }
        //Сохраняет класс в узел XML
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
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "QuantInclined", Convert.ToString(QuantInclined));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddIncTopLeft", Convert.ToString(AddIncTopLeft));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddIncTopRight", Convert.ToString(AddIncTopRight));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddIncBottomLeft", Convert.ToString(AddIncBottomLeft));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "AddIncBottomRight", Convert.ToString(AddIncBottomRight));
            return xmlNode;
        }
        public OpeningType(Building building)
        {
            Building = building;
            SetDefault();
            building.OpeningTypeList.Add(this);
        }
        public OpeningType(Building building, XmlNode xmlNode)
        {
            Building = building;
            SetDefault();
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "Name") Name = obj.Value;
                if (obj.Name == "Purpose") Purpose = obj.Value;
                if (obj.Name == "Height") Height = Convert.ToDecimal(obj.Value);
                if (obj.Name == "Width") Width = Convert.ToDecimal(obj.Value);
                if (obj.Name == "Bottom") Bottom = Convert.ToDecimal(obj.Value);
                if (obj.Name == "AddEdgeLeft") AddEdgeLeft = Convert.ToBoolean(obj.Value);
                if (obj.Name == "AddEdgeRight") AddEdgeRight = Convert.ToBoolean(obj.Value);
                if (obj.Name == "AddEdgeTop") AddEdgeTop = Convert.ToBoolean(obj.Value);
                if (obj.Name == "AddEdgeBottom") AddEdgeBottom = Convert.ToBoolean(obj.Value);
                if (obj.Name == "MoveVert") MoveVert = Convert.ToBoolean(obj.Value);
                if (obj.Name == "QuantVertLeft") QuantVertLeft = Convert.ToInt16(obj.Value);
                if (obj.Name == "QuantVertRight") QuantVertRight = Convert.ToInt16(obj.Value);
                if (obj.Name == "QuantInclined") QuantInclined = Convert.ToInt16(obj.Value);
                if (obj.Name == "AddIncTopLeft") AddIncTopLeft = Convert.ToBoolean(obj.Value);
                if (obj.Name == "AddIncTopRight") AddIncTopRight = Convert.ToBoolean(obj.Value);
                if (obj.Name == "AddIncBottomLeft") AddIncBottomLeft = Convert.ToBoolean(obj.Value);
                if (obj.Name == "AddIncBottomRight") AddIncBottomRight = Convert.ToBoolean(obj.Value);
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
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
        public OpeningPlacing(Wall wall, XmlNode xmlNode)
        {
            Wall = wall;
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "OpeningTypeNumber") OpeningType = wall.Level.Building.OpeningTypeList[Convert.ToInt16(obj.Value)];
                if (obj.Name == "Left") Left = Convert.ToDecimal(obj.Value);
                if (obj.Name == "OverrideBottom") OverrideBottom = Convert.ToBoolean(obj.Value);
                if (obj.Name == "Bottom") Bottom = Convert.ToDecimal(obj.Value);            
            }
        }
    }
}
